using DuoSoftware.DuoSoftPhone.Controllers.Common;
using System.Windows.Forms;


namespace DuoSoftware.DuoSoftPhone.Controllers
{

    public interface IUiState
    {
        void OnOperationModeChange(OperationMode mode);
        void ShowStatusMessage(string message);
        void ShowMessage(string message, ToolTipIcon type);
        void ShowCallLogs();
        void ShowSetting();
        void OnPhoneRegistered();
        void InPhoneIdleState();//void InRiggingState();

        void InOfflineState();
        void InInitiateState();
        void InPhoneInitializing();
        void InSettingPage();
        void InitializeError(string statusText, int statusCode);
        void loadDevices();
        void Error(string statusText);
        void InBreakState();
        void InCallConnectedState();
        void InCallAgentClintConnectedState();
        void InCallAgentSupConnectedState(CallActions callAction);
        void InCallConferenceState();
        void InCallDisconnectedState(string reason);
        void InCallDisconnectingState();
        void InCallHoldState(CallActions callAction);
        void InCallIdleState();
        void InCallRingingState();
        void InCallTryingState();
        void InCallIncommingState(string phoneNo);
        void OnResourceModeChanged(OperationMode value);

        void InAgentBusy(CallDirection callDirection);
        void InCallMuteUnmute(bool isMute);
        void OnCallAnswering();
        void SetPhoneNumber(string number);
    }


}
