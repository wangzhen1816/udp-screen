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

namespace screen_cast
{
    public partial class MainFrame : Form
    {
        private ScreenCapture _ObjCapture = new ScreenCapture();
        private UdpHelper _ObjUdp = new UdpHelper("224.100.0.1", 12345);
        private Boolean _bRunning = false;

        public MainFrame()
        {
            InitializeComponent();

            this.text_fps.Text = "20";
            this.text_quality.Text = "80";
            this._ObjCapture.OnScreenDataEventHandler += new EventHandler<ScreenCaptureEventArgs>(this.OnScreenData);
            this.picture_screen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            String fwNo = System.Environment.Version.ToString();
            Console.WriteLine("Ce:"+fwNo);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (this._bRunning == false)
            {
                int Fps = int.Parse(this.text_fps.Text);
                int Quality = int.Parse(this.text_quality.Text);

                this._ObjCapture.UpdateQuality(Quality);
                this._ObjCapture.StartCapture(1000L / Fps,1280,800,true);

                this._bRunning = true;
                this.btn_start.Text = "Stop";
            }
            else
            {
                this._ObjCapture.StopCapture();

                this._bRunning = false;
                this.btn_start.Text = "Start";
            }

        }

        private void OnScreenData(Object obj,ScreenCaptureEventArgs evt)
        {
            try
            {
                MemoryStream imgStream = new MemoryStream(evt.data, 0, evt.data.Length);
                Bitmap map = (Bitmap)Image.FromStream(imgStream);
                if (map != null)
                    this.picture_screen.BackgroundImage = map;

                _ObjUdp.Send(evt.data);
            }
            catch(Exception  ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
