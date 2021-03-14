using System;
using FluentValidation;

namespace qsLibPack.Validations
{
    public class FieldValidator<T> : AbstractValidator<T> 
    {
        /// <summary>
        /// Validar se e menor que zero
        /// </summary>
        /// <param name="valor">Valor a validar</param>
        /// <param name="message">Mensagem de validacao</param>
        public void NotNull(int valor, string message)
        {
            this.RuleFor(x => valor)
                .GreaterThan(0).WithMessage(message);
        }

        /// <summary>
        /// Validar se e menor que zero
        /// </summary>
        /// <param name="valor">Valor a validar</param>
        /// <param name="message">Mensagem de validacao</param>
        public void NotNull(decimal valor, string message)
        {
            this.RuleFor(x => valor)
                .GreaterThan(0).WithMessage(message);
        }

        /// <summary>
        /// Validar se e menor que zero
        /// </summary>
        /// <param name="valor">Valor a validar</param>
        /// <param name="message">Mensagem de validacao</param>
        public void NotNull(long valor, string message)
        {
            this.RuleFor(x => valor)
                .GreaterThan(0).WithMessage(message);
        }

        public void NotNullOrEmpty(string valor, string message)
        {
            this.RuleFor(x => valor)
                .NotEmpty().WithMessage(message);
        }

        public void NotNull(object valor, string message)
        {
            this.RuleFor(x => valor)
                .NotNull();
        }

        public void NotNull(Guid valor, string message)
        {
            this.RuleFor(x => valor)
                .NotEqual(Guid.Empty).WithMessage(message)
                .NotNull(message);
        }
    }
}