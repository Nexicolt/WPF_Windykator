using System.Windows;
using Windykator_PRO.ViewModel;

namespace Windykator_PRO.View.NewDocument
{
    /// <summary>
    /// Logika interakcji dla klasy AskForProductQuantityWIndow.xaml
    /// </summary>
    public partial class AddnotationModal : Window
    {
        private AddNewDocumentViewModel hanlder = null;
        public AddnotationModal(string title, string content)
        {

            InitializeComponent();
            AddnotationTitle.Text = title;
            AddnotationContent.Text = content;
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

      


    }
}
