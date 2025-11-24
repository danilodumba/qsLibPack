using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Exceptions;
using qsLibPack.UseCases.IoC;
using qsLibPack.UseCases.Models;
using Xunit;

namespace qsLibPack.UseCases.Test
{
    public sealed class FailingQuery : IQuery<Response>
    {
    }

    public sealed class FailingHandler : IUseCaseHandler<FailingQuery, Response>
    {
        public Task<Response> Handle(FailingQuery request, CancellationToken cancellationToken)
            => throw new InvalidOperationException("boom");
    }

    public class ExceptionHandlingBehaviorTests
    {
        [Fact]
        public async Task Should_Wrap_Unexpected_Exception_In_UseCaseException()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddUseCases(typeof(FailingHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();

            var ex = await Assert.ThrowsAsync<UseCaseException>(async () =>
            {
                await dispatcher.Send(new FailingQuery());
            });

            Assert.Contains("Unexpected", System.Linq.Enumerable.Select(ex.Errors, e => e.Code));
        }
    }
}