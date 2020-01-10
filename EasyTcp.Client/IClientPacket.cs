using EasyTcp.Common;
using EasyTcp.Common.Packets;
namespace EasyTcp.Client
{
    public interface IClientPacket
    {
        string PacketType { get; }
        void Execute(Message msg, EasyTcpClient client);
    }
}
