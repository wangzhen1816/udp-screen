using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.IO;

namespace screen_cast
{
    class UdpHelper
    {
       
        private UdpClient UdpHandler = null;
        private IPEndPoint RemoteIp = null;
        private long TimeStamp = 0;

        public UdpHelper(string address, int port)
        {
            try
            {
                UdpHandler = new UdpClient();
                UdpHandler.EnableBroadcast = true;
                RemoteIp = new IPEndPoint(IPAddress.Parse(address), port);
            }
            catch(Exception ex)
            {
                Console.WriteLine("UdpHelper:Init Error.");
            }
        }
        public void Dispose()
        {
            UdpHandler.Close();
        }

        public void Send(byte[] msg)
        {

            TimeStamp += 1;
            // ICollection<PacketHelper.CastPacket> Packets = PacketHelper.PacketSplitter.Split(TimeStamp, msg);
            ICollection<byte[]> Packets = PacketHelper.PacketSplitter.Split(TimeStamp, msg);
            Console.WriteLine("send Packets Count:" + Packets.Count);
            Console.WriteLine("send Packets TimeStamp:" + TimeStamp);
            int Temp = 0;
            byte[] bt= new byte[msg.Length];
           
            
            foreach (var Pac in Packets)
            {
                try
                {
                    Console.WriteLine("UdpHandler.Available:"+ UdpHandler.Available);
                    //循环发送------------
                    // Byte[] PacByte = Pac.ToArray();
                   
                    long TimeStamp =PacketHelper.PacketCollector.getLong(Pac, 0);

                    int total = PacketHelper.PacketCollector.getInt(Pac, 8);

                    int index = PacketHelper.PacketCollector.getInt(Pac, 12);

                    int dataOffset = PacketHelper.PacketCollector.getInt(Pac, 16);

                    Console.WriteLine("单包:" + Temp+ ",TimeStamp="+ TimeStamp+ ",total="+ total+ ",index="+index+ ",dataOffset="+ dataOffset);
                    UdpHandler.Send(Pac, Pac.Length, RemoteIp);
                    Temp++;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("UdpHelper:Send Error.");
                    return;
                }
            }
        }
    }
}
