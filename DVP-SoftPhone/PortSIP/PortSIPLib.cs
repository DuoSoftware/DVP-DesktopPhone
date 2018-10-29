/*!
 * @author Copyright (c) 2008-2017 PortSIP Solutions,Inc. All rights reserved.
 * @version 15
 * @see https://www.portsip.com
 * @brief The PortSIP VoIP SDK class.
 
 PortSIP VoIP SDK functions class description.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DuoSoftware.DuoTools.DuoLogger;
using PortSIP;


//////////////////////////////////////////////////////////////////////////
//
//  !!!IMPORTANT!!! DON'T EDIT BELOW SOURCE CODE  
//
//////////////////////////////////////////////////////////////////////////

/*!
*  PortSIP The PortSIP VoIP SDK namespace
*/
namespace PortSIP
{
    unsafe class PortSIPLib
    {

        private SIPCallbackEvents _SIPCallbackEvents;


        public PortSIPLib(Int32 callbackIndex, Int32 callbackObject, SIPCallbackEvents calbackevents)
        {
            initializeCallbackFunctions();

            _callbackDispatcher = IntPtr.Zero;
            _LibSDK = IntPtr.Zero;
            _callbackObject = callbackObject;
            _callbackIndex = callbackIndex;

            _SIPCallbackEvents = calbackevents;
        }


        public Boolean createCallbackHandlers() // This must be called before initialization
        {
            if (_callbackDispatcher != IntPtr.Zero)
            {
                return false;
            }

            if (createSIPCallbackHandle() == false)
            {
                return false;
            }

            setCallbackHandlers();

            return true;

        }


        private Boolean createSIPCallbackHandle()
        {
            _callbackDispatcher = PortSIP_NativeMethods.PSCallback_createCallbackDispatcher();
            if (_callbackDispatcher == IntPtr.Zero)
            {
                return false;
            }

            return true;
        }


