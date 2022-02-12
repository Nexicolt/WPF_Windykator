using IbanNet;

namespace Windykator_PRO.Validation
{
    public class BusinessValidator
    {

        public static string IsCorrectIBAN(string value)
        {
            IIbanValidator validator = new IbanValidator();
            ValidationResult validationResult = validator.Validate(value);
            if (!validationResult.IsValid)
            {
                return "Wprowadzony IBAN jest niepoprawny";
            }

            return null;
        }
    }
}
