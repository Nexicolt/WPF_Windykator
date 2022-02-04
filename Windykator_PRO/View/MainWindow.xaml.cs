using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Windykator_PRO
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /**
         * Funkcja od chwytania okna i jego prenoszenia
         */

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /**
         * Funkcja wywoływana, przy wciśnięciu 'X'
         */

        private void CloseApplication(object sender, RoutedEventArgs e)
        { Close(); }

        /**
        * Funkcja wywoływana, przy wciśnięciu '-'
        */

        private void MinimizeApplication(object sender, RoutedEventArgs e)
        { WindowState = WindowState.Minimized; }

        /**
        * Funkcja wywoływana, przy wciśnięciu przycisku maksymalizacji okna
        */

        private void MaximizeApplication(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        //Funkcja odpowiedzlana za zwijanie/rozwijanie lewego menu
        private void Hide_Show_Menu(object sender, RoutedEventArgs e)
        {
            if (LeftMenu.ActualWidth == 70)
            {
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = 70;
                myDoubleAnimation.To = 170;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

                // Apply the animation to the button's Width property.
                LeftMenu.BeginAnimation(Grid.WidthProperty, myDoubleAnimation);
                leftMenuLogo.Width = 60;

                Hide_Show_Menu_button_image.Source = new BitmapImage(new Uri(@"/Icons/Menu/hide_menu.png", UriKind.Relative));

                myDoubleAnimation.Completed += (s, ew) =>
                {
                    LeftMenu.HorizontalAlignment = HorizontalAlignment.Stretch;
                };
            }
            else
            {
                DoubleAnimation myDoubleAnimation = new DoubleAnimation();
                myDoubleAnimation.From = LeftMenu.ActualWidth;
                myDoubleAnimation.To = 70;
                myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.4));

                // Apply the animation to the button's Width property.
                LeftMenu.BeginAnimation(Grid.WidthProperty, myDoubleAnimation);
                leftMenuLogo.Width = 40;
                LeftMenu.HorizontalAlignment = HorizontalAlignment.Left;

                Hide_Show_Menu_button_image.Source = new BitmapImage(new Uri(@"/Icons/Menu/show_menu.png", UriKind.Relative));
            }
        }
    }
}