        public void releaseCallbackHandlers() // This must called after unInitialization
        {
            if (_callbackDispatcher == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PSCallback_releaseCallbackDispatcher(_callbackDispatcher);
            _callbackDispatcher = IntPtr.Zero;
        }



        private void setCallbackHandlers()
        {
            PortSIP_NativeMethods.PSCallback_setRegisterSuccessHandler(_callbackDispatcher, _rgs, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRegisterFailureHandler(_callbackDispatcher, _rgf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteIncomingHandler(_callbackDispatcher, _ivi, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteTryingHandler(_callbackDispatcher, _ivt, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteSessionProgressHandler(_callbackDispatcher, _ivsp, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteRingingHandler(_callbackDispatcher, _ivr, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteAnsweredHandler(_callbackDispatcher, _iva, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteFailureHandler(_callbackDispatcher, _ivf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteUpdatedHandler(_callbackDispatcher, _ivu, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteConnectedHandler(_callbackDispatcher, _ivc, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteBeginingForwardHandler(_callbackDispatcher, _ivbf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setInviteClosedHandler(_callbackDispatcher, _ivcl, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setDialogStateUpdatedHandler(_callbackDispatcher, _dsu, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRemoteHoldHandler(_callbackDispatcher, _rmh, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRemoteUnHoldHandler(_callbackDispatcher, _rmuh, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setReceivedReferHandler(_callbackDispatcher, _rvr, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setReferAcceptedHandler(_callbackDispatcher, _rfa, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setReferRejectedHandler(_callbackDispatcher, _rfr, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setTransferTryingHandler(_callbackDispatcher, _tft, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setTransferRingingHandler(_callbackDispatcher, _tfr, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setACTVTransferSuccessHandler(_callbackDispatcher, _ats, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setACTVTransferFailureHandler(_callbackDispatcher, _atf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setReceivedSignalingHandler(_callbackDispatcher, _rvs, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setSendingSignalingHandler(_callbackDispatcher, _sds, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setWaitingVoiceMessageHandler(_callbackDispatcher, _wvm, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setWaitingFaxMessageHandler(_callbackDispatcher, _wfm, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRecvDtmfToneHandler(_callbackDispatcher, _rdt, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setPresenceRecvSubscribeHandler(_callbackDispatcher, _prs, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setPresenceOnlineHandler(_callbackDispatcher, _pon, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setPresenceOfflineHandler(_callbackDispatcher, _pof, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRecvOptionsHandler(_callbackDispatcher, _rvo, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRecvInfoHandler(_callbackDispatcher, _rvi, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setPlayAudioFileFinishedHandler(_callbackDispatcher, _paf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setPlayVideoFileFinishedHandler(_callbackDispatcher, _pvf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRecvMessageHandler(_callbackDispatcher, _rvm, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setRecvOutOfDialogMessageHandler(_callbackDispatcher, _rodm, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setSendMessageSuccessHandler(_callbackDispatcher, _sms, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setSendMessageFailureHandler(_callbackDispatcher, _smf, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setSendOutOfDialogMessageSuccessHandler(_callbackDispatcher, _sdms, _callbackIndex, _callbackObject);
            PortSIP_NativeMethods.PSCallback_setSendOutOfDialogMessageFailureHandler(_callbackDispatcher, _sdmf, _callbackIndex, _callbackObject);
        }

        /** @defgroup groupSDK SDK functions
        * SDK functions
        * @{
        */
        /** @defgroup group1 Initialize and register functions
        * Initialize and register functions
        * @{
        */

        /*!
         * @brief Initialize the SDK.
         *
         *@param transport      Transport for SIP signaling. TRANSPORT_PERS is the PortSIP private transport for anti SIP blocking. It must be used with PERS.
         *@param localIP        The local computer IP address to be bound (for example: 192.168.1.108). It will be used for sending and receiving SIP messages and RTP packets. If this IP is passed in IPv6 format, the SDK will be using IPv6.<br>
         *                      If you want the SDK to choose correct network interface (IP) automatically, please pass the "0.0.0.0"(for IPv4) or "::" (for IPv6).
         * @param localSIPPort  The SIP message transport listener port, for example: 5060.
         * @param logLevel      Set the application log level. The SDK will generate "PortSIP_Log_datatime.log" file if the log enabled.
         * @param logFilePath   The log file path. The path (folder) MUST be existent.
         * @param maxCallLines  Theoretically, unlimited lines could be supported depending on the device capability. For SIP client recommended value ranges 1 - 100;
         * @param sipAgent      The User-Agent header to be inserted into SIP messages.
         * @param audioDeviceLayer Specify the audio device layer to be used: <br>
         *            0 = Use the OS default device.<br>
         *            1 = Virtual device, usually use this for the device which has no sound device installed.<br>
         * @param videoDeviceLayer Specifies the video device layer that should be used: <br>
         *            0 = Use the OS default device.<br>
         *            1 = Use Virtual device. Usually use this for the device which has no camera installed.
         * @param TLSCertificatesRootPath  Specify the TLS certificate path, from which the SDK will load the certificates automatically. Note: On Windows, this path will be ignored, and SDK will read the certificates from Windows certificates stored area instead.
         * @param TLSCipherList  Specify the TLS cipher list. This parameter is usually passed as empty so that the SDK will offer all available ciphers.
         * @param verifyTLSCertificate  Indicate if SDK will verify the TLS certificate. By setting to false, the SDK will not verify the validity of TLS certificate.
         * @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code
         */
        public Int32 initialize(TRANSPORT_TYPE transportType,
                                          String localIp,
                                          Int32 localSIPPort,
                                          PORTSIP_LOG_LEVEL logLevel,
                                          String logFilePath,
                                          Int32 maxCallSessions,
                                          String sipAgentString,
                                          Int32 audioDeviceLayer,
                                          Int32 videoDeviceLayer,
                                          String TLSCertificatesRootPath,
                                          String TLSCipherList,
                                          Boolean verifyTLSCertificate)
        {
            if (_callbackDispatcher == IntPtr.Zero || _LibSDK != IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            Int32 errorCode = 0;
            _LibSDK = PortSIP_NativeMethods.PortSIP_initialize(_callbackDispatcher,
                                                           transportType,
                                                           localIp,
                                                           localSIPPort,
                                                           (Int32)logLevel,
                                                           logFilePath,
                                                           maxCallSessions,
                                                           sipAgentString,
                                                           audioDeviceLayer,
                                                           videoDeviceLayer,
                                                           TLSCertificatesRootPath,
                                                           TLSCipherList,
                                                           verifyTLSCertificate,
                                                           out errorCode);

            return errorCode;
        }


        /*!
         *  @brief Un-initialize the SDK and release resources.
         */
        public void unInitialize()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_unInitialize(_LibSDK);
            _LibSDK = IntPtr.Zero;
            
        }

        /*!
           *  @brief Get the current version number of the SDK.
           *
           *  @param majorVersion Return the major version number.
           *  @param minorVersion Return the minor version number.
           *
           *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
           */
        public Int32 getVersion(out Int32 majorVersion, out Int32 minorVersion)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                majorVersion = 0;
                minorVersion = 0;

                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getVersion(_LibSDK, out majorVersion, out minorVersion);
        }


        /*!
         *  @brief Set the license key. It must be called before setUser function.
         *
         *  @param key The SDK license key, please purchase from PortSIP.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setLicenseKey(String key)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setLicenseKey(_LibSDK, key);
        }

        /** @} */
        // end of group1

        /** @defgroup group2 NIC and local IP functions
         * @{
         */

        /*!
         *  @brief Get the Network Interface Card numbers.
         *
         *  @return If the function succeeds, it will return the NIC numbers which is greater than or equal to 0. If the function fails, it will return a specific error code.
         */
        public Int32 getNICNums()
        {
            return PortSIP_NativeMethods.PortSIP_getNICNums();
        }

        /*!
         *  @brief Get the local IP address by Network Interface Card index.
         *
         *  @param index The IP address index. For example, if the PC has two NICs, and we wish to obtain the second NIC IP. Set this parameter to 1 and the first NIC IP index is 0.
         *  @param ip The buffer that is used to receive the IP. 
         *  @param ipSize The IP buffer size, which cannot be less than 32 bytes. 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
         */
        public Int32 getLocalIpAddress(Int32 index, StringBuilder ip, Int32 ipSize)
        {
            return PortSIP_NativeMethods.PortSIP_getLocalIpAddress(index, ip, ipSize);
        }


        /*!
         *  @brief Set user account info.
         *
         *  @param userName           Account (User name) of the SIP. Usually provided by an IP-Telephony service provider.
         *  @param displayName        The display name of user. You can set it as your like, such as "James Kend". It's optional.
         *  @param authName           Authorization user name (usually equal to the username).
         *  @param password           The password of user. It's optional.
         *  @param localIp            The local computer IP address to be bound. For example: 192.168.1.108. It will be used for sending and receiving SIP message and RTP packet. If pass this IP as the IPv6 format, the SDK will use IPv6.
         *  @param localSipPort       The SIP message transport listener port. For example: 5060.
         *  @param userDomain         User domain; this parameter is optional that allows to pass an empty string if you are not using the domain.
         *  @param sipServer          SIP proxy server IP or domain. For example: xx.xxx.xx.x or sip.xxx.com.
         *  @param sipServerPort      Port of the SIP proxy server. For example: 5060.
         *  @param stunServer         Stun server used for NAT traversal. It's optional and can pass empty string to disable STUN.
         *  @param stunServerPort     STUN server port. It will be ignored if the outboundServer is empty.
         *  @param outboundServer     Outbound proxy server. For example: sip.domain.com. It's optional and allows to pass a empty string if not using outbound server.
         *  @param outboundServerPort Outbound proxy server port, it will be ignored if the outboundServer is empty.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setUser(String userName,
                         String displayName,
                         String authName,
                         String password,
                         String sipDomain,
                         String sipServerAddr,
                         Int32 sipServerPort,
                         String stunServerAddr,
                         Int32 stunServerPort,
                         String outboundServerAddr,
                         Int32 outboundServerPort)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setUser(_LibSDK,
                                                             userName,
                                                             displayName,
                                                             authName,
                                                             password,
                                                             sipDomain,
                                                             sipServerAddr,
                                                             sipServerPort,
                                                             stunServerAddr,
                                                             stunServerPort,
                                                             outboundServerAddr,
                                                             outboundServerPort);

        }

        /*!
         *  @brief Remove the user. It will un-register from SIP server given that the user is already registered.
         */
        public void removeUser()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_removeUser(_LibSDK);
        }

        /*!
        *  @brief Set the display name of user.
        *
        *  @param displayName that will appear in the From/To Header. 
        *  
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        */
        public Int32 setDisplayName(String displayName)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setDisplayName(_LibSDK, displayName);
        }

        /*!
         *  @brief Set outbound (RFC5626) instanceId to be used in contact headers
         *
         *  @param uuid The ID that will appear in the contact header. Please make sure it's a unique ID.
         *  
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setInstanceId(String uuid)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setInstanceId(_LibSDK, uuid);
        }



        /*!
         *  @brief Register to SIP proxy server (login to server).
         *
         *  @param expires Registration refresh Interval in seconds with maximum 3600. It will be inserted into SIP REGISTER message headers.
          *  @param retryTimes The retry times if failed to refresh the registration. If it's set to be less than or equal to 0, the retry will be disabled and onRegisterFailure callback triggered when retry failed.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  If registration to server succeeded, onRegisterSuccess will be triggered, otherwise onRegisterFailure triggered.
         */
        public Int32 registerServer(Int32 expires, Int32 retryTimes)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_registerServer(_LibSDK, expires, retryTimes);
        }


        /*!
         *  @brief Refresh the registration manually after successfully registered.
         *
         *  @param expires Registration refresh Interval with maximum 3600, in seconds. It will be inserted into SIP REGISTER message headers.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  If registration to server succeeded, onRegisterSuccess will be triggered, otherwise onRegisterFailure triggered.
         */
        Int32 refreshRegistration(Int32 regExpires)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_refreshRegistration(_LibSDK, regExpires);
        }

        /*!
         *  @brief Un-register from the SIP proxy server.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 unRegisterServer()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_unRegisterServer(_LibSDK);
        }


        /*!
        *  @brief Enable/disable rport(RFC3581).
        *
        *  @param enable Set to true to enable the SDK to support rport. By default it is enabled.
        *
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        */
        public Int32 enableRport(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableRport(_LibSDK, enable);
        }

        /*!
        *  @brief Enable/disable Early Media.
        *
        *  @param enable Set to true to enable the SDK to support Early Media. By default Early Media is disabled.
        *
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        */
        public Int32 enableEarlyMedia(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableEarlyMedia(_LibSDK, enable);
        }

        /*!
         *  @brief Enable/disable PRACK.
         *
         *  @param enable Set to true to enable the SDK to support PRACK. The PRACK is disabled by default.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 enableReliableProvisional(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableReliableProvisional(_LibSDK, enable);
        }

        /*!
         *  @brief Enable/disable the 3Gpp tags, including "ims.icsi.mmtel" and "g.3gpp.smsip".
         *
         *  @param enable Set to true to enable SDK to support 3Gpp tags.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 enable3GppTags(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enable3GppTags(_LibSDK, enable);
        }

        /*!
         *  @brief Enable/disable callback the sent SIP messages.
         *
         *  @param enable Set as true to enable callback the sent SIP messages, or false to disable. Once enabled, the "onSendingSignaling" event will be triggered when the SDK sends a SIP message.
         */
        public void enableCallbackSendingSignaling(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableCallbackSendingSignaling(_LibSDK, enable);
        }

        /*!
         *  @brief Enable/disable callback the received SIP messages.
         *
         *  @param enable Set as true to enable callback the received SIP messages, or false to disable. Once enabled, the "onReceivedSignaling" event will be triggered when the SDK receives a SIP message.
         */
        public void enableCallbackReceivedSignaling(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableCallbackReceivedSignaling(_LibSDK, enable);
        }

        /*!
         *  @brief Set the RTP callbacks to allow access to the sent and received RTP packets.
         *
         *  @param callbackObject The callback object that you passed in can be accessed once the callback function triggered.
         *  @param enable Set to true to enable the RTP callback for received and sent RTP packets. The onSendingRtpPacket and onReceivedRtpPacket events will be triggered.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setRtpCallback(Int32 callbackObject, Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            if (enable == true)
            {
                return PortSIP_NativeMethods.PortSIP_setRtpCallback(_LibSDK, (IntPtr)callbackObject, _rrc, _src);
            }

            return PortSIP_NativeMethods.PortSIP_setRtpCallback(_LibSDK, (IntPtr)callbackObject, null, null);
        }



        /** @} */
        // end of group2

        /** @defgroup group3 Audio and video codecs functions
         * @{
         */
        /*!
         *  @brief Enable an audio codec. It will be appears in SDP.
         *
         *  @param codecType Audio codec type.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 addAudioCodec(AUDIOCODEC_TYPE codecType)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_addAudioCodec(_LibSDK, (Int32)codecType);
        }

        /*!
         *  @brief Enable a video codec. It will appear in SDP.
         *
         *  @param codecType Video codec type.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 addVideoCodec(VIDEOCODEC_TYPE codecType)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_addVideoCodec(_LibSDK, (Int32)codecType);
        }


        /*!
         *  @brief Detect if enabled audio codecs is empty or not.
         *
         *  @return If no audio codec is enabled, it will return value true, otherwise false.
         */
        public Boolean isAudioCodecEmpty()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return true;
            }

            return PortSIP_NativeMethods.PortSIP_isAudioCodecEmpty(_LibSDK);
        }


        /*!
         *  @brief Detect if enabled video codecs is empty or not.
         *
         *  @return If no video codec is enabled, it will return value true, otherwise false.
         */
        public Boolean isVideoCodecEmpty()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return true;
            }

            return PortSIP_NativeMethods.PortSIP_isVideoCodecEmpty(_LibSDK);
        }

        /*!
         *  @brief Set the RTP payload type for dynamic audio codec.
         *
         *  @param codecType   Audio codec type, which is defined in the PortSIPTypes file.
         *  @param payloadType The new RTP payload type that you want to set.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
         */
        public Int32 setAudioCodecPayloadType(AUDIOCODEC_TYPE codecType, Int32 payloadType)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioCodecPayloadType(_LibSDK, (Int32)codecType, payloadType);
        }

        /*!
         *  @brief Set the RTP payload type for dynamic Video codec.
         *
         *  @param codecType   Video codec type, which is defined in the PortSIPTypes file.
         *  @param payloadType The new RTP payload type that you want to set.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoCodecPayloadType(VIDEOCODEC_TYPE codecType, Int32 payloadType)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoCodecPayloadType(_LibSDK, (Int32)codecType, payloadType);
        }


        /*!
         *  @brief Remove all enabled audio codecs.
         */
        public void clearAudioCodec()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_clearAudioCodec(_LibSDK);
        }

        /*!
         *  @brief Remove all enabled video codecs.
         */
        public void clearVideoCodec()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_clearVideoCodec(_LibSDK);
        }

        /*!
         *  @brief Set the codec parameter for audio codec.
         *
         *  @param codecType Audio codec type, defined in the PortSIPTypes file.
         *  @param parameter The parameter in string format.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         * @remark Example:
         *@code setAudioCodecParameter(AUDIOCODEC_AMR, "mode-set=0; octet-align=1; robust-sorting=0"); @endcode
         */
        public Int32 setAudioCodecParameter(AUDIOCODEC_TYPE codecType, String parameter)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioCodecParameter(_LibSDK, (Int32)codecType, parameter);
        }

        /*!
         *  @brief Set the codec parameter for video codec.
         *
         *  @param codecType Video codec type, defined in the PortSIPTypes file.
         *  @param parameter The parameter in string format.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
         * @remark Example:
         *@code setVideoCodecParameter(VIDEO_CODEC_H264, "profile-level-id=420033; packetization-mode=0"); @endcode
         */
        public Int32 setVideoCodecParameter(VIDEOCODEC_TYPE codecType, String parameter)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoCodecParameter(_LibSDK, (Int32)codecType, parameter);
        }
        /** @} */
        // end of group3

        /** @defgroup group4 Additional setting functions
         * @{
         */



        /*!
         *  @brief Set the SRTP policy.
         *
         *  @param srtpPolicy The SRTP policy.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
         */
        public Int32 setSrtpPolicy(SRTP_POLICY srtpPolicy, Boolean allowSrtpOverUnsecureTransport)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setSrtpPolicy(_LibSDK, (Int32)srtpPolicy, allowSrtpOverUnsecureTransport);
        }

        /*!
         *  @brief Set the RTP ports range for audio and video streaming.
         *
         *  @param minimumRtpAudioPort The minimum RTP port for audio stream.
         *  @param maximumRtpAudioPort The maximum RTP port for audio stream.
         *  @param minimumRtpVideoPort The minimum RTP port for video stream.
         *  @param maximumRtpVideoPort The maximum RTP port for video stream.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         *  The port range ((max - min) % maxCallLines) should be greater than 4.
         */
        public Int32 setRtpPortRange(Int32 minimumRtpAudioPort, Int32 maximumRtpAudioPort, Int32 minimumRtpVideoPort, Int32 maximumRtpVideoPort)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setRtpPortRange(_LibSDK,
                                                         minimumRtpAudioPort,
                                                         maximumRtpAudioPort,
                                                         minimumRtpVideoPort,
                                                         maximumRtpVideoPort);
        }

        /*!
         *  @brief Set the RTCP ports range for audio and video streaming.
         *
         *  @param minimumRtcpAudioPort The minimum RTCP port for audio stream.
         *  @param maximumRtcpAudioPort The maximum RTCP port for audio stream.
         *  @param minimumRtcpVideoPort The minimum RTCP port for video stream.
         *  @param maximumRtcpVideoPort The maximum RTCP port for video stream.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         *  The port range ((max - min) % maxCallLines) should be greater than 4.
         */
        public Int32 setRtcpPortRange(Int32 minimumRtcpAudioPort, Int32 maximumRtcpAudioPort, Int32 minimumRtcpVideoPort, Int32 maximumRtcpVideoPort)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setRtcpPortRange(_LibSDK,
                                                         minimumRtcpAudioPort,
                                                         maximumRtcpAudioPort,
                                                         minimumRtcpVideoPort,
                                                         maximumRtcpVideoPort);
        }

        /*!
         *  @brief Enable call forward.
         *
         *  @param forBusyOnly If this parameter is set as true, the SDK will forward all incoming calls when currently it's busy. If it's set as false, the SDK forward all incoming calls anyway.
         *  @param forwardTo   The call forward target. It must be like sip:xxxx@sip.portsip.com.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 enableCallForward(Boolean forBusyOnly, String forwardTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableCallForward(_LibSDK, forBusyOnly, forwardTo);
        }

        /*!
         *  @brief Disable the call forwarding. The SDK is not forwarding any incoming call after this function is called.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, the return value is a specific error code.
         */
        public Int32 disableCallForward()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_disableCallForward(_LibSDK);
        }

        /*!
         *  @brief Allows to periodically refresh Session Initiation Protocol (SIP) sessions by sending INVITE requests repeatedly.
         *
         *  @param timerSeconds The value of the refreshment interval in seconds. Minimum value of 90 seconds required.
         *  @param refreshMode  Allow to set the session refresh by UAC or UAS: SESSION_REFERESH_UAC or SESSION_REFERESH_UAS;
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark The repeated INVITE requests, or re-INVITEs, are sent during an active call log to allow user agents (UA) or proxies to determine the status of a SIP session. 
         *  Without this keepalive mechanism, proxies that remember incoming and outgoing requests (stateful proxies) may continue to retain call state in vain. 
         *  If a UA fails to send a BYE message at the end of a session or if the BYE message is lost because of network problems, a stateful proxy will not know that the session has ended. 
         *  The re-INVITES ensure that active sessions stay active and completed sessions are terminated.
         */
        public Int32 enableSessionTimer(Int32 timerSeconds, SESSION_REFRESH_MODE refreshMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableSessionTimer(_LibSDK, timerSeconds, (Int32)refreshMode);
        }

        /*!
         *  @brief Disable the session timer.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 disableSessionTimer()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_disableSessionTimer(_LibSDK);
        }

        /*!
         *  @brief Enable the "Do not disturb" to enable/disable.
         *
         *  @param state If it is set to true, the SDK will reject all incoming calls anyway.
         */
        public void setDoNotDisturb(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_setDoNotDisturb(_LibSDK, state);
        }


        /*!
         *  @brief Allows to enable/disable the check MWI (Message Waiting Indication) automatically.
         *
         *  @param state If it is set as true, MWI will be checked automatically once successfully registered to a SIP proxy server.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 enableAutoCheckMwi(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableAutoCheckMwi(_LibSDK, state);
        }

        /*!
         *  @brief Enable or disable to send RTP keep-alive packet when the call is established.
         *
         *  @param state                Set to true to allow to send the keep-alive packet during the conversation.
         *  @param keepAlivePayloadType The payload type of the keep-alive RTP packet. It's usually set to 126.
         *  @param deltaTransmitTimeMS  The keep-alive RTP packet sending interval, in millisecond. Recommend value ranges 15000 - 300000.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setRtpKeepAlive(Boolean state, Int32 keepAlivePayloadType, Int32 deltaTransmitTimeMS)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setRtpKeepAlive(_LibSDK, state, keepAlivePayloadType, deltaTransmitTimeMS);
        }

        /*!
         *  @brief Enable or disable to send SIP keep-alive packet.
         *
         *  @param keepAliveTime This is the SIP keep alive time interval in seconds. Set it to 0 to disable the SIP keep alive. Recommend to set as 30 or 50.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setKeepAliveTime(Int32 keepAliveTime)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setKeepAliveTime(_LibSDK, keepAliveTime);
        }



        /*!
         *  @brief Access the SIP header of SIP message.
         *
         *  @param sipMessage        The SIP message.
         *  @param headerName        The header which wishes to access the SIP message.
         *  @param headerValue       The buffer to receive header value.
         *  @param headerValueLength The headerValue buffer size. Usually we recommend to set it more than 512 bytes.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         * @remark
         * When receiving a SIP message in the onReceivedSignaling callback event, and wishes to get SIP message header value, please use getSipMessageHeaderValue:
         * @code
            StringBuilder value = new StringBuilder();
            value.Length = 512;
            getSipMessageHeaderValue(message, name, value);
         * @endcode
         */
        public Int32 getSipMessageHeaderValue(String sipMessage, String headerName, StringBuilder headerValue, Int32 headerValueLength)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getSipMessageHeaderValue(_LibSDK, sipMessage, headerName, headerValue, headerValueLength);
        }

        /*!
         *  @brief Add the SIP Message header into the specified outgoing SIP message.
         *
         *  @param sessionId Add the header to the SIP message with the specified session Id only.
         *                    By setting to -1, it will be added to all messages.
         *  @param methodName Just add the header to the SIP message with specified method name.
         *                    For example: "INVITE", "REGISTER", "INFO" etc. If "ALL" specified, it will add all SIP messages.
         *  @param msgType 1 refers to apply to the request message,
         *                 2 refers to apply to the response message,
         *                 3 refers to apply to both request and response.
         *  @param headerName  The custom header name that will appears in every outgoing SIP message.
         *  @param headerValue The custom header value.
         *
         *  @return If the function succeeds, it will return addedSipMessageId, which is greater than 0. If the function fails, it will return a specific error code.
         */
        public Int32 addSipMessageHeader(Int32 sessionId, String methodName, Int32 msgType, String headerName, String headerValue)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_addSipMessageHeader(_LibSDK, sessionId, methodName, msgType, headerName, headerValue);
        }

        /*!
        *  @brief Remove the headers (custom header) added by addSipMessageHeader.
        *
        *  @param addedSipMessageId The addedSipMessageId return by addSipMessageHeader.
        *
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        */
        public Int32 removeAddedSipMessageHeader(Int32 sipMessageHeaderId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_removeAddedSipMessageHeader(_LibSDK, sipMessageHeaderId);
        }

        /*!
         *  @brief Clear the added extension headers (custom headers)
         *
         @remark For example, we have added two custom headers into every outgoing SIP message and wish to remove them.
         @code
            addExtensionHeader(-1, "ALL", 3, "Blling", "usd100.00");	
            addExtensionHeader(-1, "ALL", 3, "ServiceId", "8873456");
            clearAddedSipMessageHeaders();
         @endcode
         */
        public Int32 clearAddedSipMessageHeaders()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_clearAddedSipMessageHeaders(_LibSDK);
        }

        /*!
        *  @brief Modify the special SIP header value for every outgoing SIP message.
        *
        *  @param sessionId The header to the SIP message with the specified session Id.
        *                    By setting to -1, it will be added to all messages.
        *  @param methodName Modify the header to the SIP message with specified method name only.
        *                    For example: "INVITE", "REGISTER", "INFO" etc. If "ALL" specified, it will add all SIP messages.
        *  @param msgType 1 refers to apply to the request message,
        *                 2 refers to apply to the response message,
        *                 3 refers to apply to both request and response.
        *
        *
        *  @return If the function succeeds, it will return modifiedSipMessageId, which is greater than 0. If the function fails, it will return a specific error code.
        */
        public Int32 modifySipMessageHeader(Int32 sessionId, String methodName, Int32 msgType, String headerName, String headerValue)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_modifySipMessageHeader(_LibSDK, sessionId, methodName, msgType, headerName, headerValue);
        }

        /*!
         *  @brief Remove the extension header (custom header) from every outgoing SIP message.
         *
         *  @param modifiedSipMessageId The modifiedSipMessageId is returned by modifySipMessageHeader.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 removeModifiedSipMessageHeader(Int32 sipMessageHeaderId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_removeModifiedSipMessageHeader(_LibSDK, sipMessageHeaderId);
        }

        /*!
         *  @brief Clear the modified headers value, and do not modify every outgoing SIP message header values any longer.
         *
         @remark  For example, to modify two headers' value for every outgoing SIP message and wish to clear it:
         @code
            modifySipMessageHeader(-1, "ALL", 3, "Expires", "1000");	
            modifySipMessageHeader(-1, "ALL", 3, "User-Agent", "MyTest Softphone 1.0"");
            clearModifiedSipMessageHeaders();
         @endcode
        */
        public Int32 clearModifiedSipMessageHeaders()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_clearModifiedSipMessageHeaders(_LibSDK);
        }


        /** @} */
        // end of group5

        /** @defgroup group6 Audio and video functions
         * @{
         */


        /*!
         *  @brief Set the SDK to receive the SIP messages that include special mime type.
         *
         *  @param methodName  Method name of the SIP message, such as INVITE, OPTION, INFO, MESSAGE, UPDATE, ACK etc. For more details please read the RFC3261.
         *  @param mimeType    The mime type of SIP message.
         *  @param subMimeType The sub mime type of SIP message.
         *  
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *@remark         
         * By default, PortSIP VoIP SDK supports these media types (mime types) below for incoming SIP messages:
         * @code
                        "message/sipfrag" in NOTIFY message.
                        "application/simple-message-summary" in NOTIFY message.
                        "text/plain" in MESSAGE message.
                        "application/dtmf-relay" in INFO message.
                        "application/media_control+xml" in INFO message.
         * @endcode
         * The SDK allows to receive SIP messages that include above mime types. Now if remote side send an INFO
         * SIP message with its "Content-Type" header value "text/plain". SDK will reject this INFO message,
         * as "text/plain" of INFO message is not included in the default support list.
         * How should we enable the SDK to receive the SIP INFO message that includes "text/plain" mime type? The answer is
         * addSupportedMimyType: 
         * @code
                        addSupportedMimeType("INFO", "text", "plain");
         * @endcode
         * If we want to receive the NOTIFY message with "application/media_control+xml", please: 
         *@code
                        addSupportedMimeType("NOTIFY", "application", "media_control+xml");
         * @endcode
         * For more details about the mime type, please visit this website: http://www.iana.org/assignments/media-types/ 
         */
        public Int32 addSupportedMimeType(String methodName, String mimeType, String subMimeType)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_addSupportedMimeType(_LibSDK, methodName, mimeType, subMimeType);
        }



        /*!
         *  @brief Set the audio capture sample.
         *
         *  @param ptime    It should be a multiple of 10 between 10 - 60 (with 10 and 60 inclusive).
         *  @param maxPtime For the "maxptime" attribute, it should be a multiple of 10 between 10 - 60 (with 10 and 60 inclusive). It cannot be less than "ptime".
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark It will appear in the SDP of INVITE and 200 OK message as "ptime and "maxptime" attribute.
         */
        public Int32 setAudioSamples(Int32 ptime, Int32 maxPtime)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioSamples(_LibSDK, ptime, maxPtime);
        }



        /** @} */
        // end of group4

        /** @defgroup group5 Access SIP message header functions
         * @{
         */



        /*!
         *  @brief Set the audio device that will be used for audio call. 
         *
         *  @param recordingDeviceId    Device ID (index) for audio recording. (Microphone). 
         *  @param playoutDeviceId      Device ID (index) for audio playback (Speaker). 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setAudioDeviceId(Int32 recordingDeviceId, Int32 playoutDeviceId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioDeviceId(_LibSDK, recordingDeviceId, playoutDeviceId);
        }

        /*!
         *  @brief Set the video device that will be used for video call.
         *
         *  @param deviceId Device ID (index) for video device (camera).
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoDeviceId(Int32 deviceId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoDeviceId(_LibSDK, deviceId);
        }

        /*!
         *  @brief Set the video capturing resolution.
         *
         *  @param width Video width.
         *  @param height Video height.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoResolution(Int32 width, Int32 height)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoResolution(_LibSDK, width, height);
        }

        /*!
 *  @brief Set the audio bitrate.
 *
 *  @param bitrateKbps The audio bitrate in KBPS.
 *
 *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
 */
        public Int32 setAudioBitrate(Int32 sessionId, AUDIOCODEC_TYPE audioCodecType, Int32 bitrateKbps)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioBitrate(_LibSDK, sessionId, (Int32)audioCodecType, bitrateKbps);
        }

        /*!
         *  @brief Set the video bitrate.
         *
         *  @param bitrateKbps The video bitrate in KBPS.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoBitrate(Int32 sessionId, Int32 bitrateKbps)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoBitrate(_LibSDK, sessionId, bitrateKbps);
        }

        /*!
         *  @brief Set the video frame rate. 
         *
         *  @param frameRate The frame rate value with minimum value 5, and maximum value 30. A greater value will enable you better video quality but requires more bandwidth.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark Usually you do not need to call this function to set the frame rate. The SDK uses default frame rate.
         */
        public Int32 setVideoFrameRate(Int32 sessionId, Int32 frameRate)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoFrameRate(_LibSDK, sessionId, frameRate);
        }

        /*!
         *  @brief Send the video to remote side.
         *
         *  @param sessionId The session ID of the call.
         *  @param sendState Set to true to send the video, or false to stop sending.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 sendVideo(Int32 sessionId, Boolean sendState)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendVideo(_LibSDK, sessionId, sendState);
        }

        /*!
         *  @brief Mute the device microphone. It's unavailable for Android and iOS.
         *
         *  @param mute If the value is set to true, the microphone will be muted. You may also set it to false to un-mute it.
         */
        public void muteMicrophone(Boolean mute)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_muteMicrophone(_LibSDK, mute);
        }

        /*!
         *  @brief Mute the device speaker. It's unavailable for Android and iOS.
         *
         *  @param mute If the value is set to true, the speaker is muted. You may also set it to false to un-mute it.
         */
        public void muteSpeaker(Boolean mute)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_muteSpeaker(_LibSDK, mute);
        }

        /*!
         * Set a volume |scaling| to be applied to the outgoing signal of a specific
         * audio channel. 
         * 
         * @param sessionId
         *            The session ID of the call.
         * @param scaling
         *            Valid scale ranges [0, 1000]. Default is 100.
         * @return If the function succeeds, it will return value 0. If the function
         *         fails, it will return a specific error code.
         */
        public void setChannelOutputVolumeScaling(Int32 sessionId, Int32 scaling)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_setChannelOutputVolumeScaling(_LibSDK, sessionId, scaling);
        }


        /*!
         *  @brief Set the window that is used to display the local video image.
         *
         *  @param localVideoWindow The window on which the local video image from camera will be displayed. 
         */
        public void setLocalVideoWindow(IntPtr localVideoWindow)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_setLocalVideoWindow(_LibSDK, localVideoWindow);
        }

        /*!
         *  @brief Set the window for a session that is used to display the received remote video image.
         *
         *  @param sessionId         The session ID of the call.
         *  @param remoteVideoWindow The window to display received remote video image. 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setRemoteVideoWindow(Int32 sessionId, IntPtr remoteVideoWindow)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setRemoteVideoWindow(_LibSDK, sessionId, remoteVideoWindow);
        }

        /*!
         *  @brief Start/stop displaying the local video image.
         *
         *  @param state Set to true to display local video image.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 displayLocalVideo(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_displayLocalVideo(_LibSDK, state);
        }

        /*!
         *  @brief Enable/disable the NACK feature (rfc6642) that helps to improve the video quality.
         *
         *  @param state Set to true to enable.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoNackStatus(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoNackStatus(_LibSDK, state);
        }



        /** @} */
        // end of group6

        /** @defgroup group7 Call functions
         * @{
         */

        /*!
         *  @brief Make a call
         *
         *  @param callee    The callee. It can be either name or full SIP URI. For example: user001, sip:user001@sip.iptel.org or sip:user002@sip.yourdomain.com:5068
         *  @param sendSdp   If it's set to false, the outgoing call doesn't include the SDP in INVITE message.
         *  @param videoCall If it's set to true with at least one video codecs added, the outgoing call will include the video codec into SDP.
         *
         *  @return If the function succeeds, it will return the session ID of the call that is greater than 0. If the function fails, it will return a specific error code. Note: the function success just means the outgoing call is being processed. You need to detect the call final state in onInviteTrying, onInviteRinging, onInviteFailure callback events.
         */
        public Int32 call(String callee, Boolean sendSdp, Boolean videoCall)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.INVALID_SESSION_ID;
            }

            return PortSIP_NativeMethods.PortSIP_call(_LibSDK, callee, sendSdp, videoCall);

        }

        /*!
         *  @brief rejectCall Reject the incoming call.
         *
         *  @param sessionId The sessionId of the call.
         *  @param code      Reject code. For example, 486, 480 etc.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 rejectCall(Int32 sessionId, int code)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_rejectCall(_LibSDK, sessionId, code);
        }

        /*!
         *  @brief hangUp Hang up the call.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 hangUp(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_hangUp(_LibSDK, sessionId);
        }

        /*!
         *  @brief answerCall Answer the incoming call.
         *
         *  @param sessionId The session ID of call.
         *  @param videoCall If the incoming call is a video call and the video codec is matched, set it to true to answer the video call.<br>If it's set to false, the answered call will not include video codec answer anyway.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 answerCall(Int32 sessionId, Boolean videoCall)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_answerCall(_LibSDK, sessionId, videoCall);
        }

        /*!
         *  @brief Use the re-INVITE to update the established call.
         *  @param sessionId   The session ID of call.
         *  @param enableAudio Set to true to allow the audio in updated call, or false to disable audio in updated call.
         *  @param enableVideo Set to true to allow the video in updated call, or false to disable video in updated call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
         *  @remark
            Example usage:<br>
         *  Example 1: A called B with the audio only, B answered A, there has an audio conversation between A, B. Now A wants to see B visually, 
            A could use these functions to do it.
            @code
                        clearVideoCodec();	
                        addVideoCodec(VIDEOCODEC_H264);
                        updateCall(sessionId, true, true);
            @endcode
            Example 2: Remove video stream from current conversation. 
            @code
                        updateCall(sessionId, true, false);
            @endcode
         */
        public Int32 updateCall(Int32 sessionId, bool enableAudio, bool enableVideo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_updateCall(_LibSDK, sessionId, enableAudio, enableVideo);
        }

        /*!
         *  @brief To place a call on hold.
         *
         *  @param sessionId The session ID of call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 hold(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_hold(_LibSDK, sessionId);
        }

        /*!
         *  @brief Take off hold.
         *
         *  @param sessionId The session ID of call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 unHold(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_unHold(_LibSDK, sessionId);
        }

        /*!
         *  @brief Mute the specified session audio or video.
         *
         *  @param sessionId         The session ID of the call.
         *  @param muteIncomingAudio Set it to true to mute incoming audio stream, and remote side audio cannot be heard.
         *  @param muteOutgoingAudio Set it to true to mute outgoing audio stream, and the remote side can't hear the audio.
         *  @param muteIncomingVideo Set it to true to mute incoming video stream, and the remote side video will be invisible.
         *  @param muteOutgoingVideo Set it to true to mute outgoing video stream, and the remote side can't see the video.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 muteSession(Int32 sessionId,
                                Boolean muteIncomingAudio,
                                Boolean muteOutgoingAudio,
                                Boolean muteIncomingVideo,
                                Boolean muteOutgoingVideo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_muteSession(_LibSDK, sessionId, muteIncomingAudio, muteOutgoingAudio, muteIncomingVideo, muteOutgoingVideo);
        }

        /*!
         *  @brief Forward call to another one when receiving the incoming call.
         *
         *  @param sessionId The session ID of the call.
         *  @param forwardTo Target of the forwarding. It can be "sip:number@sipserver.com" or "number" only.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return value a specific error code.
         */
        public Int32 forwardCall(Int32 sessionId, String forwardTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_forwardCall(_LibSDK, sessionId, forwardTo);
        }



        /*!
        *  @brief This function will be used for picking up a call based on the BLF (Busy Lamp Field) status.
        *
        *  @param replaceDialogId The session ID of the call.
        *  @param videoCall Target of the forwarding. It can be "sip:number@sipserver.com" or "number" only.
        *
        *  @return If the function succeeds, it will return a session ID that is greater than 0 to the new call,
	    * otherwise returns a specific error code that is less than 0.
        *  @remark
            The scenario is:<br>
        *   1. User 101 subscribed the user 100's call status: sendSubscription(mSipLib, "100", "dialog");
        *   2. When 100 hold a call or 100 is ringing, onDialogStateUpdated callback will be triggered,
        *   and 101 will receive this callback. Now 101 can use pickupBLFCall function to pick the call rather than 
        *   100 to talk with caller.
        */
        public Int32 pickupBLFCall(String replaceDialogId, Boolean videoCall)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_pickupBLFCall(_LibSDK, replaceDialogId, videoCall);
        }



        /*!
         *  @brief Send DTMF tone.
         *
         *  @param sessionId    The session ID of the call.
         *  @param dtmfMethod   DTMF tone could be sent with two methods: DTMF_RFC2833 and DTMF_INFO, of which DTMF_RFC2833 is recommend.
         *  @param code         The DTMF tone (0-16).
         * <p><table>
         * <tr><th>code</th><th>Description</th></tr>
         * <tr><td>0</td><td>The DTMF tone 0.</td></tr><tr><td>1</td><td>The DTMF tone 1.</td></tr><tr><td>2</td><td>The DTMF tone 2.</td></tr>
         * <tr><td>3</td><td>The DTMF tone 3.</td></tr><tr><td>4</td><td>The DTMF tone 4.</td></tr><tr><td>5</td><td>The DTMF tone 5.</td></tr>
         * <tr><td>6</td><td>The DTMF tone 6.</td></tr><tr><td>7</td><td>The DTMF tone 7.</td></tr><tr><td>8</td><td>The DTMF tone 8.</td></tr>
         * <tr><td>9</td><td>The DTMF tone 9.</td></tr><tr><td>10</td><td>The DTMF tone *.</td></tr><tr><td>11</td><td>The DTMF tone #.</td></tr>
         * <tr><td>12</td><td>The DTMF tone A.</td></tr><tr><td>13</td><td>The DTMF tone B.</td></tr><tr><td>14</td><td>The DTMF tone C.</td></tr>
         * <tr><td>15</td><td>The DTMF tone D.</td></tr><tr><td>16</td><td>The DTMF tone FLASH.</td></tr>
         * </table></p>
         *  @param dtmfDuration The DTMF tone samples. Recommended value 160.
         *  @param playDtmfTone If it is set to true, the SDK plays local DTMF tone sound when sending DTMF.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 sendDtmf(Int32 sessionId, DTMF_METHOD dtmfMethod, int code, int dtmfDuration, bool playDtmfTone)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendDtmf(_LibSDK, sessionId, (Int32)dtmfMethod, code, dtmfDuration, playDtmfTone);
        }
        /** @} */
        // end of group7

        /** @defgroup group8 Refer functions
         * @{
         */

        /*!
         *  @brief Refer the current call to another one.<br>
         *  @param sessionId The session ID of the call.
         *  @param referTo   Target of the refer, which can be either "sip:number@sipserver.com" or "number".
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         @code
            refer(sessionId, "sip:testuser12@sip.portsip.com");
         @endcode
         You can download the demo AVI at <br>
         "http://www.portsip.com/downloads/video/blindtransfer.rar". <br>Use the Windows Media
         Player to play the AVI file after extracted, and it will demonstrate the transfer.
         */
        public Int32 refer(Int32 sessionId, String referTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_refer(_LibSDK, sessionId, referTo);
        }

        /*!
         *  @brief  Make an attended refer.
         *
         *  @param sessionId        The session ID of the call.
         *  @param replaceSessionId Session ID of the repferred call.
         *  @param referTo          Target of the refer, which can be either "sip:number@sipserver.com" or "number".
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
            Please read the sample project source code for more details, or download the demo AVI at: http://www.portsip.com/downloads/video/blindtransfer.rar<br>
           Please use the Windows Media Player to play the AVI file after extracted, and it will demonstrate the transfer.
         */
        public Int32 attendedRefer(Int32 sessionId, Int32 replaceSessionId, String referTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_attendedRefer(_LibSDK, sessionId, replaceSessionId, referTo);
        }

        /*!
         *  @brief  Make an attended refer with specified request line and specified method embedded into the "Refer-To" header.
         *
         *  @param sessionId        Session ID of the call.
         *  @param replaceSessionId Session ID of the replaced call.
         *  @param replaceMethod    The SIP method name which will be embeded in the "Refer-To" header, usually INVITE or BYE.
         *  @param target           The target to which the REFER message will be sent. It appears in the "Request Line" of REFER message.
         *  @param referTo          Target of the refer that appears in the "Refer-To" header. It can be either "sip:number@sipserver.com" or "number".
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         Please refer to the sample project source code for more details. Or you can watch the video on YouTube at https://www.youtube.com/watch?v=_2w9EGgr3FY.
         It will demonstrate the transmission.
         */
        public Int32 attendedRefer2(IntPtr libSDK,
                                    Int32 sessionId,
                                    Int32 replaceSessionId,
                                    String replaceMethod,
                                    String target,
                                    String referTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_attendedRefer2(_LibSDK, sessionId, replaceSessionId, replaceMethod, target, referTo);
        }

        /*!
         *  @brief  Send an out of dialog REFER to replace the specified call.
         *
         *  @param replaceSessionId The session ID of the session which will be replaced.
         *  @param replaceMethod    The SIP method name which will be added in the "Refer-To" header, usually INVITE or BYE.
         *  @param target           The target to which the REFER message will be sent.
         *  @param referTo          The URI to be added into the "Refer-To" header.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 outOfDialogRefer(Int32 replaceSessionId, String replaceMethod, String target, String referTo)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_outOfDialogRefer(_LibSDK, replaceSessionId, replaceMethod, target, referTo);
        }

        /*!
         *  @brief Accept the REFER request, and a new call will be made if called this function. The function is usually called after onReceivedRefer callback event.
         *
         *  @param referId        The ID of REFER request that comes from onReceivedRefer callback event.
         *  @param referSignalingMessage The SIP message of REFER request that comes from onReceivedRefer callback event.
         *
         *  @return If the function succeeds, it will return a session ID greater than 0 to the new call for REFER; otherwise a specific error code less than 0.
         */
        public Int32 acceptRefer(Int32 referId, String referSignalingMessage)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_acceptRefer(_LibSDK, referId, referSignalingMessage);
        }

        /*!
         *  @brief Reject the REFER request.
         *
         *  @param referId The ID of REFER request that comes from onReceivedRefer callback event.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 rejectRefer(Int32 referId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_rejectRefer(_LibSDK, referId);
        }
        /** @} */
        // end of group8

        /** @defgroup group9 Send audio and video stream functions
         * @{
         */

        /*!
         *  @brief Enable the SDK to send PCM stream data to remote side from another source instead of microphone.
         *
         *  @param sessionId           The session ID of call.
         *  @param state               Set to true to enable the send stream, or false to disable.
         *  @param streamSamplesPerSec The PCM stream data sample in seconds. For example: 8000 or 16000.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark This function MUST be called first to send the PCM stream data to another side.
         */
        public Int32 enableSendPcmStreamToRemote(Int32 sessionId, Boolean state, Int32 streamSamplesPerSec)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableSendPcmStreamToRemote(_LibSDK, sessionId, state, streamSamplesPerSec);
        }

        /*!
         *  @brief Send the audio stream in PCM format from another source instead of audio device capturing (microphone).
         *
         *  @param sessionId Session ID of the call conversation.
         *  @param data        The PCM audio stream data. It must be 16bit, mono.
         *  @param dataLength  The size of data. 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark Usually it should be used as below:
         *  @code
                        enableSendPcmStreamToRemote(sessionId, true, 16000);	
                        sendPcmStreamToRemote(sessionId, data, dataSize);
         *  @endcode
         */
        public Int32 sendPcmStreamToRemote(Int32 sessionId, byte[] data, Int32 dataLength)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendPcmStreamToRemote(_LibSDK, sessionId, data, dataLength);
        }

        /*!
         *  @brief Enable the SDK send video stream data to remote side from another source instead of camera.
         *
         *  @param sessionId The session ID of call.
         *  @param state     Set to true to enable the sending stream, or false to disable.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 enableSendVideoStreamToRemote(Int32 sessionId, Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableSendVideoStreamToRemote(_LibSDK, sessionId, state);
        }

        /*!
         *  @brief Send the video stream to remote side.
         *
         *  @param sessionId Session ID of the call conversation.
         *  @param data      The video stream data. It must be in i420 format.
         *  @param dataLength The size of data. 
         *  @param width     The video image width.
         *  @param height    The video image height.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark  Send the video stream in i420 from another source instead of video device capturing (camera).<br>
         Before calling this function, you MUST call the enableSendVideoStreamToRemote function.<br>
         * Usually it should be used as below:
         @code
                    enableSendVideoStreamToRemote(sessionId, true);	
                    sendVideoStreamToRemote(sessionId, data, dataSize, 352, 288);
         @endcode
         */
        public Int32 sendVideoStreamToRemote(Int32 sessionId, byte[] data, Int32 dataLength, Int32 width, Int32 height)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendVideoStreamToRemote(_LibSDK, sessionId, data, dataLength, width, height);
        }

        /** @} */
        // end of group9

        /** @defgroup group10 RTP packets, Audio stream and video stream callback functions
         * @{
         */


        /*!
         *  @brief Enable/disable the audio stream callback
         *
         *  @param callbackObject The callback object that you passed in can be accessed once callback function triggered.
         *  @param sessionId    The session ID of call.
         *  @param enable       Set to true to enable audio stream callback, or false to stop the callback.
         *  @param callbackMode The audio stream callback mode
         * <p><table>
         * <tr><th>Mode</th><th>Description</th></tr>
         * <tr><td>AUDIOSTREAM_LOCAL_MIX</td><td>Callback the audio stream from microphone for all channels.  </td></tr>
         * <tr><td>AUDIOSTREAM_LOCAL_PER_CHANNEL</td><td>Callback the audio stream from microphone for one channel based on the given sessionId. </td></tr>
         * <tr><td>AUDIOSTREAM_REMOTE_MIX</td><td>Callback the received audio stream that mixed all included channels. </td></tr>
         * <tr><td>AUDIOSTREAM_REMOTE_PER_CHANNEL</td><td>Callback the received audio stream for one channel based on the given sessionId.</td></tr>
         * </table></p>
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark The onAudioRawCallback event will be triggered if the callback is enabled.
         */
        public Int32 enableAudioStreamCallback(Int32 callbackObject, Int32 sessionId, Boolean enable, AUDIOSTREAM_CALLBACK_MODE callbackMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_enableAudioStreamCallback(_LibSDK, sessionId, enable, (Int32)callbackMode, (IntPtr)callbackObject, _arc);
        }

        /*!
         *  @brief Enable/disable the video stream callback.
         *
         *  @param callbackObject The callback object that you passed in can be accessed once callback function triggered.
         *  @param sessionId    The session ID of call.
         *  @param callbackMode The video stream callback mode.
         * <p><table>
         * <tr><th>Mode</th><th>Description</th></tr>
         * <tr><td>VIDEOSTREAM_NONE</td><td>Disable video stream callback. </td></tr>
         * <tr><td>VIDEOSTREAM_LOCAL</td><td>Local video stream callback. </td></tr>
         * <tr><td>VIDEOSTREAM_REMOTE</td><td>Remote video stream callback. </td></tr>
         * <tr><td>VIDEOSTREAM_BOTH</td><td>Both local and remote video stream callback. </td></tr>
         * </table></p>
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark The onVideoRawCallback event will be triggered if the callback is enabled.
         */
        public Int32 enableVideoStreamCallback(Int32 callbackObject, Int32 sessionId, VIDEOSTREAM_CALLBACK_MODE callbackMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }


            return PortSIP_NativeMethods.PortSIP_enableVideoStreamCallback(_LibSDK, sessionId, (Int32)callbackMode, (IntPtr)callbackObject, _vrc);
        }


