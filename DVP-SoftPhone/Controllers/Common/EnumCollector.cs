using System;

namespace DuoSoftware.DuoSoftPhone.Controllers.Common
{
    

    public enum CallFunctions
    {
        IncomingCall = 0,
        MakeCall = 1,
        EndCall = 2,
        HoldCall = 3,
        UnholdCall = 4,
        TransferCall = 5,
        AnswerCall = 6,
        RejectCall = 7,
        MuteCall = 8,
        UnmuteCall = 9,
        EtlCall = 10,
        ConfCall = 11,
        Initiate = 12,
        Registor = 13,
        Unregistor = 14,
        Initialized = 15,
        InitializFail = 16,
        Handshake = 17,
        Outbound = 18,
        Inbound = 19,
        TransferIVR = 20,
        Unauthorized = 21,
    }

    public struct CallLog
    {
        public string PhoneNo;
        public double Durations;
        public DateTime time;
        public int Direction;//0 -incoming, 1-outgoing , 2- miscall
        public string Skill;
    }

    public enum CallActions
    {
        Error = 0,
        Incomming_Call_Request = 1,
        Call_InProgress = 2,
        Call = 3,

        Hold_Requested = 4,
        Hold_InProgress = 5,
        Hold = 6,

        UnHold_Requested = 7,
        UnHold_InProgress = 8,
        UnHold = 9,

        Call_Transfer_Requested = 10,
        Call_Transfer_InProgress = 11,
        Call_Transferred = 12,

        Call_Swap_Requested = 13,
        Call_Swap_InProgress = 14,
        Call_Swap = 15,

        Conference_Call_Requested = 16,
        Conference_Call_InProgress = 17,
        Conference_Call = 18,

        ETL_Requested = 19,
        ETL_InProgress = 20,
        ETL_Call = 21,
    }

}
