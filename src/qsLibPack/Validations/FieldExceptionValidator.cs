using System;
using qsLibPack.Domain.Exceptions;

namespace qsLibPack.Validations
{
    public static class FieldExceptionValidator
    {
        public static void LessThanOrEqualZero(this decimal value)
        {
            LessThanOrEqualZero(value, null);
        }
        public static void LessThanOrEqualZero(this decimal value, string message)
        {
            if (value <= 0) RiseException(message);
        }

        public static void LessThanOrEqualZero(this int value)
        {
            LessThanOrEqualZero(value, null);
        }
        public static void LessThanOrEqualZero(this int value, string message)
        {
            if (value <= 0) RiseException(message);
        }

        public static void NotNullOrEmpty(this string value)
        {
            NotNullOrEmpty(value, null);
        }
        public static void NotNullOrEmpty(this string value, string message)
        {
            if (string.IsNullOrWhiteSpace(value)) RiseException(message);
        }

        public static void NotNullOrEmpty(this Guid value)
        {
            NotNullOrEmpty(value, null);
        }
        public static void NotNullOrEmpty(this Guid value, string message)
        {
            if (value == null || value == Guid.Empty) RiseException(message);
        }

        public static void NotNull(this object value)
        {
            NotNull(value, null);
        }

        public static void NotNull(this object value, string message)
        {
            if (value == null) RiseException(message);
        }

        private static void RiseException(string message)
        {
            var text = "Invalid field";
            if (!string.IsNullOrWhiteSpace(message))
            {
                text = message;
            }

            throw new DomainException(text);
        }
    }
}