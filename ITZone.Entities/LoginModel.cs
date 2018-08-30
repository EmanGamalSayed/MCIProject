using System;
using System.Collections.Generic;
using System.Text;

namespace ITZone.Entities
{
    public class LoginModel
    {
        //public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AD_User
    {
        public int ID { get; set; }
        public object Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public object DisplayName { get; set; }
        public object EmailAddress { get; set; }
        public object SamAccountName { get; set; }
    }
}
