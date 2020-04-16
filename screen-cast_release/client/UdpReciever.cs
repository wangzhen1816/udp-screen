using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace client
{

    public class UdpRecieverEventArgs:EventArgs
    {
        public byte[] data;
    }
    public class UdpReciever
    {
        private UdpClient UdpHandler = null;
        private IPEndPoint RemoteIp = null;
        private bool bRunning = false;
        private Thread RecvThreadHandler = null;
        private static readonly IPAddress GroupAddress = IPAddress.Parse("224.100.0.1");//定义组播地址
        public EventHandler<UdpRecieverEventArgs> OnUdpRecieverEventHandler;

        public UdpReciever(int port, bool bThread = true, int Timeout = 2000)
        {
            {
                UdpHandler = new UdpClient(port);
                UdpHandler.Client.ReceiveTimeout = Timeout;
                RemoteIp = new IPEndPoint(GroupAddress, port); //获取或设置IP地址与端口
                try
                {
                    UdpHandler.JoinMulticastGroup(GroupAddress);//加入组播组
                    UdpHandler.EnableBroadcast = true;//设置是否发送或接收广播数据包
                    if (bThread == true)
                    {
                        bRunning = true;
                        RecvThreadHandler = new Thread(new ThreadStart(RecvThread));
                        RecvThreadHandler.IsBackground = true;
                        RecvThreadHandler.Start();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
               
            }
        }

        public void Dispose()
        {
            bRunning = false;
            UdpHandler.Close();
            //WinHelper.WaitForThreadExit(RecvThreadHandler);
        }

        public void RecvThread()
        {
            byte[] msg = null;
            while(bRunning)
            {
                try
                {
                    msg = UdpHandler.Receive(ref RemoteIp);
                    
                    
                }
                catch(Exception ex)
                {
                    Console.WriteLine("mc-UdpBroadReceiver: ");
                    continue;
                }
                if (msg != null && OnUdpRecieverEventHandler != null)
                    OnUdpRecieverEventHandler(this, new UdpRecieverEventArgs() { data = msg});
            }
        }

        public byte[] ReceiveMsg()
        {
            return UdpHandler.Receive(ref RemoteIp);
        }
    }
}