        /** @} */
        // end of group10

        /** @defgroup group11 Record functions
         * @{
         */

        /*!
         *  @brief Start recording the call.
         *
         *  @param sessionId        The session ID of call conversation.
         *  @param recordFilePath   The file path to which the record file will be saved. It must be existent.
         *  @param recordFileName   The file name of record file. For example: audiorecord.wav or videorecord.avi.
         *  @param appendTimestamp  Set to true to append the timestamp to the recording file name.
         *  @param audioFileFormat  The audio record file format.
         *  @param audioRecordMode  The audio record mode.
         *  @param videoFileCodecType The codec which is used for compressing the video data to save into video record file.
         *  @param videoRecordMode  Allow to set video record mode, with record received and/or sent supported.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 startRecord(Int32 sessionId,
                                String recordFilePath,
                                String recordFileName,
                                Boolean appendTimestamp,
                                AUDIO_RECORDING_FILEFORMAT audioFileFormat,
                                RECORD_MODE audioRecordMode,
                                VIDEOCODEC_TYPE videoFileCodecType,
                                RECORD_MODE videoRecordMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_startRecord(_LibSDK,
                                                            sessionId,
                                                            recordFilePath,
                                                            recordFileName,
                                                            appendTimestamp,
                                                            (Int32)audioFileFormat,
                                                            (Int32)audioRecordMode,
                                                            (Int32)videoFileCodecType,
                                                            (Int32)videoRecordMode);
        }

        /*!
         *  @brief Stop record.
         *
         *  @param sessionId The session ID of call conversation.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 stopRecord(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_stopRecord(_LibSDK, sessionId);
        }
        /** @} */
        // end of group11

