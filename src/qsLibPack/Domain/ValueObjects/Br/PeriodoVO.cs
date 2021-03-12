using System;
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects.Br
{
    public class PeriodoVO
    {
        protected PeriodoVO() {}
        public PeriodoVO(DateTime dataInicial, DateTime dataFinal)
        {
            DataInicial = dataInicial;
            DataFinal = dataFinal;

            this.Validar();
        }

        public DateTime DataInicial { get; private set; }
        public DateTime DataFinal { get; private set; }

        private void Validar()
        {
            if (DataInicial > DataFinal)
            {
                throw new DomainException("Data inicial nao pode ser maior que a data final");
            }
        }
    }
}