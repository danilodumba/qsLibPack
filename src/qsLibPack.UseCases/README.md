# qsLibPack.UseCases

![NuGet](https://img.shields.io/nuget/v/qs.LibPack.UseCases?label=NuGet) ![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)

Abstrações para implementação de Use Cases seguindo Clean Architecture, com Requests/Handlers, Dispatcher e Pipeline Behaviors (validação, exceção e unidade de trabalho).

## Instalação

```bash
dotnet add package qs.LibPack.UseCases --version 8.0.0
```

## Exemplos

### Básico

Query retornando DTO com `Response<T>`.

```csharp
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.IoC;
using qsLibPack.UseCases.Models;

public sealed class GetUserByIdQuery : IQuery<Response<UserDto>>
{
    public long Id { get; init; }
}

public sealed class GetUserByIdHandler : IUseCaseHandler<GetUserByIdQuery, Response<UserDto>>
{
    private readonly IUserReadRepository repo;
    public GetUserByIdHandler(IUserReadRepository repo) => this.repo = repo;

    public async Task<Response<UserDto>> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await repo.GetByIdAsync(request.Id, ct);
        return user is null
            ? Response<UserDto>.Fail(new[] { new qsLibPack.Validations.ErrorValidation("NotFound", "Usuário não encontrado") })
            : Response<UserDto>.Ok(new UserDto(user.Id, user.Name, user.Email));
    }
}

public record UserDto(long Id, string Name, string Email);

var services = new ServiceCollection();
services.AddUseCases(typeof(GetUserByIdHandler).Assembly);
var provider = services.BuildServiceProvider();
var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();
var result = await dispatcher.Send(new GetUserByIdQuery { Id = 1 });
```

### Intermediário

Comando com validação FluentValidation e commit automático via `UnitOfWorkBehavior`.

```csharp
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.IoC;
using qsLibPack.UseCases.Models;

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
    private readonly IUserRepository repo;
    public CreateUserHandler(IUserRepository repo) => this.repo = repo;

    public async Task<Response> Handle(CreateUserCommand request, CancellationToken ct)
    {
        await repo.AddAsync(new User { Name = request.Name, Email = request.Email }, ct);
        return Response.Ok();
    }
}

var services = new ServiceCollection();
services.AddLogging();
services.AddUseCases(typeof(CreateUserHandler).Assembly);
services.AddScoped<qsLibPack.Repositories.Interfaces.IUnitOfWork, MyUnitOfWork>();
var provider = services.BuildServiceProvider();
var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();
var response = await dispatcher.Send(new CreateUserCommand { Name = "Alice", Email = "alice@acme.com" });
```

### Avançado

Behavior customizado para auditoria e exceções centralizadas.

```csharp
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.IoC;

public sealed class AuditBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, Func<Task<TResponse>> next)
    {
        var sw = Stopwatch.StartNew();
        var result = await next();
        sw.Stop();
        Console.WriteLine($"{typeof(TRequest).Name} -> {sw.ElapsedMilliseconds}ms");
        return result;
    }
}

var services = new ServiceCollection();
services.AddLogging();
services.AddUseCases(typeof(SomeHandler).Assembly);
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditBehavior<,>));
```

## Informações do NuGet

- Versão atual: 8.0.0
- Dependências:
  - Microsoft.Extensions.DependencyInjection 8.*
  - Microsoft.Extensions.Logging.Abstractions 8.*
  - FluentValidation 12.1.0
- Compatibilidade: .NET 8 (net8.0)
- Link direto: https://www.nuget.org/packages/qs.LibPack.UseCases/
- Histórico (resumo):
  - 8.0.0: estabilização dos behaviors, `Response` e `Dispatcher` com net8.0

## Como Contribuir

- Inclua exemplos de Requests/Handlers e behaviors com testes
- Utilize versionamento semântico e mantenha APIs estáveis
- Valide integração com DI e logging

## Links

- Repositório: https://github.com/danilodumba/qsLibPack
- Documentação relacionada: [qsLibPack](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack/README.md)
