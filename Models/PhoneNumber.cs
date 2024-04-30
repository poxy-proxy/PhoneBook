﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }
        public int AbonentId { get; set; }
        public Abonent Abonent { get; set; }
        public string Number { get; set; }
        public string Type { get; set; } // Home, Work, Mobile
    }
}
