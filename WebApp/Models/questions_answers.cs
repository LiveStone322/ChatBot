﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    [Table("questions_answers")]
    class questions_answers
    {
        public int id { get; set; }

        public int id_user { get; set; }

        public int id_question { get; set; }

        [StringLength(8000)]
        public string value { get; set; }

        public DateTime date_time { get; set; }

        public virtual biomarks questions { get; set; }

        public virtual users users { get; set; }
    }
}
