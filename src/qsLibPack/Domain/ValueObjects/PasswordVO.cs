using System;
using System.Security.Cryptography;
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Domain.ValueObjects
{
    public class PasswordVO : ValueObject
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

        public override string ToString()
            => "***";

        public override void Validate()
        {
            if (password != confirmPassword)
                throw new DomainException("Senhas nao conferem");
        }

        public bool EqualsCrypt(string obj)
        {
            if (string.IsNullOrEmpty(this.password))
            {
                return false;
            }

            var parts = this.password.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = Convert.FromBase64String(parts[1]);

            var computedHash = DeriveHash(obj, salt);
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }

        [Obsolete("Use EqualsCrypt instead.")]
        public bool EqualsCript(string obj) => EqualsCrypt(obj);

        public void CryptPassword()
        {
            var salt = new byte[16];
            RandomNumberGenerator.Fill(salt);

            var hash = DeriveHash(this.password, salt);

            this.password = string.Concat(Convert.ToBase64String(salt), ":", Convert.ToBase64String(hash));
        }

        [Obsolete("Use CryptPassword instead.")]
        public void CriptPassword() => CryptPassword();

        private static byte[] DeriveHash(string value, byte[] salt)
        {
            using var rfc = new Rfc2898DeriveBytes(value, salt, 100_000, HashAlgorithmName.SHA256);
            return rfc.GetBytes(32);
        }
    }
}