        /** @defgroup group12 Play audio and video file to remote functions
         * @{
         */


        /*!
         *  @brief Play an AVI file to remote party.
         *
         *  @param sessionId Session ID of the call.
         *  @param fileName   The full file path, such as "c:\\test.avi".
         *  @param loop      Set to false to stop playing video file when it is ended, or true to play it repeatedly.
         *  @param playAudio If it's set to true, audio and video will be played together; if false, only video will be played.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 playVideoFileToRemote(Int32 sessionId, String fileName, Boolean loop, Boolean playAudio)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_playVideoFileToRemote(_LibSDK, sessionId, fileName, loop, playAudio);
        }

        /*!
         *  @brief Stop playing video file to remote side.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 stopPlayVideoFileToRemote(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_stopPlayVideoFileToRemote(_LibSDK, sessionId);
        }

        /*!
         *  @brief Play a wave file to remote party.
         *
         *  @param sessionId         Session ID of the call.
         *  @param fileName          The full filepath, such as "c:\\test.wav".
         *  @param fileSamplesPerSec The wave file sample in seconds. It should be 8000, 16000 or 32000.
         *  @param loop              Set to false to stop playing audio file when it is ended, or true to play it repeatedly.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 playAudioFileToRemote(Int32 sessionId, String fileName, Int32 fileSamplesPerSec, Boolean loop)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_playAudioFileToRemote(_LibSDK, sessionId, fileName, fileSamplesPerSec, loop);
        }

        /*!
         *  @brief Stop playing wave file to remote side.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 stopPlayAudioFileToRemote(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_stopPlayAudioFileToRemote(_LibSDK, sessionId);
        }

        /*!
         *  @brief Play a wave file to remote party as conversation background sound.
         *
         *  @param sessionId         Session ID of the call.
         *  @param fileName          The full filepath, such as "c:\\test.wav".
         *  @param fileSamplesPerSec The wave file sample in seconds. It should be 8000, 16000 or 32000.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 playAudioFileToRemoteAsBackground(Int32 sessionId, String fileName, Int32 fileSamplesPerSec)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_playAudioFileToRemoteAsBackground(_LibSDK, sessionId, fileName, fileSamplesPerSec);
        }

        /*!
         *  @brief Stop playing wave file to remote party as conversation background sound.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 stopPlayAudioFileToRemoteAsBackground(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull; ;
            }

            return PortSIP_NativeMethods.PortSIP_stopPlayAudioFileToRemoteAsBackground(_LibSDK, sessionId);
        }

        /** @} */
        // end of group12

