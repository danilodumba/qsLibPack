# qsLibPack.UseCases

Biblioteca focada em abstrações para implementação de Use Cases seguindo princípios de Clean Architecture, com padrão semelhante ao MediatR (requests/handlers) e suporte a behaviors de pipeline (validação, exceção e unidade de trabalho).

## Instalação

- Referencie o projeto `qsLibPack.UseCases` no seu solution.
- Garanta que o projeto raiz chame o registro de DI.

## Estrutura

- `Abstractions`: Contratos principais (`IRequest<T>`, `IUseCaseHandler<,>`, `IPipelineBehavior<,>`, `IUseCaseDispatcher`, `IResponse`).
- `Behaviors`: Implementações de pipeline (`ValidationBehavior`, `ExceptionHandlingBehavior`, `UnitOfWorkBehavior`).
- `Exceptions`: Exceções customizadas (`UseCaseException`).
- `Models`: DTOs/VOs (`Response`, `Response<T>`).

## Uso

1. Crie uma requisição:

```csharp
public sealed class CreateUserCommand : qsLibPack.UseCases.Abstractions.ICommand<qsLibPack.UseCases.Models.Response>
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
```

2. Opcional: validador FluentValidation:

```csharp
using FluentValidation;

public sealed class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
```

3. Handler:

```csharp
using System.Threading;
using System.Threading.Tasks;
using qsLibPack.UseCases.Abstractions;
using qsLibPack.UseCases.Models;

public sealed class CreateUserHandler : IUseCaseHandler<CreateUserCommand, Response>
{
    public Task<Response> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => Task.FromResult(Response.Ok());
}
```

4. Registro de DI:

```csharp
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.UseCases.IoC;

var services = new ServiceCollection();
services.AddUseCases(typeof(CreateUserHandler).Assembly);
```

5. Execução:

```csharp
using System.Threading.Tasks;
using qsLibPack.UseCases;
using qsLibPack.UseCases.Abstractions;

var provider = services.BuildServiceProvider();
var dispatcher = provider.GetRequiredService<IUseCaseDispatcher>();
var response = await dispatcher.Send(new CreateUserCommand());
```

## Behaviors

- `ValidationBehavior`: executa validadores `IValidator<TRequest>`. Em falha, lança `UseCaseException` com erros.
- `ExceptionHandlingBehavior`: captura exceções não tratadas e converte para `UseCaseException` com log.
- `UnitOfWorkBehavior`: aplica `IUnitOfWork.CommitAsync` após handlers de `ICommand<TResponse>`.

## Versionamento Semântico

- O projeto segue versionamento semântico via campo `Version` no `.csproj`. Ajuste conforme mudanças.

## Testes

- Consulte o projeto `qsLibPack.UseCases.Test` para exemplos de testes de unidade e integração dos behaviors.