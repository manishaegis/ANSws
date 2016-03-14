using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ANSws.Models
{
    public class WSResponse
    {
        public WSResponse()
        {
            RESPONSE = false;
            MESSAGE = string.Empty;
            RESULT = string.Empty;
        }
        public bool RESPONSE { get; set; }
        public string MESSAGE { get; set; }
        public string RESULT { get; set; }
    }
}