        /** @defgroup group13 Conference functions
         * @{
         */

        /*!
         *  @brief Create an audio conference. It will be failed if the existent conference is not ended yet.
         *  
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 createAudioConference()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_createAudioConference(_LibSDK);
        }

        /*!
         *  @brief Create a video conference. It will be failed if the existent conference is not ended yet.
         *
         *  @param conferenceVideoWindow         The UIView used to display the conference video.
         *  @param videoResolution               The conference video resolution.
         *  @param displayLocalVideoInConference Display the local video on video window or not. 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 createVideoConference(IntPtr conferenceVideoWindow, Int32 width, Int32 height, Boolean displayLocalVideoInConference)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_createVideoConference(_LibSDK, conferenceVideoWindow, width, height, displayLocalVideoInConference);
        }

        /*!
         *  @brief End the existent conference.
         *  
         * 
         */
        public void destroyConference()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_destroyConference(_LibSDK);
        }

        /*!
         *  @brief Set the window for a conference that is used to display the received remote video image.
         *
         *  @param videoWindow The UIView used to display the conference video.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setConferenceVideoWindow(IntPtr videoWindow)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setConferenceVideoWindow(_LibSDK, videoWindow);
        }

        /*!
         *  @brief Join a session into existent conference. If the call is in hold, it will be un-hold automatically.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 joinToConference(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_joinToConference(_LibSDK, sessionId);
        }

        /*!
         *  @brief Remove a session from an existent conference.
         *
         *  @param sessionId Session ID of the call.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 removeFromConference(Int32 sessionId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_removeFromConference(_LibSDK, sessionId);
        }

        /** @} */
        // end of group13

        /** @defgroup group14 RTP and RTCP QOS functions
         * @{
         */

        /*!
         *  @brief Set the audio RTCP bandwidth parameters to the RFC3556.
         *
         *  @param sessionId The session ID of call conversation.
         *  @param BitsRR    The bits for the RR parameter.
         *  @param BitsRS    The bits for the RS parameter.
         *  @param KBitsAS   The Kbits for the AS parameter.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setAudioRtcpBandwidth(Int32 sessionId,
                                           Int32 BitsRR,
                                           Int32 BitsRS,
                                           Int32 KBitsAS)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioRtcpBandwidth(_LibSDK, sessionId, BitsRR, BitsRS, KBitsAS);
        }

        /*!
         *  @brief Set the video RTCP bandwidth parameters as the RFC3556.
         *
         *  @param sessionId The session ID of call conversation.
         *  @param BitsRR    The bits for the RR parameter.
         *  @param BitsRS    The bits for the RS parameter.
         *  @param KBitsAS   The Kbits for the AS parameter.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoRtcpBandwidth(Int32 sessionId,
                                          Int32 BitsRR,
                                          Int32 BitsRS,
                                          Int32 KBitsAS)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoRtcpBandwidth(_LibSDK, sessionId, BitsRR, BitsRS, KBitsAS);
        }


        /** @} */
        // end of group14

        /** @defgroup group15 RTP statistics functions
         * @{
         */

        /*!
         *  @brief Obtain the statistics of audio channel.
         *
         *  @param sessionId          The session ID of call conversation.
         *  @param sendBytes          The number of sent bytes.
         *  @param sendPackets        The number of sent packets.
         *  @param sendPacketsLost    The number of sent but lost packets.
         *  @param sendFractionLost   Fraction of sent but lost packets in percentage.
         *  @param sendRttMS          The round-trip time of the session, in milliseconds.
         *  @param sendCodecType      The sent Audio codec Type.
         *  @param sendJitterMS       The sent jitter, in milliseconds.
         *  @param sendAudioLevel     The sent audio level.It ranges 0 - 9.
         *  @param recvBytes          The number of received bytes.
         *  @param recvPackets        The number of received packets.
         *  @param recvPacketsLost    The number of received but lost packet.
         *  @param recvFractionLost   Fraction of received but lost packet in percentage.
         *  @param recvCodecType      Received Audio codec Type.
         *  @param recvJitterMS       The received jitter, in milliseconds.
         *  @param recvAudioLevel     The received audio level. It ranges 0 - 9.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 getAudioStatistics(Int32 sessionId,
                                                                        out Int32 sendBytes,
                                                                        out Int32 sendPackets,
                                                                        out Int32 sendPacketsLost,
                                                                        out Int32 sendFractionLost,
                                                                        out Int32 sendRttMS,
                                                                        out Int32 sendCodecType,
                                                                        out Int32 sendJitterMS,
                                                                        out Int32 sendAudioLevel,
                                                                        out Int32 recvBytes,
                                                                        out Int32 recvPackets,
                                                                        out Int32 recvPacketsLost,
                                                                        out Int32 recvFractionLost,
                                                                        out Int32 recvCodecType,
                                                                        out Int32 recvJitterMS,
                                                                        out Int32 recvAudioLevel)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                sendBytes = 0;
                sendPackets = 0;
                sendPacketsLost = 0;
                sendFractionLost = 0;
                sendRttMS = 0;
                sendCodecType = 0;
                sendJitterMS = 0;
                sendAudioLevel = 0;
                recvBytes = 0;
                recvPackets = 0;
                recvPacketsLost = 0;
                recvFractionLost = 0;
                recvCodecType = 0;
                recvJitterMS = 0;
                recvAudioLevel = 0;


                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getAudioStatistics(_LibSDK,
                                                                       sessionId,
                                                                       out sendBytes,
                                                                       out sendPackets,
                                                                       out sendPacketsLost,
                                                                       out sendFractionLost,
                                                                       out sendRttMS,
                                                                       out sendCodecType,
                                                                       out sendJitterMS,
                                                                       out sendAudioLevel,
                                                                       out recvBytes,
                                                                       out recvPackets,
                                                                       out recvPacketsLost,
                                                                       out recvFractionLost,
                                                                       out recvCodecType,
                                                                       out recvJitterMS,
                                                                       out recvAudioLevel);
        }

        /*!
         *  @brief Obtain the RTP statisics of video.
         *
         *  @param sessionId       The session ID of call conversation.
         *  @param sendBytes          The number of sent bytes.
         *  @param sendPackets        The number of sent packets.
         *  @param sendPacketsLost    The number of sent but lost packets.
         *  @param sendFractionLost   Fraction of sent but lost packets in percentage.
         *  @param sendRttMS          The round-trip time of the session, in milliseconds.
         *  @param sendCodecType      The sent Video codec type.
         *  @param sendFrameWidth     Frame width for the sent video.
         *  @param sendFrameHeight    Frame height for the sent video.
         *  @param sendBitrateBPS     Bitrate in BPS for the sent video.
         *  @param sendFramerate      Frame rate for the sent video.
         *  @param recvBytes          The number of received bytes.
         *  @param recvPackets        The number of received packets.
         *  @param recvPacketsLost    The number of received but lost packet.
         *  @param recvFractionLost   Fraction of received but lost packet in percentage.
         *  @param recvCodecType      Received Video codec type.
         *  @param recvFrameWidth     Frame width for the received video.
         *  @param recvFrameHeight    Frame height for the received video.
         *  @param recvBitrateBPS     (This parameter is not implemented yet) Bitrate in BPS for the received video.
         *  @param recvFramerate      Framerate for the received video.
         *
         *  @return  If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 getVideoStatistics(Int32 sessionId,
                                           out Int32 sendBytes,
                                           out Int32 sendPackets,
                                           out Int32 sendPacketsLost,
                                           out Int32 sendFractionLost,
                                           out Int32 sendRttMS,
                                           out Int32 sendCodecType,
                                           out Int32 sendFrameWidth,
                                           out Int32 sendFrameHeight,
                                           out Int32 sendFramerate,
                                           out Int32 recvBytes,
                                           out Int32 recvPackets,
                                           out Int32 recvPacketsLost,
                                           out Int32 recvFractionLost,
                                           out Int32 recvCodecType,
                                           out Int32 recvFrameWidth,
                                           out Int32 recvFrameHeight,
                                           out Int32 recvBitrateBPS,
                                           out Int32 recvFramerate)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                sendBytes = 0;
                sendPackets = 0;
                sendPacketsLost = 0;
                sendFractionLost = 0;
                sendRttMS = 0;
                sendCodecType = 0;
                sendFrameWidth = 0;
                sendFrameHeight = 0;
                sendFramerate = 0;
                recvBytes = 0;
                recvPackets = 0;
                recvPacketsLost = 0;
                recvFractionLost = 0;
                recvCodecType = 0;
                recvFrameWidth = 0;
                recvFrameHeight = 0;
                recvBitrateBPS = 0;
                recvFramerate = 0;


                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getVideoStatistics(_LibSDK,
                                                                       sessionId,
                                                                       out sendBytes,
                                                                       out sendPackets,
                                                                       out sendPacketsLost,
                                                                       out sendFractionLost,
                                                                       out sendRttMS,
                                                                       out sendCodecType,
                                                                       out sendFrameWidth,
                                                                       out sendFrameHeight,
                                                                       out sendFramerate,
                                                                       out recvBytes,
                                                                       out recvPackets,
                                                                       out recvPacketsLost,
                                                                       out recvFractionLost,
                                                                       out recvCodecType,
                                                                       out recvFrameWidth,
                                                                       out recvFrameHeight,
                                                                       out recvBitrateBPS,
                                                                       out recvFramerate);
        }

        /** @} */
        // end of group15

        /** @defgroup group16 Audio effect functions
         * @{
         */

        /*!
         *  @brief Enable/disable Voice Activity Detection (VAD).
         *
         *  @param state Set to true to enable VAD, or false to disable.
         */
        public void enableVAD(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableVAD(_LibSDK, state);
        }

        /*!
         *  @brief Enable/disable AEC (Acoustic Echo Cancellation).
         *
         *  @param ecMode Allow to set the AEC mode to influence different scenarios.
         *  
         * <p><table>
         * <tr><th>Mode</th><th>Description</th></tr>
         * <tr><td>EC_NONE</td><td>Disable AEC. </td></tr>
         * <tr><td>EC_DEFAULT</td><td>Platform default AEC. </td></tr>
         * <tr><td>EC_CONFERENCE</td><td>Desktop platform (Windows, MAC) Conferencing default (aggressive AEC). </td></tr>
         * </table></p>
         * 
         */
        public void enableAEC(EC_MODES ecMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableAEC(_LibSDK, (Int32)ecMode);
        }

