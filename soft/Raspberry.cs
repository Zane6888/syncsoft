using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace syncsoft
{
    public class Raspberry
    {
        /// <summary>
        /// Human readable name of this Raspberry. Returned on discovery.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// MAC Address, used for identification. 
        /// </summary>
        public String MAC { get; set; }

        /// <summary>
        /// Is null if the client never connected to this Raspberry. Returned on discovery/read from Config.
        /// </summary>
        public DateTime LastConnected { get; set; }

        /// <summary>
        /// Local IP address.
        /// </summary>
        public IPAddress IP
        {
            get
            {
                return connection.IP;
            }
        }

        /// <summary>
        /// Protocol used to communicate with this Raspberry.
        /// </summary>
        public String Protocol
        {
            get
            {
                return connection.Protocol;
            }
        }

        private ConnectionHandler connection;

        public Raspberry(IPAddress ip,String mac,String protocol,String name, DateTime last)
        {
            try
            {
                connection = ConnectionHandler.GetConnectionHandler(protocol, ip);
            }
            catch (ArgumentException ae)
            {
                throw ae;
            }

            MAC = mac;
            Name = name;
            LastConnected = last;
        }

        

        /// <summary>
        /// Scans the LAN for Available Raspberrys.
        /// </summary>
        public static void SendBroadcastDiscoverRaspberrys()
        {
            UdpClient udpClient = new UdpClient(PCCPHandler.PORT);
            try
            {
                udpClient.EnableBroadcast = true;
                IPAddress local = IPAddress.Parse("127.0.0.1");
                IPEndPoint broadcastEndPoint = new IPEndPoint(/*IPAddress.Broadcast*/local, PCCPHandler.PORT);
                
                Byte[] sendbytes = new byte[5];
                sendbytes[2] = (byte)'f';
                sendbytes[3] = (byte)'u';
                sendbytes[4] = (byte)'h';

                while (true)
                {
                    udpClient.Send(sendbytes, 0, broadcastEndPoint);
                }
                

                udpClient.Close();
            }
            catch (Exception e)
            {

            }
        }

        public static List<Raspberry> ReceiveBroadcastDiscoverRaspberrys()
        {
            UdpClient udpClient = new UdpClient(PCCPHandler.PORT);
            try
            {
                IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Any, PCCPHandler.PORT);
                udpClient.EnableBroadcast = true;
                Byte[] receivebytes = udpClient.Receive(ref broadcastEndPoint);
                string receivedata = Encoding.UTF8.GetString(receivebytes);
            }
            catch (Exception e)
            {
            }
            return new List<Raspberry>();
        }

        //public static void testsendRaspberrysanswer()
        //{
        //    UdpClient udpClient = new UdpClient(PCCPHandler.PORT);
        //    {
        //        try
        //        {
        //            udpClient.EnableBroadcast = true;
        //            IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Any, PCCPHandler.PORT);
        //            Byte[] receivebytes = udpClient.Receive(ref broadcastEndPoint);
        //            if (receivebytes[0] == (byte)1 && receivebytes[1] == (byte)0 && receivebytes[2] == (byte)0 && receivebytes[3] == (byte)0 && receivebytes[4] == (byte)0)
        //            {
        //                Byte[] sendbytes = new byte[5];
        //                sendbytes[0] = (byte)1;
        //                sendbytes[1] = (byte)251;
        //                sendbytes[2] = (byte)252;
        //                sendbytes[3] = (byte)253;
        //                sendbytes[4] = (byte)254;
        //                udpClient.Send(sendbytes, 0, broadcastEndPoint);
        //            }

        //        }
        //        catch (Exception e)
        //        {

        //        }
        //    }
        //}

    }
}
