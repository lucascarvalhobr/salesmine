using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System.Net.Http;

namespace SalesMine.Core.Extensions
{
    public class PollyExtensions
    {
        //public static AsyncRetryPolicy<HttpResponseMessage> WaitAttempt()
        //{
        //    var retry = HttpPolicyExtensions
        //        .HandleTransientHttpError()
        //        .WaitAndRetryAsync(new[] {
        //        });
        //}
    }
}
