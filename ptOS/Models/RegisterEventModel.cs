using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace ptOS.Models
{
    public class RegisterEventModel
    {
        public string ServerGuid { get; set; }
        public string ServerKey { get; set; }
        public string PlayerGuid { get; set; }
        public string PlayerName { get; set; }
        public string PlayerIp { get; set; }
        public string EventType { get; set; }
        public Dictionary<string, string> EventData { get; set; }

        public RegisterEventModel()
        {
             EventData = new Dictionary<string, string>();
        }
    }
}