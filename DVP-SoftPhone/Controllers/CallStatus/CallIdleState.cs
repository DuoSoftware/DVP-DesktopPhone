﻿using Controllers.CallStatus;
using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.CallStatus
{
    public class CallIdleState : CallState
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
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnecting(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnected(Call call, string reason)
        {
            try { call.CallCurrentState = new CallDisconnectedState(reason); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnDisconnectFail(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
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
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSwapReq(Call call, CallActions callAction)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(Call call, string reason)
        {
            try
            {
                call.CallCurrentState = new CallDisconnectedState(reason);
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndLinkLine(Call call, Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(Call call)
        {
            try { call.CallCurrentState = new CallTryingState(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnRinging(Call call, int callbackIndex, int callbackObject, int sessionId,
            string statusText, int statusCode)
        {
            call.CallCurrentState = new CallRingingState();
        }

        public override void OnInviteTrying()
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }


        public override void OnIncoming(Call call, int sessionId, string callerDisplayName, string caller, string calleeDisplayName,
            string callee)
        {
            try
            {
                var phone = Phone.Instance;
                phone.PortsipSessionId = sessionId;

                call.PhoneNo = caller.Split('@')[0].Replace("sip:", "");
                call.CallSessionId = call.PhoneNo;
                call.portSipSessionId = sessionId;
                call.currentCallLogId = Guid.NewGuid();
                call.CallCurrentState = new CallIncommingState();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "OnIncoming", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnTimeout(Call call)
        {
            try { call.CallCurrentState = new CallIdleState(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCallSession(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConference(Call call, Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallConferenceFail(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSetStatus(Call call)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnSendDTMF(Call call, int val)
        {

        }

        public override void OnSessinCreate(Call call, string sessionId)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }
    }
}