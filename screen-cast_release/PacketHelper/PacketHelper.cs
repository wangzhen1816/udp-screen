using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace PacketHelper
{
    public class SerializationUnit
    {
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            byte[] bytes = ms.GetBuffer();
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        public static object DeserializeObject(byte[] bytes)
        {
            object obj = null;
            if (bytes == null)
                return obj;
            MemoryStream ms = new MemoryStream(bytes);
            ms.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            obj = formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }
    }

    [Serializable]
    public class CastPacket
    {
        public double TimeStamp { get; set; }
        public Int32 Index { get; set; }
        public Int32 Total { get; set; }
        public Int32 TotalLength { get; set; }
        public Int32 DataOffset { get; set; }
        public Int32 DataLength { get; set; }
        public byte[] Data { get; set; }


        public CastPacket(double timeStamp, Int32 total, Int32 index, byte[] data, Int32 dataOffset, Int32 totalLength)
        {
            this.TimeStamp = timeStamp;
            this.Total = total;
            this.Index = index;
            this.Data = data;
            this.DataOffset = dataOffset;
            this.DataLength = data.Length;
            this.TotalLength = totalLength;
        }

        public byte[] ToArray()
        {
            return SerializationUnit.SerializeObject(this);
        }
    }

    public class PacketSplitter
    {
        public static ICollection<byte[]> Split(long TimeStamp, byte[] data, int chunkLength = 1024*3)
        {
            

            //List<CastPacket> packets = new List<CastPacket>();
            List<byte[]> packets = new List<byte[]>();
            //end mark
            if (data == null)
            {
                //send 3 times for import things
            }

            int chunks = data.Length / chunkLength;
            int remainder = data.Length % chunkLength;
            int total = chunks;

            if (remainder > 0) total++;
         

            for (int i = 1; i <= chunks; i++)
            {
                byte[] chunk = new byte[chunkLength];
                Buffer.BlockCopy(data, (i - 1) * chunkLength, chunk, 0, chunkLength);
                //packets.Add(new CastPacket(TimeStamp, total, i, chunk, chunkLength * (i - 1), data.Length));

                //int length = data.Length - (chunkLength * chunks);
                //Buffer.BlockCopy(data, chunkLength * chunks, chunk, 0, length);

                //TimeStamp 8个字节  total 4个字节 index 4个字节 dataOffset 数据起始偏移量 4个字节 length 当前包的长度 4个字节
                //总字节数 1024+8+4*4 = 1048  8+4+4+4+4 24
                byte[] content = new byte[chunk.Length + 8 + 4 + 4 + 4 + 4];

                PutLong(content, TimeStamp, 0);

                PubInt(content, total, 8);

                PubInt(content, i, 12);

                PubInt(content, chunkLength * (i - 1), 16);

                PubInt(content, data.Length, 20);

                Buffer.BlockCopy(chunk, 0, content, 24, chunk.Length);

                //JObject strjson = new JObject();

                //strjson.Add("TimeStamp", TimeStamp);
                //strjson.Add("total", total);
                //strjson.Add("index", total);
                //strjson.Add("data", chunk);
                //strjson.Add("dataOffset", chunkLength * chunks);
                //strjson.Add("totalLength", data.Length);


                //Console.WriteLine(strjson.ToString());
                //byte[] databy = System.Text.Encoding.Default.GetBytes(strjson.ToString());
                packets.Add(content);
            }

            if (remainder > 0)
            {
                int length = data.Length - (chunkLength * chunks);
                byte[] chunk = new byte[length];
                Buffer.BlockCopy(data, chunkLength * chunks, chunk, 0, length);

                byte[] content = new byte[chunk.Length + 8 + 4 + 4 + 4 + 4];

                PutLong(content, TimeStamp, 0);

                PubInt(content, total, 8);

                PubInt(content, total, 12);

                PubInt(content, chunkLength * chunks, 16);

                PubInt(content, data.Length, 20);

                Buffer.BlockCopy(chunk, 0, content, 24, chunk.Length);
                packets.Add(content);
            }

            //if (remainder > 0)
            //{
            //    int length = data.Length - (chunkLength * chunks);
            //    byte[] chunk = new byte[length];
            //    Buffer.BlockCopy(data, chunkLength * chunks, chunk, 0, length);

            //    JObject strjson = new JObject();

            //    strjson.Add("TimeStamp", TimeStamp);
            //    strjson.Add("total", total);
            //    strjson.Add("index", total);
            //    strjson.Add("data", chunk);
            //    strjson.Add("dataOffset", chunkLength * chunks);
            //    strjson.Add("totalLength", data.Length);
            //    Console.WriteLine(strjson.ToString());
            //    byte [] databy=System.Text.Encoding.Default.GetBytes(strjson.ToString());
            //    packets.Add(databy);
            //    // packets.Add(new CastPacket(TimeStamp, total, total, chunk, chunkLength * chunks, data.Length));
            //}

            Console.WriteLine(data.Length);
            return packets;
        }

        /**
       * 转换long型为byte数组
       *
       * @param bb
       * @param x
       * @param index
       */
        public static void PutLong(byte[] bb, long x, int index)
        {
            bb[index + 7] = (byte)(x >> 56);
            bb[index + 6] = (byte)(x >> 48);
            bb[index + 5] = (byte)(x >> 40);
            bb[index + 4] = (byte)(x >> 32);
            bb[index + 3] = (byte)(x >> 24);
            bb[index + 2] = (byte)(x >> 16);
            bb[index + 1] = (byte)(x >> 8);
            bb[index + 0] = (byte)(x >> 0);
        }

        public static void PubInt(byte[] bb, int x, int index)
        {
            bb[index + 3] = (byte)(x >> 24);
            bb[index + 2] = (byte)(x >> 16);
            bb[index + 1] = (byte)(x >> 8);
            bb[index + 0] = (byte)(x >> 0);
        }

    }



    public class PacketCollectorEventArgs : EventArgs
    {
        public byte[] data;
    }

    public class PacketCollector
    {
        private Object ObjLock = new Object();
        public EventHandler<PacketCollectorEventArgs> OnCollectorEventHandler;
        private Dictionary<double, List<byte[]>> DicCollectedPacket = new Dictionary<double, List<byte[]>>();
        private double CurrentTimeStamp = -1;

        private Thread CollectThreadHandler;
        private Boolean bRunning;

        public PacketCollector()
        {
            bRunning = true;
            CollectThreadHandler = new Thread(new ThreadStart(CollectThread)) { IsBackground = true };
            CollectThreadHandler.Start();
        }

        public void Dispose()
        {
            bRunning = false;
            //WinHelper.WaitForThreadExit(CollectThreadHandler);
            DicCollectedPacket = null;
        }

        private void CollectThread()
        {
            while (bRunning == true)
            {
                lock (ObjLock)
                {
                    if (DicCollectedPacket.Count == 0) continue;

                    var SortedDic = DicCollectedPacket.OrderBy(p => p.Key).ToDictionary(p => p.Key, p => p.Value);

                    foreach (var ListPack in SortedDic)
                    {
                        if (CurrentTimeStamp - ListPack.Key >= 2000)
                        {
                            //Console.WriteLine("Abbanded package.");
                            DicCollectedPacket.Remove(ListPack.Key);
                            continue;
                        }

                        int total = getInt(ListPack.Value[0], 8);

                        if (ListPack.Value.Count == total)
                        {

                            try
                            {
                                RePackMsg(ListPack.Value);
                                DicCollectedPacket.Remove(ListPack.Key);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }

                }
                Thread.Sleep(5);
            }
        }

        public void Collect(Byte[] Msg)
        {

            //CastPacket Pac = (CastPacket)SerializationUnit.DeserializeObject(Msg);
            //strjson.Add("TimeStamp", TimeStamp);
            //strjson.Add("total", total);
            //strjson.Add("index", total);
            //strjson.Add("data", chunk);
            //strjson.Add("dataOffset", chunkLength * chunks);
            //strjson.Add("totalLength", data.Length);


            long TimeStamp = getLong(Msg, 0);

            int total = getInt(Msg, 8);

            int index = getInt(Msg, 12);

            int dataOffset = getInt(Msg, 16);

            int totalLength = getInt(Msg, 20);

            byte[] content = subBytes(Msg, 24, Msg.Length - 24);

            Console.WriteLine("TimeStamp=" + TimeStamp + ",total=" + total + ",index=" + index + ",dataOffset=" + dataOffset + ",totalLength=" + totalLength);

            //JObject Pac = JObject.Parse(str);
            if (content == null)
                return;

            // if (Pac.Data == null)
            //    return;

            lock (ObjLock)
            {
                List<byte[]> ListPacket = null;
                Boolean bContain = DicCollectedPacket.TryGetValue(TimeStamp, out ListPacket);
                if (bContain == false)
                {
                    ListPacket = new List<byte[]>();
                    ListPacket.Add(Msg);
                    DicCollectedPacket.Add(TimeStamp, ListPacket);
                }
                else
                {
                    ListPacket.Add(Msg);
                }

                Console.WriteLine("Collect tostring>>>" + ListPacket.ToString());
                CurrentTimeStamp = Math.Max(TimeStamp, CurrentTimeStamp);
            }

        }


        /**
        * 通过byte数组取到long
        *
        * @param bb
        * @param index
        * @return
        */
        public static long getLong(byte[] bb, int index)
        {
            return ((((long)bb[index + 7] & 0xff) << 56)
                    | (((long)bb[index + 6] & 0xff) << 48)
                    | (((long)bb[index + 5] & 0xff) << 40)
                    | (((long)bb[index + 4] & 0xff) << 32)
                    | (((long)bb[index + 3] & 0xff) << 24)
                    | (((long)bb[index + 2] & 0xff) << 16)
                    | (((long)bb[index + 1] & 0xff) << 8) | (((long)bb[index + 0] & 0xff) << 0));
        }

        /**
         * 截取字节数组
         *
         * @param src
         * @param begin
         * @param count
         * @return
         */
        public static byte[] subBytes(byte[] src, int begin, int count)
        {
            byte[] bs = new byte[count];
            for (int i = begin; i < begin + count; i++) bs[i - begin] = src[i];
            return bs;
        }

        /**
         * 通过byte数组取到int
         *
         * @param src
         * @param offset 第几位开始
         * @return
         */
        public static int getInt(byte[] src, int offset)
        {

            int value;
            value = (src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24);
            return value;
        }


        private void RePackMsg(List<byte[]> listPack)
        {

            int totalLength = getInt(listPack[0], 20);

            Byte[] Data = new Byte[totalLength];
            foreach (var Pac in listPack)
            {
                try
                {
                    byte[] ct = subBytes(Pac, 24, Pac.Length - 24);
                    int dataOffset = getInt(Pac, 16);
                    Buffer.BlockCopy(ct, 0, Data, dataOffset, ct.Length);
                }
                catch
                {
                    Console.WriteLine("Wrong package.");
                    return;
                }
            }

            if (this.OnCollectorEventHandler != null)
                this.OnCollectorEventHandler(this, new PacketCollectorEventArgs() { data = Data });
        }

    }
}
