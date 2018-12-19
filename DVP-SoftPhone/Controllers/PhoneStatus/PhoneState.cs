
using DuoSoftware.DuoSoftPhone.Controllers;

namespace Controllers.PhoneStatus
{
    public abstract class PhoneState
    {
        public abstract void OnLogin(Phone phone, IUiState uiState);
        public abstract void OnLoggedOn(Phone phone);
        public abstract void OnLoggingError(Phone phone, string statusText, int statusCode);
        public abstract void OnLogOff(Phone phone);
        public abstract void OnOffline(Phone phone, string statusText);
        public abstract void OnInitializing(Phone phone);
        public abstract void OnInitializeError(Phone phone, string statusText, int statusCode);
        public abstract void OnAnswering(Phone phone);

    }
}
