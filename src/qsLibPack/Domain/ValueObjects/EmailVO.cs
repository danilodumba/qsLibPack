using System.Text.RegularExpressions;
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects
{
    public struct EmailVO
    {
        readonly string _value;
        public EmailVO(string value)
        {
            _value = value;
            this.Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return;

            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            if (!rg.IsMatch(_value))
            {
                throw new DomainException($"Email {_value} invalido!");
            }
        }

        public override string ToString() 
        => _value;

        public static implicit operator EmailVO(string value)
            => new EmailVO(value);
    }
}
