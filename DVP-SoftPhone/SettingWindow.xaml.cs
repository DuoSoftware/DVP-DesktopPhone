using Controllers;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;


namespace DVP_DesktopPhone
{
    public partial class SettingWindow : System.Windows.Window
    {
        public bool isSaved = false;
        public SettingWindow()
        {
            InitializeComponent();
            //textBlockVersion.Text = String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString(3));
        }

        

        private void saveSetting_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                FileHandler.WriteUserData(TxtUname.Text.Trim(), TxtPassword.Password.Trim(), TxtDomain.Text.Trim(), TxtDelay.Text.Trim());
                isSaved = true;
                this.Close();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "saveSetting_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var data = FileHandler.ReadUserData();
            if (data != null)
            {
                TxtDomain.Text = data.GetValue("domain").ToString();
                TxtPassword.Password = data.GetValue("password").ToString();
                TxtUname.Text = data.GetValue("name").ToString();
                TxtDelay.Text = data.GetValue("Delay").ToString();
            }
        }

       

        private void TxtDelay_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //e.Handled = !Char.IsDigit(e.Key);
        }

        private string value;
        private void TxtDelay_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            
            var temp = TxtDelay.Text;
            var output = temp.Substring(temp.Length - 1, 1);
            if (!Char.IsDigit(Convert.ToChar(output)))
            {
                TxtDelay.Text = "10";
            }
           
        }
    }
}

                      