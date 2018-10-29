using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;


namespace DuoSoftware.DuoSoftPhone.Controllers
{
    
    public interface IUiState
    {
        
        void ShowCallLogs();
        void ShowSetting();
        void InAgentIdleState();
        void InAgentAcwState();
        //void InRiggingState();
        void InCallConnectedState();
        void InOfflineState(string statusText, string msg,int statusCode);
        void InInitiateState();
        void InInitiateMsgState(bool autoAnswerchk, bool autoAnswerEnb, string userName);
        void Error(string statusText);
        void InBreakState();
        void InCallAgentClintConnectedState();
        void InCallAgentSupConnectedState(CallActions callAction);
        void InCallConferenceState();
        void InCallDisconnectedState();
        void InCallHoldState(CallActions callAction);
        void InCallIdleState();
        void InCallRingingState();
        void InCallTryingState();
        void InCallIncommingState();
        void OnResourceModeChanged(AgentStatus.AgentMode value);
        
        void InAgentBusy(CallDirection callDirection);
    }

   
}
