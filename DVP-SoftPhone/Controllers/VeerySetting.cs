using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers
{
    
    public sealed class VeerySetting
    {
        private static volatile VeerySetting instance;
        private static object syncRoot = new Object();

        private VeerySetting()
        {
            try
            {
                var section = (NameValueCollection)ConfigurationManager.GetSection("VeerySetting");
                TransferExtCode = section["TransferExtCode"].ToCharArray();
                TransferIvrCode = section["TransferIvrCode"].ToCharArray();
                TransferPhnCode = section["TransferPhnCode"].ToCharArray();
                SwapCode = section["SwapCode"].ToCharArray();
                ConferenceCode = section["ConferenceCode"].ToCharArray();
                EtlCode = section["EtlCode"].ToCharArray();
                DtmfValues = new Dictionary<char, int>
                {
                    {'0', 0},
                    {'1', 1},
                    {'2', 2},
                    {'3', 3},
                    {'4', 4},
                    {'5', 5},
                    {'6', 6},
                    {'7', 7},
                    {'8', 8},
                    {'9', 9},
                    {'*', 10},
                    {'#', 11}
                };

                AutoAnswerDelay = (int)TimeSpan.FromSeconds(Convert.ToInt16(section["AutoAnswerDelay"])).TotalMilliseconds;

                NotificationStateValidationIgnore = section["NotificationStateValidationIgnore"].Equals("1");
                AcwGap = Convert.ToInt16(section["acwGap"]);
                WebSocketlistnerEnable = section["WebSocketlistnerEnable"].ToLower().Equals("true");
                AgentConsoleintegration = section["agentConsoleintegration"].ToLower().Equals("true");
                ShowInTaskbar = section["showInTaskbar"].ToLower().Equals("true");
                WebSocketlistnerPort = Convert.ToInt16(section["WebSocketlistnerPort"]);
                stunServer = section["STUNserver"].ToString();
                stunServerPort = section["STUNserverPort"].ToString()==""?0:Convert.ToInt16(section["STUNserverPort"].ToString());
                audioDeviceLayer = string.IsNullOrEmpty(section["audioDeviceLayer"])? (0): (Convert.ToInt32(section["audioDeviceLayer"]));
                videoDeviceLayer = string.IsNullOrEmpty(section["videoDeviceLayer"]) ? (0) : (Convert.ToInt32(section["videoDeviceLayer"]));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "VeerySetting", exception, Logger.LogLevel.Error);
            }
        }

        public static VeerySetting Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new VeerySetting();
                    }
                }

                return instance;
            }
        }

        public char[] TransferExtCode { get; private set; }
        public char[] TransferIvrCode { get; private set; }
        public char[] TransferPhnCode { get; private set; }
        public char[] SwapCode { get; private set; }
        public char[] ConferenceCode { get; private set; }
        public char[] EtlCode { get; private set; }
        public Dictionary<char, int> DtmfValues { get; private set; }
        public int AutoAnswerDelay { get; private set; }
        public bool NotificationStateValidationIgnore { get; private set; }
        public int AcwGap { get; private set; }
        public short WebSocketlistnerPort { get; private set; }
        public bool WebSocketlistnerEnable { get; private set; }
        public bool AgentConsoleintegration { get; private set; }

        public string stunServer { get; private set; }
        public int stunServerPort { get; private set; }

        public int audioDeviceLayer { get; private set; }

        public int videoDeviceLayer { get; private set; }

        public bool ShowInTaskbar { get; set; }
    }
}
