using System.Text.RegularExpressions;

namespace Windykator_PRO.Validation
{
    public class StringValidator
    {
        #region ValidateErrors

        private static string Value_IsRequired = "Wartośc jest wymagana";
        private static string Value_HaveToBeNumber = "Wartośc musi być liczbą";

        #endregion ValidateErrors

        public static string IsEmpty(string text)
        {
            return (string.IsNullOrEmpty(text)) ? Value_IsRequired : null;
        }

        public static string IsNumber(string text)
        {
            return (!Regex.IsMatch(text, @"^\d+$")) ? Value_HaveToBeNumber : null;
        }
    }
}