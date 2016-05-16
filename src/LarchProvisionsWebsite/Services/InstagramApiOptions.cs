using LarchProvisionsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Services
{
    public class InstagramApiOptions : InstagramObject
    {
        public string instClientId { get; set; }
        public string instSecret { get; set; }
        public string instRedirectUri { get; set; }
        public string instAccessToken { get; set; }
        public string Code { get; set; }
    }
}