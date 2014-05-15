using System;
using syncsoft;
using System.Net;
using System.IO;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Collections.Generic;
using System.Net.Sockets;

namespace RaspberryTests
{
    [TestFixture]    
    class PCCPHandlerTest
    {
        [Test]
        public void testSync()
        {
            Config.init();

            String file = "test_1.txt";
            String p = "test\\";
            Directory.CreateDirectory("test");
            using(File.Create(p+file))
            try
            {
                Config.addPath("/" + "test_1.txt", Path.GetFullPath(p+file));
            }
            catch (Exception){}
            PCCPHandler testHandler = new PCCPHandler(IPAddress.Loopback);
            TcpListener listen = new TcpListener(PCCPHandler.PORT);
            listen.Start();
            new Thread(new ThreadStart(delegate() { testHandler.Sync(new List<String> { Path.GetDirectoryName(Path.GetFullPath(p+file)) }); })).Start();

            TcpClient client = listen.AcceptTcpClient();
            NetworkStream s = client.GetStream();

            int b = client.GetStream().ReadByte();

            Assert.AreNotEqual(-1, b);
            Assert.AreEqual((int)PacketTypes.SyncInit,b);

            Byte[] bytes = new Byte[4];
            List<Byte> filename = new List<byte>();
            s.Read(bytes, 0, 4);
            int size = BitConverter.ToInt32(bytes,0);

            Assert.AreEqual(file.Length + 1 + 1 + 16, size);

            bytes = new Byte[file.Length + 1];
            s.Read(bytes, 0, file.Length + 1);

            Assert.AreEqual("/" + file, Encoding.UTF8.GetString(bytes));
            Assert.AreEqual(PCCPHandler.DELIMITER, s.ReadByte());

            bytes = new Byte[16];
            s.Read(bytes, 0, 16);

            s.WriteByte((Byte)PacketTypes.DataRequest);
            s.Write(BitConverter.GetBytes(file.Length + 1), 0,4);
            s.Write(Encoding.UTF8.GetBytes("/" + file), 0, file.Length + 1);

            Assert.AreEqual((int)PacketTypes.DataSend, s.ReadByte());

            bytes = new Byte[4];
            s.Read(bytes, 0, 4);
            size = BitConverter.ToInt32(bytes, 0);
            bytes = new Byte[size];
            s.Read(bytes, 0, size);

            s.Write(new Byte[] { (byte)PacketTypes.SyncFinish, 0, 0, 0, 0 }, 0, 5);

            client.Close();
            listen.Stop();

        }

    }
}
