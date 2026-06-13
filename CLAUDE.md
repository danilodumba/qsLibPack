# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`qsLibPack` is a set of NuGet libraries (target `net8.0`) that standardize DDD + CQRS development across .NET projects. Four packages ship independently but layer on top of the base package:

- **qs.LibPack** (`src/qsLibPack`) — base building blocks: DDD primitives (`Entity`, `AggregateRoot`, `ValueObject`), Brazilian value objects (`CpfCnpfVO`, `EnderecoVO`, `TelefoneVO`, `EmailVO`, `NameVO`, `PasswordVO`), validation service, repository/UoW interfaces, ASP.NET middleware and base controller. No dependency on the other three.
- **qs.LibPack.UseCases** (`src/qsLibPack.UseCases`) — a hand-rolled MediatR-style dispatcher with a pipeline-behavior chain. References `qsLibPack`.
- **qs.LibPack.EF** (`src/qsLibPack.EF`) — EF Core implementation of the repository abstractions. References `qsLibPack`.
- **qs.LibPack.MongoDB** (`src/qsLibPack.MongoDB`) — MongoDB implementation with a deferred-command Unit of Work. References `qsLibPack`.

Package versions are pinned per-csproj (`<Version>8.0.0</Version>`); there is **no** central package management or `Directory.Build.props`. Each csproj packs its own `README.md`.

## Commands

```bash
dotnet build                                    # build all projects
dotnet test                                     # run both test projects
dotnet test tests/qsLibPack.Test                # run one test project
dotnet test tests/qsLibPack.UseCases.Test
dotnet test --filter "FullyQualifiedName~EmailVoTest"   # run a single test class/method
dotnet pack src/qsLibPack -c Release            # produce a .nupkg for one package
```

Tests use **xUnit + coverlet** (no FluentAssertions/Moq here despite the global profile — match the existing `Assert.*` style and hand-rolled fakes). `tests/qsLibPack.Test` covers value objects and validators against the base package; `tests/qsLibPack.UseCases.Test` covers the dispatcher and each behavior.

## Architecture

### Use Cases pipeline (qs.LibPack.UseCases)
This is the most non-obvious part. `UseCaseDispatcher.Send` resolves the handler **by reflection** (`IUseCaseHandler<TRequest,TResponse>`), then wraps it in `IPipelineBehavior<,>` instances resolved from DI. Behaviors are reversed and folded into a `Func<Task<TResponse>>` chain — registration order in `AddUseCases` is the execution order:

1. `ExceptionHandlingBehavior` — catches everything, re-throws `UseCaseException` as-is, wraps anything else (logs via `ILogger`).
2. `ValidationBehavior` — runs all registered FluentValidation `IValidator<TRequest>`; on failure throws `UseCaseException` (it does **not** return a failed `Response`).
3. `UnitOfWorkBehavior` — runs `next()` then `IUnitOfWork.CommitAsync`. Constrained to `where TRequest : ICommand<TResponse>`, so it only participates for commands, never queries.

Requests implement marker interfaces in `Abstractions/IRequest.cs`: `IRequest<T>` → `ICommand<T>` (state-changing) / `IQuery<T>` (read-only). `AddUseCases(params Assembly[])` auto-registers all handlers and validators by assembly scan, all `Scoped`.

`Response` / `Response<T>` (in `Models/`) are sealed factory-based result types (`.Ok()` / `.Fail(errors)`) carrying `ErrorValidation` from the base package.

### Domain primitives (qs.LibPack)
- `Entity<TId>` and `AggregateRoot<TId>` expose an abstract `Validate()`. Both EF and Mongo repositories call `entity.Validate()` inside `Create`/`Update` — domain validation is enforced at persistence time, not just construction.
- Value objects come in two flavors: the abstract `ValueObject` base class (with `Validate()`) and standalone `struct` VOs (e.g. `EmailVO`, `CpfCnpfVO`) that validate in their constructor and `throw DomainException` on bad input, with implicit `string` conversions.

### Validation flow (qs.LibPack)
Two distinct mechanisms — do not conflate them:
- `IValidationService` (`ValidationService`) is a **scoped error accumulator**. Code calls `AddErrors(...)`; `ValidationMiddleware` inspects it after the request and, if `!IsValid()`, short-circuits the response to `400` with the serialized errors (uses Newtonsoft + camelCase). `ApiLibController.Result(...)` does the same check inside controllers. Register with `services.AddValidationService()` and `app.UseValidationService()`.
- `FieldValidator<T>` / FluentValidation drives the `ValidationBehavior` in the UseCases pipeline. Different path, different failure mode (exception, not accumulator).

### Persistence
- **EF** (`Repositories.EF.Repository<T,TId>`): thin `DbSet` wrapper; commit is the caller's `IUnitOfWork` (typically via `UnitOfWorkBehavior`).
- **Mongo** (`Repositories.Mongo.Repository<TEntity,TId>`): writes are **deferred** — `Create`/`Update`/`Remove` enqueue commands via `IMongoContext.AddCommand`; nothing hits Mongo until `UnitOfWork.CommitAsync` drains the queue (optionally in a transaction). Register with `RepositoryMongoDBExtensions` + `MongoSettings` bound from config.

## Conventions specific to this repo

- These packages target consumer frameworks down to older .NET, so the source uses **explicit `using System.*` imports and verbose C#** (no implicit usings, no file-scoped namespaces in most files, no `record` VOs). Nullable is enabled **only** in `qsLibPack.UseCases`. Match the style of the file you are editing rather than the global "latest C#" preference.
- Code identifiers are English; XML doc comments and messages are **Portuguese (BR)** — keep that split.
- Bumping a package version means editing `<Version>` in that one csproj (and matching the install snippets in the relevant `README.md`). There is no shared version source.
