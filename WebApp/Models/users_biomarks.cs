﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp
{
    [Table("users_biomarks")]
    class users_biomarks
    {
        public int id_user { get; set; }

        public int? id_biomark { get; set; }

        public int id { get; set; }

        public virtual biomarks biomarks { get; set; }

        public virtual users users { get; set; }
    }
}
