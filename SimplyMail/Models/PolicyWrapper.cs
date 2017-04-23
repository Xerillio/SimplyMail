using Polly;
using SimplyMail.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyMail.Models
{
    static class PolicyWrapper
    {
        public static async Task<TResult> WrapRetryOnNotConnected<TResult>(
            Func<Task<TResult>> taskProvider, Func<Task> onRetry)
        {
            return await WrapRetryOnNotConnected(
                taskProvider,
                async (ex, ts) => await onRetry().ConfigureAwait(false));
        }

        public static async Task<TResult> WrapRetryOnNotConnected<TResult>(
            Func<Task<TResult>> taskProvider, Action<Exception, TimeSpan> onRetry)
        {
            return await Policy
                .Handle<MailKit.ServiceNotConnectedException>()
                .WaitAndRetryAsync
                (
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                    onRetry: onRetry
                )
                .ExecuteAsync(taskProvider);
        }
    }
}
