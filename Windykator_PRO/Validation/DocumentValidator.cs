namespace Windykator_PRO.Validation
{
    public class DocumentValidator
    {

        public static string IsCorrectDocNo(string docNO)
        {
            if (string.IsNullOrEmpty(docNO))
            {
                return "Wartość jest wymagana";
            }
            var splitted = docNO.Split('/');
            if(splitted.Length < 2)
            {
                return "Dokument musi mieć format, z rozielającym częsci slesheem (/)";
            }
            return null;
        }
    }
}
