using System.Text;
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects
{
    public class PasswordVO: ValueObject
    {
        string password;
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
        public bool EqualsCript(string obj)
        {
            obj = this.CriptPassword(obj);
            return this.password.Equals(obj);
        }

        public void CriptPassword()
        {
            this.password = this.CriptPassword(this.password);
        }

        private string CriptPassword(string password)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            
            return sb.ToString();
        }
    }
}