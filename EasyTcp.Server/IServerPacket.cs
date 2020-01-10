using EasyTcp.Common;
using EasyTcp.Common.Packets;
namespace EasyTcp.Server
{
    public interface IServerPacket
    {
        string PacketType { get; }
        void Execute(Message msg, EasyTcpServer server);
    }
}
