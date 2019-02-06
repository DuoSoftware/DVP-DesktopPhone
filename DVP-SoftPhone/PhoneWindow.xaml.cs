using Controllers.CallStatus;
using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    public partial class PhoneWindow : Window, IUiState
    {
        private static Mutex mutex;
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private Phone _phone;
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
                var _call = Call.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Conference_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                _call.CallCurrentState.OnCallConference(_call, _phone);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "buttonConference_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonEtl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var _call = Call.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Etl_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);

                _call.CallCurrentState.OnEndLinkLine(_call, _phone);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void buttontransferCall_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var _call = Call.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("buttontransferCall_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
                if (textBlockDialingNumber.Text.Length <= 3)
                {
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Invalid Number.", ToolTipIcon.Error);
                    return;
                }
                _call.CallCurrentState.OnTransferReq(_call, _phone);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }


        }

        private void buttonMute_Click(object sender, RoutedEventArgs e)
        {
            _phone.MuteMicrophone();
        }

        private void buttonHold_Click(object sender, RoutedEventArgs e)
        {
            var _call = Call.Instance;
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Hold_Click-> Session Id : {0} , Status : {1}", _call.CallSessionId, _call.CallCurrentState), Logger.LogLevel.Info);
            _phone.HoldUnholdCall();
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


        private void buttonPickUp_Click(object sender, RoutedEventArgs e)
        {

            Dispatcher.Invoke(new Action(() =>
            {

                buttonAnswer.IsEnabled = false;
                /*if (string.IsNullOrEmpty(textBlockDialingNumber.Text))
                {
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Please Enter Number To Dial.", ToolTipIcon.Warning);
                    return;
                }

                if (_phone.OprationMode == OperationMode.Inbound)
                {
                    AnswerCall(sender,e);
                }
                else
                {
                    MakeCall(sender, e);
                }*/

                MakeCall(sender, e);
            }));


        }

        private void buttonHangUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var _call = Call.Instance;
                _call.CallCurrentState.OnDisconnecting(_call);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "buttonHangUp_Click", exception, Logger.LogLevel.Error);
            }
        }

        private void buttonKeyPadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var btn = sender as Button;
                if (btn == null)
                    return;

                textBlockDialingNumber.Text += btn.Content.ToString().Trim();
                _phone.SendDtmf(GetDtmfSignalFromButtonTag(btn));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "buttonKeyPadButton_Click", exception, Logger.LogLevel.Error);
            }

        }

        private void buttonKeyPad_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void buttonKeyPad_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private string GetDtmfSignalFromButtonTag(Button button)
        {
            if (button == null)
                return "-1";

            if (button.Tag == null)
                return "-1";

            int signal;
            if (int.TryParse(button.Tag.ToString(), out signal))
                return signal.ToString();

            return "-1";
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
                    _phone.AudioPlayLoopbackTest(true);
                }
                else
                {
                    ButtonTestAudio.ToolTip = "Test Audio";
                    _phone.AudioPlayLoopbackTest(false);
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
                _phone.SetSpeakerVolume(Convert.ToInt16(TrackBarSpeaker.Value));
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
                _phone.SetMicVolume(Convert.ToInt16(TrackBarMicrophone.Value));
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
                _phone.MuteSpeaker();
                picSpek.Visibility = val ? Visibility.Visible : Visibility.Hidden;
                if (val) return;
                var volume = _phone.SetSpeakerVolume(-1);
                TrackBarSpeaker.Value = volume;

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
                _phone.MuteMicrophone();
                picMic.Visibility = val ? Visibility.Visible : Visibility.Hidden;
                if (val) return;
                var volume = _phone.SetMicVolume(-1);
                TrackBarMicrophone.Value = volume;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "frmAudio.OnMicMute", exception,
                    Logger.LogLevel.Error);
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
            _phone.UninitializePhone();
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

        private void inboundMenu_Checked(object sender, RoutedEventArgs e)
        {
            Outbound.IsChecked = false;
            _phone.OprationMode = OperationMode.Inbound;
        }

        private void inboundMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            _phone.OprationMode = OperationMode.Offline;
        }

        private void outboundMenu_Checked(object sender, RoutedEventArgs e)
        {
            Inbound.IsChecked = false;
            _phone.OprationMode = OperationMode.Outbound;
        }

        private void outboundMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            _phone.OprationMode = OperationMode.Offline;
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

            _phone = Phone.Instance;

            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string filePath = string.Format("{0}{1}", appDataFolder, "\\veery\\ringtone.wav");

            _wavPlayer = (File.Exists(filePath)) ? new System.Media.SoundPlayer(filePath) : new System.Media.SoundPlayer(Properties.Resources.ringtone);

            _phone.phoneCurrentState.OnLogin(_phone, this);

            this.ShowInTaskbar = VeerySetting.Instance.ShowInTaskbar;

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

            m_notifyIcon = new NotifyIcon
            {
                BalloonTipText = @"Facetone Phone Has Been Minimised. Click The Tray Icon To Show.",
                BalloonTipTitle = @"Facetone Phone",
                Text = @"Facetone Phone",
                Icon = Properties.Resources.facetone_logo
            };
            m_notifyIcon.DoubleClick += new EventHandler(m_notifyIcon_Click);
            WindowState = WindowState.Minimized;

            settingMenuItem.Visibility = VeerySetting.Instance.AgentConsoleintegration ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
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

        #region UI State

        public void InitializeError(string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("phoneController_OnInitializeError : {0} , statusCode: {1}, SipRegisterTryCount : {2}", statusText, statusCode, 0), Logger.LogLevel.Error);
                _sipLogined = false;
                Dispatcher.Invoke(() =>
                    { textBlockIdentifier.Text = statusText; });

                mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Unable to Communicate With Servers. Please Contact Your System Administrator.", ToolTipIcon.Error);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "phoneController_OnInitializeError", exception,
                    Logger.LogLevel.Error);
            }
        }

        public void loadDevices()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "loadDevices", Logger.LogLevel.Info);
                //InitAdioWizItems();
                Dispatcher.Invoke(() =>
                {
                    ComboBoxSpeakers.Items.Clear();
                    ComboBoxMicrophones.Items.Clear();
                    int num = _phone.getNumOfPlayoutDevices();
                    for (int i = 0; i < num; ++i)
                    {
                        StringBuilder deviceName = new StringBuilder();
                        deviceName.Length = 256;

                        if (_phone.getPlayoutDeviceName(i) == 0)
                        {
                            ComboBoxSpeakers.Items.Add(deviceName.ToString());
                        }
                        ComboBoxSpeakers.SelectedIndex = 0;
                    }

                    /*if (ComboBoxSpeakers.Items.Count > 0)
                        ComboBoxSpeakers.SelectedIndex = 0;*/

                    num = _phone.getNumOfRecordingDevices();
                    for (int i = 0; i < num; ++i)
                    {
                        var deviceName = new StringBuilder { Length = 256 };

                        if (_phone.getRecordingDeviceName(i) == 0)
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
                    TrackBarSpeaker.Value = _phone.getSpeakerVolume();

                    //TrackBarMicrophone.SetRange(0, 255);
                    TrackBarMicrophone.Value = _phone.getMicVolume();

                    Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Mic : {0}, Spk : {1}", TrackBarMicrophone.Value, TrackBarSpeaker.Value), Logger.LogLevel.Info);
                });

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "loadDevices", exception, Logger.LogLevel.Error);
            }
        }

        public void OnOperationModeChange(OperationMode mode)
        {
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    /*switch (mode)
                    {
                        case OperationMode.Offline:
                            break;
                        case OperationMode.Inbound:
                            this.buttonAnswer.Click += AnswerCall;
                            break;
                        case OperationMode.Outbound:
                            this.buttonAnswer.Click += MakeCall;
                            break;
                        case OperationMode.initiate:
                            break;

                    }*/

                }));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowMassage", exception, Logger.LogLevel.Error);
            }
        }




        public void ShowStatusMessage(string message)
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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowMassage", exception, Logger.LogLevel.Error);
            }
        }

        public void ShowMessage(string message, ToolTipIcon type)
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    textBlockCallStateInfo.Text = message;
                    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", message, type);
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowMessage", exception, Logger.LogLevel.Error);
            }
        }

        public void ShowCallLogs()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowCallLogs", Logger.LogLevel.Debug);
        }

        public void ShowSetting()
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ShowSetting", Logger.LogLevel.Debug);
        }

        public void OnPhoneRegistered()
        {
            Dispatcher.Invoke(new Action(() =>
            {

                GrdCallButton.Visibility = Visibility.Visible;
                GrdDailpad.Visibility = Visibility.Visible;
                textBlockRegStatus.Text = "Online";
                textBlockIdentifier.Text = _phone._SipProfile.DisplayName;
                textBlockCallStateInfo.Text = "Idle";
                textBlockDialingNumber.Text = "0000000000";

                TrackBarSpeaker.Value = _phone.getSpeakerVolume();

                //TrackBarMicrophone.SetRange(0, 255);
                TrackBarMicrophone.Value = _phone.getMicVolume();
                mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Phone Initialized.", ToolTipIcon.Info);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("Mic : {0}, Spk : {1}", TrackBarMicrophone.Value, TrackBarSpeaker.Value), Logger.LogLevel.Info);
            }));

        }

        public void InPhoneIdleState()
        {
            try
            {
                var _call = Call.Instance;
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

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger7, string.Format("in Phone Idle CallSessionId set to Empty . {0}", _phone.PhoneSessionId), Logger.LogLevel.Debug);

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

                var _call = Call.Instance;
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

        public void InOfflineState()
        {

            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    GrdCallFunctions.Visibility = Visibility.Hidden;
                    GrdDailpad.Visibility = Visibility.Hidden;


                    textBlockCallStateInfo.Text = "OFFLINE";
                    textBlockDialingNumber.Text = "0000000000";
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
                    textBlockCallStateInfo.Text = "Standby";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;

                    Inbound.IsChecked = _phone.OprationMode == OperationMode.Inbound;
                    Outbound.IsChecked = _phone.OprationMode == OperationMode.Outbound;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InInitiateState", exception, Logger.LogLevel.Error);
            }
        }

        public void InPhoneInitializing()
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

        public void InSettingPage()
        {
            try
            {
                Dispatcher.Invoke(new Action(() => { SettingMenuItem_Click(null, null); }));
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InSettingPage", Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InSettingPage", exception, Logger.LogLevel.Error);
            }



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
                return; // disble till call server send trnasfer state in sip message
                Dispatcher.Invoke(new Action(() =>
                {
                    //GrdCallFunctions.Visibility = Visibility.Visible;
                    //GrdDailpad.Visibility = Visibility.Hidden;
                    //
                    //textBlockCallStateInfo.Text = "INITIALIZING";

                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
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
                return; // disble till call server send trnasfer state in sip message
                Dispatcher.Invoke(new Action(() =>
                {
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
                return; // disble till call server send trnasfer state in sip message
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

        public void InCallDisconnectingState()
        {
            try
            {

                StopRingTone();
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
                    textBlockCallStateInfo.Text = "Disconnecting";
                    picMic.Visibility = Visibility.Hidden;
                    picSpek.Visibility = Visibility.Hidden;
                }));

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallConferenceState", exception, Logger.LogLevel.Error);
            }
        }
        public void InCallDisconnectedState(string reason)
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
                    textBlockCallStateInfo.Text = string.IsNullOrEmpty(reason) ? "DISCONNECTED" : reason;
                    picMic.Visibility = Visibility.Hidden;
                    picSpek.Visibility = Visibility.Hidden;
                }));

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
                StopRingTone();
                Dispatcher.Invoke(new Action(() =>
                {
                    this.buttonAnswer.Click -= MakeCall;
                    this.buttonAnswer.Click -= AnswerCall;
                    this.buttonAnswer.Click += MakeCall;
                    buttonDialPad.Visibility = Visibility.Hidden;
                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = true;
                    buttonReject.IsEnabled = true;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                    textBlockCallStateInfo.Text = "IDLE";
                    textBlockDialingNumber.Text = "";
                    _callDurations.Stop();
                    _callDurations.Enabled = false;
                    textBlockCallStateInfo.Text = "IDLE";
                    
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

        public void InCallMuteUnmute(bool isMute)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                buttonMute.Content = isMute ? "mute" : "Unmute";
                isMute = !isMute;
                picMic.Visibility = isMute ? Visibility.Visible : Visibility.Hidden;
            }));
        }

        public void OnCallAnswering()
        {
            try
            {
                StopRingTone();
                Dispatcher.Invoke(new Action(() =>
                {
                    buttonHold.Content = "Hold";
                    buttonHold.IsEnabled = false;
                    buttonAnswer.IsEnabled = false;
                    buttonReject.IsEnabled = false;
                    buttontransferIvr.IsEnabled = false;
                    buttontransferCall.IsEnabled = false;
                    buttonEtl.IsEnabled = false;
                    buttonswapCall.IsEnabled = false;
                    buttonConference.IsEnabled = false;
                    textBlockCallStateInfo.Text = "ANSWERING";
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "OnCallAnswering", exception, Logger.LogLevel.Error);
            }
        }

        public void SetPhoneNumber(string number)
        {
            try
            {

                Dispatcher.Invoke(new Action(() =>
                {
                    textBlockDialingNumber.Text = number;
                    Call.Instance.PhoneNo = number;
                }));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "setPhoneNumber", exception, Logger.LogLevel.Error);
            }
        }

        public void InCallRingingState()
        {
            try
            {
                Dispatcher.Invoke(new Action(() =>
                {
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

        public void InCallIncommingState(string phoneNo)
        {
            try
            {
                PlayRingTone();
                Dispatcher.Invoke(new Action(() =>
                {
                    this.buttonAnswer.Click -= MakeCall;
                    this.buttonAnswer.Click -= AnswerCall;
                    this.buttonAnswer.Click += AnswerCall;
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
                    textBlockDialingNumber.Text = phoneNo;
                }));




            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InCallIncommingState", exception, Logger.LogLevel.Error);
            }
        }

        public void OnResourceModeChanged(OperationMode mode)
        {
            try
            {
                switch (mode)
                {
                    case OperationMode.Offline:
                        break;
                    case OperationMode.Inbound:
                        {

                        }
                        break;
                    case OperationMode.Outbound:
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

        #region private methods

        private void AnswerCall(object sender, RoutedEventArgs e)
        {
            _phone.AnswerCall();
        }
        private void MakeCall(object sender, RoutedEventArgs e)
        {
            _phone.MakeCall(textBlockDialingNumber.Text);
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
        #endregion




    }
}
