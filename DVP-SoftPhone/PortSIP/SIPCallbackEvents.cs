/*!
 * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
 * @version 15
 * @see https://www.portsip.com
 * @brief PortSIP SDK Callback events.
 
 PortSIP VoIP SDK Callback events description.
 */

//////////////////////////////////////////////////////////////////////////
//
//  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
//
//////////////////////////////////////////////////////////////////////////  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortSIP
{
    /*!
    *  SIPCallbackEvents PortSIP VoIP SDK Callback events
    */
    interface SIPCallbackEvents
    {
        /** @defgroup groupDelegate SDK Callback events
         * SDK Callback events
         * @{
         */
        /** @defgroup group21 Register events
         * Register events
         * @{
         */

        /*!
         *  When successfully registered to server, this event will be triggered.
         *
         *  @param callbackIndex This is a callback index passed in when creating the SDK library.
         *  @param callbackObject This is a callback object passed in when creating the SDK library.
         *  @param statusText The status text.
         *  @param statusCode The status code.
         *  @param sipMessage        The SIP message received. 
         */
        Int32 onRegisterSuccess(Int32 callbackIndex, Int32 callbackObject, String statusText, Int32 statusCode, StringBuilder sipMessage);

        /*!
         *  If registration to SIP server fails, this event will be triggered.
         *
         *  @param statusText The status text.
         *  @param statusCode The status code.
         *  @param sipMessage        The SIP message received. 
         */
        Int32 onRegisterFailure(Int32 callbackIndex, Int32 callbackObject, String statusText, Int32 statusCode, StringBuilder sipMessage);

        /** @} */
        // end of group21

        /** @defgroup group22 Call events
         * @{
         */

        /*!
         *  When the call is coming, this event will be triggered.
         *
         *  @param sessionId         The session ID of the call.
         *  @param callerDisplayName The display name of caller
         *  @param caller            The caller.
         *  @param calleeDisplayName The display name of callee.
         *  @param callee            The callee.
         *  @param audioCodecNames   The matched audio codecs. It's separated by "#" if there are more than one codecs.
         *  @param videoCodecNames   The matched video codecs. It's separated by "#" if there are more than one codecs.
         *  @param existsAudio       If it's true, it indicates that this call includes the audio.
         *  @param existsVideo       If it's true, it indicates that this call includes the video.
         *  @param sipMessage        The SIP message received.         
         */
        Int32 onInviteIncoming(Int32 callbackIndex,
                                             Int32 callbackObject,
                                             Int32 sessionId,
                                             String callerDisplayName,
                                             String caller,
                                             String calleeDisplayName,
                                             String callee,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage);

        /*!
         *  If the outgoing call is being processed, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onInviteTrying(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  Once the caller received the "183 session progress" message, this event will be triggered.
         *
         *  @param sessionId        The session ID of the call.
         *  @param audioCodecNames  The matched audio codecs. It's separated by "#" if there are more than one codecs.
         *  @param videoCodecNames  The matched video codecs. It's separated by "#" if there are more than one codecs.
         *  @param existsEarlyMedia If it's true, it indicates that the call has early media.
         *  @param existsAudio      If it's true, it indicates that this call includes the audio.
         *  @param existsVideo      If it's true, it indicates that this call includes the video.
         *  @param sipMessage       The SIP message received.   
         */
        Int32 onInviteSessionProgress(Int32 callbackIndex,
                                            Int32 callbackObject,
                                            Int32 sessionId,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsEarlyMedia,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage);

        /*!
         *  If the outgoing call was ringing, this event would be triggered.
         *
         *  @param sessionId  The session ID of the call.
         *  @param statusText The status text.
         *  @param statusCode The status code.
         *  @param sipMessage The SIP message received. 
         */
        Int32 onInviteRinging(Int32 callbackIndex,
                                            Int32 callbackObject,
                                            Int32 sessionId,
                                            String statusText,
                                            Int32 statusCode,
                                            StringBuilder sipMessage);

        /*!
         *  If the remote party answered the call, this event would be triggered.
         *
         *  @param sessionId         The session ID of the call.
         *  @param callerDisplayName The display name of caller
         *  @param caller            The caller.
         *  @param calleeDisplayName The display name of callee.
         *  @param callee            The callee.
         *  @param audioCodecNames   The matched audio codecs. It's separated by "#" if there are more than one codecs.
         *  @param videoCodecNames   The matched video codecs. It's separated by "#" if there are more than one codecs.
         *  @param existsAudio       If it's true, it indicates that this call includes the audio.
         *  @param existsVideo       If it's true, it indicates that this call includes the video.
         *  @param sipMessage           The SIP message received. 
         */
        Int32 onInviteAnswered(Int32 callbackIndex,
                                             Int32 callbackObject,
                                             Int32 sessionId,
                                             String callerDisplayName,
                                             String caller,
                                             String calleeDisplayName,
                                             String callee,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage);

        /*!
         *  If the outgoing call fails, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         *  @param reason    The failure reason.
         *  @param code      The failure code.
         *  @param sipMessage The SIP message received. 
         */
        Int32 onInviteFailure(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code, StringBuilder sipMessage);

        /*!
         *  This event will be triggered when remote party updates this call.
         *
         *  @param sessionId   The session ID of the call.
         *  @param audioCodecNames The matched audio codecs. It's separated by "#" if there are more than one codecs.
         *  @param videoCodecNames The matched video codecs. It's separated by "#" if there are more than one codecs.
         *  @param existsAudio If it's true, it indicates that this call includes the audio.
         *  @param existsVideo If it's true, it indicates that this call includes the video.
         *  @param sipMessage  The SIP message received. 
         */
        Int32 onInviteUpdated(Int32 callbackIndex,
                                             Int32 callbackObject,
                                             Int32 sessionId,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage);

        /*!
         *  This event would be triggered when UAC sent/UAS received ACK(the call is connected). Some functions (hold, updateCall etc...) can be called only after the call connected, otherwise these functions will return error.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onInviteConnected(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  If the enableCallForward method is called and a call is incoming, the call will be forwarded automatically and this event will be triggered.
         *
         *  @param forwardTo The forwarding target SIP URI.
         */
        Int32 onInviteBeginingForward(Int32 callbackIndex, Int32 callbackObject, String forwardTo);

        /*!
         *  This event is triggered once remote side closes the call.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onInviteClosed(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  If a user subscribed and his dialog status monitored, when the monitored user is holding a call
         *  or being rang, this event will be triggered
         *
         *  @param BLFMonitoredUri the monitored user's URI
         *  @param BLFDialogState - the status of the call
         *  @param BLFDialogId - the id of the call
         *  @param BLFDialogDirection - the direction of the call
         */

        Int32 onDialogStateUpdated(Int32 callbackIndex,
                                        Int32 callbackObject,
                                        String BLFMonitoredUri,
                                        String BLFDialogState,
                                        String BLFDialogId,
                                        String BLFDialogDirection);

        /*!
         *  If the remote side placed the call on hold, this event would be triggered.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onRemoteHold(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  If the remote side un-hold the call, this event would be triggered.
         *
         *  @param sessionId   The session ID of the call.
         *  @param audioCodecNames The matched audio codecs. It's separated by "#" if there are more than one codecs.
         *  @param videoCodecNames The matched video codecs. It's separated by "#" if there are more than one codecs.
         *  @param existsAudio If it's true, it indicates that this call includes the audio.
         *  @param existsVideo If it's true, it indicates that this call includes the video.
         */
        Int32 onRemoteUnHold(Int32 callbackIndex, 
                            Int32 callbackObject, 
                            Int32 sessionId, 
                            String audioCodecNames,
                            String videoCodecNames,
                             Boolean existsAudio,
                             Boolean existsVideo);


        /** @} */
        // end of group22

        /** @defgroup group23 Refer events
         * @{
         */

        /*!
         *  This event will be triggered once receiving a REFER message.
         *
         *  @param sessionId       The session ID of the call.
         *  @param referId         The ID of the REFER message. Pass it to acceptRefer or rejectRefer
         *  @param to              The refer target.
         *  @param from            The sender of REFER message.
         *  @param referSipMessage The SIP message of "REFER". Pass it to "acceptRefer" function.
         */
        Int32 onReceivedRefer(Int32 callbackIndex,
                             Int32 callbackObject,
                             Int32 sessionId,
                             Int32 referId,
                             String to,
                             String from,
                             StringBuilder referSipMessage);

        /*!
         *  This callback will be triggered once remote side called "acceptRefer" to accept the REFER.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onReferAccepted(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  This callback will be triggered once remote side called "rejectRefer" to reject the REFER.
         *
         *  @param sessionId The session ID of the call.
         *  @param reason    Reject reason.
         *  @param code      Reject code.
         */
        Int32 onReferRejected(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code);

        /*!
         *  When the refer call is being processed, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onTransferTrying(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  When the refer call is ringing, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onTransferRinging(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  When the refer call succeeds, this event will be triggered. The ACTV means Active.
            For example: A established the call with B, and A transferred B to C. When C accepts the refer call, A will receive this event.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onACTVTransferSuccess(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /*!
         *  When the refer call fails, this event will be triggered. The ACTV means Active.
         For example: A established the call with B, and A transfered B to C. When C rejects the refer call, A will receive this event.
         *
         *  @param sessionId The session ID of the call.
         *  @param reason    The error reason.
         *  @param code      The error code.
         */
        Int32 onACTVTransferFailure(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code);

        /** @} */
        // end of group23

        /** @defgroup group24 Signaling events
         * @{
         */
        /*!
         *  This event will be triggered when receiving an SIP message.
         *
         *  @param sessionId The session ID of the call.
         *  @param signaling The SIP message received.
         */
        Int32 onReceivedSignaling(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, StringBuilder signaling);

        /*!
         *  This event will be triggered when a SIP message sent.
         *
         *  @param sessionId The session ID of the call.
         *  @param signaling The SIP message sent.
         */
        Int32 onSendingSignaling(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, StringBuilder signaling);

        /** @} */
        // end of group24

        /** @defgroup group25 MWI events
         * @{
         */

        /*!
         *  If there is voice message (MWI) waiting, this event will be triggered.
         *
         *  @param messageAccount        Voice message account
         *  @param urgentNewMessageCount Urgent new message count.
         *  @param urgentOldMessageCount Urgent old message count.
         *  @param newMessageCount       New message count.
         *  @param oldMessageCount       Old message count.
         */
        Int32 onWaitingVoiceMessage(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                  String messageAccount,
                                                  Int32 urgentNewMessageCount,
                                                  Int32 urgentOldMessageCount,
                                                  Int32 newMessageCount,
                                                  Int32 oldMessageCount);

        /*!
         *  If there is fax message (MWI) waiting, this event will be triggered.
         *
         *  @param messageAccount        Fax message account
         *  @param urgentNewMessageCount Urgent new message count.
         *  @param urgentOldMessageCount Urgent old message count.
         *  @param newMessageCount       New message count.
         *  @param oldMessageCount       Old message count.
         */
        Int32 onWaitingFaxMessage(Int32 callbackIndex,
                                                       Int32 callbackObject,
                                                  String messageAccount,
                                                  Int32 urgentNewMessageCount,
                                                  Int32 urgentOldMessageCount,
                                                  Int32 newMessageCount,
                                                  Int32 oldMessageCount);

        /** @} */
        // end of group25

        /** @defgroup group26 DTMF events
         * @{
         */

        /*!
         *  This event will be triggered when receiving a DTMF tone from remote side.
         *
         *  @param sessionId The session ID of the call.
         *  @param tone      DTMF tone.
         * <p><table>
         * <tr><th>code</th><th>Description</th></tr>
         * <tr><td>0</td><td>The DTMF tone 0.</td></tr><tr><td>1</td><td>The DTMF tone 1.</td></tr><tr><td>2</td><td>The DTMF tone 2.</td></tr>
         * <tr><td>3</td><td>The DTMF tone 3.</td></tr><tr><td>4</td><td>The DTMF tone 4.</td></tr><tr><td>5</td><td>The DTMF tone 5.</td></tr>
         * <tr><td>6</td><td>The DTMF tone 6.</td></tr><tr><td>7</td><td>The DTMF tone 7.</td></tr><tr><td>8</td><td>The DTMF tone 8.</td></tr>
         * <tr><td>9</td><td>The DTMF tone 9.</td></tr><tr><td>10</td><td>The DTMF tone *.</td></tr><tr><td>11</td><td>The DTMF tone #.</td></tr>
         * <tr><td>12</td><td>The DTMF tone A.</td></tr><tr><td>13</td><td>The DTMF tone B.</td></tr><tr><td>14</td><td>The DTMF tone C.</td></tr>
         * <tr><td>15</td><td>The DTMF tone D.</td></tr><tr><td>16</td><td>The DTMF tone FLASH.</td></tr>
         * </table></p>
         */
        Int32 onRecvDtmfTone(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, Int32 tone);

        /** @} */
        // end of group26

        /** @defgroup group27 INFO/OPTIONS message events
         * @{
         */

        /*!
         *  This event will be triggered when receiving the OPTIONS message.
         *
         *  @param optionsMessage The received whole OPTIONS message in text format.
         */
        Int32 onRecvOptions(Int32 callbackIndex, Int32 callbackObject, StringBuilder optionsMessage);

        /*!
         *  This event will be triggered when receiving the INFO message.
         *
         *  @param infoMessage The received whole INFO message in text format.
         */
        Int32 onRecvInfo(Int32 callbackIndex, Int32 callbackObject, StringBuilder infoMessage);

        /*!
         *  This event will be triggered when receiving a NOTIFY message of the subscription.
         *
         *  @param subscribeId       The ID of SUBSCRIBE request.
         *  @param notifyMessage     The received INFO message in text format.
         *  @param contentData       The received message body. It's can be either text or binary data.
         *  @param contentLenght The length of "messageData".
         */
        Int32 onRecvNotifyOfSubscription(Int32 callbackIndex, 
                                        Int32 callbackObject,
                                        Int32 subscribeId, 
                                        StringBuilder notifyMsg,
                                        byte[] contentData, 
                                        Int32 contentLenght);

        /*!
         *  This event will be triggered on sending SUBSCRIBE failure.
         *
         *  @param subscribeId     The ID of SUBSCRIBE request.
         *  @param statusCode The status code.
         */
        Int32 onSubscriptionFailure(Int32 callbackIndex, Int32 callbackObject, Int32 subscribeId, Int32 statusCode);

        /*!
         *  This event will be triggered when a SUBSCRIPTION is terminated or expired.
         *
         *  @param subscribeId     The ID of SUBSCRIBE request.
         */
        Int32 onSubscriptionTerminated(Int32 callbackIndex, Int32 callbackObject, Int32 subscribeId);


        /** @} */
        // end of group27

        /** @defgroup group28 Presence events
         * @{
         */
        /*!
         *  This event will be triggered when receiving the SUBSCRIBE request from a contact.
         *
         *  @param subscribeId     The ID of SUBSCRIBE request.
         *  @param fromDisplayName The display name of contact.
         *  @param from            The contact who sends the SUBSCRIBE request.
         *  @param subject         The subject of the SUBSCRIBE request.
         */
        Int32 onPresenceRecvSubscribe(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                    Int32 subscribeId,
                                                    String fromDisplayName,
                                                    String from,
                                                    String subject);

        /*!
         *  When the contact is online or changes presence status, this event will be triggered.
         *
         *  @param fromDisplayName The display name of contact.
         *  @param from            The contact who sends the SUBSCRIBE request.
         *  @param stateText       The presence status text.
         */
        Int32 onPresenceOnline(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                    String fromDisplayName,
                                                    String from,
                                                    String stateText);

        /*!
         *  When the contact goes offline, this event will be triggered.
         *
         *  @param fromDisplayName The display name of contact.
         *  @param from            The contact who sends the SUBSCRIBE request
         */
        Int32 onPresenceOffline(Int32 callbackIndex, Int32 callbackObject, String fromDisplayName, String from);

        /*!
         *  This event will be triggered when receiving a MESSAGE message in dialog.
         *
         *  @param sessionId         The session ID of the call.
         *  @param mimeType          The message mime type.
         *  @param subMimeType       The message sub mime type.
         *  @param messageData       The received message body. It can be text or binary data.
         *  @param messageDataLength The length of "messageData".
         */
        Int32 onRecvMessage(Int32 callbackIndex,
                                                 Int32 callbackObject,
                                                 Int32 sessionId,
                                                 String mimeType,
                                                 String subMimeType,
                                                 byte[] messageData,
                                                 Int32 messageDataLength);

        /*!
         *  This event will be triggered when receiving a MESSAGE message out of dialog. For example: pager message.
         *
         *  @param fromDisplayName   The display name of sender.
         *  @param from              The message sender.
         *  @param toDisplayName     The display name of recipient.
         *  @param to                The recipient.
         *  @param mimeType          The message mime type.
         *  @param subMimeType       The message sub mime type.
         *  @param messageData       The received message body. It can be text or binary data.
         *  @param messageDataLength The length of "messageData".
         */
        Int32 onRecvOutOfDialogMessage(Int32 callbackIndex,
                                                 Int32 callbackObject,
                                                 String fromDisplayName,
                                                 String from,
                                                 String toDisplayName,
                                                 String to,
                                                 String mimeType,
                                                 String subMimeType,
                                                 byte[] messageData,
                                                 Int32 messageDataLength);

        /*!
         *  If the message is sent successfully in dialog, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         *  @param messageId The message ID. It's equal to the return value of sendMessage function.
         */
        Int32 onSendMessageSuccess(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 sessionId, 
                                                        Int32 messageId);

        /*!
         *  If the message fails to be sent out of dialog, this event will be triggered.
         *
         *  @param sessionId The session ID of the call.
         *  @param messageId The message ID. It's equal to the return value of sendMessage function.
         *  @param reason    The failure reason.
         *  @param code      Failure code.
         */
        Int32 onSendMessageFailure(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 sessionId,
                                                        Int32 messageId,
                                                        String reason,
                                                        Int32 code);



        /*!
         *  If the message is sent succeeded out of dialog, this event will be triggered.
         *
         *  @param messageId       The message ID. It's equal to the return value of SendOutOfDialogMessage function.
         *  @param fromDisplayName The display name of message sender.
         *  @param from            The message sender.
         *  @param toDisplayName   The display name of message recipient.
         *  @param to              The message receiver.
         */
        Int32 onSendOutOfDialogMessageSuccess(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 messageId,
                                                        String fromDisplayName,
                                                        String from,
                                                        String toDisplayName,
                                                        String to);

        /*!
         *  If the message was sent failure out of dialog, this event will be triggered.
         *
         *  @param messageId       The message ID. It's equal to the return value of SendOutOfDialogMessage function.
         *  @param fromDisplayName The display name of message sender
         *  @param from            The message sender.
         *  @param toDisplayName   The display name of message recipient.
         *  @param to              The message recipient.
         *  @param reason          The failure reason.
         *  @param code            The failure code.
         */
        Int32 onSendOutOfDialogMessageFailure(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 messageId,
                                                        String fromDisplayName,
                                                        String from,
                                                        String toDisplayName,
                                                        String to,
                                                        String reason,
                                                        Int32 code);

        /** @} */
        // end of group29

        /** @defgroup group30 Play audio and video file finished events
         * @{
         */

        /*!
         *  If playAudioFileToRemote function is called with no loop mode, this event will be triggered once the file play finished.
         *
         *  @param sessionId The session ID of the call.
         *  @param fileName  The name of the file played.
         */
        Int32 onPlayAudioFileFinished(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String fileName);

        /*!
         *  If playVideoFileToRemote function is called with no loop mode, this event will be triggered once the file play finished.
         *
         *  @param sessionId The session ID of the call.
         */
        Int32 onPlayVideoFileFinished(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId);

        /** @} */
        // end of group30

        /** @defgroup group31 RTP callback events
         * @{
         */
        /*!
         *  If setRTPCallback function is called to enable the RTP callback, this event will be triggered once receiving a RTP packet.
         *
         *  @param sessionId  The session ID of the call.
         *  @param isAudio    If the received RTP packet is of audio, this parameter returns true, otherwise false.
         *  @param RTPPacket  The memory of whole RTP packet.
         *  @param packetSize The size of received RTP Packet.
          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
         */
        Int32 onReceivedRtpPacket(IntPtr callbackObject,
                          Int32 sessionId,
                          Boolean isAudio,
                          byte[] RTPPacket,
                          Int32 packetSize);

        /*!
         *  If setRTPCallback function is called to enable the RTP callback, this event will be triggered once a RTP packet sent.
         *
         *  @param sessionId  The session ID of the call.
         *  @param isAudio    If the received RTP packet is of audio, this parameter returns true, otherwise false.
         *  @param RTPPacket  The memory of whole RTP packet.
         *  @param packetSize The size of received RTP Packet.
          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
         */
        Int32 onSendingRtpPacket(IntPtr callbackObject,
                                  Int32 sessionId,
                                  Boolean isAudio,
                                  byte[] RTPPacket,
                                  Int32 packetSize);

        /** @} */
        // end of group31

        /** @defgroup group32 Audio and video stream callback events
        * @{
        */

        /*!
         *  This event will be triggered once receiving the audio packets if called enableAudioStreamCallback function.
         *
         *  @param sessionId         The session ID of the call.
         *  @param audioCallbackMode The type that is passed in enableAudioStreamCallback function.
         *  @param data              The memory of audio stream. It's in PCM format.
         *  @param dataLength        The data size.
         *  @param samplingFreqHz    The audio stream sample in HZ. For example, it's 8000 or 16000.
          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
         */
        Int32 onAudioRawCallback(IntPtr callbackObject,
                                               Int32 sessionId,
                                               Int32 callbackType,
                                               byte[] data,
                                               Int32 dataLength,
                                               Int32 samplingFreqHz);

        /*!
         *  This event will be triggered once receiving the video packets if called enableVideoStreamCallback function.
         *
         *  @param sessionId         The session ID of the call.
         *  @param videoCallbackMode The type which is passed in enableVideoStreamCallback function.
         *  @param width             The width of video image.
         *  @param height            The height of video image.
         *  @param data              The memory of video stream. It's in YUV420 format, YV12.
         *  @param dataLength        The data size.
          @note Don't call any SDK API functions in this event directly. If you want to call the API functions or other code which is time-consuming, you should post a message to another thread and execute SDK API functions or other code in another thread.
         */
        Int32 onVideoRawCallback(IntPtr callbackObject,
                                               Int32 sessionId,
                                               Int32 callbackType,
                                               Int32 width,
                                               Int32 height,
                                               byte[] data,
                                               Int32 dataLength);


        /** @} */
        // end of group32
        /** @} */
        // end of groupDelegate

    }
}
