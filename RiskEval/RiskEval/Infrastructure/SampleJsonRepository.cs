using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace RiskEval.Infrastructure
{
    public class SampleJsonRepository
    {
        // Keys the UI will use
        public const string WildfireKey = "wildfire";
        public const string FloodKey = "flood";

        // Stores the raw JSON strings
        private static readonly Dictionary<string, string> _jsonPayloads =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public static void Initialize(HttpServerUtility server)
        {
            // Map file paths
            string wildfirePath = server.MapPath("~/App_Data/sample-wildfire-business.json");
            string floodPath = server.MapPath("~/App_Data/sample-flood-business.json");

            _jsonPayloads[WildfireKey] = File.ReadAllText(wildfirePath);
            _jsonPayloads[FloodKey] = File.ReadAllText(floodPath);
        }

        public static IEnumerable<string> GetKeys()
        {
            return _jsonPayloads.Keys;
        }

        public static string GetJson(string key)
        {
            string value;
            if (_jsonPayloads.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }

    }
}