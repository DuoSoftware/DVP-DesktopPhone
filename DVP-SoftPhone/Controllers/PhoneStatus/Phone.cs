
using Controllers.CallStatus;
using DuoCallTesterLicenseKey;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using PortSIP;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

namespace Controllers.PhoneStatus
{
    public sealed class Phone
    {

        #region private property



        private PhoneState _phoneCurrentState;
        private NameValueCollection settingObject;



        private PortSIPLib _phoneController;
        private static volatile Phone instance;
        private static object syncRoot = new Object();
        private bool _isMuteMicrophone;
        private bool _isMuteSpeaker;
        public SipProfile _SipProfile { get; private set; }
        #endregion

        #region public property

        internal int AutoAnswerDelay = 10000;
        internal bool AutoAnswerEnable { get; set; }

        public WebSocketServiceHost WebSocketlistner { get; private set; }
        public string PhoneSessionId { get; private set; }
        public IUiState UiState { get; protected internal set; }
        public int acwTime { private set; get; }

        public OperationMode OprationMode { get; set; }

        public PhoneState phoneCurrentState
        {
            get { return _phoneCurrentState; }
            set
            {
                try
                {
                    _phoneCurrentState = value;
                    ChangeUI(value);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "phoneCurrentState", exception, Logger.LogLevel.Error);
                }
            }
        }

        #endregion

        #region private methods

        private Phone()
        {
            try
            {
                PhoneSessionId = Guid.NewGuid().ToString();
                _phoneCurrentState = new PhoneOffline();
                settingObject = (NameValueCollection)ConfigurationManager.GetSection("VeerySetting");
                OprationMode = OperationMode.Offline;

                if (VeerySetting.Instance.WebSocketlistnerEnable)
                {
                    Task.Delay(10000).ContinueWith(_ =>
                        {
                            InitiateWebSocket();
                        }
                    );


                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "VeerySetting", exception, Logger.LogLevel.Error);
            }
        }

