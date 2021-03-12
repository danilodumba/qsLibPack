namespace qs.Domain.Core.Validations
{
    public class DomainError
    {
        public DomainError(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; private set; }
        public string Description { get; private set; }
    }
}