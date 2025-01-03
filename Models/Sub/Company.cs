﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string InnKpp { get; set; }
        public string Address { get; set; }
        public string Phones { get;set; }
        public string Emails { get; set; }
    }
}
