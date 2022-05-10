using System;
namespace qsLibPack.Test.Validations.Builder
{
    public class FieldValidatorBuilder
    {
        public int FieldInt { get; set; }
        public long FieldLong { get; set; }
        public decimal FieldDecimal { get; set; }
        public string FieldString { get; set; }
        public Guid FieldGuid { get; set; }
        public object FieldObject { get; set; }

        public static FieldValidatorBuilder FieldInvalid()
        {
            return new FieldValidatorBuilder 
            {
                FieldDecimal = -1, 
                FieldGuid = Guid.Empty,
                FieldInt = -1, 
                FieldLong = -1, 
                FieldObject = null, 
                FieldString = string.Empty
            };
        }

        public static FieldValidatorBuilder FieldValid()
        {
            return new FieldValidatorBuilder 
            {
                FieldDecimal = 1, 
                FieldGuid = Guid.NewGuid(),
                FieldInt = 1, 
                FieldLong = 1, 
                FieldObject = new object(), 
                FieldString = "teste"
            };
        }

    }
}