using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Exceptions;
using qsLibPack.UseCases.IoC;
using qsLibPack.UseCases.Models;
using Xunit;

namespace qsLibPack.UseCases.Test
{
    public sealed class CreateUserCommand : ICommand<Response>
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }

    public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithErrorCode("Name");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithErrorCode("Email");
        }
    }

    public sealed class CreateUserHandler : IUseCaseHandler<CreateUserCommand, Response>
    {
        public Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            => Task.FromResult(Response.Ok());
    }

    public sealed class DummyUnitOfWork : qsLibPack.Repositories.Interfaces.IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    public class ValidationBehaviorTests
    {
        [Fact]
        public async Task Should_Throw_UseCaseException_On_Validation_Failure()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddUseCases(typeof(CreateUserHandler).Assembly);
            services.AddSingleton<qsLibPack.Repositories.Interfaces.IUnitOfWork, DummyUnitOfWork>();
            var provider = services.BuildServiceProvider();

            var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();

            await Assert.ThrowsAsync<UseCaseException>(async () =>
            {
                await dispatcher.Send(new CreateUserCommand { Name = "", Email = "invalid" });
            });
        }
    }
}