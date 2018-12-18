using Controllers.CallStatus;
using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class CallAgentSupConnectedState : CallState
    {
        public override void OnAnswering(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswered(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswerFail(Call call, string reason)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnHold(Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnUnHold(Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnNoAnswer(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnReset(Call call)
        {
            call.CallCurrentState = new CallConnectedState();
        }

        public override void OnDisconnecting(Call call)
        {
            try { call.CallCurrentState = new CallDisconnectingState(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnected(Call call, string reason)
        {
            try { call.CallCurrentState = new CallDisconnectedState(reason); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnectFail(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTransferReq(Call call, Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTranferFail(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOperationFail(Call call)
        {
            try
            {
                switch (call.CallCurrentState.CallAction)
                {
                    case CallActions.Error:
                        break;
                    case CallActions.Call_Swap_Requested:
                    case CallActions.Call_Swap_InProgress:
                    case CallActions.Call_Swap:
                        call.CallCurrentState = new CallAgentClintConnectedState();
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

        public override void OnSwapReq(Call call, CallActions callAction)
        {
            CallAction = callAction;
        }

        public override void OnCallReject(Call call, string reason)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndLinkLine(Call call, Phone phone)
        {
            try
            {
                phone.ETLCall();
                call.CallCurrentState = new CallConnectedState();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnEndLinkLine", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRinging(Call call, int callbackIndex, int callbackObject, int sessionId,
            string statusText, int statusCode)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnInviteTrying()
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnIncoming(Call call, int sessionId, string callerDisplayName, string caller, string calleeDisplayName,
            string callee)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnTimeout(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCallSession(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConference(Call call, Phone phone)
        {
            try { call.CallCurrentState = new CallConferenceState(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConferenceFail(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSetStatus(Call call)
        {
            try
            {
                switch (CallAction)
                {
                    case CallActions.Call_Swap_Requested:
                    case CallActions.Call_Swap_InProgress:
                    case CallActions.Call_Swap:
                        call.CallCurrentState = new CallAgentClintConnectedState();

                        break;
                    case CallActions.Conference_Call_Requested:
                    case CallActions.Conference_Call_InProgress:
                    case CallActions.Conference_Call:
                        call.CallCurrentState = new CallConferenceState();
                        break;
                    case CallActions.ETL_Requested:
                    case CallActions.ETL_InProgress:
                    case CallActions.ETL_Call:
                        call.CallCurrentState = new CallConnectedState();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnSetStatus", exception, Logger.LogLevel.Error); }

        }

        public override void OnSendDTMF(Call call, int val)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSessinCreate(Call call, string sessionId)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }
    }
}