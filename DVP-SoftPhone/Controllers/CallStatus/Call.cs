#region About

/*
Object				: 
Purpose             : 
Developed By		: Rajinda waruna
Developed On		: Oct 29, 2010
Modified By		    : 
Last Updated 		: 
Notes				: 
*/

#endregion

using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;
using System;

#region References

#endregion

#region Content

namespace Controllers.CallStatus
{

    public class Call
    {
        #region Local Variables
        private static volatile Call instance;
        private static object syncRoot = new Object();

        private CallState _callCurrentState;
        private CallState _callPvState;
        public Guid currentCallLogId;

        #endregion

        #region Properties

        public WebSocketServiceHost WebSocketlistner { get; private set; }
        public IUiState UiState { get; protected internal set; }



        public int portSipSessionId { get; set; }

        public CallState CallPrvState
        {
            get
            {
                return _callPvState;
            }
            set
            {
                _callPvState = value;

            }
        }

        public CallState CallCurrentState
        {
            get
            {
                return _callCurrentState;
            }
            set
            {
                _callPvState = _callCurrentState;
                _callCurrentState = value;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, string.Format("Call Current States [{0}] To [{1}]", _callPvState, value), Logger.LogLevel.Info);
                ChangeUI(value);
            }
        }

        public string PhoneNo { get; set; }
        public string CallSessionId { get; set; }
        public JArray call_data { get; set; }


        #endregion

        #region Constructors

        public Call()
        {
            CallCurrentState = new CallinitiateState();
            UiState = Phone.Instance.UiState;
            WebSocketlistner = Phone.Instance.WebSocketlistner;
        }

        #endregion

        #region Interface Implementations


        #endregion

        #region Events

        #region Phone events

        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void ChangeUI(CallState state)
        {
            try
            {
                WebSocketlistner = Phone.Instance.WebSocketlistner;
                new ComMethods.SwitchOnType<CallState>(state)
                    .Case<CallAgentClintConnectedState>(initiate => UiState.InCallAgentClintConnectedState())
                    .Case<CallAgentSupConnectedState>(i => UiState.InCallAgentSupConnectedState(state.CallAction))
                    .Case<CallConferenceState>(b =>
                    {
                        var setting = VeerySetting.Instance;
                        Phone.Instance.SendDtmf(setting.ConferenceCode);
                        UiState.InCallConferenceState();
                        WebSocketlistner.SendMessageToClient(CallFunctions.ConfCall);
                    })
                     .Case<CallConnectedState>(b =>
                    {
                        UiState.InCallConnectedState();
                        UiState.ShowStatusMessage("Call Established");
                    })
                     .Case<CallIncommingState>(b =>
                    {

                        UiState.InCallIncommingState(PhoneNo);

                        try
                        {
                            dynamic expando1 = new JObject();
                            expando1.Number = PhoneNo;


                            //JArray jarrayObj = new JArray();
                            //foreach (string parameterName in call_data)
                            //{
                            //    jarrayObj.Add(parameterName);
                            //}

                            expando1.veery_data = call_data;
                            WebSocketlistner.SendMessageToClient(CallFunctions.ReciveCallInfo, expando1);
                        }
                        catch (Exception exception)
                        {
                            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "ReciveCallInfo", exception, Logger.LogLevel.Error);
                        }

                        dynamic expando = new JObject();
                        expando.number = PhoneNo;
                        WebSocketlistner.SendMessageToClient(CallFunctions.IncomingCall, expando);

                    })
                      .Case<CallDisconnectingState>(b =>
                    {
                        Phone.Instance.EndCall();
                        UiState.InCallDisconnectingState();
                        this.CallCurrentState = new CallDisconnectedState("Callee Disconnected");

                    }).Case<CallDisconnectedState>(b =>
                    {
                        UiState.InCallDisconnectedState(this.CallCurrentState.Reason);
                        WebSocketlistner.SendMessageToClient(CallFunctions.EndCall);
                        this.CallCurrentState = new CallIdleState();
                    })
                        .Case<CallHoldState>(b => UiState.InCallHoldState(state.CallAction))
                         .Case<CallIdleState>(b =>
                        UiState.InCallIdleState()
                        )
                    .Case<CallRingingState>(b =>
                    {
                        UiState.InCallRingingState();
                        WebSocketlistner.SendMessageToClient(CallFunctions.MakeCall);
                    })
                         .Case<CallTryingState>(b =>
                    {
                        UiState.InCallTryingState();
                        WebSocketlistner.SendMessageToClient(CallFunctions.MakeCall);
                    }).Case<CallAnsweringState>(b =>
                    {
                        UiState.OnCallAnswering();
                    })
                    .Default<CallIdleState>(t => UiState.InCallIdleState());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "Call ChangeUI", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #region protected Methods

        #endregion

        #region Internal

        #endregion

        #region Public Methods
        public static Call Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Call();
                    }
                }

                return instance;
            }
        }

        #endregion

        #endregion


    }
}

#endregion