        private void ChangeUI(PhoneState state)
        {
            try
            {
                new ComMethods.SwitchOnType<PhoneState>(state)
                    .Case<PhoneInitializing>(initiate =>
                    {
                        UiState.InInitiateState();
                    })
                    .Case<PhoneOffline>(i =>
                    {
                        UninitializePhone();
                        UiState.InOfflineState();
                        WebSocketlistner.SendMessageToClient(CallFunctions.Unregistor);
                    })
                    .Case<PhoneOnline>(b =>
                    {
                        UiState.OnPhoneRegistered();
                        WebSocketlistner.SendMessageToClient(CallFunctions.Initialized);
                    });
                //.Default<PhoneState>(t => UiState.ShowStatusMassage());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent ChangeUI", exception, Logger.LogLevel.Error);
            }
        }
        private bool IsValidIP(params object[] list)
        {
            try
            {
                var addr = list[0].ToString();
                //create our match pattern
                //            const string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
                //    ([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

                const string pattern = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";
                //create our Regular Expression object
                var check = new Regex(pattern);
                //boolean variable to hold the status
                bool valid = false;
                //check to make sure an ip address was provided
                valid = addr != "" && check.IsMatch(addr, 0);
                //return the results
                return valid;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "IsValidIP", exception, Logger.LogLevel.Error);
                //Console.WriteLine(exception.Message);
                return false;
            }
        }
        private string GetLocalIpAddress()
        {
            try
            {
                string myHost = Dns.GetHostName();

                string myIp = (from ipAddress in Dns.GetHostEntry(myHost).AddressList
                               where ipAddress.IsIPv6LinkLocal == false
                               where ipAddress.IsIPv6Multicast == false
                               where ipAddress.IsIPv6SiteLocal == false
                               where ipAddress.IsIPv6Teredo == false
                               select ipAddress).Select(ipAddress => ipAddress.ToString()).FirstOrDefault();

                if (!IsValidIP(myIp))
                {
                    IPAddress[] myIp1 = Dns.GetHostAddresses(myHost);
                    foreach (IPAddress ipAddress in
                        myIp1.Where(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork))
                    {
                        myIp = ipAddress.ToString();
                        break;
                    }
                }

                return myIp;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetLocalIPAddress", exception,
                    Logger.LogLevel.Error);
                return string.Empty;
            }
        }

        private void initAutioCodecs()
        {
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_OPUS);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_SPEEX);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_ISACWB);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMA);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_PCMU);
            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_G729);


            _phoneController.addAudioCodec(AUDIOCODEC_TYPE.AUDIOCODEC_DTMF); // For RTP event - DTMF (RFC2833)
        }

        #region phone function

        public void AudioPlayLoopbackTest(bool value)
        {
            try
            {
                _phoneController.audioPlayLoopbackTest(value);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "audioPlayLoopbackTest", exception, Logger.LogLevel.Error);
            }
        }
        public int SetMicVolume(int volume)
        {
            try
            {
                if (volume < 0)
                    volume = _phoneController.getMicVolume();
                _phoneController.setMicVolume(volume);
                return volume;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "setMicVolume", exception, Logger.LogLevel.Error);
            }

            return 0;
        }

        public int SetSpeakerVolume(int volume)
        {
            try
            {
                if (volume < 0)
                    volume = _phoneController.getSpeakerVolume();
                _phoneController.setSpeakerVolume(volume);
                return volume;

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SetSpeakerVolume", exception, Logger.LogLevel.Error);
            }

            return 0;
        }

        public void MuteSpeaker()
        {
            try
            {
                _phoneController.muteSpeaker(!_isMuteSpeaker);
                _isMuteSpeaker = !_isMuteSpeaker;
                //WebSocketlistner.SendMessageToClient(val ? CallFunctions. : CallFunctions.UnmuteCall);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MuteUnmute", exception, Logger.LogLevel.Error);
            }
        }

        public void MuteMicrophone()
        {
            try
            {
                _phoneController.muteMicrophone(!_isMuteMicrophone);
                this.UiState.InCallMuteUnmute(_isMuteMicrophone);
                _isMuteMicrophone = !_isMuteMicrophone;
                WebSocketlistner.SendMessageToClient(_isMuteMicrophone ? CallFunctions.MuteCall : CallFunctions.UnmuteCall);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MuteUnmute", exception, Logger.LogLevel.Error);
            }
        }

        public void HoldUnholdCall()
        {
            try
            {
                var status = string.Empty;
                int res = -1;
                var _call = Call.Instance;
                if (_call.CallCurrentState.GetType() == typeof(CallConnectedState))
                {
                    res = _phoneController.hold(_call.portSipSessionId);
                    if (res == 0)
                    {
                        _call.CallCurrentState.OnHold(_call, CallActions.Hold);
                        status = "Hold Call";
                    }
                    WebSocketlistner.SendMessageToClient(CallFunctions.HoldCall);

                }
                else if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                {
                    res = _phoneController.unHold(_call.portSipSessionId);
                    if (res == 0)
                    {
                        _call.CallCurrentState.OnUnHold(_call, CallActions.UnHold);
                        status = "Connected";
                    }
                    WebSocketlistner.SendMessageToClient(CallFunctions.UnholdCall);
                }
                if (!string.IsNullOrEmpty(status))
                {
                    UiState.ShowStatusMessage(status);
                }


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "HoldUnholdCall", exception, Logger.LogLevel.Error);
            }
        }

        public void SendDtmf(string digitsSet)
        {
            try
            {
                var setting = VeerySetting.Instance;
                foreach (var chr in digitsSet.ToCharArray())
                {
                    var digit = setting.DtmfValues[chr];
                    if (digit < 0)
                        continue;
                    var reply = _phoneController.sendDtmf(Call.Instance.portSipSessionId, DTMF_METHOD.DTMF_RFC2833, Convert.ToInt16(digit), 160, true);//DTMF_RFC2833
                    Console.WriteLine(reply);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "SendDTMF", exception, Logger.LogLevel.Error);
            }
        }

        public void EndCall()
        {
            try
            {
                var _call = Call.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("End call. Agent Status : [{0}], Call Status : [{1}]", this.phoneCurrentState, _call.CallCurrentState), Logger.LogLevel.Info);

                var status = "Call Ended";
                int reply;
                /*if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallTryingState))
                {
                    reply = _phoneController.hangUp(_call.portSipSessionId);

                    /*if (this.OprationMode == OperationMode.Outbound)
                    {
                        reply = _phoneController.hangUp(_call.portSipSessionId);
                    }
                    else
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger9, string.Format("End call[486]. Agent Status : [{0}], Call Status : [{1}]", this.phoneCurrentState, _call.CallCurrentState), Logger.LogLevel.Debug);
                        reply = _phoneController.rejectCall(_call.portSipSessionId, 486);
                        status = "Call Rejected";
                    }#1#

                }
                else
                    reply = _phoneController.hangUp(_call.portSipSessionId);*/


                if (_call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    reply = _phoneController.rejectCall(_call.portSipSessionId, 486);
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger9, string.Format("Reject Call. Agent Status : [{0}], Call Status : [{1}]", this.phoneCurrentState, _call.CallCurrentState), Logger.LogLevel.Debug);
                }
                else
                {
                    reply = _phoneController.hangUp(_call.portSipSessionId);
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger9, string.Format("hangUp. Agent Status : [{0}], Call Status : [{1}]", this.phoneCurrentState, _call.CallCurrentState), Logger.LogLevel.Debug);
                }



                if (reply == 0)
                {
                    _call.CallCurrentState.OnDisconnecting(_call);
                }
                else
                {
                    _call.CallCurrentState.OnDisconnectFail(_call);
                }

                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("End call. Agent Status : [{0}], Call Status : [{1}] , status : [{2}]", this.phoneCurrentState, _call.CallCurrentState, status), Logger.LogLevel.Info);
                if (_isMuteMicrophone)
                    MuteMicrophone();
                // webSocketlistner.SendMessageToClient(CallFunctions.EndCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EndCall", exception, Logger.LogLevel.Error);
            }
        }

        public void AnswerCall()
        {
            try
            {
                Console.WriteLine(@"calling answer......");

                var _call = Call.Instance;
                if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallIncommingState))
                {
                    Console.WriteLine(@"calling answer...... true");
                    var isAnswer = _phoneController.answerCall(_call.portSipSessionId, false);
                    if (isAnswer >= 0)
                    {
                        _call.CallCurrentState.OnAnswering(_call);
                    }
                    else
                    {
                        _call.CallCurrentState.OnAnswerFail(_call, "Fail To Answer");
                    }
                }
                else
                {
                    Console.WriteLine(@"calling answer...... false");
                    //mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Answer Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                    UiState.ShowStatusMessage("Fail to Answer Call");
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}", this.phoneCurrentState, _call.CallCurrentState), Logger.LogLevel.Error);

                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        public void MakeCall(string no)
        {
            try
            {

                if (this.phoneCurrentState.GetType() == typeof(PhoneOnline) && this.OprationMode == OperationMode.Outbound)
                {
                    if (!string.IsNullOrEmpty(no))
                    {
                        var call = Call.Instance;
                        call.CallCurrentState.OnMakeCall(call);
                        call.portSipSessionId = _phoneController.call(no, true, false);
                        if (call.portSipSessionId >= 0) return;
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "MakeCall-Fail", Logger.LogLevel.Error);
                        call.CallCurrentState.OnTimeout(call);
                        return;

                    }
                    else
                    {
                        UiState.ShowMessage("Invalid Phone Number.", ToolTipIcon.Error);
                    }
                }
                else
                {
                    UiState.ShowMessage("Invalid Phone State or operation Mode.", ToolTipIcon.Error);
                    WebSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                }
                //else if (_call.CallCurrentState.GetType() == typeof(CallRingingState) || _call.CallCurrentState.GetType() == typeof(CallIncommingState))
                //{
                //    SetStatusMessage("Answering");
                //    _call.CallCurrentState.OnAnswering(ref _call);
                //    _phoneController.answerCall(_call.portSipSessionId, false);
                //    if (WebSocketlistner != null)
                //        WebSocketlistner.SendMessageToClient(CallFunctions.AnswerCall);
                //}
                //else if (_agent.AgentCurrentState.GetType() == typeof(AgentIdle) && _agent.AgentMode == AgentMode.Inbound && _call.CallCurrentState.GetType() == typeof(CallIdleState))
                //{
                //    //if (!AutoAnswer.Checked)
                //    //{
                //    //    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone",
                //    //        "Fail to Make Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                //    //}
                //    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Make Call.\nPlease change Mode to Outbound.", ToolTipIcon.Warning);
                //    SetStatusMessage("Fail to Make Call");
                //    if (WebSocketlistner != null)
                //        WebSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("MakeCall-Fail. AgentCurrentState: {0}, CallCurrentState: {1}", _agent.AgentCurrentState, _call.CallCurrentState), Logger.LogLevel.Error);

                //}
                //else
                //{
                //    mynotifyicon.ShowBalloonTip(1000, "FaceTone - Phone", "Fail to Make Call.", ToolTipIcon.Warning);
                //    WebSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                //    SetStatusMessage("Fail to Make Call");
                //}



            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }

        public void ConferenceCall()
        {
            try
            {
                var setting = VeerySetting.Instance;
                SendDtmf(setting.ConferenceCode);
                WebSocketlistner.SendMessageToClient(CallFunctions.ConfCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }
        }
        public void ETLCall()
        {
            try
            {
                var setting = VeerySetting.Instance;
                this.SendDtmf(setting.EtlCode);
                WebSocketlistner.SendMessageToClient(CallFunctions.EtlCall);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MakeCall", exception, Logger.LogLevel.Error);
            }

        }
        public void TransferCall(string number, string callSessionId, string callCurrentState)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("transferCall_Click-> Session Id : {0} , Status : {1}", callSessionId, callCurrentState), Logger.LogLevel.Info);
                if (!String.IsNullOrEmpty(number))
                {
                    var setting = VeerySetting.Instance;
                    var tranNo = number.Trim();

                    var dtmfSet = tranNo.Length <= 5 ? setting.TransferExtCode : setting.TransferPhnCode;

                    SendDtmf(dtmfSet);

                    Thread.Sleep(1000);
                    tranNo = string.Format("{0}#", tranNo);
                    SendDtmf(tranNo);
                    WebSocketlistner.SendMessageToClient(CallFunctions.TransferCall);
                }

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        private void TransferIVR(string tranNo, string callSessionId, string callCurrentState)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("TransferIVR-> Session Id : {0} , Status : {1}", callSessionId, callCurrentState), Logger.LogLevel.Info);
                if (!String.IsNullOrEmpty(tranNo))
                {
                    var setting = VeerySetting.Instance;

                    var dtmfSet = setting.TransferExtCode;
                    SendDtmf(dtmfSet);
                    Thread.Sleep(1000);
                    tranNo = string.Format("{0}#", tranNo);
                    SendDtmf(tranNo);
                    WebSocketlistner.SendMessageToClient(CallFunctions.TransferIVR);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        private void RegistorPhone(string message)
        {
            try
            {
                if (!VeerySetting.Instance.AgentConsoleintegration) return;

                UiState.ShowStatusMessage(". . .Registering. . .");
                var callInfo = message.Split('-'); // "name-password-domain"
                if (callInfo.Length < 3 || string.IsNullOrEmpty(callInfo[0]) || callInfo[0] == "undefined" ||
                    string.IsNullOrEmpty(callInfo[1]) || callInfo[1] == "undefined" ||
                    string.IsNullOrEmpty(callInfo[2]) || callInfo[2] == "undefined")
                {
                    WebSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
                    return;
                }

                UninitializePhone();
                var domain = callInfo[2];
                var password = callInfo[1];
                var username = callInfo[0];

                var _sipServerPort = Convert.ToInt32(settingObject["sipServerPort"]);

                var overridePort = settingObject["overridePort"].ToLower().Equals("true");
                if (overridePort)
                {                    
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
                                _sipServerPort = Convert.ToInt32(values[1]);
                            }
                        }
                    }
                    else if (callInfo[2].Contains(':'))
                    {
                        var values = domain.Split(':');
                        if (values.Any() && values.Length >= 2)
                        {
                            domain = values[0];
                            _sipServerPort = Convert.ToInt32(values[1]);
                        }
                    }
                }
                

                _SipProfile = new SipProfile()
                {
                    DisplayName = username,
                    UserName = username,
                    Password = password,
                    Domain = domain,
                    AuthorizationName = username,
                    localIPAddress = GetLocalIpAddress(),
                    SipServerPort = _sipServerPort,
                    PublicIdentity = username
                };

                if (this.phoneCurrentState.GetType() == typeof(PhoneOffline))
                {
                    this.phoneCurrentState.OnLogin(this, this.UiState);
                }
                else
                {
                    this.phoneCurrentState.OnLoggedOn(this);
                }

               // InitializePhone(true);
            }
            catch (Exception exception)
            {
                WebSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "RegistorPhone", exception, Logger.LogLevel.Error);

            }

        }

        private void UnregistorPhone(string message)
        {
            try
            {
                this.phoneCurrentState.OnOffline(this, "Offline");

                /*Dispatcher.Invoke(() =>
                {
                    textBlockIdentifier.Text = "Offline";
                    buttonAnswer.IsEnabled = false;
                    textBlockRegStatus.Text = "Offline";
                    textBlockCallStateInfo.Text = "Offline";
                    textBlockDialingNumber.Text = "0000000000";
                    _callDurations.Stop();
                    _callDurations.Enabled = false;
                });*/
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UnregistorPhone", exception, Logger.LogLevel.Error);
            }

        }

        public void UninitializePhone()
        {
            try
            {
                var callid = Call.Instance.portSipSessionId;
                _phoneController.hangUp(callid);
                _phoneController.rejectCall(callid, 486);
                _phoneController.unRegisterServer();
                _phoneController.unInitialize();
                _phoneController.releaseCallbackHandlers();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "UninitializePhone", exception, Logger.LogLevel.Error);
            }
        }


        #endregion

        #region Public methods
        public static Phone Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Phone();
                    }
                }

                return instance;
            }
        }

        public Int32 getNumOfPlayoutDevices()
        {
            try
            {
                return _phoneController.getNumOfPlayoutDevices();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getNumOfPlayoutDevices", exception, Logger.LogLevel.Error);
            }

            return 0;
        }
        public Int32 getNumOfRecordingDevices()
        {
            try
            {
                return _phoneController.getNumOfRecordingDevices();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getNumOfRecordingDevices", exception, Logger.LogLevel.Error);
            }

            return 0;
        }

        public int getPlayoutDeviceName(int deviveId, StringBuilder deviceName)
        {
            try
            {

                return _phoneController.getPlayoutDeviceName(deviveId, deviceName, 256);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getNumOfPlayoutDevices", exception, Logger.LogLevel.Error);
            }

            return 0;
        }
        public int getRecordingDeviceName(int deviveId, StringBuilder deviceName)
        {
            try
            {

                return _phoneController.getRecordingDeviceName(deviveId, deviceName, 256);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getNumOfPlayoutDevices", exception, Logger.LogLevel.Error);
            }

            return 0;
        }

        public int getMicVolume()
        {
            try
            {
                return _phoneController.getMicVolume();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getMicVolume", exception, Logger.LogLevel.Error);
                return 0;
            }
        }

        public int getSpeakerVolume()
        {
            try
            {
                return _phoneController.getSpeakerVolume();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "getSpeakerVolume", exception, Logger.LogLevel.Error);
                return 0;
            }
        }


        public void GenarateSipProfile(string username, string txtPassword, string domain, int delay)
        {
            _SipProfile = new SipProfile()
            {
                DisplayName = username,
                UserName = username,
                Password = txtPassword,
                Domain = domain,
                AuthorizationName = username,
                localIPAddress = GetLocalIpAddress(),
                SipServerPort = Convert.ToInt16(settingObject["sipServerPort"])
            };

            acwTime = 5;

            AutoAnswerDelay = delay;
        }

        public void InitializePhone(bool isReInit)
        {
            try
            {

                var userName = _SipProfile.AuthorizationName;
                var password = _SipProfile.Password;
                var displayName = _SipProfile.DisplayName;
                var authName = _SipProfile.AuthorizationName;
                Random rd = new Random();
                var localPort = string.IsNullOrEmpty(settingObject["localPort"]) ? (rd.Next(1000, 5000) + 4000) : (Convert.ToInt32(settingObject["localPort"]));
                var sipProfile = _SipProfile;
                sipProfile.SipServerPort = _SipProfile.SipServerPort > 0 ? _SipProfile.SipServerPort : Convert.ToInt16(settingObject["sipServerPort"]);
                var sipServer = _SipProfile.Domain;
                var localIp = _SipProfile.localIPAddress;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}", userName, authName, password, localPort), Logger.LogLevel.Info);

                int errorCode = 0;

                _phoneController = new PortSIPLib(0, 0, new SipCallbackEvents());
                _phoneController.createCallbackHandlers();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "createCallbackHandlers-end, call initialize method", Logger.LogLevel.Info);

                var rt = _phoneController.initialize(TRANSPORT_TYPE.TRANSPORT_UDP,
                    // Use 0.0.0.0 for local IP then the SDK will choose an available local IP automatically.
                    // You also can specify a certain local IP to instead of "0.0.0.0", more details please read the SDK User Manual
                    localIp,
                    localPort,
                    PORTSIP_LOG_LEVEL.PORTSIP_LOG_DEBUG,
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
                    this.phoneCurrentState.OnInitializeError(this, "failed.", 408);
                    return;
                }
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "initialize-end", Logger.LogLevel.Info);

                UiState.loadDevices();

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
                    _SipProfile.SipServerPort,
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
                        this.phoneCurrentState.OnInitializeError(this, "Fail", rt_userInfo);
                        return;
                    }
                }

                _phoneController.setSrtpPolicy(SRTP_POLICY.SRTP_POLICY_NONE, false);
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "setSrtpPolicy-end", Logger.LogLevel.Info);

                string licenseKey =  LicenseKeyHandler.GetLicenseKey("DuoS123");
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

                _phoneController.enableVAD(VeerySetting.Instance.enableVAD);
                _phoneController.enableAEC(VeerySetting.Instance.enableAEC);
                _phoneController.enableCNG(VeerySetting.Instance.enableCNG);
                _phoneController.enableAGC(VeerySetting.Instance.enableAGC);
                _phoneController.enableANS(VeerySetting.Instance.enableANS);
                _phoneController.enableReliableProvisional(VeerySetting.Instance.enableReliableProvisional);

                var rt_register = _phoneController.registerServer(3600, 3);

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "registerServer-end", Logger.LogLevel.Info);
                if (rt_register != 0)
                {
                    _phoneController.unInitialize();
                    _phoneController.releaseCallbackHandlers();
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, string.Format("userName : {0}, authName : {1}, password : {2}, localPort : {3}..............................Registration Failed. errorCode : {4}", userName, authName, password, localPort, rt_register), Logger.LogLevel.Info);
                    this.phoneCurrentState.OnInitializeError(this, "Fail", rt_register);
                    return;
                }

                _phoneController.setAudioDeviceId(0, 0);

                _phoneController.addSupportedMimeType("INFO", "text", "plain");


                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger1, "InitializePhone-end", Logger.LogLevel.Info);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InitializePhone", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region WebSocket Server
        public void InitiateWebSocket()
        {

            try
            {


                if (VeerySetting.Instance.WebSocketlistnerEnable)
                {
                    WebSocketlistner = new WebSocketServiceHost(VeerySetting.Instance.WebSocketlistnerPort, VeerySetting.Instance.WebSocketSslPassword, VeerySetting.Instance.WebSocketSslPath);

                    WebSocketServiceHost.OnRecive += (callFunction, no, othr) =>
                    {
                        try
                        {


                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                        string.Format(
                            "[webSocketlistner]External Application send Commands. callFunction : {0}, Phone No : {1}",
                            callFunction, no), Logger.LogLevel.Info);

                            if (VeerySetting.Instance.AgentConsoleintegration)
                            {
                                var _call = Call.Instance;
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
                                            UiState.SetPhoneNumber(no);
                                            MakeCall(no);
                                        }
                                        break;
                                    case CallFunctions.AnswerCall:
                                        AnswerCall();
                                        break;
                                    case CallFunctions.EndCall:
                                    case CallFunctions.RejectCall:
                                        _call.CallCurrentState.OnDisconnecting(_call);
                                        break;

                                    case CallFunctions.HoldCall:
                                        HoldUnholdCall();
                                        break;
                                    case CallFunctions.MuteCall:
                                        MuteMicrophone();
                                        break;
                                    case CallFunctions.EtlCall:
                                        _call.CallCurrentState.OnEndLinkLine(_call, this);
                                        break;
                                    case CallFunctions.ConfCall:
                                        _call.CallCurrentState.OnCallConference(_call, this);
                                        break;
                                    case CallFunctions.Inbound:
                                        this.OprationMode = OperationMode.Inbound;
                                        break;
                                    case CallFunctions.Outbound:
                                        this.OprationMode = OperationMode.Outbound;
                                        break;
                                    case CallFunctions.TransferIVR:
                                        {
                                            if (!string.IsNullOrEmpty(no))
                                            {
                                                if (_call.CallCurrentState.GetType() == typeof(CallHoldState))
                                                {
                                                    HoldUnholdCall();
                                                }
                                                UiState.SetPhoneNumber(no);
                                                this.TransferIVR(no, _call.CallSessionId, _call.CallCurrentState.ToString());
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
                                                UiState.SetPhoneNumber(no);
                                                _call.CallCurrentState.OnTransferReq(_call, this);
                                                //this.TransferCall(no, _call.CallSessionId, _call.CallCurrentState.ToString());
                                            }
                                        }
                                        break;

                                    case CallFunctions.EnableAutoAnswer:
                                        {
                                            this.AutoAnswerEnable = true;
                                            this.AutoAnswerDelay = Convert.ToInt16(no);
                                        }
                                        break;
                                    case CallFunctions.DisableAutoAnswer:
                                        {
                                            this.AutoAnswerEnable = false;
                                            this.AutoAnswerDelay = Convert.ToInt16(no);
                                        }
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException("callFunction");
                                }
                            }
                            else
                            {
                                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "InitializePhone", new Exception("command not implipented"), Logger.LogLevel.Error);
                            }

                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault,
                                "[webSocketlistner]FormDialPad_Load-OnSocketMessageRecive-Dispatcher", exception,
                                Logger.LogLevel.Error);
                        }
                    };

                    UiState.InInitiateState();
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

        }

        #endregion WebSocket Server
    }
}
