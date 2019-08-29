using Controllers.CallStatus;
using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using Newtonsoft.Json.Linq;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public abstract class CallState
    {
        public string Reason { get; set; }
        public CallActions CallAction { get; set; }
        public abstract void OnAnswering(Call call);
        public abstract void OnAnswered(Call call);
        public abstract void OnAnswerFail(Call call, string reason);
        public abstract void OnHold(Call call, CallActions callAction);
        public abstract void OnUnHold(Call call, CallActions callAction);
        public abstract void OnNoAnswer(Call call);
        public abstract void OnReset(Call call);
        public abstract void OnDisconnecting(Call call);
        public abstract void OnDisconnected(Call call, string reason);
        public abstract void OnDisconnectFail(Call call);
        public abstract void OnTransferReq(Call call, Phone phone);
        public abstract void OnTranferFail(Call call);
        public abstract void OnOperationFail(Call call);
        public abstract void OnSwapReq(Call call, CallActions callAction);
        public abstract void OnCallReject(Call call, string reason);
        public abstract void OnEndLinkLine(Call call, Phone phone);
        public abstract void OnMakeCall(Call call);
        public abstract void OnRinging(Call call, int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode);
        public abstract void OnInviteTrying();

        public abstract void OnIncoming(Call call, int sessionId,
            string callerDisplayName,
            string caller, string calleeDisplayName, string callee, JArray call_data);
        public abstract void OnTimeout(Call call);
        public abstract void OnEndCallSession(Call call);
        public abstract void OnCallConference(Call call, Phone phone);
        public abstract void OnCallConferenceFail(Call call);
        public abstract void OnSetStatus(Call call);
        public abstract void OnSendDTMF(Call call, int val);
        public abstract void OnSessinCreate(Call call, string sessionId);
    }
}