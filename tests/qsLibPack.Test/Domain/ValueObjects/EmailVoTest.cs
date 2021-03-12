using qsLibPack.Domain.Exceptions;
using qsLibPack.Domain.ValueObjects;
using Xunit;

namespace qsLibPack.Test.Domain.ValueObjects
{
    public class EmailVoTest
    {
        [Theory]
        [InlineData("a@combr")]
        [InlineData("a@a")]
        [InlineData("asdasdsadsdsaadasdasda")]
        [InlineData("123456@12345")]
        public void Deve_Retornar_Email_Invalido(string email)
        {
           Assert.Throws<DomainException>(() => new EmailVO(email));
        }

        [Theory]
        [InlineData("")]
        [InlineData("teste@teste.com")]
        [InlineData("teste@teste.com.br")]
        [InlineData("teste@teste.info")]
        public void Deve_Retornar_Email_Valido(string email)
        {
           var vo = new EmailVO(email);
           Assert.Equal(email, vo.ToString());
        }
    }
}