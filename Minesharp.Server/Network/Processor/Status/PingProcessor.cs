using Minesharp.Server.Network.Packet.Status.Client;
using Minesharp.Server.Network.Packet.Status.Server;

namespace Minesharp.Server.Network.Processor.Status;

public class PingProcessor : PacketProcessor<PingPacket>
{
    protected override void Process(NetworkSession session, PingPacket packet)
    {
        session.SendPacket(new PongPacket
        {
            Payload = packet.Payload
        });
    }
}