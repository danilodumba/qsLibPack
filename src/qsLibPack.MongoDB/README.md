# qsLibPack.MongoDB

![NuGet](https://img.shields.io/nuget/v/qs.LibPack.MongoDB?label=NuGet) ![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet) ![MongoDB.Driver](https://img.shields.io/badge/MongoDB.Driver-3.5.1-47A248)

Integração com MongoDB para `qsLibPack`: contexto, repositório base e unidade de trabalho com DI.

## Instalação

```bash
dotnet add package qs.LibPack.MongoDB --version 8.1.0
```

## Exemplos

### Básico

Registrar serviços com DI e configurar conexão.

```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using qsLibPack.Repositories.Mongo.Extensions;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection();
services.AddQsLibPackMongoDB(configuration);
var provider = services.BuildServiceProvider();
```

`appsettings.json`:

```json
{
  "MongoConnection": {
    "ConnectionString": "mongodb://localhost:27017",
    "Database": "mydb"
  }
}
```

### Intermediário

Criar repositório concreto e realizar operações com commit.

```csharp
using System;
using System.Threading.Tasks;
using qsLibPack.Domain.Entities;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.Repositories.Mongo;
using qsLibPack.Repositories.Mongo.Core;

public sealed class User : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public override void Validate()
    {
        if (string.IsNullOrEmpty(Name)) throw new qsLibPack.Domain.Exceptions.DomainException("Nome obrigatório");
    }
}

public sealed class UserRepository : Repository<User, Guid>
{
    public UserRepository(IMongoContext ctx) : base(ctx) { }
}

// Uso
var repo = provider.GetRequiredService<UserRepository>();
var uow = provider.GetRequiredService<IUnitOfWork>();

var user = new User { Id = Guid.NewGuid(), Name = "Alice" };
await repo.CreateAsync(user);
await uow.CommitAsync();
```

### Avançado

Executar múltiplas operações e consultar por id.

```csharp
// Atualizar
user.Name = "Alice Silva";
await repo.UpdateAsync(user);

// Remover
await repo.RemoveAsync(user);

// Commit das operações pendentes
await uow.CommitAsync();

// Consulta por id
var loaded = await repo.GetByIDAsync(user.Id);
```

## Informações do NuGet

- Versão atual: 8.1.0
- Dependências:
  - MongoDB.Driver 3.5.1
  - Microsoft.Extensions.DependencyInjection 8.*
  - Microsoft.Extensions.Options.ConfigurationExtensions 8.*
- Compatibilidade: .NET 8 (net8.0)
- Link direto: https://www.nuget.org/packages/qs.LibPack.MongoDB/
- Histórico (resumo):
  - 8.0.0: suporte a net8.0, ajustes de DI e empacotamento do README
  - 8.1.0: `MongoContext` registra `IMongoClient` como singleton (preserva connection pool) e executa os comandos do Unit of Work **sequencialmente**, limpando a fila mesmo em caso de erro; `GetByID`/`GetByIDAsync` retornam tipo anulável; Nullable habilitado

## Como Contribuir

- Abra issues/PRs no repositório principal
- Mantenha exemplos compatíveis com a API atual
- Valide localmente com uma instância MongoDB antes de enviar PR

## Links

- Repositório: https://github.com/danilodumba/qsLibPack
- Documentação relacionada: [qsLibPack](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack/README.md)
