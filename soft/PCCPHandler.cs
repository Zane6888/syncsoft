using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;

namespace syncsoft
{
    /// <summary>
    /// Handles a connection using PCCP
    /// </summary>
    class PCCPHandler : ConnectionHandler
    {
        public const int PORT = 2727;
        private TcpClient tcp;

        public PCCPHandler(IPAddress ip)
        {
            this.ip = ip;
        }

        public override List<string> getFileList()
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override List<ClientInfo> getClientList()
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override void sync(List<String> dirs)
        {
            List<String> basePaths = new List<String>();
            List<KeyValuePair<String, Byte[]>> lines = new List<KeyValuePair<string, byte[]>>();
            List<Byte[]> binary = new List<byte[]>();
            MD5 md5 = MD5.Create();

            //TODO use Config and Helper classes for File/Path operations
            foreach (String s in dirs)
            {
                //using "/"on all platforms for compatibility 
                s.Replace(@"\","/");

                int index = s.LastIndexOf(@"/");
                string basePath = s.Substring(0,index);

                string[] files = Directory.GetFiles(s,"*",SearchOption.AllDirectories);
                foreach (String file in files)
                {
                    String relPath = new String(file.Skip(index).ToArray());
                    Stream st = File.OpenRead(file);
                    Byte[] hash = md5.ComputeHash(st);
                    st.Dispose();
                    lines.Add(new KeyValuePair<string,byte[]>(relPath,hash));
                    List<byte> b = new List<byte>();
                    b.AddRange(Encoding.UTF8.GetBytes(relPath));
                    b.Add(0x3A);
                    b.AddRange(hash);
                    binary.Add(b.ToArray());
                }
            }
            md5.Dispose();

            List<Byte> sendBytes = new List<byte>();

            foreach (Byte[] b in binary)
                sendBytes.AddRange(b);
            byte type;
            Connect();
            SendPacket(PacketTypes.SyncInit, sendBytes);
            Byte[] packet;
            Boolean exit = false;
            while (!exit)
            {
                packet = RecivePacket(out type);
                switch ((PacketTypes)type)
                {
                    case PacketTypes.DataRequest:

                        break;
                    case PacketTypes.DataSend:
                        
                        break;
                    case PacketTypes.SyncFinish:

                        break;
                    default:
                        throw new ProtocolViolationException("unexpected PacketType: " + type);
                }
            }
            

            throw new NotImplementedException();
        }

        private void SendPacket(PacketTypes type, IEnumerable<Byte> data)
        {
            NetworkStream n = tcp.GetStream();
            n.WriteByte((Byte)type);
            n.Write(BitConverter.GetBytes(data != null ? data.Count() : 0), 0, 4);
            if(data != null)
                n.Write(data.ToArray(), 0, data.Count());
            n.Flush();
        }

        private void SendPacket(PacketTypes type, String data)
        {
            SendPacket(type, Encoding.UTF8.GetBytes(data));
        }

        private void SendFile(String diskLoc,String Path)
        {
            NetworkStream n = tcp.GetStream();
            n.WriteByte((Byte)PacketTypes.DataSend);
            n.Write(BitConverter.GetBytes((int)new FileInfo(diskLoc).Length), 0, 4);
            n.Write(Encoding.UTF8.GetBytes(Path), 0, Encoding.UTF8.GetBytes(Path).Length);

            FileStream s = File.OpenRead(diskLoc);
            while (s.Position < s.Length)
                n.WriteByte((byte)s.ReadByte());
            
        }

        public void ReciveFile()
        {
            NetworkStream n = tcp.GetStream();
            int type = n.ReadByte();
            if (n.ReadByte() != (int)PacketTypes.DataSend)
                throw new ProtocolViolationException("Recived type " + type +", expected type " + PacketTypes.DataSend);

        }

        public Byte[] RecivePacket(out Byte type)
        {
            NetworkStream n = tcp.GetStream();
            type = (Byte)n.ReadByte();
            Byte[] bSize = new Byte[4];
            n.Read(bSize, 0, 4);
            int size = BitConverter.ToInt32(bSize, 0);
            Byte[] data = new Byte[size];
            n.Read(data, 0,size);
            return data;
        }

        private void Connect()
        {
            tcp = new TcpClient();
            tcp.Connect(ip,PORT);
        }

        private void Disconnect()
        {
            tcp.Close();
            tcp = null;
        }
    }
}
