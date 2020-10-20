#region Ref
/*
 * Auth : CodeMax
 * Email : codemax699@gmail.com
 * 
 * 
 * 
 * 
 */
#endregion


#region Using
using Controllers.PhoneStatus;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion
namespace Controllers.CallStatus
{
    #region Class
    internal class CallinitiateState : CallState
    {
        #region private method

        #endregion
        #region interfae
        public override void OnAnswering(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnAnswered(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnAnswerFail(Call call, string reason)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnHold(Call call, CallActions callAction)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnUnHold(Call call, CallActions callAction)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnNoAnswer(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnReset(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnDisconnecting(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnDisconnected(Call call, string reason)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnDisconnectFail(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnTransferReq(Call call, Phone phone)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnTranferFail(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnOperationFail(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnSwapReq(Call call, CallActions callAction)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnCallReject(Call call, string reason)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnEndLinkLine(Call call, Phone phone)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnMakeCall(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnRinging(Call call, int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnInviteTrying()
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnIncoming(Call call, int sessionId, string callerDisplayName, string caller, string calleeDisplayName, string callee, JArray call_data)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }
        public override void OnTimeout(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnEndCallSession(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnCallConference(Call call, Phone phone)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnCallConferenceFail(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnSetStatus(Call call)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnSendDTMF(Call call, int val)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

        public override void OnSessinCreate(Call call, string sessionId)
        {
            try
            {
                throw new NotImplementedException("Invalid Call Status.");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error);
            }
        }

       
        #endregion
        #region public method

        #endregion
    }
    #endregion

}

