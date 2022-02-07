using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Windykator_PRO.Database;
using Windykator_PRO.ViewModel;

namespace Windykator_PRO
{
    /// <summary>
    /// Logika interakcji dla klasy LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        //Połączenie do bazy
        private VindicationDatabase db = null;

        public LoginForm()
        {
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();
            Close();

            //InitializeComponent();
            //db = new VindicationDatabase();
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
            string login = this.LoginInput.Text;
            string password = this.PasswordInput.Password;

            string hashedPassword = Hash(password);
           

            var userDto = db.User.Where(row => row.Login == login && row.Password == hashedPassword && row.IsEnable).FirstOrDefault();
            if (userDto != null)
            {
                var mainWindow = new MainWindow();
                mainWindow.DataContext = new MainWindowViewModel();
                mainWindow.Show();
                ErrorPanel.Visibility = Visibility.Collapsed;
                Application.Current.Resources["User_Login"] = userDto.Login;
                Application.Current.Resources["User_Id"] = userDto.Id;
                Close();
            }
            else
            {
                LoginWindow.Height = 330;
                ErrorPanel.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Funkcja hashująca zadany string
        /// </summary>
        private static string Hash(string password)
        {
            using (SHA384 sha = new SHA384CryptoServiceProvider())
            {
                byte[] preHash = Encoding.ASCII.GetBytes(password);

                byte[] hash = sha.ComputeHash(preHash);

                return Convert.ToBase64String(hash);
            }
        }
    }
}