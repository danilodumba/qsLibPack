# qsLibPack

![NuGet](https://img.shields.io/nuget/v/qs.LibPack?label=NuGet) ![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)

Biblioteca base com utilitários para DDD, CQRS, Repositórios, Validações e Extensões.

## Instalação

```bash
dotnet add package qs.LibPack --version 8.0.0
```

## Exemplos

### Básico

Registrar o serviço de validação e utilizá-lo em qualquer camada.

```csharp
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Validations.IoC;
using qsLibPack.Validations.Interface;

var services = new ServiceCollection();
services.AddValidationService();
var provider = services.BuildServiceProvider();

var validation = provider.GetRequiredService<IValidationService>();
validation.AddErrors("Email", "Email inválido");
var ok = validation.IsValid(); // false
```

### Intermediário

Criar uma unidade de trabalho e orquestrar validação dentro de um serviço de aplicação.

```csharp
using System.Threading;
using System.Threading.Tasks;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Validations.Interface;
using qsLibPack.Application;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly Microsoft.EntityFrameworkCore.DbContext db;
    public EfUnitOfWork(Microsoft.EntityFrameworkCore.DbContext db) { this.db = db; }
    public Task CommitAsync(CancellationToken ct = default) => db.SaveChangesAsync(ct);
}

public sealed class UserService : ApplicationService
{
    public UserService(IValidationService v, IUnitOfWork uow) : base(v, uow) { }

    public async Task CreateAsync(User entity, CancellationToken ct = default)
    {
        entity.Validate();
        if (!_validationService.IsValid()) return;
        await _uow.CommitAsync(ct);
    }
}
```

### Avançado

Integrar validação ao pipeline HTTP com `ApiLibController` e middleware.

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Controllers;
using qsLibPack.Validations.IoC;
using qsLibPack.Validations.Interface;
using qsLibPack.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddValidationService();
var app = builder.Build();
app.UseValidationService();
app.MapControllers();
app.Run();

public sealed class UsersController : ApiLibController
{
    private readonly IValidationService validation;
    public UsersController(IValidationService v) : base(v) { validation = v; }

    [HttpPost("/users")]
    public IActionResult Post(UserDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email))
            validation.AddErrors("Email", "Obrigatório");

        return Result(Ok());
    }
}
```

## Informações do NuGet

- Versão atual: 8.0.0
- Dependências:
  - FluentValidation 12.1.0
  - Microsoft.Extensions.DependencyInjection 8.*
  - Microsoft.Extensions.Options.ConfigurationExtensions 8.*
  - Microsoft.AspNetCore.Http.Features 5.0.17
  - Microsoft.AspNetCore.Mvc.Core 2.3.0
- Compatibilidade: .NET 8 (net8.0)
- Link direto: https://www.nuget.org/packages/qs.LibPack/
- Histórico (resumo):
  - 8.0.0: migração para .NET 8, melhorias nas validações e organização dos pacotes

## Como Contribuir

- Abra issues e PRs no repositório principal
- Siga o versionamento semântico (MAJOR.MINOR.PATCH)
- Execute testes localmente antes de enviar alterações

## Links

- Repositório: https://github.com/danilodumba/qsLibPack
- Pacotes relacionados:
  - [qsLibPack.MongoDB](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.MongoDB/README.md)
  - [qsLibPack.EF](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.EF/README.md)
  - [qsLibPack.UseCases](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.UseCases/README.md)
