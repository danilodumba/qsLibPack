
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects.Br
{
    public class EnderecoVO
    {
        public EnderecoVO() {}
        public EnderecoVO(string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado)
        {
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;

            this.SetCep(cep);
        }


        #region [ Propriedades ]

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

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
        public void ValidarTodos()
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
            if (string.IsNullOrEmpty(this.Numero))
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
