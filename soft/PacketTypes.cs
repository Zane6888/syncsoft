enum PacketTypes : byte
{
    Discover = 1,
    DiscoverReply = 2,
    SyncInit = 3,
    DataRequest = 4,
    DataSend = 5,
    SyncFinish = 6,
    ClientListRequest = 7,
    ClientList = 8,
    FileListRequest = 9,
    FileList = 10,
    ErrorProtocol = 11,
    ErrorSystem = 12
}