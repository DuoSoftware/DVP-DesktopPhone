using DuoSoftware.DuoSoftPhone.Controllers;
using System;
using System.Threading.Tasks;
using DuoSoftware.DuoTools.DuoLogger;

namespace Controllers.PhoneStatus
{
    class PhoneOnline : PhoneState
    {
        public override void OnLogin(Phone phone, IUiState uiState)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
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
            try
            {
                phone.phoneCurrentState=new PhoneOffline();
            }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
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
            try
            {
                if (phone.AutoAnswerEnable)
                {
                    Task.Delay(phone.AutoAnswerDelay).ContinueWith(_ =>
                        {
                            phone.AnswerCall();
                        }
                    );
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
