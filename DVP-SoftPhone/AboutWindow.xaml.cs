using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace DVP_DesktopPhone
{
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            textBlockVersion.Text = String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://www.yourwebsite.com/");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void email_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("mailto:info@yourwebsite.com")); e.Handled = true;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

        }
    }
}
