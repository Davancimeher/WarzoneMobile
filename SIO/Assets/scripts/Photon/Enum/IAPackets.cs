public enum NetworkEvent : byte
{
    //IA
    sendIATime = 0,
    sendIAOwner = 1,
    OnCapturingIA=2,
    sendIADestination=3,
    SendIAOwnerLeave=4,
    // Game core
    sendStartTime=5,
    sendUpdateMatchTime=6,
    sendEndMatch=7
}