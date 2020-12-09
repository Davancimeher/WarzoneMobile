public enum NetworkEvent : byte
{
    //IA
    sendIATime = 0,
    sendIAOwner = 1,
    OnCapturingIA=2,
    sendIADestination=3,
    SendIAOwnerLeave=4,
    // Game core
    sendUpdateMatchState=5,
    sendUpdateMatchTime=6,
    sendUpdateCountdown = 7,
}