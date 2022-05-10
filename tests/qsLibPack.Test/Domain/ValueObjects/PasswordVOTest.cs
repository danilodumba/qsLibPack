using qsLibPack.Domain.Exceptions;
using qsLibPack.Domain.ValueObjects;
using Xunit;

namespace qsLibPack.Test.Domain.ValueObjects
{
    public class PasswordVOTest
    {
        [Theory]
        [InlineData("123456", "1234567")]
        [InlineData("abcd123", "abcd@123")]
        public void Deve_Retornar_Password_Invalido(string password, string confirmPassword)
        {
           Assert.Throws<DomainException>(() => new PasswordVO(password, confirmPassword));
        }

        [Theory]
        [InlineData("123456", "123456")]
        [InlineData("abcd123", "abcd123")]
        public void Deve_Retornar_Password_Valido(string password, string confirmPassword)
        {
           var vo = new PasswordVO(password, confirmPassword);
           Assert.Equal(password, vo.ToString());
        }

        [Theory]
        [InlineData("123456", "123456")]
        [InlineData("abcd123", "abcd123")]
        public void Deve_Testar_Criptografia(string password, string confirmPassword)
        {
           var vo = new PasswordVO(password, confirmPassword);
           vo.CriptPassword();
           Assert.True(vo.EqualsCript(password));
        }
    }
}