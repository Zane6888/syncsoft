using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace syncsoft
{
    /// <summary>
    /// Handles a connection using PCCP
    /// </summary>
    class PCCPHandler : ConnectionHandler
    {
        public const int PORT = 2727;
        public const int HEADER_LENGTH = 5;

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

        public override bool sync(List<String> dirs)
        {
            List<String> basePaths = new List<String>();
            List<KeyValuePair<String, Byte[]>> lines = new List<KeyValuePair<string, byte[]>>();
            List<Byte[]> binary = new List<byte[]>();
            MD5 md5 = MD5.Create();
            foreach (String s in dirs)
            {
                //using on all platforms for compatibility 
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
                    b.Add((Byte)0x3A);
                    b.AddRange(hash);
                    binary.Add(b.ToArray());
                }
            }
            md5.Dispose();
            /* TODO rewite (send method)
            uint count = 0;
            foreach (Byte[] b in binary)
                count += (uint)binary.Count;


            Byte[] sendBytes = new Byte[HEADER_LENGTH + count];
            sendBytes[0] = (Byte)3;
            BitConverter.GetBytes(count).CopyTo(sendBytes,1);
            int i = HEADER_LENGTH;

            foreach (Byte[] b in binary)
            {
                b.CopyTo(sendBytes, i);
                i += b.Count();
            }*/
            
            //TODO implement
            throw new NotImplementedException();
        }
    }
}
