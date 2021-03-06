﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using WebApp.Models;
using Newtonsoft.Json;
using Telegram.Bot.Args;

//dotnet publish -c Release -r linux-x64 --self-contained true
namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        public async Task<StatusCodeResult> Post()
        {
            string body;
            using (var reader = new StreamReader(Request.Body))
                body = await reader.ReadToEndAsync();

            var update =  JsonConvert.DeserializeObject<Update>(body);

            if (update.Type != Telegram.Bot.Types.Enums.UpdateType.Message)
                return Ok();

            ProcessMessage(update.Message.From, update.Message);

            return Ok();
        }

        public static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            ProcessMessage(e.Message.From, e.Message);
        }

        private static async void ProcessMessage(User tlgrmUser, Message message)
        {
            DialogueFrame df;

            using (var ctx = new HealthBotContext())
            {
                var dbUser = ctx.users.Where(t => t.loginTelegram == tlgrmUser.Username).FirstOrDefault();

                if (dbUser == null) //если пользователя нет
                {
                    dbUser = new users()
                    {

                        loginTelegram = tlgrmUser.Username,
                        fio = tlgrmUser.FirstName + " " + tlgrmUser.LastName,
                        telegram_chat_id = message.Chat.Id
                    };
                    ctx.users.Add(dbUser);
                }

                //обработка сообщения (Dialogue state tracker)
                df = DialogueFrame.GetDialogueFrame(message, ctx, dbUser);

                //внутренняя работа в рамках платформы
                if (df.Activity == DialogueFrame.EnumActivity.DoNothing) return;
                switch (df.Activity)
                {
                    case DialogueFrame.EnumActivity.Answer:
                        await ctx.questions_answers.AddAsync(new questions_answers
                        {
                            id_user = dbUser.id,
                            id_question = dbUser.id_last_question.Value,
                            value = df.Entity,
                            date_time = DateTime.Now
                        });
                        break;
                    case DialogueFrame.EnumActivity.SystemAnswer:
                        break;
                    case DialogueFrame.EnumActivity.LoadFile:
                        var path = Path.GetFullPath(@"..\..\");
                        var name = message.Photo[message.Photo.Length - 1].FileId;
                        DownloadFile(name, path + name);
                        ctx.files.Add(new files
                        {
                            content_hash = name,
                            directory = "test",
                            id_user = dbUser.id,
                            file_name = name,
                            file_format = "jpg",
                            id_source = 1
                        });
                        break;
                    case DialogueFrame.EnumActivity.ReadMyBiomarkers:
                        dbUser.id_last_question = null;
                        dbUser.is_last_question_system = false;
                        break;
                    case DialogueFrame.EnumActivity.ConversationStart: break;
                    case DialogueFrame.EnumActivity.Unknown: break;
                }

                //обработка следующего сообщения (Dialogue state manager)
                await DialogueFrame.SendNextMessage(df, ctx, dbUser, message.Chat, Shared.telegramBot);
                await ctx.SaveChangesAsync();
            }
        }

        private static async void DownloadFile(string fileId, string path)
        {
            try
            {
                var file = await Shared.telegramBot.GetFileAsync(fileId);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Shared.telegramBot.DownloadFileAsync(file.FilePath, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading: " + ex.Message);
            }
        }
    }
}