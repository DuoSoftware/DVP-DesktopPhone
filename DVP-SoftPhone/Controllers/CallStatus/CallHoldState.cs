using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class CallHoldState : CallState
    {


        public CallHoldState(CallActions callAction)
        {
            CallAction = callAction;
        }

        public override void OnAnswering(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswer(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnHold(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnUnHold(ref Call call, CallActions callAction)
        {
            CallAction = callAction;
            call.CallCurrentState = new CallConnectedState();
        }

        public override void OnNoAnswer(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnReset(ref Call call)
        {
            call.CallCurrentState = new CallConnectedState();
        }

        public override void OnDisconnected(ref Call call)
        {
            try { call.CallCurrentState = new CallDisconnectedState(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTransferReq(ref Call call, CallActions callAction)
        {
            CallAction = callAction;
        }

        public override void OnTranferFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOperationFail(ref Call call)
        {
            try
            {
                switch (call.CallCurrentState.CallAction)
                {
                    case CallActions.Error:
                        break;
                    case CallActions.Incomming_Call_Request:
                        break;
                    case CallActions.Call_InProgress:
                        break;
                    case CallActions.Call:
                        break;
                    case CallActions.Hold_Requested:
                    case CallActions.Hold_InProgress:
                    case CallActions.Hold:
                        break;
                    case CallActions.UnHold_Requested:
                    case CallActions.UnHold_InProgress:
                    case CallActions.UnHold:
                    case CallActions.Call_Transfer_Requested:
                    case CallActions.Call_Transfer_InProgress:
                    case CallActions.Call_Transferred:
                        call.CallCurrentState = new CallHoldState(CallActions.Hold);
                        break;
                    case CallActions.Call_Swap_Requested:
                        break;
                    case CallActions.Call_Swap_InProgress:
                        break;
                    case CallActions.Call_Swap:
                        break;
                    case CallActions.Conference_Call_Requested:
                    case CallActions.Conference_Call_InProgress:
                    case CallActions.Conference_Call:
                        call.CallCurrentState = new CallAgentClintConnectedState();
                        break;
                    case CallActions.ETL_Requested:
                    case CallActions.ETL_InProgress:
                    case CallActions.ETL_Call:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnOperationFail", exception, Logger.LogLevel.Error); }
        }

        public override void OnSwapReq(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndLinkLine(ref Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRinging(ref Call call, int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnIncoming(ref Call call, int callbackIndex, int callbackObject, int sessionId, string callerDisplayName,
            string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames,
            bool existsAudio, bool existsVideo)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTimeout(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCallSession(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConference(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConferenceFail(ref Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSetStatus(ref Call call)
        {
            try
            {
                switch (CallAction)
                {
                    case CallActions.Hold:
                    case CallActions.Hold_InProgress:
                    case CallActions.Hold_Requested:
                        call.CallCurrentState = new CallHoldState(CallAction);
                        break;
                    case CallActions.UnHold_Requested:
                    case CallActions.UnHold_InProgress:
                    case CallActions.UnHold:
                        call.CallCurrentState = new CallConnectedState();
                        break;
                    case CallActions.Call_Transfer_Requested:
                    case CallActions.Call_Transfer_InProgress:
                    case CallActions.Call_Transferred:
                        call.CallCurrentState = new CallAgentSupConnectedState();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnSetStatus", exception, Logger.LogLevel.Error); }
        }

        public override void OnSendDTMF(ref Call call, int val)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSessinCreate(ref Call call, string sessionId)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }
    }
}