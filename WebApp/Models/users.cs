﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    [Table("users")]
    class users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public users()
        {
            files = new HashSet<files>();
            notifications = new HashSet<notifications>();
            questions_answers = new HashSet<questions_answers>();
            users_biomarks = new HashSet<users_biomarks>();
        }

        public int id { get; set; }

        [StringLength(8000)]
        public string loginTelegram { get; set; }

        [StringLength(8000)]
        public string fio { get; set; }

        [StringLength(12)]
        public string phone_number { get; set; }

        public int? id_last_question { get; set; }

        public bool? is_last_question_system { get; set; }

        [StringLength(8000)]
        public string loginViber { get; set; }

        public long? loginIcq { get; set; }

        public long? telegram_chat_id { get; set; }

        public long? icq_chat_id { get; set; }


        [StringLength(8000)]
        public string token { get; set; }

        [StringLength(8000)]
        public string context { get; set; }

        [StringLength(8000)]
        public string chatting { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<files> files { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<notifications> notifications { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<questions_answers> questions_answers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<users_biomarks> users_biomarks { get; set; }
    }
}
