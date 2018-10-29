using Controllers;
using DuoCallTesterLicenseKey;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;
using PortSIP;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;


namespace DVP_DesktopPhone
{
    public partial class PhoneWindow : Window, SIPCallbackEvents, IUiState
    {
        private static Mutex mutex;
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private PortSIPLib _phoneController;
        private Agent _agent;
        private Call _call;
        private bool _sipLogined = false;
        NotifyIcon mynotifyicon = new NotifyIcon();
        private bool isCallAnswerd;
        private System.Media.SoundPlayer _wavPlayer = new System.Media.SoundPlayer();
        private int AutoAnswerDelay = 10000;
        private bool AutoAnswerEnable = false;
        System.Timers.Timer _callDurations;
        DateTime _callStarTime;
        int audioDivID = 0;
        #region Keypad events

        private void buttonConference_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Conference_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                var setting = VeerySetting.Instance;
                foreach (var c in setting.ConferenceCode)
                {
                    SendDtmf(setting.DtmfValues[c]);
                }
                webSocketlistner.SendMessageToClient(CallFunctions.ConfCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonEtl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Etl_Click-> Session Id : {0} , Status : {1}", _agent.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                var setting = VeerySetting.Instance;
                foreach (var c in setting.EtlCode)
                {
                    SendDtmf(setting.DtmfValues[c]);
                }
                webSocketlistner.SendMessageToClient(CallFunctions.EtlCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferCall_Click(object sender, RoutedEventArgs e)
        {
            if (textBlockDialingNumber.Text.Length <= 3)
            {
                mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Invalid Number.", ToolTipIcon.Error);
                return;
            }
            TransferCall();
        }

        private void buttonMute_Click(object sender, RoutedEventArgs e)
        {
            MuteUnmute();
        }

        private void buttonHold_Click(object sender, RoutedEventArgs e)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Hold_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
            HoldUnholdCall();
        }

        private void buttonDialPad_Click(object sender, RoutedEventArgs e)
        {
            GrdCallFunctions.Visibility = Visibility.Visible;
            GrdDailpad.Visibility = Visibility.Hidden;


            buttonDialPad.Visibility = Visibility.Hidden;

        }

        private void buttonDilapadshow_Click(object sender, RoutedEventArgs e)
        {
            GrdCallFunctions.Visibility = Visibility.Hidden;
            GrdDailpad.Visibility = Visibility.Visible;


            buttonDialPad.Visibility = Visibility.Visible;
        }


        string _userName = String.Empty;
        string _password = String.Empty;
        private BackgroundWorker _bw = new BackgroundWorker();


        private void SetStatusMessage(string message)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                   {
                       textBlockCallStateInfo.Text = message;
                   }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SetStatusMessage", exception, Logger.LogLevel.Error);
            }
        }
        private void buttonPickUp_Click(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(new Action(() =>
            {

                buttonAnswer.IsEnabled = false;
                if (string.IsNullOrEmpty(textBlockDialingNumber.Text))
                {
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Please Enter Number To Dial.", ToolTipIcon.Warning);
                    return;
                }

                MakeCall(textBlockDialingNumber.Text);

            }));


        }

        private void buttonHangUp_Click(object sender, RoutedEventArgs e)
        {
            EndCall();
        }

        private void buttonKeyPadButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
                return;

