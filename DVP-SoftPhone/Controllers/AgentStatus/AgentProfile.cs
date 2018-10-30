using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

using DuoSoftware.DuoTools.DuoLogger;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Controllers;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    

   
    public sealed class AgentProfile
    {
        private static volatile AgentProfile instance;
        private static object syncRoot = new Object();
        public NameValueCollection settingObject;
        public JavaScriptSerializer jsonSerializer;

        private AgentProfile()
        {
            settingObject = (NameValueCollection)ConfigurationManager.GetSection("VeerySetting");
            jsonSerializer = new JavaScriptSerializer();
        }

        public static AgentProfile Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new AgentProfile();
                    }
                }

                return instance;
            }
        }
        
        public string id { private set; get; }
        public string localIPAddress { private set; get; }
        public string UserName { private set; get; }
        public string Password { private set; get; }
        public string displayName { private set; get; }
        public string authorizationName { private set; get; }
        public string Domain { private set; get; }
        public string publicIdentity { private set; get; }        
        public object veeryFormat { private set; get; }
        public int acwTime { private set; get; }

        private string GetLocalIPAddress()
        {
            try
            {
                string myHost = Dns.GetHostName();

                string myIp = (from ipAddress in Dns.GetHostEntry(myHost).AddressList
                    where ipAddress.IsIPv6LinkLocal == false
                    where ipAddress.IsIPv6Multicast == false
                    where ipAddress.IsIPv6SiteLocal == false
                    where ipAddress.IsIPv6Teredo == false
                    select ipAddress).Select(ipAddress => ipAddress.ToString()).FirstOrDefault();

                if (!IsValidIP(myIp))
                {
                    IPAddress[] myIp1 = Dns.GetHostAddresses(myHost);
                    foreach (IPAddress ipAddress in
                        myIp1.Where(ipAddress => ipAddress.AddressFamily == AddressFamily.InterNetwork))
                    {
                        myIp = ipAddress.ToString();
                        break;
                    }
                }

                return myIp;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "GetLocalIPAddress", exception,
                    Logger.LogLevel.Error);
                return string.Empty;
            }
        }

        private bool IsValidIP(params object[] list)
        {
            try
            {
                var addr = list[0].ToString();
                //create our match pattern
                //            const string pattern = @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.
                //    ([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";

                const string pattern = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";
                //create our Regular Expression object
                var check = new Regex(pattern);
                //boolean variable to hold the status
                bool valid = false;
                //check to make sure an ip address was provided
                valid = addr != "" && check.IsMatch(addr, 0);
                //return the results
                return valid;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "IsValidIP", exception, Logger.LogLevel.Error);
                //Console.WriteLine(exception.Message);
                return false;
            }
        }


       
        private struct HandlingTypes
        {
            public string Type;
            public object Contact;
        }


        public bool Login(string username, string txtPassword, string domain)
        {
            try
            {
                                
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Login...............", Logger.LogLevel.Info);               
                displayName = username;
                UserName = username;
                Password = txtPassword;
                Domain = domain;
                authorizationName = username;                
                localIPAddress = GetLocalIPAddress();
                acwTime = 5;// ardsHandler.GetAcwTime(this);

                return true;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoDefault, "Login fail", exception, Logger.LogLevel.Error);
                return false;
            }
        }

        
    }
}