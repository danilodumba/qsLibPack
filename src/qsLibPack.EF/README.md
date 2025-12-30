# qsLibPack.EF

![NuGet](https://img.shields.io/nuget/v/qs.LibPack.EF?label=NuGet) ![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet) ![EFCore](https://img.shields.io/badge/EF%20Core-8.*-blue)

Integração com Entity Framework Core para `qsLibPack`: repositórios de consulta via LINQ/SQL e utilitários.

## Instalação

```bash
dotnet add package qs.LibPack.EF --version 8.0.0
```

## Exemplos

### Básico

Consultar entidades usando LINQ com `QueryRepository`.

```csharp
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Repositories.EF;

public sealed class MyQueryRepository : QueryRepository
{
    public MyQueryRepository(DbContext ctx) : base(ctx) { }

    public IEnumerable<User> GetActiveUsers()
        => SelectLinq<User>(x => x.IsActive);
}
```

### Intermediário

Executar SQL e mapear resultados para DTO.

```csharp
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using qsLibPack.Repositories.EF;

public sealed class ReportRepository : QueryRepository
{
    public ReportRepository(DbContext ctx) : base(ctx) { }

    public IList<UserReportDto> GetUsersReport()
        => SelectSql<UserReportDto>("SELECT Id, Name FROM Users");
}

public sealed class UserReportDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### Avançado

Executar agregações com `ExecuteScalar`.

```csharp
using Microsoft.EntityFrameworkCore;
using qsLibPack.Repositories.EF;

public sealed class MetricsRepository : QueryRepository
{
    public MetricsRepository(DbContext ctx) : base(ctx) { }

    public int CountActive()
        => (int)(ExecuteScalar("SELECT COUNT(1) FROM Users WHERE IsActive = 1") ?? 0);
}
```

## Informações do NuGet

- Versão atual: 8.0.0
- Dependências:
  - Microsoft.EntityFrameworkCore 8.*
  - Microsoft.EntityFrameworkCore.Relational 8.*
  - Microsoft.EntityFrameworkCore.Proxies 8.*
- Compatibilidade: .NET 8 (net8.0)
- Link direto: https://www.nuget.org/packages/qs.LibPack.EF/
- Histórico (resumo):
  - 8.0.0: atualização para EF Core 8, melhorias em consultas e mapeamento

## Como Contribuir

- Forneça exemplos cobrindo LINQ e SQL
- Garanta compatibilidade com provedores comuns (SqlServer, PostgreSQL, etc.)
- Execute testes com seu DbContext antes de abrir PR

## Links

- Repositório: https://github.com/danilodumba/qsLibPack
- Documentação relacionada: [qsLibPack](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack/README.md)
