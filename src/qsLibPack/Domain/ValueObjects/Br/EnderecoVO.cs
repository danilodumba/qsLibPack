
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects.Br
{
    public class EnderecoVO
    {
        protected EnderecoVO() { }

        public EnderecoVO(string street, string number, string complement, string district, string zipCode, string city, string state)
        {
            Logradouro = street;
            Nummero = number;
            Complemento = complement;
            Bairro = district;
            Cidade = city;
            Estado = state;

            this.SetCep(zipCode);
        }

        #region [ Propriedades ]

        public string Logradouro { get; private set; }
        public string Nummero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cep { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }

        #endregion

        #region [ Private Methods ]
        private void SetCep(string value)
        {
            string newValue = "";
            foreach(var c in value)
            {
                if (char.IsNumber(c))
                {
                    newValue += c;
                }
            }

            this.Cep = newValue;
        }

        #endregion

        #region [ Public Methods ]
        public void ValidateAll()
        {
            this.ValidarLogradouro();
            this.ValidarCidade();
            this.ValidarBairro();
            this.ValidarNumero();
            this.ValidarEstado();
            this.ValidarCep();
        }

        public void ValidarLogradouro()
        {
            if (string.IsNullOrEmpty(this.Logradouro))
            {
                throw new DomainException("Infome um endereco");
            }
        }

        public void ValidarNumero()
        {
            if (string.IsNullOrEmpty(this.Nummero))
            {
                throw new DomainException("Infome um numero");
            }
        }

        public void ValidarCidade()
        {
            if (string.IsNullOrEmpty(this.Cidade))
            {
                throw new DomainException("Infome uma cidade");
            }
        }

        public void ValidarEstado()
        {
            if (string.IsNullOrEmpty(this.Estado))
            {
                throw new DomainException("Infome um estado");
            }
        }

        public void ValidarCep()
        {
            if (string.IsNullOrEmpty(this.Cep))
            {
                throw new DomainException("Infome um CEP");
            }
        }

        public void ValidarBairro()
        {
            if (string.IsNullOrEmpty(this.Bairro))
            {
                throw new DomainException("Infome um bairro");
            }
        }

        #endregion
    }
}
