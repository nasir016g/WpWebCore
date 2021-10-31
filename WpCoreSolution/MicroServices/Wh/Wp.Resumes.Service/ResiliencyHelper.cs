using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Threading.Tasks;

namespace Wp.Wh.Services
{
    public class ResiliencyHelper
    {
        private ILogger _logger;

        public ResiliencyHelper(ILogger logger)
        {
            _logger = logger;
        }
                                                                                                                                                                                                                                                                                                                                
        public async Task<T> ExecuteResilient<T>(Func<Task<T>> action, T fallbackResult)
        {
            var retryPolicy = Policy
                .Handle<Exception>((ex) =>
                {
                    _logger.LogWarning($"Error occured during request-execution. Polly will retry. Exception: {ex.Message}");
                    return true;
                })
                .RetryAsync(5);

            var fallbackPolicy = Policy<T>
                .Handle<Exception>()
                .FallbackAsync(
                    fallbackResult,
                    (e, c) => Task.Run(() => _logger.LogError($"Error occured during request-execution. Polly will fallback. Exception: {e.Exception.ToString()}")));

            return await fallbackPolicy
                .WrapAsync(retryPolicy)
                .ExecuteAsync(action)
                .ConfigureAwait(false);
        }
    }
}
