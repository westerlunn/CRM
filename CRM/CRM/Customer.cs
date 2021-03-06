﻿using System.Collections.Generic;

namespace CRM
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<string> PhoneNumbers { get; set; }
    }
}