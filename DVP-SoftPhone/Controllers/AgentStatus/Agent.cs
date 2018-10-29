using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DuoSoftware.DuoTools.DuoLogger;
using DuoSoftware.DuoSoftPhone.Controllers.Common;
using DuoSoftware.DuoSoftPhone.Controllers.Service;


namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    public class Agent
    {
        #region Property

        public bool SipStatus { get; set; }
        public string ErrorMsg { get; set; }
        public string StatusText { get; set; }
        public int StatusCode { get; set; }
        public DateTime LastActivity { get; private set; }
        private AgentEvent _agentCurrentState;
        private AgentEvent _agentPvState;

        private AgentMode _agentMode;
        private AgentMode _agentReqMode;
        
        
        private IUiState UiState { get; set; }
        public string IP { get; private set; }
        public string AgentLoginSessionId { get; set; }

        public AgentProfile Profile;
        public Agent(string sessionId, IUiState uiState)
        {
            try
            {
                
                AgentLoginSessionId = sessionId;
                UiState = uiState;
                AgentCurrentState = new AgentInitiate();
                InitializeArdsEvent();
                LastActivity = DateTime.Now;
                Profile = AgentProfile.Instance;
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent", exception, Logger.LogLevel.Error);
            }
        }

        public AgentMode AgentReqMode
        {
            private get { return _agentReqMode; }
            set
            {
                try
                {
                    var isAccept = false;
                    _agentReqMode = value;
                    switch (value)
                    {
                        case AgentMode.Inbound:
                            isAccept = ardsHandler.SendModeChangeRequestInbound();
                            break;
                        case AgentMode.Offline:
                        case AgentMode.Outbound:
                            isAccept = ardsHandler.SendModeChangeRequestOutbound();
                            break;
                        case AgentMode.initiate:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("value");

                    }
                    if (isAccept)
                    {
                        AgentMode = _agentReqMode;
                    }
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentReqMode", exception,
                        Logger.LogLevel.Error);
                }
            }
        }

        public AgentMode AgentMode
        {
        get { return _agentMode; }
            set
            {
                try
                {
                    _agentMode = value;
                    UiState.OnResourceModeChanged(value);
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentMode", exception,Logger.LogLevel.Error);
                }
            }
        }

        public AgentEvent AgentCurrentState
        {
            get { return _agentCurrentState; }
            set
            {
                try
                {
                    _agentPvState = _agentCurrentState;
                    _agentCurrentState = value;
                    ChangeUI(value);
                    LastActivity = DateTime.Now;
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2,string.Format("Agent Status Change [{0}] To [{1}]", _agentPvState, value), Logger.LogLevel.Info);
                    if (!IsBreakRequest) return;
                    if (AgentCurrentState.GetType() != typeof(AgentIdle)) return;
                    IsBreakRequest = false;
                    AgentCurrentState = new AgentBreak();
                    
                }
                catch (Exception exception)
                {
                    Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentCurrentState", exception,Logger.LogLevel.Error);
                }
            }
        }

        public string CallSessionId { get; set; }
        public int PortsipSessionId { get; set; }
        public CallDirection CallDirection { get; set; }
        public bool IsCallAnswer { get; set; }
        public bool IsBreakRequest { get; private set; }

        #endregion

        #region Methods

        #region Private

        private void ChangeUI(AgentEvent state)
        {
            try
            {
                new ComMethods.SwitchOnType<AgentEvent>(state)
                       .Case<AgentInitiate>(initiate =>
                       {
                           UiState.InInitiateState();
                       })
                       .Case<AgentIdle>(i =>
                       {
                           UiState.InAgentIdleState();
                       })
                       .Case<AgentBusy>(b =>
                       {
                           UiState.InAgentBusy(CallDirection);
                           
                       })
                        .Case<AgentAcw>(b =>
                        {
                            UiState.InAgentAcwState();
                        })
                         .Case<AgentBreak>(b =>
                         {
                             UiState.InBreakState();
                         })
                           .Case<AgentOffline>(b => UiState.InOfflineState(StatusText, ErrorMsg, StatusCode))
                       .Default<AgentEvent>(t => UiState.InOfflineState(StatusText, ErrorMsg, StatusCode));
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "Agent ChangeUI", exception, Logger.LogLevel.Error);
            }
        }

        private void InitializeArdsEvent()
        {
            try
            {
                
                

                #region Breake

               


                
                
                
                #endregion Breake

                #region Reregistor

                

                #endregion Reregistor

               

                #region StatusChangeRequestIdel

                

                #endregion StatusChangeRequestIdel

                #region Mode
                

               
                #endregion

                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "InitializeArdsEvent", exception,Logger.LogLevel.Error);
            }
        }

        #endregion

        #region public

        public void ResourceStatusChangeBusy()
        {
            try
            {
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceStatusChangeBusy", exception, Logger.LogLevel.Error);
            }
        }

        public void ResourceModeChange()
        {
            try
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentInitiate-OnLogin", Logger.LogLevel.Info);
                
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "ResourceModeChange", exception, Logger.LogLevel.Error);
               
            }
        }

        public void EndBreakRequest()
        {
            try
            {
                ardsHandler.EndBreak("EndBreak");
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "EndBreakRequest", exception, Logger.LogLevel.Error);

            }
        }

        public void SendStatusChangeRequestBreak(string breakReason)
        {
            try
            {
               IsBreakRequest = ardsHandler.BreakRequest(breakReason);
                if (!IsBreakRequest) return;
                if (AgentCurrentState.GetType() != typeof (AgentIdle)) return;
                IsBreakRequest = false;
                AgentCurrentState = new AgentBreak();
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
            }
        }

        public void ResourceUnregistration()
        {

            try
            {

                Profile.Unregistration();
               
            }
            catch (Exception exception)
            {
                Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "SendStatusChangeRequestBreak", exception, Logger.LogLevel.Error);
            }
        }

        #endregion

        #endregion

       
    }
}
