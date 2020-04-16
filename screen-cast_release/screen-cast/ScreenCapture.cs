using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace screen_cast
{

    class ScreenCaptureEventArgs:EventArgs
    {
        public int width, height;
        public byte[] data;
    }

    public class TimerHelper
    {
        public TimerHelper(double interval)
        {
            TimerHandler = new System.Timers.Timer(interval);
            TimerHandler.AutoReset = true;
            TimerHandler.Enabled = false;

        }
        ~TimerHelper()
        {
            TimerHandler.Enabled = false;
            TimerHandler.Dispose();
        }

        public double Interval { get { return TimerHandler.Interval; } set { TimerHandler.Interval = value; } }
        public bool Enabled { get { return TimerHandler.Enabled; } set { TimerHandler.Enabled = value; } }

        public void DelegateFunc(System.Timers.ElapsedEventHandler ElapsedHandler)
        {
            TimerHandler.Elapsed += ElapsedHandler;
        }

        private System.Timers.Timer TimerHandler;
    }

    class ScreenCapture
    {
        #region Extern Funcs
        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public Int32 cbSize;
            public Int32 flags;
            public IntPtr hCursor;
            public POINTAPI ptScreenPos;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll")]
        static extern bool DrawIcon(IntPtr hDC, int X, int Y, IntPtr hIcon);

        const Int32 CURSOR_SHOWING = 0x00000001;
        #endregion

        #region Variables
        private ImageCodecInfo JpegEncoder = null;
        private EncoderParameters EncoderParams = null;
        private TimerHelper ScreenTimer = new TimerHelper(100);
        private int DstWidth, DstHeight;
        private Boolean ScaleEnabled = false;
        public EventHandler<ScreenCaptureEventArgs> OnScreenDataEventHandler;
        #endregion
        public ScreenCapture(long Quality = 70L)
        {
            ScreenTimer.DelegateFunc(OnTimer);

            JpegEncoder = GetEncoder(ImageFormat.Jpeg);
            EncoderParams = new EncoderParameters(1);
            EncoderParameter EncoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            EncoderParams.Param[0] = EncoderParam;
        }

        ~ScreenCapture()
        {

        }

        #region Native Functions
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }

            return null;
        }


        private void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            DoCapture();
        }

        public Bitmap CaptureScreenFun()
        {
            Bitmap result = null;

            try
            {
                result = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(result);
                {
                    g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

                    CURSORINFO pci;
                    pci.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(CURSORINFO));

                    if (GetCursorInfo(out pci))
                    {
                        if (pci.flags == CURSOR_SHOWING)
                        {
                            DrawIcon(g.GetHdc(), pci.ptScreenPos.x, pci.ptScreenPos.y, pci.hCursor);
                            g.ReleaseHdc();
                        }
                    }

                    g.Dispose();
                    g = null;
                }
            }
            catch
            {
                result = null;
            }

            if (ScaleEnabled == true && result != null &&
                (this.DstWidth != result.Width || this.DstHeight != result.Height))
            {
                return KiResizeImage(result, this.DstWidth, this.DstHeight);
            }


            return result;
        }

        private Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                g = null;

                return b;
            }
            catch
            {
                return null;
            }
        }

        private void DoCapture()
        {
            Bitmap Bmp = CaptureScreenFun();
            if (Bmp == null)
                return;

            MemoryStream MSBmp = new MemoryStream();

            Bmp.Save(MSBmp, JpegEncoder, EncoderParams);

            Byte[] ArrBmp = MSBmp.ToArray();
            MSBmp.Dispose();
            MSBmp.Close();
            MSBmp = null;

            GC.Collect();
            OnScreenDataEventHandler(this, new ScreenCaptureEventArgs() {width = Bmp.Width,height = Bmp.Height,data = ArrBmp });
        }
        #endregion

        #region Interface
        public void StartCapture(double Interval, int dstWidth = 0, int dstHeight = 0, bool bScale = false)
        {
            this.ScaleEnabled = bScale;
            this.DstWidth = dstWidth;
            this.DstHeight = dstHeight;
            ScreenTimer.Interval = Interval;
            ScreenTimer.Enabled = true;
            DoCapture();
        }

        public void UpdateQuality(long Quality)
        {
            EncoderParameter EncoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);
            EncoderParams.Param[0] = EncoderParam;
        }

        public void UpdateInterval(double Interval)
        {
            ScreenTimer.Interval = Interval;
        }

        public void UpdateScale(bool bEnable)
        {
            this.ScaleEnabled = bEnable;
        }

        public void StopCapture()
        {
            ScreenTimer.Enabled = false;
        }
        #endregion
    }
}
