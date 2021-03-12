
using qsLibPack.Validations;

namespace qs.Domain.Core.ValueObjects
{
    public struct NameVO
    {
        readonly string _nome;
        public NameVO(string nome)
        {
            _nome = nome;
        }

        public void Validate()
        {
            _nome.NotNullOrEmpty("O campo nome nao pode ser vazio.");
        }

         public override string ToString()
          => _nome;

        public static implicit operator NameVO(string value)
            => new NameVO(value);
    }
}