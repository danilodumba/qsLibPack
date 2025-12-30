# qsLibPack
![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet) ![NuGet](https://img.shields.io/nuget/v/qs.LibPack?label=qs.LibPack)

Conjunto de bibliotecas para padronizar desenvolvimento com DDD e CQRS: utilitários base, abstrações de Use Cases, repositórios e integrações com Entity Framework Core e MongoDB.

## Pacotes
- qs.LibPack — utilitários base (NuGet: https://www.nuget.org/packages/qs.LibPack, Documentação: [qsLibPack](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack/README.md))
- qs.LibPack.UseCases — abstrações de Use Cases e pipeline behaviors (NuGet: https://www.nuget.org/packages/qs.LibPack.UseCases, Documentação: [qsLibPack.UseCases](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.UseCases/README.md))
- qs.LibPack.EF — integração com Entity Framework Core (NuGet: https://www.nuget.org/packages/qs.LibPack.EF, Documentação: [qsLibPack.EF](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.EF/README.md))
- qs.LibPack.MongoDB — integração com MongoDB (NuGet: https://www.nuget.org/packages/qs.LibPack.MongoDB, Documentação: [qsLibPack.MongoDB](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.MongoDB/README.md))

## Instalação

```bash
dotnet add package qs.LibPack --version 8.0.0
dotnet add package qs.LibPack.UseCases --version 8.0.0
dotnet add package qs.LibPack.EF --version 8.0.0
dotnet add package qs.LibPack.MongoDB --version 8.0.0
```

## Exemplo Rápido

Registrar o serviço de validação do pacote base e utilizá-lo em qualquer camada.

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

Para exemplos mais completos, consulte:
- [qsLibPack](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack/README.md)
- [qsLibPack.UseCases](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.UseCases/README.md)
- [qsLibPack.EF](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.EF/README.md)
- [qsLibPack.MongoDB](file:///Volumes/danilo_ssd/Projetos/qsLibPack/src/qsLibPack.MongoDB/README.md)

## Desenvolvimento
- Requisitos: .NET 8 SDK
- Construir: `dotnet build`
- Testar: `dotnet test`

## Links
- Repositório: https://github.com/danilodumba/qsLibPack
- NuGet (todos os pacotes): https://www.nuget.org/packages?q=qsLibPack
