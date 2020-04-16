using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;

namespace client
{
    public partial class MainFrame : Form
    {
        private PacketHelper.PacketCollector _ObjPicCollector = new PacketHelper.PacketCollector();
        private UdpReciever _ObjUdpReciever = new UdpReciever(12345);
        public MainFrame()
        {
            InitializeComponent();

            this.pciture_screen.BackgroundImageLayout = ImageLayout.Stretch;

            _ObjPicCollector.OnCollectorEventHandler += new EventHandler<PacketHelper.PacketCollectorEventArgs>(this.OnCollectorEvent);
            _ObjUdpReciever.OnUdpRecieverEventHandler += new EventHandler<UdpRecieverEventArgs>(this.OnUdpRecieverEvent);
        }

        private void OnCollectorEvent(Object obj, PacketHelper.PacketCollectorEventArgs evt)
        {
            try
            {
                MemoryStream imgStream = new MemoryStream(evt.data, 0, evt.data.Length);
                Bitmap map = (Bitmap)Image.FromStream(imgStream);
                if (map != null)
                    this.pciture_screen.BackgroundImage = map;
            }
            catch
            {

            }
        }

        private void OnUdpRecieverEvent(Object obj,UdpRecieverEventArgs evt)
        {
            _ObjPicCollector.Collect(evt.data);

        }
    }
}
