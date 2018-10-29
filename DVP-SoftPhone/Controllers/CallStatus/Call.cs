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

using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;

#region References

using System;

#endregion

#region Content

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    
    public class Call
    {
        #region Local Variables

        private CallState _callCurrentState;
        private CallState _callPvState;
        public Guid currentCallLogId;
        
        #endregion

        #region Properties

        private IUiState UiState { get; set; }

        

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
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, string.Format("Call Current States [{0}] To [{1}]",_callPvState,value),  Logger.LogLevel.Info);
                ChangeUI(value);
            }
        }

        public string PhoneNo { get; set; }
        public string CallSessionId { get; set; }
        
        #endregion

        #region Constructors

        public Call(string phoneNo, IUiState uiState)
        {
            UiState = uiState;
            PhoneNo = phoneNo;
            CallCurrentState = new CallIdleState();
            
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

                new ComMethods.SwitchOnType<CallState>(state)
                    .Case<CallAgentClintConnectedState>(initiate => UiState.InCallAgentClintConnectedState())
                    .Case<CallAgentSupConnectedState>(i => UiState.InCallAgentSupConnectedState(state.CallAction))
                    .Case<CallConferenceState>(b => UiState.InCallConferenceState())
                     .Case<CallConnectedState>(b => UiState.InCallConnectedState())
                     .Case<CallIncommingState>(b => UiState.InCallIncommingState())
                      .Case<CallDisconnectedState>(b => UiState.InCallDisconnectedState())
                        .Case<CallHoldState>(b => UiState.InCallHoldState(state.CallAction))
                         .Case<CallIdleState>(b => UiState.InCallIdleState())
                         //.Case<CallRingingState>(b => UiState.InCallRingingState())
                         .Case<CallTryingState>(b => UiState.InCallTryingState())
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

        public void SetDialInfo(int portsipSessionId, Guid newGuid)
        {
            currentCallLogId = newGuid;
            portSipSessionId = portsipSessionId;
        }

        #endregion

        #endregion

       
    }
}

#endregion