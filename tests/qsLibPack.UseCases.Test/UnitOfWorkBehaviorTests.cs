using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.IoC;
using qsLibPack.UseCases.Models;
using Xunit;

namespace qsLibPack.UseCases.Test
{
    public sealed class SaveCommand : ICommand<Response>
    {
        public string Data { get; init; } = string.Empty;
    }

    public sealed class SaveHandler : IUseCaseHandler<SaveCommand, Response>
    {
        public Task<Response> Handle(SaveCommand request, CancellationToken cancellationToken)
            => Task.FromResult(Response.Ok());
    }

    public sealed class FakeUnitOfWork : IUnitOfWork
    {
        public int Commits { get; private set; }
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            Commits++;
            return Task.CompletedTask;
        }
    }

    public class UnitOfWorkBehaviorTests
    {
        [Fact]
        public async Task Should_Commit_After_Command_Handler()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddUseCases(typeof(SaveHandler).Assembly);
            var uow = new FakeUnitOfWork();
            services.AddSingleton<IUnitOfWork>(uow);
            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();
            var response = await dispatcher.Send(new SaveCommand { Data = "x" });

            Assert.True(response.Success);
            Assert.Equal(1, uow.Commits);
        }
    }
}