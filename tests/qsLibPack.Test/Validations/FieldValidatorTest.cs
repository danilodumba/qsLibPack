using qsLibPack.Test.Validations.Builder;
using qsLibPack.Validations;
using Xunit;

namespace qsLibPack.Test.Validations
{
    public class FieldValidatorTest
    {
        [Fact]
        public void Deve_Validar_Int_Invalido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldInvalid();

            validation.NotNull(builder.FieldInt, "Valor inteiro invalido");
            var validator = validation.Validate(builder);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Decimal_Invalido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldInvalid();

            validation.NotNull(builder.FieldDecimal, "Valor decimal invalido");
            var validator = validation.Validate(builder);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Long_Invalido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldInvalid();

            validation.NotNull(builder.FieldLong, "Valor long invalido");
            var validator = validation.Validate(builder);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_String_Invalido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldInvalid();

            validation.NotNullOrEmpty(builder.FieldString, "Valor string invalido");
            var validator = validation.Validate(builder);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Guid_Invalido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldInvalid();

            validation.NotNull(builder.FieldGuid, "Valor Guid invalido");
            var validator = validation.Validate(builder);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Int_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNull(builder.FieldInt, "Valor inteiro invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Long_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNull(builder.FieldLong, "Valor long invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Decimal_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNull(builder.FieldDecimal, "Valor decimal invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Object_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNull(builder.FieldObject, "Valor Object invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_Guid_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNull(builder.FieldGuid, "Valor Guid invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void Deve_Validar_String_Valido()
        {
            var validation = new FieldValidator<FieldValidatorBuilder>();
            var builder = FieldValidatorBuilder.FieldValid();

            validation.NotNullOrEmpty(builder.FieldString, "Valor String invalido");
            var validator = validation.Validate(builder);

            Assert.True(validator.IsValid);
        }
    }
}