            textBlockDialingNumber.Text += btn.Content.ToString().Trim();
            SendDtmf(GetDtmfSignalFromButtonTag(btn));
        }

        private void buttonKeyPad_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void buttonKeyPad_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private int GetDtmfSignalFromButtonTag(Button button)
        {
            if (button == null)
                return -1;

            if (button.Tag == null)
                return -1;

            int signal;
            if (int.TryParse(button.Tag.ToString(), out signal))
                return signal;

            return -1;
        }


        #endregion

        #region Menu events

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do You Want To Exit?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
            }
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start("http://www.facetone.com/");
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        private void phone_minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonTestAudio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ButtonTestAudio.IsChecked == true)
                {
                    ButtonTestAudio.ToolTip = "Stop Test";
                    _phoneController.audioPlayLoopbackTest(true);
                }
                else
                {
                    ButtonTestAudio.ToolTip = "Test Audio";
                    _phoneController.audioPlayLoopbackTest(false);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ButtonTestAudio_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void TrackBarSpeaker_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _phoneController.setSpeakerVolume(Convert.ToInt16(TrackBarSpeaker.Value));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerVolumeChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void TrackBarMicrophone_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                _phoneController.setMicVolume(Convert.ToInt16(TrackBarMicrophone.Value));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicVolumeChanged", exception, Logger.LogLevel.Error);
            }
        }

        private void chkboxMuteSpeaker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var val = chkboxMuteSpeaker.IsChecked == true;
                _phoneController.muteSpeaker(val);
                picSpek.Visibility = val ? Visibility.Visible : Visibility.Hidden;
                if (val) return;
                _phoneController.setSpeakerVolume(_phoneController.getSpeakerVolume());
                TrackBarSpeaker.Value = _phoneController.getSpeakerVolume();

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnSpeakerMute", exception, Logger.LogLevel.Error);
            }
        }

        private void CheckBoxMute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var val = CheckBoxMute.IsChecked == true;
                _phoneController.muteMicrophone(val);
                picMic.Visibility = val ? Visibility.Visible : Visibility.Hidden;
                if (val) return;
                _phoneController.setMicVolume(_phoneController.getMicVolume());
                TrackBarMicrophone.Value = _phoneController.getMicVolume();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicMute", exception, Logger.LogLevel.Error);
            }

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            GrdCallButton.Visibility = Visibility.Visible;
            GrdVolume.Visibility = Visibility.Hidden;
        }
        private void volumeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if (_agent.AgentCurrentState.GetType() != typeof(AgentIdle))
            //{
            //    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Not Allow", ToolTipIcon.Warning);
            //    return;
            //}
            GrdCallButton.Visibility = Visibility.Hidden;
            GrdVolume.Visibility = Visibility.Visible;
        }

        private void SettingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UninitializePhone();
            SettingWindow wnd = new SettingWindow { Owner = this };
            wnd.Closing += (a, k) =>
            {
                //var data = FileHandler.ReadUserData();
                //if (data != null)
                //{
                //    _agent.Profile.Login(data.GetValue("name").ToString(), data.GetValue("password").ToString(), data.GetValue("domain").ToString());

                //   InitializePhone(true);                    
                //}
                if (!wnd.isSaved) return;
                System.Windows.Forms.Application.Restart();
                System.Windows.Application.Current.Shutdown();
            };
            wnd.ShowDialog();
        }

        private void autoanswerMenu_Checked(object sender, RoutedEventArgs e)
        {
            AutoAnswerEnable = true;
        }

        private void autoanswerMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            AutoAnswerEnable = false;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Window_MouseDown", exception, Logger.LogLevel.Error);
            }
        }
        #endregion

        #region FormEvents


        private System.Windows.Forms.NotifyIcon MyNotifyIcon;
        public PhoneWindow()
        {
            InitializeComponent();

        }


        WebSocketServiceHost webSocketlistner;
        private void InitiateWebSocket()
        {
            #region WebSocket Server
            try
            {


                if (VeerySetting.Instance.WebSocketlistnerEnable)
                {
                    webSocketlistner = new WebSocketServiceHost(VeerySetting.Instance.WebSocketlistnerPort);

                    WebSocketServiceHost.OnRecive += (callFunction, no, othr) =>
                    {


                        try
                        {

                            Dispatcher.Invoke(new Action(() =>
                            {
                                try
                                {


                                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                string.Format(
                                    "[webSocketlistner]External Application send Commands. callFunction : {0}, Phone No : {1}",
                                    callFunction, no), Logger.LogLevel.Info);
                                    switch (callFunction)
                                    {
                                        case CallFunctions.Registor:
                                            RegistorPhone(othr);
                                            break;
                                        case CallFunctions.Unregistor:
                                            UnregistorPhone(othr);
                                            break;
                                        case CallFunctions.MakeCall:
                                            {
                                                Dispatcher.Invoke(new Action(() =>
                                                {
                                                    textBlockDialingNumber.Text = no;
                                                    SetStatusMessage("Dialing");
                                                    MakeCall(no);
                                                }));

                                            }
                                            break;
                                        case CallFunctions.AnswerCall:
                                            AnswerCall();
                                            break;
                                        case CallFunctions.EndCall:
                                        case CallFunctions.RejectCall:
                                            EndCall();
                                            break;

                                        case CallFunctions.HoldCall:
                                            HoldUnholdCall();
                                            break;
                                        case CallFunctions.MuteCall:
                                            MuteUnmute();
                                            break;
                                        case CallFunctions.EtlCall:
                                            buttonEtl_Click(null, null);
                                            break;
                                        case CallFunctions.ConfCall:
                                            buttonConference_Click(null, null);
                                            break;
                                        case CallFunctions.Inbound:
                                            _agent.AgentMode = AgentMode.Inbound;
                                            break;
                                        case CallFunctions.Outbound:
                                            _agent.AgentMode = AgentMode.Outbound;
                                            break;
                                        case CallFunctions.TransferIVR:
                                            {
                                                if (!string.IsNullOrEmpty(no))
                                                {
                                                    if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                                                    {
                                                        HoldUnholdCall();
                                                    }

                                                    Dispatcher.Invoke(new Action(() =>
                                                    {
                                                        textBlockDialingNumber.Text = no;
                                                        TransferIVR();

                                                    }));

                                                }
                                            }
                                            break;
                                        case CallFunctions.TransferCall:
                                            {
                                                if (!string.IsNullOrEmpty(no))
                                                {
                                                    if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                                                    {
                                                        HoldUnholdCall();
                                                    }

                                                    Dispatcher.Invoke(new Action(() =>
                                                    {
                                                        textBlockDialingNumber.Text = no;
                                                        TransferCall();

                                                    }));

                                                }
                                            }
                                            break;

                                        default:
                                            throw new ArgumentOutOfRangeException("callFunction");
                                    }
                                }
                                catch (Exception exception)
                                {
                                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                        "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive-Dispatcher", exception,
                                        Logger.LogLevel.Error);
                                }

                            }));

                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive", exception,
                                Logger.LogLevel.Error);
                        }
                    };
                }
                else
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                        "FormDialPad_Load-[webSocketlistner]-Disable", Logger.LogLevel.Info);
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                    "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive", exception,
                    Logger.LogLevel.Error);
            }
            #endregion WebSocket Server
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bool ok;

            //The name used when creating the mutex can be any string you want
            mutex = new Mutex(true, "Facetone_soft_phone", out ok);

            if (!ok)
            {
                MessageBox.Show("Application Already Running", "Facetone", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(100);
            }

            _agent = new Agent(Guid.NewGuid().ToString(), this) { AgentReqMode = AgentMode.initiate };

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = string.Format("{0}{1}", appDataFolder, "\\veery\\ringtone.wav");

            _wavPlayer = (File.Exists(filePath)) ? new System.Media.SoundPlayer(filePath) : new System.Media.SoundPlayer(Properties.Resources.ringtone);

            //_wavPlayer.Open(new Uri(String.Format("{0}/{1}", System.AppDomain.CurrentDomain.BaseDirectory, "ringtone.wav")));

            if (VeerySetting.Instance.AgentConsoleintegration)
            {
                textBlockCallStateInfo.Text = "Loading.....";
                Task.Delay(10000).ContinueWith(_ =>
                    {
                        InitiateWebSocket();
                        Dispatcher.Invoke(() =>
                        {
                            textBlockCallStateInfo.Text = "Idle";
                        });
                    }
                );

            }
            else
            {
                var data = FileHandler.ReadUserData();
                if (data == null)
                {
                    SettingMenuItem_Click(sender, e);
                    //data = FileHandler.WriteUserData("9502", "DuoS123", "duo.media1.veery.cloud");
                    data = FileHandler.ReadUserData();
                }
                _agent.Profile.Login(data.GetValue("name").ToString(), data.GetValue("password").ToString(), data.GetValue("domain").ToString());

                //            _agent.Profile.Login("9502", "DuoS123", "duo.media1.veery.cloud");
                InitializePhone(false);
                AutoAnswerDelay = Convert.ToInt16(data.GetValue("Delay").ToString()) * 1000;
                settingMenuItem.Visibility = System.Windows.Visibility.Visible;
            }

            this.ShowInTaskbar = VeerySetting.Instance.ShowInTaskbar;

            //Application.ApplicationExit += new EventHandler(this.OnApplicationExit);


            _callDurations = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalSeconds);
            _callDurations.Elapsed += (s, e1) =>
                {
                    var ts = e1.SignalTime.Subtract(_callStarTime);
                    var elapsedTime = ts.Hours > 0
                        ? String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds)
                        : String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);

                    Dispatcher.Invoke(new Action(() =>
                    {
                        textBlockCallStateInfo.Text = elapsedTime;
                    }));

                };

            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "Facetone Phone Has Been Minimised. Click The Tray Icon To Show.";
            m_notifyIcon.BalloonTipTitle = "Facetone Phone";
            m_notifyIcon.Text = "Facetone Phone";
            m_notifyIcon.Icon = Properties.Resources.facetone_logo;//E:\DuoProject\Applications\DVP-DesktopPhone\DVP-SoftPhone\Resources\facetone-logo.ico
            m_notifyIcon.DoubleClick += new EventHandler(m_notifyIcon_Click);

            WindowState = System.Windows.WindowState.Minimized;
        }

        void OnClose(object sender, CancelEventArgs args)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;
        }

        private WindowState m_storedWindowState = WindowState.Normal;
        void OnStateChanged(object sender, EventArgs args)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
                if (m_notifyIcon != null)
                    m_notifyIcon.ShowBalloonTip(2000);
            }
            else
                m_storedWindowState = WindowState;
        }
        void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        void m_notifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
        }
        void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }

        void ShowTrayIcon(bool show)
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = show;
        }

        private void Window_Closed(object sender, EventArgs e)
        {


        }

        private void btnDialpad_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Phone

        private void RegistorPhone(string message)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    SetStatusMessage(". . .Registoring. . .");
                }));
                var callInfo = message.Split('-'); // "name-password-domain"
                if (callInfo.Length < 3 || string.IsNullOrEmpty(callInfo[0]) || callInfo[0] == "undefined" ||
                    string.IsNullOrEmpty(callInfo[1]) || callInfo[1] == "undefined" ||
                    string.IsNullOrEmpty(callInfo[2]) || callInfo[2] == "undefined")
                {
                    webSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
                    return;
                }

                UninitializePhone();
                var domain = callInfo[2]; var password = callInfo[1]; var username = callInfo[0];


                if (callInfo[2].Contains('@'))
                {
                    var values = callInfo[2].Split('@');
                    if (values.Any() && values.Length >= 2)
                    {
                        domain = values[1];
                        values = domain.Split(':');
                        if (values.Any() && values.Length >= 2)
                        {
                            domain = values[0];
                            sipServerPort = Convert.ToInt32(values[1]);
                        }
                    }
                }
                else if (callInfo[2].Contains(':'))
                {
                    var values = domain.Split(':');
                    if (values.Any() && values.Length >= 2)
                    {
                        domain = values[0];
                        sipServerPort = Convert.ToInt32(values[1]);
                    }
                }


                _agent.Profile.Login(username, password, domain);
                InitializePhone(true);
            }
            catch (Exception exception)
            {
                webSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "RegistorPhone", exception, Logger.LogLevel.Error);

            }

        }

        private void UnregistorPhone(string message)
        {
            try
            {
                UninitializePhone();

                Dispatcher.Invoke(() =>
                {
                    textBlockIdentifier.Text = "Offline";
                    buttonAnswer.IsEnabled = false;
                    textBlockRegStatus.Text = "Offline";
                    textBlockCallStateInfo.Text = "Offline";
                    textBlockDialingNumber.Text = "0000000000";
                    _callDurations.Stop();
                    _callDurations.Enabled = false;
                });
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UnregistorPhone", exception, Logger.LogLevel.Error);
            }

        }

        private void TransferCall()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("transferCall_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                if (!String.IsNullOrEmpty(textBlockDialingNumber.Text))
                {
                    var setting = VeerySetting.Instance;
                    var tranNo = textBlockDialingNumber.Text.Trim();

                    var dtmfSet = tranNo.Length <= 5 ? setting.TransferExtCode : setting.TransferPhnCode;

                    foreach (var d in dtmfSet)
                    {
                        try
                        {
                            SendDtmf(setting.DtmfValues[d]);
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "TransferCall-SendDTMF", exception, Logger.LogLevel.Error);
                        }
                    }
                    Thread.Sleep(1000);
                    tranNo = string.Format("{0}#", tranNo);
                    foreach (var d in tranNo.ToCharArray())
                    {
                        try
                        {
                            SendDtmf(setting.DtmfValues[d]);

                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "TransferCall-SendDTMF", exception, Logger.LogLevel.Error);
                        }
                    }
                    webSocketlistner.SendMessageToClient(CallFunctions.TransferCall);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void TransferIVR()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("TransferIVR-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                if (!String.IsNullOrEmpty(textBlockDialingNumber.Text))
                {
                    var setting = VeerySetting.Instance;
                    var tranNo = textBlockDialingNumber.Text.Trim();

                    var dtmfSet = setting.TransferExtCode;

                    foreach (var d in dtmfSet)
                    {
                        try
                        {
                            SendDtmf(setting.DtmfValues[d]);
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "TransferIVR-SendDTMF", exception, Logger.LogLevel.Error);
                        }
                    }
                    Thread.Sleep(1000);
                    tranNo = string.Format("{0}#", tranNo);
                    foreach (var d in tranNo.ToCharArray())
                    {
                        try
                        {
                            SendDtmf(setting.DtmfValues[d]);

                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "TransferIVR-SendDTMF", exception, Logger.LogLevel.Error);
                        }
                    }
                    webSocketlistner.SendMessageToClient(CallFunctions.TransferIVR);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private bool _isMute;
        private void MuteUnmute()
        {
            try
            {
                Dispatcher.Invoke(() =>
                {
                    _phoneController.muteMicrophone(!_isMute);
                    buttonMute.Content = _isMute ? "mute" : "Unmute";
                    _isMute = !_isMute;
                    picMic.Visibility = _isMute ? Visibility.Visible : Visibility.Hidden;
                });
                webSocketlistner.SendMessageToClient(_isMute ? CallFunctions.MuteCall : CallFunctions.UnmuteCall);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MuteUnmute", exception, Logger.LogLevel.Error);
            }
        }

        private void HoldUnholdCall()
        {
            try
            {
                var status = string.Empty;
                int res = -1;
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                {
                    res = _phoneController.hold(_call.portSipSessionId);
                    if (res == 0)
                    {
                        _call.CallCurrentState.OnHold(ref _call, CallActions.Hold);
                        status = "Hold Call";
                    }
                    webSocketlistner.SendMessageToClient(CallFunctions.HoldCall);

                }
                else if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    res = _phoneController.unHold(_call.portSipSessionId);
                    if (res == 0)
                    {
                        _call.CallCurrentState.OnUnHold(ref _call, CallActions.UnHold);
                        status = "Connected";
                    }
                    webSocketlistner.SendMessageToClient(CallFunctions.UnholdCall);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    SetStatusMessage(status);
                }


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "HoldUnholdCall", exception, Logger.LogLevel.Error);
            }
        }

        private void SendDtmf(int digit)
        {
            try
            {
                if (digit < 0)
                    return;

                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                    _phoneController.sendDtmf(_agent.PortsipSessionId, DTMF_METHOD.DTMF_RFC2833, Convert.ToInt16(digit), 160, true);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SendDTMF", exception, Logger.LogLevel.Error);
            }
        }

        private void EndCall()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("End call. Agent Status : [{0}], Call Status : [{1}]", _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Info);
                StopRingTone();
                var status = "Call Ended";
                if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallTryingState))
                {
                    if (_agent.CallDirection == CallDirection.Outgoing)
                    {
                        _phoneController.hangUp(_call.portSipSessionId);
                    }
                    else
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger9, string.Format("End call[486]. Agent Status : [{0}], Call Status : [{1}]", _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Debug);
                        _phoneController.rejectCall(_call.portSipSessionId, 486);
                        status = "Call Rejected";
                    }
                    //webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                }
                else
                    _phoneController.hangUp(_call.portSipSessionId);

                SetStatusMessage(status);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("End call. Agent Status : [{0}], Call Status : [{1}] , status : [{2}]", _agent.AgentCurrentState, _call.CallCurrentState, status), Logger.LogLevel.Info);
                _call.CallCurrentState.OnDisconnected(ref _call);
                _agent.AgentCurrentState.OnEndCall(ref _agent, true);
                if (_isMute)
                    MuteUnmute();
                // webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EndCall", exception, Logger.LogLevel.Error);
            }
        }

        private void AnswerCall()
        {
            try
            {
                StopRingInTone();
                StopRingTone();
                if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    SetStatusMessage("Answering");
                    _phoneController.answerCall(_call.portSipSessionId, false);
                    webSocketlistner.SendMessageToClient(CallFunctions.AnswerCall);
                }
                else
                {

                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Answer Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                    SetStatusMessage("Fail to Answer Call");
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}", _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Error);

                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        private void MakeCall(string no)
        {
            try
            {
                StopRingInTone();
                StopRingTone();
                if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle) && _agent.AgentMode == AgentMode.Outbound)
                {
                    if (!String.IsNullOrEmpty(no))
                    {
                        _agent.AgentCurrentState.OnMakeCall(ref _agent);
                        InAgentBusy(CallDirection.Outgoing);

                        _call = new Call(no, this)
                        {
                            portSipSessionId = _phoneController.call(no, true, false)
                        };
                        if (_call.portSipSessionId < 0)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "MakeCall-Fail", Logger.LogLevel.Error);
                            mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Make Call", ToolTipIcon.Error);
                            _agent.AgentCurrentState.OnFailMakeCall(ref _agent);
                            _call.CallCurrentState.OnTimeout(ref _call);
                            webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                            return;
                        }
                        SetStatusMessage("Dialing");
                        _agent.PortsipSessionId = _call.portSipSessionId;
                        _call.SetDialInfo(_call.portSipSessionId, Guid.NewGuid());
                        webSocketlistner.SendMessageToClient(CallFunctions.MakeCall);
                    }
                    else
                    {
                        mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Invalid Phone Number.", ToolTipIcon.Error);
                    }
                }
                else if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    SetStatusMessage("Answering");
                    _phoneController.answerCall(_call.portSipSessionId, false);
                    webSocketlistner.SendMessageToClient(CallFunctions.AnswerCall);
                }
                else if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle) && _agent.AgentMode == AgentMode.Inbound && _call.CallCurrentState.GetType() == typeof(CallIdleState))
                {
                    //if (!AutoAnswer.Checked)
                    //{
                    //    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone",
                    //        "Fail to Make Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                    //}
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Make Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                    SetStatusMessage("Fail to Make Call");
                    webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}", _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Error);

                }
                else
                {
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Make Call.", ToolTipIcon.Warning);
                    webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                    SetStatusMessage("Fail to Make Call");
                }



            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        private int sipServerPort = 5060;
        private void InitializePhone(bool isReInit)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    SetStatusMessage(". . .Initializing. . .");
                }));
                StopRingInTone();
                StopRingTone();
                //var settingObject = AgentProfile.Instance.settingObject;
                var agentProfile = AgentProfile.Instance;
                var userName = agentProfile.authorizationName;
                var password = agentProfile.Password;
                var displayName = agentProfile.displayName;
                var authName = agentProfile.authorizationName;
                Random rd = new Random();
                var localPort = string.IsNullOrEmpty(agentProfile.settingObject["localPort"]) ? (rd.Next(1000, 5000) + 4000) : (Convert.ToInt32(agentProfile.settingObject["localPort"]));
                sipServerPort = sipServerPort > 0 ? sipServerPort : Convert.ToInt16(agentProfile.settingObject["sipServerPort"]);
                var sipServer = agentProfile.Domain;
                var localIp = agentProfile.localIPAddress;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName, password, localPort), Logger.LogLevel.Info);

                int errorCode = 0;

                _phoneController = new PortSIPLib(0, 0, this);
                _phoneController.createCallbackHandlers();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "createCallbackHandlers-end, call initialize method", Logger.LogLevel.Info);

                var rt = _phoneController.initialize(TRANSPORT_TYPE.TRANSPORT_UDP,
                    // Use 0.0.0.0 for local IP then the SDK will choose an available local IP automatically.
                    // You also can specify a certain local IP to instead of "0.0.0.0", more details please read the SDK User Manual
                    localIp,
                    localPort,
                    PORTSIP_LOG_LEVEL.PORTSIP_LOG_NONE,
                    System.AppDomain.CurrentDomain.BaseDirectory,
                    1,
                    "veery_soft_phone",
                    VeerySetting.Instance.audioDeviceLayer,
                    VeerySetting.Instance.videoDeviceLayer,
                    "/",
                    "",
                    false);
                if (rt != 0)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "initialize-failed", Logger.LogLevel.Info);
                    _phoneController.releaseCallbackHandlers();
                    InitializeError("failed.", 408);
                    return;
                }
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "initialize-end", Logger.LogLevel.Info);

                loadDevices();

                var outboundServer = "";
                var outboundServerPort = 0;
                var userDomain = "";
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "loadDevices-end", Logger.LogLevel.Info);

                var rt_userInfo = _phoneController.setUser(userName,
                    displayName,
                    authName,
                    password,
                    userDomain,
                    sipServer,
                    sipServerPort,
                    VeerySetting.Instance.stunServer,
                    VeerySetting.Instance.stunServerPort,
                    outboundServer,
                    outboundServerPort);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "setUser-end", Logger.LogLevel.Info);
                if (rt_userInfo != 0)
                {
                    if (!isReInit)
                    {
                        _phoneController.unInitialize();
                        _phoneController.releaseCallbackHandlers();
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1,
                            string.Format(
                                "userName : {0}, authName : {1}, password : {2}, localPort : {3}.............................SetUserInfo Failed. errorCode : {4}",
                                userName, authName, password, localPort, errorCode), Logger.LogLevel.Info);
                        InitializeError("Fail", rt_userInfo);
                        return;
                    }
                }

                _phoneController.setSrtpPolicy(SRTP_POLICY.SRTP_POLICY_NONE, false);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "setSrtpPolicy-end", Logger.LogLevel.Info);

                string licenseKey = LicenseKeyHandler.GetLicenseKey("DuoS123");
                rt = _phoneController.setLicenseKey(licenseKey);

                if (rt == PortSIP_Errors.ECoreTrialVersionLicenseKey)
                {
                    MessageBox.Show("This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.");

                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "This sample was built base on evaluation key, which allows only three minutes conversation. The conversation will be cut off automatically after three minutes, then you can't hearing anything. Feel free contact us at: waruna@duosoftware.com to purchase the official version.", Logger.LogLevel.Info);
                }
                else if (rt == PortSIP_Errors.ECoreWrongLicenseKey)
                {
                    MessageBox.Show("The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com");
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "The wrong license key was detected, please check with waruna@duosoftware.com or support@duosoftware.com", Logger.LogLevel.Info);
                }

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "setLocalVideoWindow", Logger.LogLevel.Info);
                _phoneController.setLocalVideoWindow(IntPtr.Zero);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "setLocalVideoWindow-end", Logger.LogLevel.Info);
                initAutioCodecs();

                _phoneController.enableVAD(false);
                _phoneController.enableAEC(EC_MODES.EC_DEFAULT);
                _phoneController.enableCNG(false);
                _phoneController.enableAGC(AGC_MODES.AGC_DEFAULT);
                _phoneController.enableANS(NS_MODES.NS_DEFAULT);
                _phoneController.enableReliableProvisional(false);

                var rt_register = _phoneController.registerServer(3600, 3);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "registerServer-end", Logger.LogLevel.Info);
                if (rt_register != 0)
                {
                    _phoneController.unInitialize();
                    _phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................Registration Failed. errorCode : {4}", userName, authName, password, localPort, rt_register), Logger.LogLevel.Info);
                    InitializeError("Fail", rt_register);
                    return;
                }

                _phoneController.setAudioDeviceId(0, 0);
                //_phoneController.setAudioCodecParameter(AUDIOCODEC_TYPE.AUDIOCODEC_AMRWB, "mode-set=0; octet-align=0; robust-sorting=0");



                _phoneController.addSupportedMimeType("INFO", "text", "plain");


                //phoneController.setSpeakerVolume(26214);//40% volume
                //phoneController.setMicVolume(52428);//80%
                if (!isReInit && !VeerySetting.Instance.WebSocketlistnerEnable)
                {
                    textBlockCallStateInfo.Text = "Loading.....";
                    Task.Delay(10000).ContinueWith(_ =>
                        {
                            InitiateWebSocket();
                            Dispatcher.Invoke(() =>
                            {
                                textBlockCallStateInfo.Text = "Idle";
                            });
                        }
                    );
                }

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "InitializePhone-end", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void UninitializePhone()
        {
            try
            {
                _phoneController.hangUp(_agent.PortsipSessionId);

                _phoneController.rejectCall(_agent.PortsipSessionId, 486);
                _phoneController.unRegisterServer();
                _phoneController.unInitialize();
                _phoneController.releaseCallbackHandlers();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        private void initAutioCodecs()
        {
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS);

            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF); // For RTP event - DTMF (RFC2833)
        }

        private void loadDevices()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "loadDevices", Logger.LogLevel.Info);
                //InitAdioWizItems();
                Dispatcher.Invoke(() =>
                {
                    ComboBoxSpeakers.Items.Clear();
                    ComboBoxMicrophones.Items.Clear();
                    int num = _phoneController.getNumOfPlayoutDevices();
                    for (int i = 0; i < num; ++i)
                    {
                        StringBuilder deviceName = new StringBuilder();
                        deviceName.Length = 256;

                        if (_phoneController.getPlayoutDeviceName(i, deviceName, 256) == 0)
                        {
                            ComboBoxSpeakers.Items.Add(deviceName.ToString());
                        }
                        ComboBoxSpeakers.SelectedIndex = 0;
                    }

                    /*if (ComboBoxSpeakers.Items.Count > 0)
                        ComboBoxSpeakers.SelectedIndex = 0;*/

                    num = _phoneController.getNumOfRecordingDevices();
                    for (int i = 0; i < num; ++i)
                    {
                        var deviceName = new StringBuilder { Length = 256 };

                        if (_phoneController.getRecordingDeviceName(i, deviceName, 256) == 0)
                        {
                            ComboBoxMicrophones.Items.Add(deviceName.ToString());
                        }
                        ComboBoxMicrophones.SelectedIndex = 0;
                    }

                    /*if (ComboBoxMicrophones.Items.Count > 0)
                        ComboBoxMicrophones.SelectedIndex = 0;*/

                    //int volume = _phoneController.getSpeakerVolume();

                    //volume = _phoneController.getMicVolume();

                    //TrackBarSpeaker.SetRange(0, 255);
                    TrackBarSpeaker.Value = _phoneController.getSpeakerVolume();

                    //TrackBarMicrophone.SetRange(0, 255);
                    TrackBarMicrophone.Value = _phoneController.getMicVolume();

                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Mic : {0}, Spk : {1}", TrackBarMicrophone.Value, TrackBarSpeaker.Value), Logger.LogLevel.Info);
                });

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "loadDevices", exception, Logger.LogLevel.Error);
            }
        }


        private void InitAdioWizItems()
        {
            try
            {
                ManagementObjectSearcher objSearcher = new ManagementObjectSearcher(
                    "SELECT * FROM Win32_SoundDevice");

                ManagementObjectCollection objCollection = objSearcher.Get();


                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "**********************************",
                    Logger.LogLevel.Info);
                foreach (ManagementObject obj in objCollection)
                {
                    foreach (PropertyData property in obj.Properties)
                    {
                        //Console.Out.WriteLine(String.Format("{0}:{1}", property.Name, property.Value));
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                            String.Format("{0}:{1}", property.Name, property.Value), Logger.LogLevel.Info);

                    }

                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                        "-----------------------------------------------------------------", Logger.LogLevel.Info);

                }

                foreach (ManagementObject obj in objCollection)
                {

                    if (obj.Properties["Status"].Value.ToString().ToLower().Equals("ok"))
                    {
                        break;
                    }
                    audioDivID++;

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InitAdioWizItems", exception,
                    Logger.LogLevel.Error);
            }



            //this.ComboBoxMicrophones = new ComboBox();
            //this.ComboBoxSpeakers = new ComboBox();
            ////
            //// ComboBoxMicrophones
            ////
            //this.ComboBoxMicrophones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.ComboBoxMicrophones.FormattingEnabled = true;
            //this.ComboBoxMicrophones.Location = new System.Drawing.Point(90, 42);
            //this.ComboBoxMicrophones.Name = "ComboBoxMicrophones";
            //this.ComboBoxMicrophones.Size = new System.Drawing.Size(308, 23);
            //this.ComboBoxMicrophones.TabIndex = 49;


            ////
            //// ComboBoxSpeakers
            ////
            //this.ComboBoxSpeakers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.ComboBoxSpeakers.FormattingEnabled = true;
            //this.ComboBoxSpeakers.Location = new System.Drawing.Point(90, 16);
            //this.ComboBoxSpeakers.Name = "ComboBoxSpeakers";
            //this.ComboBoxSpeakers.Size = new System.Drawing.Size(308, 23);
            //this.ComboBoxSpeakers.TabIndex = 48;

            //this.ComboBoxMicrophones.SelectedIndexChanged += (s, e) =>
            //{
            //    try
            //    {
            //        phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
            //        selectedMic = ComboBoxMicrophones.SelectedItem.ToString();

            //    }
            //    catch (Exception exception)
            //    {
            //        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxMicrophones.SelectedIndexChanged", exception, Logger.LogLevel.Error);
            //    }
            //};


            //this.ComboBoxSpeakers.SelectedIndexChanged += (s, e) =>
            //{
            //    try
            //    {
            //        phoneController.setAudioDeviceId(ComboBoxMicrophones.SelectedIndex, ComboBoxSpeakers.SelectedIndex);
            //        selectedSpeaker = ComboBoxSpeakers.SelectedItem.ToString();
            //    }
            //    catch (Exception exception)
            //    {
            //        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ComboBoxSpeakers.SelectedIndexChanged", exception, Logger.LogLevel.Error);
            //    }
            //};


        }

        private void InitializeError(string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("phoneController_OnInitializeError : {0} , statusCode: {1}, SipRegisterTryCount : {2}", statusText, statusCode, 0), Logger.LogLevel.Error);
                _sipLogined = false;
                Dispatcher.Invoke(() =>
                { textBlockIdentifier.Text = statusText; });
                _agent.AgentCurrentState.OnError(ref _agent, statusText, statusCode, "Unable to Communicate With Servers. Please Contact Your System Administrator.");

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "phoneController_OnInitializeError", exception,
                    Logger.LogLevel.Error);
            }
        }

        private void PlayRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ring tone", Logger.LogLevel.Info);
                Dispatcher.Invoke(() =>
                    { _wavPlayer.Play(); });
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>Start", Logger.LogLevel.Info);
                Dispatcher.Invoke(() =>
                { _wavPlayer.Stop(); });
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingTone", exception, Logger.LogLevel.Error);
            }
        }

        private void PlayRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ringIn tone", Logger.LogLevel.Info);
                //if (!playRingInToneMenually)
                //{
                //    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Start to play ringIn tone- No Audio Files.or disable", Logger.LogLevel.Error);
                //    return;
                //}
                //if (playingRingIntone)
                //    return;
                //if (_wavPlayerRingIn == null) return;
                //_wavPlayerRingIn.PlayLooping();

                //playingRingIntone = true;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone > End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "PlayRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void StopRingInTone()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>Start", Logger.LogLevel.Info);
                //playingRingIntone = false;
                //if (_wavPlayerRingIn == null) return;
                //_wavPlayerRingIn.Stop();
                //_wavPlayerRingIn.Dispose();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone>End", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "StopRingInTone", exception, Logger.LogLevel.Error);
            }
        }

        private void ReceveMeassge(string status, string fullMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ReceveMeassge-> status : {0} , DialPadEventArgs: {1} , Agent State : {2} , Agent Mode : {3} , CallSessionId : {4}", status, fullMessage, _agent.AgentCurrentState, _agent.AgentMode, _call.CallSessionId), Logger.LogLevel.Info);


                if (String.IsNullOrEmpty(fullMessage)) return;
                if (!fullMessage.Contains(',')) return;
                var splitData = fullMessage.Split(',');
                var msgString = splitData.First().ToUpper();

                if (msgString != "SESSIONCREATED") return;
                var sessionId = splitData[1];
                _call.CallSessionId = sessionId;
                _agent.CallSessionId = sessionId;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ReceveMeassge", exception, Logger.LogLevel.Error);
            }
        }

        private string GetString(byte[] bytes)
        {
            return System.Text.Encoding.Default.GetString(bytes);
        }

        #region SIPCallbackEvents

        public int onRegisterSuccess(int callbackIndex, int callbackObject, string statusText, int statusCode, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", Logger.LogLevel.Info);

                _sipLogined = true;
                _agent.SipStatus = false;

                mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Phone Initialized.", ToolTipIcon.Info);
                _call = new Call(string.Empty, this);
                _agent.AgentCurrentState.OnLogin(ref _agent);

                Dispatcher.Invoke(new Action(() =>
                {

                    GrdCallButton.Visibility = Visibility.Visible;
                    GrdDailpad.Visibility = Visibility.Visible;
                    textBlockRegStatus.Text = "Online";
                    textBlockIdentifier.Text = _agent.Profile.displayName;
                    textBlockCallStateInfo.Text = "Idle";
                    textBlockDialingNumber.Text = "0000000000";

                    TrackBarSpeaker.Value = _phoneController.getSpeakerVolume();

                    //TrackBarMicrophone.SetRange(0, 255);
                    TrackBarMicrophone.Value = _phoneController.getMicVolume();

                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Mic : {0}, Spk : {1}", TrackBarMicrophone.Value, TrackBarSpeaker.Value), Logger.LogLevel.Info);
                }));
                _agent.AgentMode = AgentMode.Offline;
                mynotifyicon.ShowBalloonTip(1000, "FaceTone", "Phone Initialized.", ToolTipIcon.Info);
                _call = new Call(string.Empty, this);
                _agent.AgentCurrentState.OnLogin(ref _agent);
                webSocketlistner.SendMessageToClient(CallFunctions.Initialized);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterFailure(int callbackIndex, int callbackObject, string statusText, int statusCode, StringBuilder sipMessage)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onRegisterFailure. statusText: {0}, statusCode: {1}", statusText, statusCode), Logger.LogLevel.Info);
            _agent.SipStatus = false;
            _sipLogined = false;
            InitializeError(statusText, statusCode);
            webSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
            return 0;
        }

        public int onInviteIncoming(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onInviteIncoming. caller : {0} Agent State : {1}, Call State : {2}", caller, _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Info);

                if (_agent.AgentCurrentState.GetType() != typeof(AgentIdle))
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("Call receive in Invalid Agent State.. caller : {0}, Agent State : {1}", caller, _agent.AgentCurrentState), Logger.LogLevel.Error);
                    _agent.AgentCurrentState = new AgentIdle();
                }
                if (_call.CallCurrentState.GetType() != typeof(CallIdleState))
                    _call.CallCurrentState = new CallIdleState();

                _agent.AgentCurrentState.OnIncomingCall(ref _agent, caller, sessionId);
                _agent.PortsipSessionId = sessionId;

                _call.PhoneNo = caller.Split('@')[0].Replace("sip:", "");
                _call.CallSessionId = _call.PhoneNo;
                _call.SetDialInfo(sessionId, Guid.NewGuid());
                _call.CallCurrentState.OnIncoming(ref _call, callbackIndex, callbackObject, sessionId, calleeDisplayName, caller, calleeDisplayName, callee, audioCodecNames, videoCodecNames, existsAudio, existsVideo);
                _call.currentCallLogId = Guid.NewGuid();

                textBlockDialingNumber.Dispatcher.Invoke(((MethodInvoker)(() =>
                {
                    textBlockDialingNumber.Text = _call.PhoneNo;
                })));
                PlayRingTone();
                if (AutoAnswerEnable)
                {
                    Task.Delay(AutoAnswerDelay).ContinueWith(_ =>
                    {
                        MakeCall(_call.PhoneNo);
                    }
                    );
                }
                dynamic expando = new JObject();
                expando.number = _call.PhoneNo;
                webSocketlistner.SendMessageToClient(CallFunctions.IncomingCall, expando);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteIncoming", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", Logger.LogLevel.Info);
                PlayRingInTone();
                _call.CallCurrentState.OnMakeCall(ref _call);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteSessionProgress(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames, bool existsEarlyMedia, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteSessionProgress", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }


        public int onInviteRinging(int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", Logger.LogLevel.Info);
                SetStatusMessage(statusText);
                _call.CallCurrentState.OnRinging(ref _call, callbackIndex, callbackObject, sessionId, statusText, statusCode);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteAnswered(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", Logger.LogLevel.Info);
                StopRingTone();
                StopRingInTone();


                _call.CallCurrentState.OnAnswer(ref _call);

                isCallAnswerd = true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", Logger.LogLevel.Info);

                StopRingInTone();
                StopRingTone();
                SetStatusMessage("Call Rejected from Other End" + reason);
                _call.CallCurrentState.OnCallReject(ref _call);
                _agent.AgentCurrentState.OnFailMakeCall(ref _agent);

                isCallAnswerd = false;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteUpdated(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteUpdated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onInviteConnected(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteConnected", Logger.LogLevel.Info);

                StopRingTone();
                StopRingInTone();
                SetStatusMessage("Call Established");
                _call.CallCurrentState.OnAnswer(ref _call);

                isCallAnswerd = true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteBeginingForward(int callbackIndex, int callbackObject, string forwardTo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", Logger.LogLevel.Info);
                SetStatusMessage("Call Begining Forward");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteClosed(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", Logger.LogLevel.Info);

                StopRingInTone();
                StopRingTone();
                _call.CallCurrentState.OnDisconnected(ref _call);
                _agent.AgentCurrentState.OnEndCall(ref _agent, isCallAnswerd);
                isCallAnswerd = false;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteHold(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteHold", Logger.LogLevel.Info);
                SetStatusMessage("Remote Hold");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRemoteUnHold(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteUnHold", Logger.LogLevel.Info);
                SetStatusMessage("In-Call");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onReceivedRefer(int callbackIndex, int callbackObject, int sessionId, int referId, string to, string from, StringBuilder referSipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRefer", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferAccepted(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferAccepted", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReferRejected(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferRejected", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferTrying", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onTransferRinging(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferRinging", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferSuccess(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onACTVTransferFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, String.Format("onReceivedSignaling : {0}", signaling), Logger.LogLevel.Info);
                using (StringReader reader = new StringReader(signaling.ToString()))
                {
                    var readText = "";
                    var isInvite = false;
                    while ((readText = reader.ReadLine()) != null)
                    {
                        if (!isInvite)
                        {
                            if (readText.StartsWith("INVITE"))
                            {
                                isInvite = true;
                            }
                            else
                            {
                                return 0;
                            }
                        }


                        if (!readText.StartsWith("X-session")) continue;
                        var data = readText.Split(':');
                        if (data.Length < 1) return 0;
                        _agent.CallSessionId = data[1].Trim();
                        _call.CallSessionId = data[1].Trim();
                        return 0;
                    }
                }
                //foreach (var prop in signaling.GetType().GetProperties())
                //{
                //    if (prop.GetIndexParameters().Length == 0)
                //    {
                //        Console.Write("{0}: {1:N0}    ", prop.Name, prop.GetValue(signaling));
                //        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, String.Format("onReceivedSignaling ---------- {0}: {1:N0}    ", prop.Name, prop.GetValue(signaling)), Logger.LogLevel.Info);
                //    }
                //}
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, String.Format("onReceivedSignaling : {0}", signaling), Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendingSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingSignaling", Logger.LogLevel.Info);

                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingVoiceMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingVoiceMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onWaitingFaxMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount,
            int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingFaxMessage", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvDtmfTone(int callbackIndex, int callbackObject, int sessionId, int tone)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvDtmfTone", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvOptions(int callbackIndex, int callbackObject, StringBuilder optionsMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOptions", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvInfo(int callbackIndex, int callbackObject, StringBuilder infoMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvInfo", Logger.LogLevel.Info);
                ReceveMeassge("Receive Information", infoMessage.ToString());

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onPresenceRecvSubscribe(int callbackIndex, int callbackObject, int subscribeId, string fromDisplayName, string @from,
            string subject)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceRecvSubscribe", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOnline(int callbackIndex, int callbackObject, string fromDisplayName, string @from, string stateText)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOnline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPresenceOffline(int callbackIndex, int callbackObject, string fromDisplayName, string @from)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOffline", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvMessage(int callbackIndex, int callbackObject, int sessionId, string mimeType, string subMimeType,
            byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onRecvOutOfDialogMessage(int callbackIndex, int callbackObject, string fromDisplayName, string @from,
            string toDisplayName, string to, string mimeType, string subMimeType, byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOutOfDialogMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = GetString(messageData);
                    ReceveMeassge("Receive Information", mesageText);

                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            } return 0;
        }

        public int onSendMessageSuccess(int callbackIndex, int callbackObject, int sessionId, int messageId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendMessageFailure(int callbackIndex, int callbackObject, int sessionId, int messageId, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageSuccess(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageSuccess", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSendOutOfDialogMessageFailure(int callbackIndex, int callbackObject, int messageId, string fromDisplayName,
            string @from, string toDisplayName, string to, string reason, int code)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayAudioFileFinished(int callbackIndex, int callbackObject, int sessionId, string fileName)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayAudioFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onPlayVideoFileFinished(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayVideoFileFinished", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onReceivedRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            return 0;
            /*try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }*/
        }

        public int onSendingRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            /*try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingRtpPacket", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }*/
            return 0;
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength,
            int samplingFreqHz)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onAudioRawCallback", Logger.LogLevel.Info);
                AUDIOSTREAM_CALLBACK_MODE type = (AUDIOSTREAM_CALLBACK_MODE)callbackType;

                if (type == AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_LOCAL_PER_CHANNEL)
                {
                    // The callback data is from local record device of each session, use the sessionId to identifying the session.
                }
                else if (type == AUDIOSTREAM_CALLBACK_MODE.AUDIOSTREAM_REMOTE_PER_CHANNEL)
                {
                    // The callback data is received from remote side of each session, use the sessionId to identifying the session.
                }




                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }



        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data,
            int dataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoRawCallback", Logger.LogLevel.Info);
                VIDEOSTREAM_CALLBACK_MODE type = (VIDEOSTREAM_CALLBACK_MODE)callbackType;

                if (type == VIDEOSTREAM_CALLBACK_MODE.VIDEOSTREAM_LOCAL)
                {

                }
                else if (type == VIDEOSTREAM_CALLBACK_MODE.VIDEOSTREAM_REMOTE)
                {

                }


                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onDialogStateUpdated(int callbackIndex, int callbackObject, string BLFMonitoredUri, string BLFDialogState, string BLFDialogId, string BLFDialogDirection)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onDialogStateUpdated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onDialogStateUpdated", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onRecvNotifyOfSubscription(int callbackIndex, int callbackObject, int subscribeId, StringBuilder notifyMsg, byte[] contentData, int contentLenght)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvNotifyOfSubscription", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvNotifyOfSubscriptions", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSubscriptionFailure(int callbackIndex, int callbackObject, int subscribeId, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionFailure", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionFailure", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onSubscriptionTerminated(int callbackIndex, int callbackObject, int subscribeId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionTerminated", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionTerminated", exception, Logger.LogLevel.Error);
                return -1;
            }
        }

        public int onVideoDecoderCallback(IntPtr callbackObject, int sessionId, int width, int height, int framerate, int bitrate)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoDecoderCallback", Logger.LogLevel.Info);
                return 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoDecoderCallback", exception, Logger.LogLevel.Error);
                return -1;
            }
        }
        #endregion SIPCallbackEvents


        #endregion

        #region UI State


        public void ShowCallLogs()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowCallLogs", Logger.LogLevel.Debug);
        }

        public void ShowSetting()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowSetting", Logger.LogLevel.Debug);
        }

        public void InAgentIdleState()
        {
            try
            {
                //_agent.CallSessionId = string.Empty;
                _agent.PortsipSessionId = -1;
                _agent.IsCallAnswer = false;
                _call.CallSessionId = string.Empty;
                _call.portSipSessionId = -1;
                _call.PhoneNo = string.Empty;

                Dispatcher.Invoke(new Action(() =>
                {


                    waitPanel.Visibility = Visibility.Hidden;
                    GrdCallButton.Visibility = Visibility.Visible;
                    GrdDailpad.Visibility = Visibility.Visible;
                    buttonAnswer.IsEnabled = true;
                    buttonReject.IsEnabled = true;


                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    buttonDialPad.Visibility = Visibility.Hidden;
                    textBlockCallStateInfo.Text = "IDLE";
                    textBlockDialingNumber.Text = String.Empty;

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;

                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;


                }));

                Task.Delay(1000).ContinueWith(_ =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        _callDurations.Stop();
                        _callDurations.Enabled = false;
                        textBlockCallStateInfo.Text = "IDLE";
                    }));
                }
                    );

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger7, string.Format("in Agent Idle CallSessionId set to Empty . {0}", _agent.CallSessionId), Logger.LogLevel.Debug);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InIdleState", exception, Logger.LogLevel.Error);
            }
        }


        public void InCallConnectedState()
        {
            try
            {


                Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Visible;
                    GrdDailpad.Visibility = Visibility.Hidden;


                    textBlockCallStateInfo.Text = "IN-CALL";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = true;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = true;
                    buttonEtl.IsEnabled = true;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = true;

                }));

                _agent.IsCallAnswer = true;
                if (_call.CallPrvState.GetType() == typeof(CallHoldState))
                {
                    return;
                }
                _callStarTime = DateTime.Now;
                _callDurations.Enabled = true;
                _callDurations.Start();
                /*Task.Delay(10000).ContinueWith(_ =>
                    {                        
                        _callDurations.Enabled = true;
                        _callDurations.Start();

                        Dispatcher.Invoke(new Action(() => { buttonReject.IsEnabled = true; }));
                    }
                );*/

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConnectedState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InOfflineState(string statusText, string msg, int statusCode)
        {

            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    GrdDailpad.Visibility = Visibility.Hidden;


                    textBlockCallStateInfo.Text = "OFFLINE";
                    textBlockDialingNumber.Text = String.Empty;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InOfflineState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InInitiateState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    GrdDailpad.Visibility = Visibility.Visible;


                    GrdCallButton.Visibility = Visibility.Visible;
                    textBlockCallStateInfo.Text = "";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateState", exception, Logger.LogLevel.Error);
            }
        }

        public void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName)
        {
            Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    GrdDailpad.Visibility = Visibility.Hidden;


                    textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                }));

            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateMsgState", Logger.LogLevel.Debug);
        }

        public void Error(string statusText)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Error" + statusText, Logger.LogLevel.Error);
        }

        public void InBreakState()
        {
            try
            {

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InBreakState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentClintConnectedState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Visible;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = true;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = true;
                    buttonEtl.IsEnabled = true;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = true;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentClintConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallAgentSupConnectedState(CallActions callAction)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = true;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = true;
                    buttonEtl.IsEnabled = true;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = true;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallAgentSupConnectedState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallConferenceState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = true;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = true;
                    buttonEtl.IsEnabled = true;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = true;
                }));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallDisconnectedState()
        {
            try
            {



                Dispatcher.Invoke(new Action(() =>
                {
                    _callDurations.Stop();
                    _callDurations.Enabled = false;
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                    textBlockCallStateInfo.Text = "DISCONNECTED";
                    _phoneController.muteMicrophone(false);
                    _phoneController.muteSpeaker(false);
                    picMic.Visibility = Visibility.Hidden;
                    picSpek.Visibility = Visibility.Hidden;
                }));
                webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallHoldState(CallActions callAction)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Unhold";
                    buttonHold.IsEnabled = true;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = true;
                    buttonEtl.IsEnabled = true;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallHoldState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallIdleState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //GrdAcw.Visibility=Visibility.Hidden;
                    //textBlockCallStateInfo.Text = "";
                    buttonDialPad.Visibility = Visibility.Hidden;
                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                    textBlockCallStateInfo.Text = "IDLE";

                    _callDurations.Stop();
                    _callDurations.Enabled = false;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallIdleState", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void InAgentBusy(CallDirection callDirection)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    var val = callDirection == CallDirection.Incoming;
                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = val;
                    buttonReject.IsEnabled = val;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InAgentBusy", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallRingingState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Hidden;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                    textBlockCallStateInfo.Text = "RINGING";
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallRingingState", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallTryingState()
        {
            InCallRingingState();
        }

        public void InCallIncommingState()
        {
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    GrdDailpad.Visibility = Visibility.Visible;


                    textBlockCallStateInfo.Text = "Incoming Call";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = true;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                }));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallIncommingState", exception, Logger.LogLevel.Error);
            }
        }

        public void OnResourceModeChanged(AgentMode mode)
        {
            try
            {
                switch (mode)
                {
                    case AgentMode.Offline:
                        break;
                    case AgentMode.Inbound:
                        {

                        }
                        break;
                    case AgentMode.Outbound:
                        {

                        }

                        break;
                    default:
                        throw new ArgumentOutOfRangeException("mode");
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnResourceModeChanged", exception, Logger.LogLevel.Error);
            }
        }


        #endregion









    }
}
