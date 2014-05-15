using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

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
        
                NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
                IPAddress[] LocalBroadcastIPs = new IPAddress[Interfaces.Count()];
                foreach (NetworkInterface Interface in Interfaces)
                {
                    if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                    if (Interface.OperationalStatus != OperationalStatus.Up) continue;

                    byte[] MaskBytes = new byte[4];
                    byte[] LocalBroadcastIPBytes = new byte[4];
                    int i = 0;
                    UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                    foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                    {
                        try
                        {
                            if (UnicatIPInfo.Address.AddressFamily == AddressFamily.InterNetworkV6)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    MaskBytes[j] = (byte)~(UnicatIPInfo.IPv4Mask.GetAddressBytes().ElementAt(j));
                                    LocalBroadcastIPBytes[j] = (byte)((UnicatIPInfo.Address.GetAddressBytes().ElementAt(j)) | MaskBytes[j]);
                                }
                                LocalBroadcastIPs[i] = new IPAddress(LocalBroadcastIPBytes);
                                i++;
                            }

                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
                
                Byte[] sendbytes = new byte[5];
                sendbytes[0] = (byte)'b';
                sendbytes[1] = (byte)'r';
                sendbytes[2] = (byte)'o';
                sendbytes[3] = (byte)'a';
                sendbytes[4] = (byte)'d';
                //broadcast WLAN: 10.51.51.255
                foreach (IPAddress LocalBroadcastIP in LocalBroadcastIPs)
                {
                    udpClient.Send(sendbytes, 5, new IPEndPoint(LocalBroadcastIP, PCCPHandler.PORT));
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

    

    }
}
