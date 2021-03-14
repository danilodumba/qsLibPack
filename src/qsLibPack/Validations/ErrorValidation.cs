namespace qsLibPack.Validations
{
    public class ErrorValidation
    {
        public ErrorValidation(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; private set; }
        public string Description { get; private set; }
    }
}