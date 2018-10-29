using System;
using DuoSoftware.DuoTools.DuoLogger;

namespace DuoSoftware.DuoSoftPhone.Controllers.AgentStatus
{
    internal class AgentBreak : AgentEvent
    {
        
        public override void OnResetStatus(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnTimeOut(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnIncomingCall(ref Agent agent, string caller, int sessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnMakeCall(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnAnswerCall(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnFailMakeCall(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnCallReject(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndCall(ref Agent agent, bool isCallAnswerd)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndACW(ref Agent agent, string callSessionId, bool afterAcw)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogin(ref Agent agent)
        {
            try { agent.AgentCurrentState = new AgentIdle(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnLoggedOn(ref Agent agent, string callSessionId)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnLogOff(ref Agent agent)
        {
            try { agent.ResourceUnregistration(); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnOffline(ref Agent agent, string statusText)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentModeChange(ref Agent agent, AgentMode agentMode)
        {
            try { agent.AgentReqMode = agentMode; }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnAgentChangeMode(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnBreak(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnEndBreak(ref Agent agent)
        {
            try
            {
                agent.EndBreakRequest();
                agent.AgentCurrentState = new AgentIdle();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreak(ref Agent agent, string breakReason)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnRequestAgentBreakCancel(ref Agent agent)
        {
            try { throw new NotImplementedException("Invalid Agent Status."); }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }

        public override void OnError(ref Agent agent, string statusText, int statusCode, string msg)
        {
            try
            {
                agent.ErrorMsg = msg;
                agent.StatusCode = statusCode;
                agent.StatusText = statusText;
                agent.AgentCurrentState = new AgentOffline();
            }
            catch (Exception exception) { Logger.Instance.LogMessage(Logger.LogAppender.DuoLogger2, "AgentBreak", exception, Logger.LogLevel.Error); }
        }
    }
}