        /*!
         *  @brief Enable/disable Comfort Noise Generator (CNG).
         *
         *  @param state Set it to true to enable CNG, or false to disable.
         */
        public void enableCNG(Boolean state)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableCNG(_LibSDK, state);
        }

        /*!
         *  @brief Enable/disable Automatic Gain Control (AGC).
         *
         *  @param agcMode  Allow to set the AGC mode to influence different scenarios.
         *  
         * <p><table>
         * <tr><th>Mode</th><th>Description</th></tr>
         * <tr><td>AGC_DEFAULT</td><td>Disable AGC. </td></tr>
         * <tr><td>AGC_DEFAULT</td><td>Platform default. </td></tr>
         * <tr><td>AGC_ADAPTIVE_ANALOG</td><td>Desktop platform (Windows, MAC) adaptive mode for use when analog volume control exists. </td></tr>
         * <tr><td>AGC_ADAPTIVE_DIGITAL</td><td>Scaling takes place in the digital domain (e.g. for conference servers and embedded devices). </td></tr>
         * <tr><td>AGC_FIXED_DIGITAL</td><td>It can be used on embedded devices where the capture signal level is predictable. </td></tr>
         * </table></p>
         */
        public void enableAGC(AGC_MODES agcMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableAGC(_LibSDK, (Int32)agcMode);
        }

        /*!
         *  @brief Enable/disable Audio Noise Suppression (ANS).
         *
         *  @param nsMode Allow to set the NS mode to influence different scenarios.
         *  
         * <p><table>
         * <tr><th>Mode                     </th><th>Description</th></tr>
         * <tr><td>NS_NONE                  </td><td>Disable NS. </td></tr>
         * <tr><td>NS_DEFAULT               </td><td>Platform default. </td></tr>
         * <tr><td>NS_Conference            </td><td>Conferencing default. </td></tr>
         * <tr><td>NS_LOW_SUPPRESSION       </td><td>Lowest suppression. </td></tr>
         * <tr><td>NS_MODERATE_SUPPRESSION  </td><td>Moderate suppression. </td></tr>
         * <tr><td>NS_HIGH_SUPPRESSION      </td><td>High suppression </td></tr>
         * <tr><td>NS_VERY_HIGH_SUPPRESSION </td><td>Highest suppression. </td></tr>
         * </table></p>
         */
        public void enableANS(NS_MODES nsMode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_enableANS(_LibSDK, (Int32)nsMode);
        }



        /*!
         *  @brief Set the DSCP (differentiated services code point) value of QoS (Quality of Service) for audio channel.
         *
         *  @param state        Set to true to enable audio QoS.
         *  @param DSCPValue    The six-bit DSCP value. Valid value ranges 0-63. As defined in RFC 2472, the DSCP value is the high-order 6 bits of the IP version 4 (IPv4) TOS field and the IP version 6 (IPv6) Traffic Class field.
         *  @param priority     The 802.1p priority (PCP) field in an 802.1Q/VLAN tag. Values 0-7 indicate the priority, while value -1 leaves the priority setting unchanged.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setAudioQos(Boolean state, Int32 DSCPValue, Int32 priority)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setAudioQos(_LibSDK, state, DSCPValue, priority);
        }

        /*!
         *  @brief Set the DSCP (differentiated services code point) value of QoS (Quality of Service) for video channel.
         *
         *  @param state    Set as true to enable QoS, or false to disable.
         *  @param DSCPValue The six-bit DSCP value. Valid value ranges 0-63. As defined in RFC 2472, the DSCP value is the high-order 6 bits of the IP version 4 (IPv4) TOS field and the IP version 6 (IPv6) Traffic Class field.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoQos(Boolean state, Int32 DSCPValue)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoQos(_LibSDK, state, DSCPValue);
        }

        /*!
         *  @brief Set the MTU size for video RTP packet.
         *
         *  @param mtu    Set MTU value. Allow value ranges (512-65507). Other value will be modified to the default 1450.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setVideoMTU(Int32 mtu)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setVideoMTU(_LibSDK, mtu);
        }



        /** @} */
        // end of group16

        /** @defgroup group17 Send OPTIONS/INFO/MESSAGE functions
         * @{
         */

        /*!
         *  @brief Send OPTIONS message.
         *
         *  @param to  The recipient of OPTIONS message.
         *  @param sdp The SDP of OPTIONS message. It's optional if user does not wish to send the SDP with OPTIONS message.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 sendOptions(String to, String sdp)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendOptions(_LibSDK, to, sdp);
        }

        /*!
         *  @brief Send a INFO message to remote side in a call.
         *
         *  @param sessionId    The session ID of call.
         *  @param mimeType     The mime type of INFO message.
         *  @param subMimeType  The sub mime type of INFO message.
         *  @param infoContents The contents that is sent with INFO message.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 sendInfo(Int32 sessionId, String mimeType, String subMimeType, String infoContents)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendInfo(_LibSDK, sessionId, mimeType, subMimeType, infoContents);
        }


        /*!
          *  @brief Send a SUBSCRIBE message to subscribe an event.
          *
          *  @param to    The user/extension to be subscribed.
          *  @param eventName    The event name to be subscribed.
          *
          *  @return If the function succeeds, it will return the ID of SUBSCRIBE which is greater than 0. If the function fails, it will return a specific error code which is less than 0.
          *  @remark
          *   Example 1, below code indicates that user/extension 101 is subscribed to MWI (Message Waiting notifications) for checking his voicemail: 
          *   int32 mwiSubId = sendSubscription("sip:101@test.com", "message-summary");
          *   
          *   Example 2, to monitor a user/extension call status, 
          *   You can use code: sendSubscription(mSipLib, "100", "dialog");
          *   Extension 100 refers to the user/extension to be monitored. Once being monitored, when extension 100 hold a call or is ringing, the
          *    onDialogStateUpdated callback will be triggered.
          */
        public Int32 sendSubscription(String to, String eventName)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendSubscription(_LibSDK, to, eventName);
        }


        /*!
         *  @brief Terminate the given subscription.
         *
         *  @param subscribeId    The ID of the subscription.
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         *  For example, if you want stop check the MWI, use below code:
           @code 
           terminateSubscription(mwiSubId);
           @endcode 
         */
        public Int32 terminateSubscription(Int32 subscribeId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_terminateSubscription(_LibSDK, subscribeId);
        }

        /*!
         *  @brief Send a MESSAGE message to remote side in dialog.
         *
         *  @param sessionId     The session ID of the call.
         *  @param mimeType      The mime type of MESSAGE message.
         *  @param subMimeType   The sub mime type of MESSAGE message.
         *  @param message       The contents which is sent with MESSAGE message. Binary data allowed.
         *  @param messageLength The message size.
         *
         *  @return If the function succeeds, it will return a message ID that allows to track the message sending state in onSendMessageSuccess and onSendMessageFailure. If the function fails, it will return a specific error code less than 0.
         *  @remark  Example 1: send a plain text message. Note: to send text in other languages, please use the UTF-8 to encode the message before sending.
         @code
         sendMessage(sessionId, "text", "plain", "hello",6);
         @endcode
         Example 2: send a binary message.
         @code
         sendMessage(sessionId, "application", "vnd.3gpp.sms", binData, binDataSize);
         @endcode
         */
        public Int32 sendMessage(Int32 sessionId, String mimeType, String subMimeType, byte[] message, Int32 messageLength)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendMessage(_LibSDK, sessionId, mimeType, subMimeType, message, messageLength);
        }

        /*!
         *  @brief Send an out of dialog MESSAGE message to remote side.
         *
         *  @param to            The message recipient, such as sip:receiver@portsip.com
         *  @param mimeType      The mime type of MESSAGE message.
         *  @param subMimeType   The sub mime type of MESSAGE message.
         *  @isSMS isSMS         Set to YES to specify "messagetype=SMS" in the To line, or NO to disable.
         *  @param message       The contents which is sent with MESSAGE message. Binary data allowed.
         *  @param messageLength The message size.
         *
         *  @return If the function succeeds, it will return a message ID that allows to track the message sending state in onSendOutOfMessageSuccess and onSendOutOfMessageFailure. If the function fails, it will return a specific error code less than 0.
         *  @remark
         *  Example 1: send a plain text message. Note: to send text in other languages, please use the UTF-8 to encode the message before sending.
         *  @code
            sendOutOfDialogMessage("sip:user1@sip.portsip.com", "text", "plain", false, "hello", 6);
         *  @endcode
         Example 2: send a binary message.
         *  @code
           sendOutOfDialogMessage("sip:user1@sip.portsip.com","application",  "vnd.3gpp.sms", false, binData, binDataSize);
         @endcode
         */
        public Int32 sendOutOfDialogMessage(String to, String mimeType, String subMimeType, Boolean isSMS, byte[] message, Int32 messageLength)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_sendOutOfDialogMessage(_LibSDK, to, mimeType, subMimeType, isSMS, message, messageLength);
        }


        /*!
         *  @brief Set the default expiration time to be used when creating a subscription.
         *
         *  @param secs    The default expiration time of subscription.
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setDefaultSubscriptionTime(Int32 secs)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setDefaultSubscriptionTime(_LibSDK, secs);
        }

        /*!
        *  @brief Set the default expiration time to be used when creating a publication.
        *  
        *  @param secs    The default expiration time of publication.
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        */
        public Int32 setDefaultPublicationTime(Int32 secs)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setDefaultPublicationTime(_LibSDK, secs);
        }

        /*!
        *  @brief Indicate the SDK uses the P2P mode for presence or presence agent mode.
        *  
        *  @param mode    0 - P2P mode; 1 - Presence Agent mode, default is P2P mode.
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        *  @remark
        *  Since presence agent mode requires the PBX/Server support the PUBLISH,
        *  please ensure you have your and PortSIP PBX support this 
        *  feature. For more details please visit: https://www.portsip.com/portsip-pbx
        */
        public Int32 setPresenceMode(Int32 mode)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setPresenceMode(_LibSDK, mode);
        }


        /** @} */
        // end of group17

        /** @defgroup group18 Presence functions
         * @{
         */


        /*!
         *  @brief Send a SUBSCRIBE message for subscribing the contact's presence status.
         *
         *  @param to The target contact. It must be like sip:contact001@sip.portsip.com.
         *  @param subject This subject text will be inserted into the SUBSCRIBE message. For example: "Hello, I'm Jason".<br>
         The subject maybe in UTF-8 format. You should use UTF-8 to decode it.
         *
         *  @return If the function succeeds, it will return value subscribeId. If the function fails, it will return a specific error code.
         */
        public Int32 presenceSubscribe(String to, String subject)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_presenceSubscribe(_LibSDK, to, subject);
        }

        /*!
         *  @brief Terminate the given presence subscription.
         *
         *  @param subscribeId    The ID of the subscription.
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 presenceTerminateSubscribe(Int32 subscribeId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_presenceTerminateSubscribe(_LibSDK, subscribeId);
        }


        /*!
         *  @brief Reject a presence SUBSCRIBE request which is received from contact.
         *
         *  @param subscribeId Subscription ID. When receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. The event includes the subscription ID.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         *  If the P2P presence mode is enabled, when someone subscribe your presence status,
         *  you will receive the subscribe request in the callback, and you can use this function to accept it.
         */
        public Int32 presenceRejectSubscribe(Int32 subscribeId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_presenceRejectSubscribe(_LibSDK, subscribeId);
        }

        /*!
        *  @brief Accept the presence SUBSCRIBE request which is received from contact.
        *
        *  @param subscribeId Subscription ID. When receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. The event will include the subscription ID.
        *
        *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
        *  @remark
        *  If the P2P presence mode is enabled, when someone subscribes your presence status,
        *  you will receive the subscription request in the callback, and you can use this function to reject it.
        */
        public Int32 presenceAcceptSubscribe(Int32 subscribeId)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_presenceAcceptSubscribe(_LibSDK, subscribeId);
        }

        /*!
         *  @brief Set the presence status.
         *
         *  @param subscribeId Subscription ID. 
         *  @param stateText   The state text of presence online. For example: "I'm here".
         *      If you want to appear as offline to others, please pass the "Offline" to "statusText" parameter.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         *  @remark
         *   With P2P presence mode, when receiving a SUBSCRIBE request from contact, the event onPresenceRecvSubscribe will be triggered. 
         *   The event includes the subscription ID. This function will cause the SDK sending a NOTIFY message to
         *   update your presence status, and you must pass the correct subscribeId.
         *   
         *   With presence agent mode, this function will cause the SDK to send a PUBLISH message to 
         *    update your presence status, and you must pass 0 to the "subscribeId" parameter.
         */
        public Int32 setPresenceStatus(Int32 subscribeId, String stateText)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setPresenceStatus(_LibSDK, subscribeId, stateText);
        }

        /** @} */
        // end of group18

        /** @defgroup group19 Device Manage functions.
         * @{
         */

        /*!
         *  @brief Gets the count of audio devices available for audio recording.
         *
         *  @return It will return the count of recording devices. If the function fails, it will return a specific error code less than 0.
         */
        public Int32 getNumOfRecordingDevices()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getNumOfRecordingDevices(_LibSDK);
        }

        /*!
         *  @brief Gets the number of audio devices available for audio playout
         *
         *  @return It will return the count of playout devices. If the function fails, it will return a specific error code less than 0.
         */
        public Int32 getNumOfPlayoutDevices()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getNumOfPlayoutDevices(_LibSDK);
        }

        /*!
         *  @brief Gets the name of a specific recording device given by an index.
         *
         *  @param deviceIndex Device index (0, 1, 2, ..., N-1), where N is given by getNumOfRecordingDevices (). Also -1 is a valid value and will return the name of the default recording device.
         *  @param nameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format. 
         *  @param nameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
         */
        public Int32 getRecordingDeviceName(Int32 deviceIndex, StringBuilder nameUTF8, Int32 nameUTF8Length)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getRecordingDeviceName(_LibSDK, deviceIndex, nameUTF8, nameUTF8Length);
        }

        /*!
         *  @brief Get the name of a specific playout device given by an index.
         *
         *  @param deviceIndex 
         *  @param deviceIndex Device index (0, 1, 2, ..., N-1), where N is given by getNumOfRecordingDevices (). Also -1 is a valid value and will return the name of the default recording device.
         *  @param nameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format. 
         *  @param nameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code. 
         */
        public Int32 getPlayoutDeviceName(Int32 deviceIndex, StringBuilder nameUTF8, Int32 nameUTF8Length)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getPlayoutDeviceName(_LibSDK, deviceIndex, nameUTF8, nameUTF8Length);
        }

        /*!
         *  @brief Set the speaker volume level
         *
         *  @param volume Volume level of speaker. Valid value ranges 0 - 255.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 setSpeakerVolume(Int32 volume)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setSpeakerVolume(_LibSDK, volume);
        }

        /*!
         *  @brief Gets the speaker volume level.
         *
         *  @return If the function succeeds, it will return the speaker volume with valid range 0 - 255. If the function fails, it will return a specific error code.
         */
        public Int32 getSpeakerVolume()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getSpeakerVolume(_LibSDK);
        }

        /*!
         *  @brief Sets the microphone volume level.
         *
         *  @param volume The microphone volume level. Valid value ranges 0 - 255.
         *
         *  @return If the function succeeds, the return value is 0. If the function fails, the return value is a specific error code.
         */
        public Int32 setMicVolume(Int32 volume)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_setMicVolume(_LibSDK, volume);
        }

        /*!
         *  @brief Retrieves the current microphone volume.
         *
         *  @return If the function succeeds, it will return the microphone volume. If the function fails, it will return a specific error code.
         */
        public Int32 getMicVolume()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getMicVolume(_LibSDK);
        }

        /*!
         *  @brief Use it for the audio device loop back test
         *
         *  @param enable Set to true to start audio look back test; or fase to stop.
         */
        public void audioPlayLoopbackTest(Boolean enable)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return;
            }

            PortSIP_NativeMethods.PortSIP_audioPlayLoopbackTest(_LibSDK, enable);
        }

        /*!
         *  @brief Get the number of available capturing devices.
         *
         *  @return It will return the count of video capturing devices. If it fails, it will return a specific error code less than 0.
         */
        public Int32 getNumOfVideoCaptureDevices()
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getNumOfVideoCaptureDevices(_LibSDK);
        }

        /*!
         *  @brief Get the name of a specific video capture device given by an index.
         *
         *  @param deviceIndex          Device index (0, 1, 2, ..., N-1), where N is given by getNumOfVideoCaptureDevices (). Also -1 is a valid value and will return the name of the default capturing device.
         *  @param uniqueIdUTF8   Unique identifier of the capturing device.
         *  @param uniqueIdUTF8Length Size in bytes of uniqueIdUTF8. 
         *  @param deviceNameUTF8 A character buffer to which the device name will be copied as a null-terminated string in UTF-8 format.
         *  @param deviceNameUTF8Length The size of nameUTF8 buffer. It cannot be less than 128.
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 getVideoCaptureDeviceName(Int32 deviceIndex,
                                               StringBuilder uniqueIdUTF8,
                                               Int32 uniqueIdUTF8Length,
                                               StringBuilder deviceNameUTF8,
                                               Int32 deviceNameUTF8Length)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_getVideoCaptureDeviceName(_LibSDK,
                                                                           deviceIndex,
                                                                           uniqueIdUTF8,
                                                                           uniqueIdUTF8Length,
                                                                           deviceNameUTF8,
                                                                           deviceNameUTF8Length);
        }

        /*!
         *  @brief Display the capture device property dialog box for the specified capture device.
         *
         *  @param uniqueIdUTF8     Unique identifier of the capture device. 
         *  @param uniqueIdUTF8Length Size in bytes of uniqueIdUTF8. 
         *  @param dialogTitle      The title of the video settings dialog. 
         *  @param parentWindow     Parent window used for the dialog box. It should originally be a HWND. 
         *  @param x                Horizontal position for the dialog relative to the parent window, in pixels. 
         *  @param y                Vertical position for the dialog relative to the parent window, in pixels. 
         *
         *  @return If the function succeeds, it will return value 0. If the function fails, it will return a specific error code.
         */
        public Int32 showVideoCaptureSettingsDialogBox(String uniqueIdUTF8,
                                                                    Int32 uniqueIdUTF8Length,
                                                                    String dialogTitle,
                                                                    IntPtr parentWindow,
                                                                    Int32 x,
                                                                    Int32 y)
        {
            if (_LibSDK == IntPtr.Zero)
            {
                return PortSIP_Errors.ECoreSDKObjectNull;
            }

            return PortSIP_NativeMethods.PortSIP_showVideoCaptureSettingsDialogBox(_LibSDK,
                                                                                    uniqueIdUTF8,
                                                                                    uniqueIdUTF8Length,
                                                                                    dialogTitle,
                                                                                    parentWindow,
                                                                                    x,
                                                                                    y);
        }
        /** @} */
        // end of group19
        /** @} */
        // end of groupSDK
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Private members and methods
        /// </summary>


        private PortSIP_NativeMethods.registerSuccess _rgs;
        private PortSIP_NativeMethods.registerFailure _rgf;
        private PortSIP_NativeMethods.inviteIncoming _ivi;
        private PortSIP_NativeMethods.inviteTrying _ivt;
        private PortSIP_NativeMethods.inviteSessionProgress _ivsp;
        private PortSIP_NativeMethods.inviteRinging _ivr;
        private PortSIP_NativeMethods.inviteAnswered _iva;
        private PortSIP_NativeMethods.inviteFailure _ivf;
        private PortSIP_NativeMethods.inviteUpdated _ivu;
        private PortSIP_NativeMethods.inviteConnected _ivc;
        private PortSIP_NativeMethods.inviteBeginingForward _ivbf;
        private PortSIP_NativeMethods.inviteClosed _ivcl;
        private PortSIP_NativeMethods.dialogStateUpdated _dsu;
        private PortSIP_NativeMethods.remoteHold _rmh;
        private PortSIP_NativeMethods.remoteUnHold _rmuh;
        private PortSIP_NativeMethods.receivedRefer _rvr;
        private PortSIP_NativeMethods.referAccepted _rfa;
        private PortSIP_NativeMethods.referRejected _rfr;
        private PortSIP_NativeMethods.transferTrying _tft;
        private PortSIP_NativeMethods.transferRinging _tfr;
        private PortSIP_NativeMethods.ACTVTransferSuccess _ats;
        private PortSIP_NativeMethods.ACTVTransferFailure _atf;
        private PortSIP_NativeMethods.receivedSignaling _rvs;
        private PortSIP_NativeMethods.sendingSignaling _sds;
        private PortSIP_NativeMethods.waitingVoiceMessage _wvm;
        private PortSIP_NativeMethods.waitingFaxMessage _wfm;
        private PortSIP_NativeMethods.recvDtmfTone _rdt;
        private PortSIP_NativeMethods.presenceRecvSubscribe _prs;
        private PortSIP_NativeMethods.presenceOnline _pon;
        private PortSIP_NativeMethods.presenceOffline _pof;
        private PortSIP_NativeMethods.recvOptions _rvo;
        private PortSIP_NativeMethods.recvInfo _rvi;
        private PortSIP_NativeMethods.recvNotifyOfSubscription _rns;
        private PortSIP_NativeMethods.subscriptionFailure _sbf;
        private PortSIP_NativeMethods.subscriptionTerminated _sbt;
        private PortSIP_NativeMethods.recvMessage _rvm;
        private PortSIP_NativeMethods.recvOutOfDialogMessage _rodm;
        private PortSIP_NativeMethods.sendMessageSuccess _sms;
        private PortSIP_NativeMethods.sendMessageFailure _smf;
        private PortSIP_NativeMethods.sendOutOfDialogMessageSuccess _sdms;
        private PortSIP_NativeMethods.sendOutOfDialogMessageFailure _sdmf;
        private PortSIP_NativeMethods.playAudioFileFinished _paf;
        private PortSIP_NativeMethods.playVideoFileFinished _pvf;
        private PortSIP_NativeMethods.audioRawCallback _arc;
        private PortSIP_NativeMethods.videoRawCallback _vrc;
        private PortSIP_NativeMethods.receivedRTPCallback _rrc;
        private PortSIP_NativeMethods.sendingRTPCallback _src;


        private void initializeCallbackFunctions()
        {

            _arc = new PortSIP_NativeMethods.audioRawCallback(onAudioRawCallback);
            _vrc = new PortSIP_NativeMethods.videoRawCallback(onVideoRawCallback);
            _rrc = new PortSIP_NativeMethods.receivedRTPCallback(onReceivedRtpPacket);
            _src = new PortSIP_NativeMethods.sendingRTPCallback(onSendingRtpPacket);


            _paf = new PortSIP_NativeMethods.playAudioFileFinished(onPlayAudioFileFinished);
            _pvf = new PortSIP_NativeMethods.playVideoFileFinished(onPlayVideoFileFinished);

            _rgs = new PortSIP_NativeMethods.registerSuccess(onRegisterSuccess);
            _rgf = new PortSIP_NativeMethods.registerFailure(onRegisterFailure);
            _ivi = new PortSIP_NativeMethods.inviteIncoming(onInviteIncoming);
            _ivt = new PortSIP_NativeMethods.inviteTrying(onInviteTrying);
            _ivsp = new PortSIP_NativeMethods.inviteSessionProgress(onInviteSessionProgress);
            _ivr = new PortSIP_NativeMethods.inviteRinging(onInviteRinging);
            _iva = new PortSIP_NativeMethods.inviteAnswered(onInviteAnswered);
            _ivf = new PortSIP_NativeMethods.inviteFailure(onInviteFailure);
            _ivu = new PortSIP_NativeMethods.inviteUpdated(onInviteUpdated);
            _ivc = new PortSIP_NativeMethods.inviteConnected(onInviteConnected);
            _ivbf = new PortSIP_NativeMethods.inviteBeginingForward(onInviteBeginingForward);
            _ivcl = new PortSIP_NativeMethods.inviteClosed(onInviteClosed);
            _dsu = new PortSIP_NativeMethods.dialogStateUpdated(onDialogStateUpdated);
            _rmh = new PortSIP_NativeMethods.remoteHold(onRemoteHold);
            _rmuh = new PortSIP_NativeMethods.remoteUnHold(onRemoteUnHold);
            _rvr = new PortSIP_NativeMethods.receivedRefer(onReceivedRefer);
            _rfa = new PortSIP_NativeMethods.referAccepted(onReferAccepted);
            _rfr = new PortSIP_NativeMethods.referRejected(onReferRejected);
            _tft = new PortSIP_NativeMethods.transferTrying(onTransferTrying);
            _tfr = new PortSIP_NativeMethods.transferRinging(onTransferRinging);
            _ats = new PortSIP_NativeMethods.ACTVTransferSuccess(onACTVTransferSuccess);
            _atf = new PortSIP_NativeMethods.ACTVTransferFailure(onACTVTransferFailure);
            _rvs = new PortSIP_NativeMethods.receivedSignaling(onReceivedSignaling);
            _sds = new PortSIP_NativeMethods.sendingSignaling(onSendingSignaling);
            _wvm = new PortSIP_NativeMethods.waitingVoiceMessage(onWaitingVoiceMessage);
            _wfm = new PortSIP_NativeMethods.waitingFaxMessage(onWaitingFaxMessage);
            _rdt = new PortSIP_NativeMethods.recvDtmfTone(onRecvDtmfTone);
            _prs = new PortSIP_NativeMethods.presenceRecvSubscribe(onPresenceRecvSubscribe);
            _pon = new PortSIP_NativeMethods.presenceOnline(onPresenceOnline);
            _pof = new PortSIP_NativeMethods.presenceOffline(onPresenceOffline);
            _rvo = new PortSIP_NativeMethods.recvOptions(onRecvOptions);
            _rvi = new PortSIP_NativeMethods.recvInfo(onRecvInfo);

            _rns = new PortSIP_NativeMethods.recvNotifyOfSubscription(onRecvNotifyOfSubscription);
            _sbf = new PortSIP_NativeMethods.subscriptionFailure(onSubscriptionFailure);
            _sbt = new PortSIP_NativeMethods.subscriptionTerminated(onSubscriptionTerminated);

            _rvm = new PortSIP_NativeMethods.recvMessage(onRecvMessage);
            _rodm = new PortSIP_NativeMethods.recvOutOfDialogMessage(onRecvOutOfDialogMessage);
            _sms = new PortSIP_NativeMethods.sendMessageSuccess(onSendMessageSuccess);
            _smf = new PortSIP_NativeMethods.sendMessageFailure(onSendMessageFailure);
            _sdms = new PortSIP_NativeMethods.sendOutOfDialogMessageSuccess(onSendOutOfDialogMessageSuccess);
            _sdmf = new PortSIP_NativeMethods.sendOutOfDialogMessageFailure(onSendOutOfDialogMessageFailure);

        }

        private unsafe Int32 onRegisterSuccess(Int32 callbackIndex, Int32 callbackObject, String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onRegisterSuccess(callbackIndex, callbackObject, statusText, statusCode, sipMessage);
            return 0;
        }

        private unsafe Int32 onRegisterFailure(Int32 callbackIndex, Int32 callbackObject, String statusText, Int32 statusCode, StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onRegisterFailure(callbackIndex, callbackObject, statusText, statusCode, sipMessage);
            return 0;
        }


        private unsafe Int32 onInviteIncoming(Int32 callbackIndex,
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
                                             StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteIncoming(callbackIndex,
                                                callbackObject,
                                                sessionId,
                                                callerDisplayName,
                                                caller,
                                                calleeDisplayName,
                                                callee,
                                                audioCodecNames,
                                                videoCodecNames,
                                                existsAudio,
                                                existsVideo,
                                                sipMessage);
            return 0;
        }

        private unsafe Int32 onInviteTrying(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onInviteTrying(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        private unsafe Int32 onInviteSessionProgress(Int32 callbackIndex,
                                            Int32 callbackObject,
                                            Int32 sessionId,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsEarlyMedia,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteSessionProgress(_callbackIndex,
                                                        _callbackObject,
                                                        sessionId,
                                                        audioCodecNames,
                                                        videoCodecNames,
                                                        existsEarlyMedia,
                                                        existsAudio,
                                                        existsVideo,
                                                        sipMessage);

            return 0;
        }

        private unsafe Int32 onInviteRinging(Int32 callbackIndex,
                                            Int32 callbackObject,
                                            Int32 sessionId,
                                            String statusText,
                                            Int32 statusCode,
                                            StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteRinging(_callbackIndex, _callbackObject, sessionId, statusText, statusCode, sipMessage);

            return 0;
        }

        private unsafe Int32 onInviteAnswered(Int32 callbackIndex,
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
                                             StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteAnswered(_callbackIndex,
                                                _callbackObject,
                                                sessionId,
                                                callerDisplayName,
                                                caller,
                                                calleeDisplayName,
                                                callee,
                                                audioCodecNames,
                                                videoCodecNames,
                                                existsAudio,
                                                existsVideo,
                                                sipMessage);

            return 0;
        }

        private unsafe Int32 onInviteFailure(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code, StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteFailure(_callbackIndex, _callbackObject, sessionId, reason, code, sipMessage);
            return 0;
        }

        private unsafe Int32 onInviteUpdated(Int32 callbackIndex,
                                             Int32 callbackObject,
                                             Int32 sessionId,
                                             String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsAudio,
                                             Boolean existsVideo,
                                             StringBuilder sipMessage)
        {
            _SIPCallbackEvents.onInviteUpdated(_callbackIndex, _callbackObject, sessionId, audioCodecNames, videoCodecNames, existsAudio, existsVideo, sipMessage);
            return 0;
        }

        private unsafe Int32 onInviteConnected(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onInviteConnected(_callbackIndex, _callbackObject, sessionId);

            return 0;

        }

        private unsafe Int32 onInviteBeginingForward(Int32 callbackIndex, Int32 callbackObject, String forwardTo)
        {
            _SIPCallbackEvents.onInviteBeginingForward(_callbackIndex, _callbackObject, forwardTo);
            return 0;
        }

        private unsafe Int32 onInviteClosed(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onInviteClosed(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        // 
        // If a user subscribed and his dialog status monitored, when the monitored user is holding a call
        // or is being ring, this callback will be triggered.
        //
        // BLFMonitoredUri - the monitored user's URI
        // BLFDialogState - the status of the call
        // BLFDialogId - the ID of the call
        // BLFDialogDirection - the direction of the call
        //
        private unsafe Int32 onDialogStateUpdated(Int32 callbackIndex,
                                                Int32 callbackObject,
                                                String BLFMonitoredUri,
                                                String BLFDialogState,
                                                String BLFDialogId,
                                                String BLFDialogDirection)
        {
            _SIPCallbackEvents.onDialogStateUpdated(callbackIndex, callbackObject, BLFMonitoredUri, BLFDialogState, BLFDialogId, BLFDialogDirection);
            return 0;
        }

        private unsafe Int32 onRemoteHold(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onRemoteHold(_callbackIndex, _callbackObject, sessionId);

            return 0;

        }

        private unsafe Int32 onRemoteUnHold(Int32 callbackIndex,
                                            Int32 callbackObject,
                                            Int32 sessionId,
                                            String audioCodecNames,
                                             String videoCodecNames,
                                             Boolean existsAudio,
                                             Boolean existsVideo)
        {
            _SIPCallbackEvents.onRemoteUnHold(_callbackIndex,
                                                _callbackObject,
                                                sessionId,
                                                audioCodecNames,
                                                videoCodecNames,
                                                existsAudio,
                                                existsVideo);
            return 0;
        }

        private unsafe Int32 onReceivedRefer(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                    Int32 sessionId,
                                                    Int32 referId,
                                                    String to,
                                                    String from,
                                                    StringBuilder referSipMessage)
        {
            _SIPCallbackEvents.onReceivedRefer(_callbackIndex, _callbackObject, sessionId, referId, to, from, referSipMessage);
            return 0;
        }

        private unsafe Int32 onReferAccepted(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onReferAccepted(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        private unsafe Int32 onReferRejected(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code)
        {
            _SIPCallbackEvents.onReferRejected(_callbackIndex, _callbackObject, sessionId, reason, code);
            return 0;
        }

        private unsafe Int32 onTransferTrying(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onTransferTrying(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        private unsafe Int32 onTransferRinging(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onTransferRinging(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        private unsafe Int32 onACTVTransferSuccess(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onACTVTransferSuccess(_callbackIndex, _callbackObject, sessionId);
            return 0;
        }

        private unsafe Int32 onACTVTransferFailure(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String reason, Int32 code)
        {
            _SIPCallbackEvents.onACTVTransferFailure(_callbackIndex, _callbackObject, sessionId, reason, code);
            return 0;
        }

        private unsafe Int32 onReceivedSignaling(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, StringBuilder signaling)
        {
            _SIPCallbackEvents.onReceivedSignaling(_callbackIndex, _callbackObject, sessionId, signaling);
            return 0;
        }

        private unsafe Int32 onSendingSignaling(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, StringBuilder signaling)
        {
            _SIPCallbackEvents.onSendingSignaling(_callbackIndex, _callbackObject, sessionId, signaling);
            return 0;
        }

        private unsafe Int32 onWaitingVoiceMessage(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                  String messageAccount,
                                                  Int32 urgentNewMessageCount,
                                                  Int32 urgentOldMessageCount,
                                                  Int32 newMessageCount,
                                                  Int32 oldMessageCount)
        {
            _SIPCallbackEvents.onWaitingVoiceMessage(_callbackIndex,
                                                    _callbackObject,
                                                    messageAccount,
                                                    urgentNewMessageCount,
                                                    urgentOldMessageCount,
                                                    newMessageCount,
                                                    oldMessageCount);
            return 0;
        }

        private unsafe Int32 onWaitingFaxMessage(Int32 callbackIndex,
                                                       Int32 callbackObject,
                                                  String messageAccount,
                                                  Int32 urgentNewMessageCount,
                                                  Int32 urgentOldMessageCount,
                                                  Int32 newMessageCount,
                                                  Int32 oldMessageCount)
        {
            _SIPCallbackEvents.onWaitingFaxMessage(_callbackIndex,
                                        _callbackObject,
                                        messageAccount,
                                        urgentNewMessageCount,
                                        urgentOldMessageCount,
                                        newMessageCount,
                                        oldMessageCount);
            return 0;
        }

        private unsafe Int32 onRecvDtmfTone(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, Int32 tone)
        {
            _SIPCallbackEvents.onRecvDtmfTone(_callbackIndex, _callbackObject, sessionId, tone);
            return 0;
        }

        private unsafe Int32 onRecvOptions(Int32 callbackIndex, Int32 callbackObject, StringBuilder optionsMessage)
        {
            _SIPCallbackEvents.onRecvOptions(_callbackIndex, _callbackObject, optionsMessage);
            return 0;
        }

        private unsafe Int32 onRecvInfo(Int32 callbackIndex, Int32 callbackObject, StringBuilder infoMessage)
        {
            _SIPCallbackEvents.onRecvInfo(_callbackIndex, _callbackObject, infoMessage);
            return 0;
        }


        private unsafe Int32 onRecvNotifyOfSubscription(Int32 callbackIndex,
                                Int32 callbackObject,
                                Int32 subscribeId,
                                StringBuilder notifyMsg,
                                byte[] contentData,
                                Int32 contentLenght)
        {
            _SIPCallbackEvents.onRecvNotifyOfSubscription(_callbackIndex, _callbackObject, subscribeId, notifyMsg, contentData, contentLenght);
            return 0;
        }

        private unsafe Int32 onSubscriptionFailure(Int32 callbackIndex, Int32 callbackObject, Int32 subscribeId, Int32 statusCode)
        {
            _SIPCallbackEvents.onSubscriptionFailure(_callbackIndex, _callbackObject, subscribeId, statusCode);
            return 0;
        }

        private unsafe Int32 onSubscriptionTerminated(Int32 callbackIndex, Int32 callbackObject, Int32 subscribeId)
        {
            _SIPCallbackEvents.onSubscriptionTerminated(_callbackIndex, _callbackObject, subscribeId);
            return 0;
        }


        /*!
         *  This event will be triggered when receiving the SUBSCRIBE request from a contact.
         *
         *  @param subscribeId     The ID of SUBSCRIBE request.
         *  @param fromDisplayName The display name of contact.
         *  @param from            The contact who send the SUBSCRIBE request.
         *  @param subject         The subject of the SUBSCRIBE request.
         */
        private unsafe Int32 onPresenceRecvSubscribe(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                    Int32 subscribeId,
                                                    String fromDisplayName,
                                                    String from,
                                                    String subject)
        {
            _SIPCallbackEvents.onPresenceRecvSubscribe(_callbackIndex,
                                                        _callbackObject,
                                                    subscribeId,
                                                    fromDisplayName,
                                                    from,
                                                    subject);
            return 0;
        }

        private unsafe Int32 onPresenceOnline(Int32 callbackIndex,
                                                    Int32 callbackObject,
                                                    String fromDisplayName,
                                                    String from,
                                                    String stateText)
        {
            _SIPCallbackEvents.onPresenceOnline(_callbackIndex,
                                                    _callbackObject,
                                                    fromDisplayName,
                                                    from,
                                                    stateText);
            return 0;
        }

        private unsafe Int32 onPresenceOffline(Int32 callbackIndex, Int32 callbackObject, String fromDisplayName, String from)
        {
            _SIPCallbackEvents.onPresenceOffline(_callbackIndex,
                                                    _callbackObject,
                                                    fromDisplayName,
                                                    from);
            return 0;
        }

        private unsafe Int32 onRecvMessage(Int32 callbackIndex,
                                                 Int32 callbackObject,
                                                 Int32 sessionId,
                                                 String mimeType,
                                                 String subMimeType,
                                                 byte[] messageData,
                                                 Int32 messageDataLength)
        {
            _SIPCallbackEvents.onRecvMessage(_callbackIndex,
                                                _callbackObject,
                                                sessionId,
                                                 mimeType,
                                                 subMimeType,
                                                 messageData,
                                                 messageDataLength);
            return 0;
        }

        private unsafe Int32 onRecvOutOfDialogMessage(Int32 callbackIndex,
                                                 Int32 callbackObject,
                                                 String fromDisplayName,
                                                 String from,
                                                 String toDisplayName,
                                                 String to,
                                                 String mimeType,
                                                 String subMimeType,
                                                 byte[] messageData,
                                                 Int32 messageDataLength)
        {
            _SIPCallbackEvents.onRecvOutOfDialogMessage(_callbackIndex,
                                                _callbackObject,
                                                fromDisplayName,
                                                 from,
                                                 toDisplayName,
                                                 to,
                                                 mimeType,
                                                 subMimeType,
                                                 messageData,
                                                 messageDataLength);
            return 0;
        }

        private unsafe Int32 onSendMessageSuccess(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 sessionId,
                                                        Int32 messageId)
        {
            _SIPCallbackEvents.onSendMessageSuccess(_callbackIndex, _callbackObject, sessionId, messageId);
            return 0;
        }

        private unsafe Int32 onSendMessageFailure(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 sessionId,
                                                        Int32 messageId,
                                                        String reason,
                                                        Int32 code)
        {
            _SIPCallbackEvents.onSendMessageFailure(_callbackIndex,
                                    _callbackObject,
                                    sessionId,
                                    messageId,
                                    reason,
                                    code);
            return 0;
        }

        private unsafe Int32 onSendOutOfDialogMessageSuccess(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 messageId,
                                                        String fromDisplayName,
                                                        String from,
                                                        String toDisplayName,
                                                        String to)
        {
            _SIPCallbackEvents.onSendOutOfDialogMessageSuccess(_callbackIndex,
                                     _callbackObject,
                                     messageId,
                                     fromDisplayName,
                                     from,
                                     toDisplayName,
                                     to);
            return 0;
        }

        private unsafe Int32 onSendOutOfDialogMessageFailure(Int32 callbackIndex,
                                                        Int32 callbackObject,
                                                        Int32 messageId,
                                                        String fromDisplayName,
                                                        String from,
                                                        String toDisplayName,
                                                        String to,
                                                        String reason,
                                                        Int32 code)
        {
            _SIPCallbackEvents.onSendOutOfDialogMessageFailure(_callbackIndex,
                                     _callbackObject,
                                     messageId,
                                     fromDisplayName,
                                     from,
                                     toDisplayName,
                                     to,
                                     reason,
                                     code);
            return 0;
        }

        private unsafe Int32 onPlayAudioFileFinished(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId, String fileName)
        {
            _SIPCallbackEvents.onPlayAudioFileFinished(_callbackIndex, _callbackObject, sessionId, fileName);
            return 0;
        }

        private unsafe Int32 onPlayVideoFileFinished(Int32 callbackIndex, Int32 callbackObject, Int32 sessionId)
        {
            _SIPCallbackEvents.onPlayVideoFileFinished(_callbackIndex, _callbackObject, sessionId);

            return 0;
        }

        private unsafe Int32 onReceivedRtpPacket(IntPtr callbackObject,
                                       Int32 sessionId,
                                       [MarshalAs(UnmanagedType.I1)] Boolean isAudio,
                                       [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] RTPPacket,
                                       Int32 packetSize)
        {
            _SIPCallbackEvents.onReceivedRtpPacket(callbackObject,
                                         sessionId,
                                         isAudio,
                                         RTPPacket,
                                         packetSize);


            return 0;
        }

        private unsafe Int32 onSendingRtpPacket(IntPtr callbackObject,
                                               Int32 sessionId,
                                               [MarshalAs(UnmanagedType.I1)] Boolean isAudio,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] RTPPacket,
                                               Int32 packetSize)
        {
            _SIPCallbackEvents.onSendingRtpPacket(callbackObject,
                                          sessionId,
                                          isAudio,
                                          RTPPacket,
                                          packetSize);


            return 0;
        }

        private unsafe Int32 onAudioRawCallback(IntPtr callbackObject,
                                               Int32 sessionId,
                                               Int32 callbackType,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] byte[] data,
                                               Int32 dataLength,
                                               Int32 samplingFreqHz)
        {
            _SIPCallbackEvents.onAudioRawCallback(callbackObject,
                                         sessionId,
                                         callbackType,
                                         data,
                                         dataLength,
                                         samplingFreqHz);
            return 0;
        }

        private unsafe Int32 onVideoRawCallback(IntPtr callbackObject,
                                               Int32 sessionId,
                                               Int32 callbackType,
                                               Int32 width,
                                               Int32 height,
                                               [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] byte[] data,
                                               Int32 dataLength)
        {
            return _SIPCallbackEvents.onVideoRawCallback(callbackObject,
                                          sessionId,
                                          callbackType,
                                          width,
                                          height,
                                          data,
                                          dataLength);

        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private IntPtr _callbackDispatcher;
        private Int32 _callbackObject;
        private IntPtr _LibSDK;
        private Int32 _callbackIndex;
    }
}
