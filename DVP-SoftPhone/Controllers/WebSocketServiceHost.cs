using Alchemy;
using Alchemy.Classes;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;
using DuoSoftware.DuoTools.DuoLogger;
using System;
using System.Dynamic;
using System.Net;
using System.Security;
using DuoSoftware.DuoSoftPhone.Controllers.AgentStatus;
using Newtonsoft.Json.Linq;


namespace DuoSoftware.DuoSoftPhone.Controllers
{

    //delegate 
    public delegate void SocketMessage(CallFunctions callFunction, string message);
    /// <summary>
    /// Socket Listener 
    /// </summary>
    public class WebSocketServiceHost
    {

        private static UserContext currentContext;
        //event
        public event SocketMessage OnRecive;

        // key
        private static string _duoKey;

        /// <summary>
        /// -- WebSocketServiceHost
        /// </summary>
        /// <param name="port"></param>
        /// <param name="securityToken"></param>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        public WebSocketServiceHost(int port)
        {
            try
            {
                _duoKey = Guid.NewGuid().ToString();
                var server = new WebSocketServer(port, IPAddress.Any)
                {
                    OnConnected = OnConnected,
                    OnDisconnect = OnDisconnected,
                    OnSend = OnSend,
                    OnConnect = OnConnect,
                    OnReceive = OnRecieve,
                    TimeOut = new TimeSpan(0, 5, 0)
                };

                Console.WriteLine(server.ListenAddress);
                server.Start();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "WebSocketServiceHost", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// On Send
        /// </summary>
        /// <param name="aContext"></param>
        /// <param name="message"></param>
        private static void OnSend(UserContext aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("OnSend : {0}", aContext.ClientAddress), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSend", exception, Logger.LogLevel.Error);
            }
        }

        private static void Reply(string message)
        {
            try
            {
                if (currentContext == null) return;
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("Reply : {0}, message : {1}", currentContext.ClientAddress, message), Logger.LogLevel.Info);
                currentContext.Send(message);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Reply", exception, Logger.LogLevel.Error);
            }
        }

        private bool ValidateToken(string token)
        {
            try
            {
                return ardsHandler.ValidateToken(token);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "validateToken", exception, Logger.LogLevel.Error);
                return false;
            }

        }
        /// <summary>
        /// On Receive 
        /// </summary>
        /// <param name="aContext"></param>
        private void OnRecieve(UserContext aContext)
        {
            try
            {

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve : " + aContext.ClientAddress, Logger.LogLevel.Info);
                if (aContext.DataFrame == null) return;
                var message = aContext.DataFrame.ToString();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, string.Format("message : {0} form : {1}", message, aContext.ClientAddress), Logger.LogLevel.Info);

                var callInfo = message.Split('|');// "token|callfunction|no|othr"
                if (callInfo.Length != 4)
                {
                    Reply("Invalid Call Information's.");
                    throw new FieldAccessException("Invalid Call Information's.");
                }

                var callFunction = (CallFunctions)Enum.Parse(typeof(CallFunctions), callInfo[1]);
                if (callFunction == CallFunctions.Initiate)
                {
                    currentContext = aContext;
                    if (ValidateToken(callInfo[0]))
                    {
                        _duoKey = Guid.NewGuid().ToString();
                        dynamic expando = new JObject();
                        expando.veery_api_key = _duoKey;
                        Reply(expando.ToString());
                    }
                    else
                    {
                        Reply("Invalid SecurityToken or Expired.");
                    }
                    return;
                }
                if (!_duoKey.Equals(callInfo[0]))
                {
                    Reply("Invalid veery API Key.");
                    throw new SecurityException("Invalid SecurityToken or Expired.");
                }

                if (OnRecive != null)
                    OnRecive(callFunction, callInfo[2]);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// On Connect
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnConnect(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //try
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnect", exception, Logger.LogLevel.Error);
            //}
        }

        /// <summary>
        /// On Connected 
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnConnected(UserContext aContext)
        {

            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //try
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected", exception, Logger.LogLevel.Error);
            //}
        }

        /// <summary>
        /// on disconnect
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnDisconnected(UserContext aContext)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected : " + aContext.ClientAddress, Logger.LogLevel.Info);
            currentContext = null;
            _duoKey = Guid.NewGuid().ToString();
            //try
            //{

            //}
            //catch (Exception exception)
            //{
            //    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected", exception, Logger.LogLevel.Error);
            //}
        }
    }
}