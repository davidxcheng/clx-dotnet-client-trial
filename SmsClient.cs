using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public static class SmsClient
{
    public async static Task Send(string json)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.clxcommunications.com"),
        };

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "secret");

        var jsonPayload = new StringContent(json, Encoding.UTF8, "application/json");

        var res = await httpClient.PostAsync("/xms/v1/hmm/batches", jsonPayload);
        
        var x = await res.Content.ReadAsStringAsync();
    }
}