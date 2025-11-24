using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Models;
using qsLibPack.UseCases.IoC;
using Xunit;

namespace qsLibPack.UseCases.Test
{
    public sealed class PingQuery : IQuery<Response<string>>
    {
        public string Message { get; init; } = "hello";
    }

    public sealed class PingHandler : IUseCaseHandler<PingQuery, Response<string>>
    {
        public Task<Response<string>> Handle(PingQuery request, CancellationToken cancellationToken)
            => Task.FromResult(Response<string>.Ok($"pong: {request.Message}"));
    }

    public class DispatcherTests
    {
        [Fact]
        public async Task Send_Should_Invoke_Handler_And_Return_Response()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddUseCases(typeof(PingHandler).Assembly);
            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();
            var result = await dispatcher.Send(new PingQuery { Message = "world" });

            Assert.True(result.Success);
            Assert.Equal("pong: world", result.Data);
        }
    }
}