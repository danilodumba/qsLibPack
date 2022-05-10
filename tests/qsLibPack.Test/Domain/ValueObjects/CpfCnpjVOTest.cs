using System.Security.Cryptography;
using qsLibPack.Domain.Exceptions;
using qsLibPack.Domain.ValueObjects;
using qsLibPack.Domain.ValueObjects.Br;
using Xunit;

namespace qsLibPack.Test.Domain.ValueObjects
{
    public class CpfCnpjVOTest
    {
        [Theory]
        [InlineData("11111111111")]
        [InlineData("22222222222")]
        [InlineData("33333333333")]
        [InlineData("44444444444")]
        [InlineData("55555555555")]
        [InlineData("66666666666")]
        [InlineData("77777777777")]
        [InlineData("88888888888")]
        [InlineData("99999999999")]
        [InlineData("00000000000")]
        [InlineData("111.111.111-11")]
        [InlineData("111-111-111-11")]
        [InlineData("3323232232232232322332323232323")]
        [InlineData("0517992260")]
        public void Deve_Retornar_Cpf_Invalido(string cpf)
        {
           CpfCnpjVO testCpf = cpf;
           Assert.False(testCpf.IsValid(), $"Cpf {cpf} é valido");
           Assert.Throws<DomainException>(() => testCpf.Validate());
        }

        [Theory]
        [InlineData("676.498.650-96")]
        [InlineData("718.654.200-00")]
        [InlineData("972.783.440-00")]
        [InlineData("05074175003")]
        [InlineData("13036530010")]
        [InlineData("asdsad82464378011sdsadas")]
        public void Deve_Retornar_Cpf_Valido(string cpf)
        {
           CpfCnpjVO testCpf = cpf;
           Assert.True(testCpf.IsValid(), $"Cpf {cpf} é invalido");
        }

        [Theory]
        [InlineData("11111111111111")]
        [InlineData("22222222222222")]
        [InlineData("33333333333333")]
        [InlineData("44444444444444")]
        [InlineData("55555555555555")]
        [InlineData("66666666666666")]
        [InlineData("77777777777777")]
        [InlineData("88888888888888")]
        [InlineData("99999999999999")]
        [InlineData("00000000000000")]
        [InlineData("95.502.113/0001-01")]
        [InlineData("58.323.205/0001-83")]
        [InlineData("3323232232232232322332323232323")]
        [InlineData("051799226033234")]
        public void Deve_Retornar_Cnpj_Invalido(string cnpj)
        {
           CpfCnpjVO testCnpj = cnpj;
           Assert.False(testCnpj.IsValid(), $"CNPJ {cnpj} é valido");
           Assert.Throws<DomainException>(() => testCnpj.Validate());
        }

        [Theory]
        [InlineData("58.383.205/0001-83")]
        [InlineData("45.094.633/0001-44")]
        [InlineData("06.216.877/0001-09")]
        [InlineData("93748221000123")]
        [InlineData("57824761000185")]
        [InlineData("14919123000135")]
        public void Deve_Retornar_Cnpj_Valido(string cnpj)
        {
           CpfCnpjVO testCnpj = cnpj;
           Assert.True(testCnpj.IsValid(), $"CNPJ {cnpj} é invalido");
        }
    }
}