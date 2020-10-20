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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortSIP;
using Controllers.CallStatus;
using Controllers.PhoneStatus;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.IO;
#endregion
namespace Controllers.CallStatus
{
    #region Class
    public class SipCallbackEvents : SIPCallbackEvents
    {
        #region private method
        private void ReceveMeassge(string status, string fullMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("ReceveMeassge-> status : {0} , DialPadEventArgs: {1} ,Phone Sttus : {2} Call State : {3} , Call Mode : {4} , CallSessionId : {5}", new object[]
                {
                    status,
                    fullMessage,
                    Phone.Instance.phoneCurrentState,
                    Call.Instance.CallCurrentState,
                    Phone.Instance.OprationMode,
                    Call.Instance.CallSessionId
                }), Logger.LogLevel.Info);
                if (!string.IsNullOrEmpty(fullMessage))
                {
                    if (fullMessage.Contains(','))
                    {
                        string[] splitData = fullMessage.Split(new char[]
                        {
                            ','
                        });
                        string msgString = splitData.First<string>().ToUpper();
                        if (!(msgString != "SESSIONCREATED"))
                        {
                            Call.Instance.CallSessionId = splitData[1];
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ReceveMeassge", exception, Logger.LogLevel.Error);
            }
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }
        #endregion
        #region interfae
        public int onRegisterSuccess(int callbackIndex, int callbackObject, string statusText, int statusCode, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", Logger.LogLevel.Info);
                Phone phone = Phone.Instance;
                phone.phoneCurrentState.OnLoggedOn(phone);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRegisterSuccess", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRegisterFailure(int callbackIndex, int callbackObject, string statusText, int statusCode, StringBuilder sipMessage)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onRegisterFailure. statusText: {0}, statusCode: {1}", statusText, statusCode), Logger.LogLevel.Info);
            Phone phone = Phone.Instance;
            phone.phoneCurrentState.OnLoggingError(phone, statusText, statusCode);
            return 0;
        }

