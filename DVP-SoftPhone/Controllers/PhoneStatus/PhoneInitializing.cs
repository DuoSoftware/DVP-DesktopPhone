using Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers;
using DuoSoftware.DuoSoftPhone.Controllers.CallStatus;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Threading.Tasks;

namespace Controllers.PhoneStatus
{
    public class PhoneInitializing : PhoneState
    {
        public PhoneInitializing(Phone phone, IUiState uiState)
        {
            phone.UiState = uiState;
        }
        public override void OnLogin(Phone phone, IUiState uiState)
        {

            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }

        }

        public override void OnLoggedOn(Phone phone)
        {
            try
            {
                phone.phoneCurrentState = new PhoneOnline();
                phone.OprationMode = OperationMode.Inbound;
                Call.Instance.CallCurrentState = new CallIdleState();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", exception, Logger.LogLevel.Error);
            }

        }

        public override void OnLoggingError(Phone phone, string statusText, int statusCode)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("phoneController_OnInitializeError : {0} , statusCode: {1}, SipRegisterTryCount : {2}", statusText, statusCode, 0), Logger.LogLevel.Error);
                phone.UiState.InitializeError(statusText, statusCode);
                phone.WebSocketlistner.SendMessageToClient(CallFunctions.InitializFail);
                phone.phoneCurrentState = new PhoneOffline();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "OnLoggingError", exception, Logger.LogLevel.Error);
            }


        }

        public override void OnLogOff(Phone phone)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnOffline(Phone phone, string statusText)
        {
            try { phone.phoneCurrentState = new PhoneOffline(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }

        public override void OnInitializing(Phone phone)
        {
            try
            {
                if (VeerySetting.Instance.AgentConsoleintegration)
                {
                    phone.UiState.InPhoneInitializing();
                    phone.InitializePhone(false);
                }
                else
                {
                    var data = FileHandler.ReadUserData();
                    if (data == null)
                    {
                        phone.UiState.InSettingPage();
                        //data = FileHandler.WriteUserData("9502", "DuoS123", "duo.media1.veery.cloud");
                        data = FileHandler.ReadUserData();
                    }
                    phone.GenarateSipProfile(data.GetValue("name").ToString(), data.GetValue("password").ToString(), data.GetValue("domain").ToString(), Convert.ToInt16(data.GetValue("Delay").ToString()) * 1000);
                    phone.InitializePhone(false);
                    phone.phoneCurrentState.OnLoggedOn(phone);
                }
                
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "PhoneInitializing", exception, Logger.LogLevel.Error); }
        }

        public override void OnInitializeError(Phone phone, string statusText, int statusCode)
        {
            phone.UiState.InitializeError(statusText, statusCode);
            phone.phoneCurrentState = new PhoneOffline();
        }

        public override void OnAnswering(Phone phon)
        {
            try { throw new NotImplementedException("Invalid Call Status."); }catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger3, "", exception, Logger.LogLevel.Error); }
        }
    }
}
