using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using DuoSoftware.DuoTools.DuoLogger;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace DVP_DesktopPhone
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs ex)
        {
            base.OnStartup(ex);

            DispatcherUnhandledException += (s, e) =>
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Main - Lost Network Or Unhandled Exception UnhandledException", e.Exception, Logger.LogLevel.Error);
                MessageBox.Show("Main - Lost Network Or Unhandled Exception", "Facetone", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Main - Lost Network Or Unhandled Exception UnhandledException", (Exception)e.ExceptionObject, Logger.LogLevel.Error);
                MessageBox.Show("Main - Lost Network Or Unhandled Exception", "Facetone", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            AppDomain.CurrentDomain.FirstChanceException += (s, e) =>
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Main - Lost Network Or Unhandled Exception FirstChanceException", e.Exception, Logger.LogLevel.Error);
                MessageBox.Show("Main - Lost Network Or Unhandled Exception", "Facetone", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            System.Windows.Forms.Application.ThreadException += (s, e) =>
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Main - Lost Network Or Unhandled Exception ThreadException", e.Exception, Logger.LogLevel.Error);
                MessageBox.Show("Main - Lost Network Or Unhandled Exception", "Facetone", MessageBoxButton.OK, MessageBoxImage.Error);
            };

        }
    }
}
