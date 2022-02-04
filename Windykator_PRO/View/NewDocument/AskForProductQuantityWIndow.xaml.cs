using System.Windows;
using Windykator_PRO.ViewModel;

namespace Windykator_PRO.View.NewDocument
{
    /// <summary>
    /// Logika interakcji dla klasy AskForProductQuantityWIndow.xaml
    /// </summary>
    public partial class AskForProductQuantityWIndow : Window
    {
        private AddNewDocumentViewModel hanlder = null;
        public AskForProductQuantityWIndow(AddNewDocumentViewModel baseWindowsHandler)
        {
            hanlder = baseWindowsHandler;
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            decimal parsedInput;

            if(decimal.TryParse(this.Quantity.Text, out parsedInput))
            {
                if(parsedInput <= 0)
                {
                    MessageBox.Show("Wartośc musi być większa od 0", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    hanlder.UserInputQuantity = parsedInput;
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Niepoprawna wartość", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

            }


        }


    }
}
