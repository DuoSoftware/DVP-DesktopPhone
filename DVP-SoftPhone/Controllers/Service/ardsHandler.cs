using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    class ardsHandler
    {
        public static bool SendModeChangeRequestInbound()
        {
            return EndBreak("Inbound");
        }

        public static bool SendModeChangeRequestOutbound()
        {
           return BreakRequest("Outbound");
        }

        public static bool BreakRequest(string reason)
        {
            try
            {
                var profile = AgentProfile.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger7, string.Format("BreakRequest . Resourceid : {0} :{1}", profile.id, reason.Replace(" ", string.Empty)), Logger.LogLevel.Debug);
                var url = profile.settingObject["ardsUrl"] + "/" + profile.id + "/state/NotAvailable/reason/" +  reason.Replace(" ",string.Empty);
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, null, "put");
                var data = profile.jsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseData.ToString());
                return data["IsSuccess"];
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "BreakRequest", exception, Logger.LogLevel.Error);
                return false;
            }

        }

        public static bool EndBreak(string reason)
        {

            try
            {
                var profile = AgentProfile.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger7, string.Format("EndBreak . Resourceid : {0} ", profile.id), Logger.LogLevel.Debug);
                var url = profile.settingObject["ardsUrl"] + "/" + profile.id + "/state/Available/reason/"+reason;
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, null, "put");
                var data = profile.jsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseData.ToString());
                return data["IsSuccess"];
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EndBreak", exception, Logger.LogLevel.Error);
                return false;
            }

        }

        
        
        public static void FreezeAcw(string callSessionId,bool freeze)
        {
            try
            {
                var profile = AgentProfile.Instance;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger7, string.Format("FreezeAcw . Resourceid : {0} : {1} : {2}", profile.id, callSessionId, freeze ? "Freeze" : "EndFreeze"), Logger.LogLevel.Debug);
                
                dynamic data = new JObject();
                data.RequestType = "CALL";
                data.State = freeze ? "Freeze":"EndFreeze" ;
                data.Reason = "";
                data.OtherInfo = "";

                var url = profile.settingObject["ardsUrl"] + "/" + profile.id + "/concurrencyslot/session/"+callSessionId;
               // HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, data, "put");

                Uri uri = new Uri(url);
                var d = data.ToString();
                HttpHandler.CallService(uri, d, "put", profile.server.token, callSessionId, freeze ? "Freeze" : "EndFreeze");

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "FreezeAcw", exception, Logger.LogLevel.Error);
            }
        }

        public static int GetAcwTime(AgentProfile profile)
        {
            try
            {
                var url = profile.settingObject["ardsUrl"].Replace("resource", "requestmeta") + "/CALLSERVER/CALL";
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, null, "get");
                var data = profile.jsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseData.ToString());
                var resultData = profile.jsonSerializer.Deserialize<Dictionary<string, dynamic>>(data["Result"]);


                return resultData["MaxAfterWorkTime"] - VeerySetting.Instance.AcwGap;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "EndBreak", exception, Logger.LogLevel.Error);
                return 10;
            }
        }

        public static bool ValidateToken(string token)
        {

            try
            {
                var profile = AgentProfile.Instance;
                var url = profile.settingObject["ardsUrl"].Replace("resource", "requestmeta") + "/CALLSERVER/CALL";
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, null, "get");
                var data = profile.jsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseData.ToString());
                return data["IsSuccess"];
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "ValidateToken", exception, Logger.LogLevel.Error);
                return false;
            }

        }
    }
}
