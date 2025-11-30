using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
namespace RiskEval.Infrastructure
{


    public class CustomWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(address);
            request.Timeout = 300000; // 60 seconds for response headers
            request.ReadWriteTimeout = 300000; // 5 minutes for data read/write
            return request;
        }
    }
}