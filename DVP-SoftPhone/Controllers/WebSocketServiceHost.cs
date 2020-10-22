
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoTools.DuoLogger;
using Fleck;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace DuoSoftware.DuoSoftPhone.Controllers
{

    //delegate 
    public delegate void SocketMessage(CallFunctions callFunction, string message, string othr);
    /// <summary>
    /// Socket Listener 
    /// </summary>
    public class WebSocketServiceHost
    {

        private static string ListenAddress;
        //event
        public static event SocketMessage OnRecive;

        // key
        private static string _duoKey;

        private static IWebSocketConnection webSocket;

        /// <summary>
        /// -- WebSocketServiceHost
        /// </summary>
        /// <param name="port"></param>
        /// <param name="securityToken"></param>
        /// <param name="tenantId"></param>
        /// <param name="companyId"></param>
        public WebSocketServiceHost(int port, string password, string fileName)
        {
            try
            {
                _duoKey = Guid.NewGuid().ToString();
                ListenAddress = "wss://127.0.0.1:" + port;

                //var server = new WebSocketServer("wss://0.0.0.0:8431");
                var server = new WebSocketServer(ListenAddress);

                server.Certificate =   new X509Certificate2(fileName, password);
                Console.WriteLine(ListenAddress);
                server.Start(socket =>
                {
                    socket.OnOpen =()=> OnConnected(socket.ConnectionInfo.Id);
                    socket.OnClose = () => OnDisconnected(socket.ConnectionInfo.Id);
                    socket.OnMessage = message => OnRecieve(message);
                    webSocket = socket; 
                });
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
        private static void OnSend(string aContext)
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("OnSend : {0}", aContext), Logger.LogLevel.Info);
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnSend", exception, Logger.LogLevel.Error);
            }
        }

        public void SendMessageToClient(CallFunctions message)
        {
            Reply(message.ToString());
        }

        private static void SendMessageToClient_test(CallFunctions message, dynamic expando)
        {
            try
            {
                if (webSocket == null) return;

                expando.veery_api_key = _duoKey;
                expando.veery_command = message.ToString();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("Reply : {0}, message : {1}", webSocket.ConnectionInfo.Id, expando.ToString()), Logger.LogLevel.Info);
                webSocket.Send(expando.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Reply", exception, Logger.LogLevel.Error);
            }
        }

        public void SendMessageToClient(CallFunctions message, dynamic expando)
        {
            try
            {
                if (webSocket == null) return;

                expando.veery_api_key = _duoKey;
                expando.veery_command = message.ToString();
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("Reply : {0}, message : {1}", webSocket.ConnectionInfo.Id, expando.ToString()), Logger.LogLevel.Info);
                webSocket.Send(expando.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Reply", exception, Logger.LogLevel.Error);
            }
        }

        private static void Reply(string message)
        {
            try
            {
                if (webSocket == null) return;
                //abc123|Initiate|123456789|othr
                dynamic expando = new JObject();
                expando.veery_api_key = _duoKey;
                expando.veery_command = message;

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, String.Format("Reply : {0}, message : {1}", webSocket.ConnectionInfo.Id, expando.ToString()), Logger.LogLevel.Info);
                webSocket.Send(expando.ToString());
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Reply", exception, Logger.LogLevel.Error);
            }
        }


        /// <summary>
        /// On Receive 
        /// </summary>
        /// <param name="aContext"></param>
        private void OnRecieve(string message)
        {
            try
            {
                //1) . abc123|Initiate|123456789|othr 
                //2). 721dab71-f9ae-44eb-9bbe-955261d4a726|Registor|123456789|9502-DuoS123-duo.media1.veery.cloud
                //3). 94625283-874f-4de6-b870-b3aaecc9c930|MakeCall|94112375000|othr

                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve : " + webSocket.ConnectionInfo.Id, Logger.LogLevel.Info);
                if (string.IsNullOrEmpty(message)) return;
                
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, string.Format("message : {0} form : {1}", message, webSocket.ConnectionInfo.Id), Logger.LogLevel.Info);

                var callInfo = message.Split('|');// "token|callfunction|no|othr"
                if (callInfo.Length != 4)
                {
                    Reply("Invalid Call Information's.");
                    throw new FieldAccessException("Invalid Call Information's. " + webSocket.ConnectionInfo.Id);
                }

                var callFunction = (CallFunctions)Enum.Parse(typeof(CallFunctions), callInfo[1]);
                if (callFunction == CallFunctions.Initiate)
                {
                    _duoKey = Guid.NewGuid().ToString();
                    Reply(CallFunctions.Handshake.ToString());
                    return;
                }
                if (!_duoKey.Equals(callInfo[0]))
                {
                    Reply("Invalid veery API Key.");
                    throw new SecurityException("Invalid SecurityToken or Expired. " + webSocket.ConnectionInfo.Id);
                }

                if (OnRecive != null)
                    OnRecive(callFunction, callInfo[2], callInfo[3]);

            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnRecieve", exception, Logger.LogLevel.Error);
            }
        }

        /// <summary>
        /// On Connected 
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnConnected(Guid id)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnConnected : " + id, Logger.LogLevel.Info);
           
        }

        /// <summary>
        /// on disconnect
        /// </summary>
        /// <param name="aContext"></param>
        private static void OnDisconnected(Guid ClientAddress)
        {
            Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected : " + ClientAddress, Logger.LogLevel.Info);
            //currentContext = null;
            //_duoKey = Guid.NewGuid().ToString();


            try
            {
                if (OnRecive != null && ClientAddress == webSocket.ConnectionInfo.Id)
                {
                    OnRecive(CallFunctions.Unregistor, null, null);
                    Reply(CallFunctions.Unregistor.ToString());
                    webSocket = null;
                }
                else
                {
                    dynamic expando = new JObject();
                    expando.description = "Unauthorized user try to communicate.[" + ClientAddress + "]";
                    SendMessageToClient_test(CallFunctions.Unauthorized, expando);
                    throw new InvalidOperationException("unauthorized user try to communicate.[" + ClientAddress + "]");
                }


            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "OnDisconnected", exception, Logger.LogLevel.Error);
            }
        }
    }
}