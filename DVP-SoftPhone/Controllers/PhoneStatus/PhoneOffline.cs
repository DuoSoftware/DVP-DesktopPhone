using DuoSoftware.DuoSoftPhone.Controllers;
using System;
using DuoSoftware.DuoTools.DuoLogger;

namespace Controllers.PhoneStatus
{
    class PhoneOffline : PhoneState
    {
        public override void OnLogin(Phone phone, IUiState uiState)
        {
            phone.phoneCurrentState = new PhoneInitializing(phone, uiState);
            phone.phoneCurrentState.OnInitializing(phone);
        }

        public override void OnLoggedOn(Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLoggingError(Phone phone, string statusText, int statusCode)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogOff(Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOffline(Phone phone, string statusText)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnInitializing(Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnInitializeError(Phone phone, string statusText, int statusCode)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswering(Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }


    }
}
