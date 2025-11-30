using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RiskEval.Models
{
    public class RiskRequestViewModel
    {
        public string SelectedKey { get; set; }
        public IList<string> AvailableKeys { get; set; }

        // For display only
        public string RequestJson { get; set; }
        public string ResponseJson { get; set; }
        public string WildfireJson { get; set; }
        public string FloodJson { get; set; }

        // n8n webhook URL
        public string WebhookUrl { get; set; }
    }
}