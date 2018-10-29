using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using PortSIP;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public abstract class CallState
    {
        public  CallActions CallAction { get; set; }
        public abstract void OnAnswer(ref Call call);
        public abstract void OnHold(ref Call call,CallActions callAction);
        public abstract void OnUnHold(ref Call call,CallActions callAction);
        public abstract void OnNoAnswer(ref Call call);
        public abstract void OnReset(ref Call call);
        public abstract void OnDisconnected(ref Call call);
        public abstract void OnTransferReq(ref Call call,CallActions callAction);
        public abstract void OnTranferFail(ref Call call);
        public abstract void OnOperationFail(ref Call call);
        public abstract void OnSwapReq(ref Call call,CallActions callAction);
        public abstract void OnCallReject(ref Call call);
        public abstract void OnEndLinkLine(ref Call call,CallActions callAction);
        public abstract void OnMakeCall(ref Call call);
        public abstract void OnRinging(ref Call call, int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode);
        public abstract void OnIncoming(ref Call call, int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller,
            string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio,
            bool existsVideo);
        public abstract void OnTimeout(ref Call call);
        public abstract void OnEndCallSession(ref Call call);
        public abstract void OnCallConference(ref Call call);
        public abstract void OnCallConferenceFail(ref Call call);
        public abstract void OnSetStatus(ref Call call);
        public abstract void OnSendDTMF(ref Call call,int val);
        public abstract void OnSessinCreate(ref Call call, string sessionId);
    }
}