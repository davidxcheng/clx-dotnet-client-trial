using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dotnet_core
{
    class Program
    {
        static void Main(string[] args)
        {
            var recipients = new List<string>();
            var parameters = new RunnerOnDemandParameters();
            var nearbyRunners = GetNearbyRunners();

            foreach(var runner in nearbyRunners) {
                recipients.Add(runner.MobilePhone);
                parameters.Name.Add(runner.MobilePhone, runner.Name);
                parameters.Url.Add(runner.MobilePhone, $"https://foo.se/x/{GenerateToken()}");
            }

            var sms = new Sms {
                To = recipients.ToArray(),
                From = "Me",
                Message = "Hi ${name}! Go here ${url}",
                Parameters = parameters,
            };

            Console.WriteLine(JsonConvert.SerializeObject(sms));
            Task.Run(async () => { await SmsClient.Send(JsonConvert.SerializeObject(sms)); }).Wait();
        }

        private static string GenerateToken() {
            var thirtytwoChars = "ABCDEFGH2JKLMN3PQRSTUVWXYZ456789";

            var randomBytes = new Byte[5];
            new Random().NextBytes(randomBytes);

            var sb = new StringBuilder();

            foreach(var b in randomBytes) {
                sb.Append(thirtytwoChars[b % 32]);
            }

            return sb.ToString();
        }

        private static List<Runner> GetNearbyRunners(){
            return new List<Runner> {
                new Runner { Name = "Foo", MobilePhone = "+12345678876" },
                new Runner { Name = "Boo", MobilePhone = "1234567890" },
            };
        }

        private class Runner {
            public string Name { get; set; }
            public string MobilePhone { get; set; }
        }
    }

    public class Sms {
        [JsonProperty("to")]
        public string[] To { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("body")]
        public string Message { get; set; }

        [JsonProperty("parameters", NullValueHandling=NullValueHandling.Ignore)]
        public dynamic Parameters { get; set; }
    }

    public class RunnerOnDemandParameters {
        [JsonProperty("name")]
        public Dictionary<string, string> Name { get; set; }

        [JsonProperty("url")]
        public Dictionary<string, string> Url { get; set; }

        public RunnerOnDemandParameters()
        {
            Name = new Dictionary<string, string>();
            Url = new Dictionary<string, string>();
        }
    }
}
