using Amazon.Lambda.Core;
using System.Text;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EsepWebhook;

public class Function
{
    public string FunctionHandler(string input, ILambdaContext context)
    {
        dynamic json = JsonConvert.DeserializeObject<dynamic>(input.ToString());
        string payload = $"{{'text':'Issue Created: {json.issue.html_url}'}}";
        
        var client = new HttpClient();

        var webRequest = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("SLACK_URL"))
        {
            Content = new StringContent(payload, Encoding.UTF8, "application/json")
        };

        var response = client.Send(webRequest);
        using var reader = new StreamReader(response.Content.ReadAsStream());
            
        return reader.ReadToEnd();
    }
}
