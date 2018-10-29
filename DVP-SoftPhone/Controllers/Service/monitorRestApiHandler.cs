using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;

namespace DuoSoftware.DuoSoftPhone.Controllers.Service
{
    class MonitorRestApiHandler
    {
        
        public static void MapResourceToVeery()
        {
            try
            {
                var profile = AgentProfile.Instance;
                var url = profile.settingObject["monitorRestApi"] + "/MonitorRestAPI/BindResourceToVeeryAccount";
                dynamic data = new JObject();
                data.SipURI = profile.publicIdentity.Replace("sip:", "");
                data.ResourceId = profile.id;
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, data, "post");
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, string.Format("MapResourceToVeery {0}" , responseData), Logger.LogLevel.Error);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "MapResourceToVeery", exception, Logger.LogLevel.Error);
            }
        }

        public struct onlineAgentList
        {
            public string SipUsername { get; set; }
            public string Extension { get; set; }
        }

        public static List<onlineAgentList> GetOnlineAgentList()
        {
            try
            {
                var profile = AgentProfile.Instance;
                var url = profile.settingObject["monitorRestApi"] + "/MonitorRestAPI/SipRegistrations";
                var responseData = HttpHandler.MakeRequest(url, "Bearer " + profile.server.token, null, "GET");


                if (responseData == null) return null;
                var data = new JavaScriptSerializer().Deserialize<Dictionary<string, dynamic>>(responseData.ToString());
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger6, string.Format("GetOnlineAgentList {0}", responseData), Logger.LogLevel.Error);
                var reply = new List<onlineAgentList>();
                foreach (var d in data["Result"])
                {
                    try
                    {
                        if (d == null) continue;
                        if (d["Status"].Equals("REGISTERED"))
                            reply.Add(new onlineAgentList { Extension = d["Extension"], SipUsername = d["SipUsername"] });
                    }
                    catch (Exception exception)
                    {
                        Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetOnlineAgentList-foreach", exception, Logger.LogLevel.Error);
                    }
                }

                return reply;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetOnlineAgentList", exception, Logger.LogLevel.Error);
                return null;
            }
        }
    }
}
