using qsLibPack.Domain.Exceptions;
using qsLibPack.Extensions;

namespace qsLibPack.Domain.ValueObjects.Br
{
    public struct TelefoneVO
    {
        readonly string _value;
        private TelefoneVO(string value)
        {
            _value = string.IsNullOrEmpty(value) ? value : value.OnlyNumbers();
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(_value)) return;

            if (_value.Length < 10 || _value.Length > 11) throw new DomainException("Telefone inválido");
        }

        public override string ToString()
            => _value;

        public static implicit operator TelefoneVO(string value)
            => new TelefoneVO(value);
    }
}
