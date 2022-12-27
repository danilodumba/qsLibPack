
using qsLibPack.Validations;

namespace qsLibPack.Domain.ValueObjects
{
    public struct NameVO
    {
        readonly string _nome;
        private NameVO(string nome)
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