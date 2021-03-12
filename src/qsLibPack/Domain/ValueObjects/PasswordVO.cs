using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects
{
    public class PasswordVO: ValueObject
    {
        readonly string password;
        readonly string confirmPassword;

        protected PasswordVO() {}
        public PasswordVO(string password, string confirmPassword)
        {
            this.password = password;
            this.confirmPassword = confirmPassword;

            this.Validate();
        }

        public string Value 
        { 
            get => password;
        }

         public override string ToString() 
         => this.Value;

        public override void Validate()
        {
            if (password != confirmPassword) 
                throw new DomainException("Senhas nao conferem");
        }
    }
}