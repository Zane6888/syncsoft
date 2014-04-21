using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;


namespace syncsoft
{
    /// <summary>
    /// Handles a connection using PCCP
    /// </summary>
    public class PCCPHandler : ConnectionHandler
    {
        public const int PORT = 2727;
        public const Char DELIMITER = ':';
        public const int SINGLE_TIMEOUT = 100;
        public const int TIMEOUT_COUNT = 10;

        private TcpClient tcp;

        public PCCPHandler(IPAddress ip)
        {
            this.ip = ip;
        }

        public override string Protocol
        {
            get
            {
                return "PCCP";
            }
        }

        public override List<string> GetFileList()
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override List<ClientInfo> GetClientList()
        {
            //TODO implement
            throw new NotImplementedException();
        }

        public override void Sync(List<String> baseDirs)
        {
            Sync(baseDirs, new List<string>());
        }

        public override void Sync(List<String> dirs, List<String> exclude)
        {
            List<String> basePaths = new List<String>();
            List<KeyValuePair<String, Byte[]>> lines = new List<KeyValuePair<string, byte[]>>();
            List<Byte[]> binary = new List<byte[]>();
            
            foreach (String dir in dirs)
            {
                if(exclude.Contains(dir))
                    continue;

                string[] files = Directory.GetFiles(dir, "*", SearchOption.AllDirectories);

                foreach (String file in files)
                {
                    if (exclude.ContainsFile(file))
                        continue;

                    lines.Add(new KeyValuePair<string,byte[]>(FileHelper.GetRelative(file),FileHelper.GetMD5(file)));

                    List<byte> b = new List<byte>();
                    b.AddRange(Encoding.UTF8.GetBytes(lines.Last().Key));
                    b.Add((Byte)DELIMITER);
                    b.AddRange(lines.Last().Value);

                    binary.Add(b.ToArray());             
                }
            }
           

            List<Byte> sendBytes = new List<byte>();

            foreach (Byte[] b in binary)
                sendBytes.AddRange(b);

            foreach (String e in exclude)
            {
                sendBytes.Add((Byte)DELIMITER);
                sendBytes.AddRange(Encoding.UTF8.GetBytes(FileHelper.GetRelative(e)));
            }

            byte type;
            Connect();
            SendPacket(PacketTypes.SyncInit, sendBytes);
            Byte[] packet;
            Boolean exit = false;
            while (!exit)
            {
                type = GetNextType();
                switch ((PacketTypes)type)
                {
                    case PacketTypes.DataRequest:
                        packet = GetNextPacket();
                        String[] files = Encoding.UTF8.GetString(packet).Split(DELIMITER);
                        foreach (String file in files)
                            SendFile(FileHelper.GetAbsolute(file));
                        
                        break;
                    case PacketTypes.DataSend:
                        ReciveFile();
                        break;
                    case PacketTypes.DataDelete:
                        File.Delete(FileHelper.GetAbsolute(Encoding.UTF8.GetString(GetNextPacket())));
                        break;
                    case PacketTypes.SyncFinish:
                        exit = true;
                        packet = GetNextPacket();
                        Debug.WriteLine(Encoding.UTF8.GetString(packet));
                        break;
                    default:
                        throw new ProtocolViolationException("unexpected PacketType: " + type);
                }
            }
            Disconnect();

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

        private void SendFile(String diskLoc)
        {
            String path = FileHelper.GetRelative(diskLoc);
            NetworkStream n = tcp.GetStream();
            n.WriteByte((Byte)PacketTypes.DataSend);
            n.Write(BitConverter.GetBytes((int)new FileInfo(diskLoc).Length), 0, 4);
            n.Write(Encoding.UTF8.GetBytes(path), 0, Encoding.UTF8.GetBytes(path).Length);

            FileStream s = File.OpenRead(diskLoc);
            while (s.Position < s.Length)
                n.WriteByte((byte)s.ReadByte());
            
        }
        /// <summary>
        /// Reads the next File from the stream and Saves it. Only call after GetNextType(). Only use for PacketType.SendFile.
        /// </summary>
        private void ReciveFile()
        {
            NetworkStream n = tcp.GetStream();
            Byte[] bSize = new Byte[4];
            n.Read(bSize, 0, 4);
            int size = BitConverter.ToInt32(bSize, 0);

            List<Byte> pathBytes = new List<byte>();
            Byte[] buffer = new Byte[1];
            int timeOutCount = 0;
            while (true)
            {
                if (timeOutCount >= TIMEOUT_COUNT)
                    throw new TimeoutException("Timeout: did not recive any data for "  + SINGLE_TIMEOUT * TIMEOUT_COUNT + " ms.");
                if (pathBytes.Count == size)
                    throw new ProtocolViolationException("The Packet's length is to short or the delimiter is missing.");

                int b = n.ReadByte();
                if (b < 0)
                {
                    timeOutCount++;
                    System.Threading.Thread.Sleep(SINGLE_TIMEOUT);
                    continue;
                }
                timeOutCount = 0;

                buffer[0] = (byte)b;
                
                if (Encoding.UTF8.GetString(buffer) == DELIMITER.ToString())
                    break;

                pathBytes.Add(buffer[0]);
            }

            int remaining = size - pathBytes.Count - 1;
            String path = Encoding.UTF8.GetString(pathBytes.ToArray());

            String location = FileHelper.GetAbsolute(path);

            Directory.CreateDirectory(Path.GetDirectoryName(location));

            FileStream s = new FileStream(location, FileMode.Create);

            for (int i = 0; i < remaining; i++)
            {
                while (true)
                {
                    if (timeOutCount >= TIMEOUT_COUNT)
                        throw new TimeoutException("Timeout: did not recive any data for " + SINGLE_TIMEOUT * TIMEOUT_COUNT + " ms.");

                    int b = n.ReadByte();
                    if (b < 0)
                    {
                        timeOutCount++;
                        System.Threading.Thread.Sleep(SINGLE_TIMEOUT);
                        continue;
                    }
                    timeOutCount = 0;

                    s.WriteByte((Byte)b);
                    break;
                }
            }
            s.Close();

        }


        /// <summary>
        /// Reads a packet from the Networkstream. Only call after GetNextType().
        /// </summary>
        /// <returns>The data part of the Packet</returns>
        private Byte[] GetNextPacket()
        {
            NetworkStream n = tcp.GetStream();
            Byte[] bSize = new Byte[4];
            n.Read(bSize, 0, 4);
            int size =  BitConverter.ToInt32(bSize, 0);
            Byte[] data = new Byte[size];
            n.Read(data, 0,size);
            return data;
        }

        private Byte GetNextType()
        {
            NetworkStream n = tcp.GetStream();
            return (Byte) n.ReadByte();  
        }

        private void Connect()
        {
            Connect(PORT);
        }

        private void Connect(int port)
        {
            tcp = new TcpClient();
            tcp.Connect(IP, port);
        }

        private void Disconnect()
        {
            tcp.Close();
            tcp = null;
        }
    }
}
