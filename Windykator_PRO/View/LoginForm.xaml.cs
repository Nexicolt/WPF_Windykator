using System.Windows;
using System.Windows.Input;
using Windykator_PRO.ViewModel;

namespace Windykator_PRO
{
    /// <summary>
    /// Logika interakcji dla klasy LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void VerifyLoginData(object sender, RoutedEventArgs e)
        {
            //NOwy obiekt okna głównego, wyświetlany po zalogowaniu. Zbindowany z ViewModelem
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();

            Close();
        }
    }
}
