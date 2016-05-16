using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Services
{
    public class InstagramAPI
    {
        public string instClientId { get; set; }
        public string instSecret { get; set; }
        public string instRequestUri { get; set; }
        public string instAccessToken { get; set; }

        public string instCode { get; set; }
    }
}