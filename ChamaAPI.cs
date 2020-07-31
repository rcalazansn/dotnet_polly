using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using Polly;
using System.Net.Http;
using Polly.Retry;
using System.Threading;

namespace RCN.Polly
{
    public class ChamaAPI
    {
        private async Task<HttpStatusCode> Get()
        {
            var client = new RestClient("http://demo7743575.mockable.io/polly");
            var request = new RestRequest(Method.GET);

            IRestResponse response = await client.ExecuteAsync(request);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> Retentativa_get1()
        {

            var response = await Policy
                .HandleResult<HttpStatusCode>(r => r != HttpStatusCode.OK)
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(5), (result, timeSpan, retryCount, context) =>
                {
                    System.Console.WriteLine($" Tentativa:{retryCount} - status:{result} espera:{timeSpan}");
                })
                .ExecuteAsync(() => Get());

            return response;
        }

        public async Task<HttpStatusCode> Get2()
        {
            var client = new RestClient("http://demo7743575.mockable.io/polly");
            var request = new RestRequest(Method.GET);

            var response = await Policy
                .HandleResult<IRestResponse>(r => r.StatusCode != HttpStatusCode.OK)
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(5), (result, timeSpan, retryCount, context) =>
                {
                    System.Console.WriteLine($" Tentativa:{retryCount} - status:{result.Result.StatusCode} espera:{timeSpan}");
                })
                .ExecuteAsync(() => client.ExecuteAsync(request));

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> Get3()
        {
            var httpClient = new HttpClient();
            var response = await Policy
                .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (result, timeSpan, retryCount, context) =>
                {
                    System.Console.WriteLine($" Tentativa:{retryCount} - status:{result.Result.StatusCode} espera:{timeSpan}");
                })
                .ExecuteAsync(() => httpClient.GetAsync("http://demo7743575.mockable.io/polly"));

            return response.StatusCode;
        }
    }
}