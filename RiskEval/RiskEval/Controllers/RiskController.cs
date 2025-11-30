using RiskEval.Infrastructure;
using RiskEval.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace SendWebhook.Controllers
{
    public class RiskController : Controller
    {
        // GET: RiskController
        // GET: /Risk/
        [HttpGet]
        public ActionResult Index()
        {
            var model = new RiskRequestViewModel
            {
                AvailableKeys = new List<string>(SampleJsonRepository.GetKeys()),
                SelectedKey = SampleJsonRepository.WildfireKey, // default
                RequestJson = SampleJsonRepository.GetJson(SampleJsonRepository.WildfireKey),
                WildfireJson = SampleJsonRepository.GetJson(SampleJsonRepository.WildfireKey),
                FloodJson = SampleJsonRepository.GetJson(SampleJsonRepository.FloodKey),
                ResponseJson = string.Empty,
                WebhookUrl = GetWebhookUrl()
            };

            return View(model);
        }

        // POST: /Risk/Send
        [HttpPost]
        public ActionResult Send(string selectedKey)
        {
            var model = new RiskRequestViewModel
            {
                AvailableKeys = new List<string>(SampleJsonRepository.GetKeys()),
                SelectedKey = selectedKey,
                WebhookUrl = GetWebhookUrl()
            };

            // Get the JSON payload the user selected
            string json = SampleJsonRepository.GetJson(selectedKey);
            model.RequestJson = json ?? string.Empty;

            model.WildfireJson = SampleJsonRepository.GetJson(SampleJsonRepository.WildfireKey);
            model.FloodJson = SampleJsonRepository.GetJson(SampleJsonRepository.FloodKey);

            if (string.IsNullOrEmpty(json))
            {
                model.ResponseJson = "No JSON found for selected key.";
                return View("Index", model);
            }

            try
            {
                var responseJson = PostJsonToWebhook(model.WebhookUrl, json);
                model.ResponseJson = responseJson;
            }
            catch (Exception ex)
            {
                model.ResponseJson = "Error calling n8n webhook: " + ex.Message;
            }

            return View("Index", model);
        }

        private string GetWebhookUrl()
        {
            // You can configure this in Web.config <appSettings>
            // <add key="N8N_WebhookUrl" value="https://your-n8n-host/webhook/risk-analysis" />
            string url = WebConfigurationManager.AppSettings["N8N_WebhookUrl"];
           // return string.IsNullOrEmpty(url) ? "https://your-n8n-host/webhook/risk-analysis" : url;
            return url;
        }

        private string PostJsonToWebhook(string url, string jsonBody)
        {
            string credentials = WebConfigurationManager.AppSettings["N8N_Webhook_Username"] + ":" + WebConfigurationManager.AppSettings["N8N_Webhook_Password"];
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(credentials);
            string base64Credentials = Convert.ToBase64String(plainTextBytes);
            using (var client = new CustomWebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + base64Credentials;
                 
                // Synchronous call in .NET 3.5 style
                string response = client.UploadString(url, "POST", jsonBody);
                return response;
            }
        }

    }
}