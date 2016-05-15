using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Services
{
    public class AuthMessageSenderOptions
    {
        public string SendGridUser { get; set; }
        public string SendGridKey { get; set; }
        public string SID { get; set; }
        public string AuthToken { get; set; }
        public string SendNumber { get; set; }
    }
}