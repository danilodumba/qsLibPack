using System;
using System.Threading;
using System.Threading.Tasks;
using qsLibPack.Repositories.Interfaces;
using qsLibPack.UseCases.Abstractions;

namespace qsLibPack.UseCases.Behaviors
{
    /// <summary>
    /// Garante commit de unidade de trabalho para comandos após execução do handler.
    /// </summary>
    /// <typeparam name="TRequest">Tipo da requisição (comando).</typeparam>
    /// <typeparam name="TResponse">Tipo da resposta.</typeparam>
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICommand<TResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, Func<Task<TResponse>> next)
        {
            var response = await next();
            await unitOfWork.CommitAsync(cancellationToken);
            return response;
        }
    }
}