        public int onInviteIncoming(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onInviteIncoming. caller : {0} Agent State : {1}, Call State : {2}", caller, Phone.Instance.phoneCurrentState, Call.Instance.CallCurrentState), Logger.LogLevel.Info);
                Call instance = Call.Instance;
                
                try
                {
                    var data = sipMessage.ToString().Split('\n');

                    JArray jarrayObj = new JArray();
                    foreach (string parameterName in data)
                    {
                       jarrayObj.Add(parameterName);
                    }
                    instance.CallCurrentState.OnIncoming(instance, sessionId, callerDisplayName, caller, calleeDisplayName, callee, jarrayObj);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events call_data", exception, Logger.LogLevel.Error);
                }
                
            }
            catch (Exception exception2)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteIncoming", exception2, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnInviteTrying();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteTrying", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteSessionProgress(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames, bool existsEarlyMedia, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteSessionProgress", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onInviteRinging(int callbackIndex, int callbackObject, int sessionId, string statusText, int statusCode, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnRinging(call, callbackIndex, callbackObject, sessionId, statusText, statusCode);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteRinging", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteAnswered(int callbackIndex, int callbackObject, int sessionId, string callerDisplayName, string caller, string calleeDisplayName, string callee, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnAnswered(call);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code, StringBuilder sipMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnCallReject(call, reason);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteFailure", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteUpdated(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo, StringBuilder sipMessage)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteUpdated", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onInviteConnected(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteConnected", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnAnswered(call);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteAnswered", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteBeginingForward(int callbackIndex, int callbackObject, string forwardTo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", Logger.LogLevel.Info);
                Call.Instance.UiState.ShowMessage("Call Begining Forward", ToolTipIcon.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteBeginingForward", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onInviteClosed(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", Logger.LogLevel.Info);
                Call call = Call.Instance;
                call.CallCurrentState.OnDisconnected(call, "Caller Disconnected");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onInviteClosed", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onDialogStateUpdated(int callbackIndex, int callbackObject, string BLFMonitoredUri, string BLFDialogState, string BLFDialogId, string BLFDialogDirection)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onDialogStateUpdated", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onDialogStateUpdated", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onRemoteHold(int callbackIndex, int callbackObject, int sessionId)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteHold", Logger.LogLevel.Info);
                Call.Instance.UiState.ShowStatusMessage("Remote Hold");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRemoteUnHold(int callbackIndex, int callbackObject, int sessionId, string audioCodecNames, string videoCodecNames, bool existsAudio, bool existsVideo)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRemoteUnHold", Logger.LogLevel.Info);
                Call.Instance.UiState.ShowStatusMessage("In-Call");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onReceivedRefer(int callbackIndex, int callbackObject, int sessionId, int referId, string to, string from, StringBuilder referSipMessage)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRefer", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onReferAccepted(int callbackIndex, int callbackObject, int sessionId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferAccepted", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onReferRejected(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReferRejected", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onTransferTrying(int callbackIndex, int callbackObject, int sessionId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferTrying", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onTransferRinging(int callbackIndex, int callbackObject, int sessionId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onTransferRinging", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onACTVTransferSuccess(int callbackIndex, int callbackObject, int sessionId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferSuccess", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onACTVTransferFailure(int callbackIndex, int callbackObject, int sessionId, string reason, int code)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onACTVTransferFailure", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onReceivedSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onReceivedSignaling : {0}", signaling), Logger.LogLevel.Info);
                using (StringReader reader = new StringReader(signaling.ToString()))
                {
                    bool isInvite = false;
                    string readText;
                    while ((readText = reader.ReadLine()) != null)
                    {
                        if (!isInvite)
                        {
                            if (!readText.StartsWith("INVITE"))
                            {
                                result = 0;
                                return result;
                            }
                            isInvite = true;
                        }
                        if (readText.StartsWith("X-session"))
                        {
                            string[] data = readText.Split(new char[]
                            {
                                ':'
                            });
                            if (data.Length < 1)
                            {
                                result = 0;
                                return result;
                            }
                            Call.Instance.CallSessionId = data[1].Trim();
                            result = 0;
                            return result;
                        }
                    }
                }
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, string.Format("onReceivedSignaling : {0}", signaling), Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSendingSignaling(int callbackIndex, int callbackObject, int sessionId, StringBuilder signaling)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingSignaling", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onWaitingVoiceMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingVoiceMessage", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onWaitingFaxMessage(int callbackIndex, int callbackObject, string messageAccount, int urgentNewMessageCount, int urgentOldMessageCount, int newMessageCount, int oldMessageCount)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onWaitingFaxMessage", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onRecvDtmfTone(int callbackIndex, int callbackObject, int sessionId, int tone)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvDtmfTone", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onRecvOptions(int callbackIndex, int callbackObject, StringBuilder optionsMessage)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOptions", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onRecvInfo(int callbackIndex, int callbackObject, StringBuilder infoMessage)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvInfo", Logger.LogLevel.Info);
                this.ReceveMeassge("Receive Information", infoMessage.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRecvNotifyOfSubscription(int callbackIndex, int callbackObject, int subscribeId, StringBuilder notifyMsg, byte[] contentData, int contentLenght)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceRecvSubscribe", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSubscriptionFailure(int callbackIndex, int callbackObject, int subscribeId, int statusCode)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionFailure", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionFailure", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSubscriptionTerminated(int callbackIndex, int callbackObject, int subscribeId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionTerminated", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSubscriptionTerminated", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onPresenceRecvSubscribe(int callbackIndex, int callbackObject, int subscribeId, string fromDisplayName, string from, string subject)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceRecvSubscribe", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onPresenceOnline(int callbackIndex, int callbackObject, string fromDisplayName, string from, string stateText)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOnline", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onPresenceOffline(int callbackIndex, int callbackObject, string fromDisplayName, string from)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPresenceOffline", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onRecvMessage(int callbackIndex, int callbackObject, int sessionId, string mimeType, string subMimeType, byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = this.GetString(messageData);
                    this.ReceveMeassge("Receive Information", mesageText);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onRecvOutOfDialogMessage(int callbackIndex, int callbackObject, string fromDisplayName, string from, string toDisplayName, string to, string mimeType, string subMimeType, byte[] messageData, int messageDataLength)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onRecvOutOfDialogMessage", Logger.LogLevel.Info);
                if (mimeType == "text" && subMimeType == "plain")
                {
                    string mesageText = this.GetString(messageData);
                    this.ReceveMeassge("Receive Information", mesageText);
                }
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
            }
            return 0;
        }

        public int onSendMessageSuccess(int callbackIndex, int callbackObject, int sessionId, int messageId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageSuccess", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSendMessageFailure(int callbackIndex, int callbackObject, int sessionId, int messageId, string reason, int code)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendMessageFailure", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSendOutOfDialogMessageSuccess(int callbackIndex, int callbackObject, int messageId, string fromDisplayName, string from, string toDisplayName, string to)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageSuccess", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSendOutOfDialogMessageFailure(int callbackIndex, int callbackObject, int messageId, string fromDisplayName, string from, string toDisplayName, string to, string reason, int code)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendOutOfDialogMessageFailure", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onPlayAudioFileFinished(int callbackIndex, int callbackObject, int sessionId, string fileName)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayAudioFileFinished", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onPlayVideoFileFinished(int callbackIndex, int callbackObject, int sessionId)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onPlayVideoFileFinished", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "Sip Callback Events", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onReceivedRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRtpPacket", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onReceivedRtpPacket", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onSendingRtpPacket(IntPtr callbackObject, int sessionId, bool isAudio, byte[] RTPPacket, int packetSize)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingRtpPacket", Logger.LogLevel.Info);
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onSendingRtpPacket", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onAudioRawCallback(IntPtr callbackObject, int sessionId, int callbackType, byte[] data, int dataLength, int samplingFreqHz)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onAudioRawCallback", Logger.LogLevel.Info);
                if (callbackType != 1)
                {
                }
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onAudioRawCallback", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }

        public int onVideoRawCallback(IntPtr callbackObject, int sessionId, int callbackType, int width, int height, byte[] data, int dataLength)
        {
            int result;
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoRawCallback", Logger.LogLevel.Info);
                if (callbackType != 1)
                {
                }
                result = 0;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger4, "onVideoRawCallback", exception, Logger.LogLevel.Error);
                result = -1;
            }
            return result;
        }
        #endregion
        #region public method

        #endregion
    }
    #endregion

}
