using qsLibPack.Domain.Exceptions;
using qsLibPack.Extensions;

namespace qsLibPack.Domain.ValueObjects.Br
{
    public struct TelefoneVO
    {
        string _value;
        private TelefoneVO(string value)
        {
            _value = value;
            Validar();
        }

        private void Validar()
        {
            if (string.IsNullOrEmpty(_value)) return;

            string telefone = _value.OnlyNumbers();
            
            if (telefone.Length < 10 || telefone.Length > 11) throw new DomainException("Telefone inválido");

            _value = telefone;
        }

        public override string ToString() 
            => _value;

        public static implicit operator TelefoneVO(string value)
            => new TelefoneVO(value);
    }
}
