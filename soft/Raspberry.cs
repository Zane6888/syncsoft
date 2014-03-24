using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace syncsoft
{
    class Raspberry
    {
        /// <summary>
        /// Human readable name of this Raspberry. Returned on discovery.
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// Local IP address.
        /// </summary>
        public IPAddress IP { get; set; }
        /// <summary>
        /// MAC Address, used for identification. 
        /// </summary>
        public String MAC { get; set; }
        /// <summary>
        /// A List of sync protocols that can be used to communicate with the Raspberry. Returned on discovery.
        /// </summary>
        public List<String> AvailableProtocols { get; set; }
        /// <summary>
        /// Is null if the client never connected to this Raspberry. Returned on discovery/read from Config.
        /// </summary>
        public DateTime LastConnected { get; set; }

        private String _prefered;
        /// <summary>
        /// The prefered sync protocol for the Raspberry. Returned on request. Cached after first Call. Is null if the Raspberry knows multiple can't state it's prefered protocol.
        /// </summary>
        public String preferedProtocol
        {
            get
            {
                if (_prefered != null) 
                    return _prefered;

                if (AvailableProtocols == null || AvailableProtocols.Count == 0)
                    _prefered = null;
                else if (AvailableProtocols.Count == 1)
                    _prefered = AvailableProtocols[0];
                else
                {
                    //TODO fetch from Server
                }

                return _prefered;
            }
        }


        /// <summary>
        /// Scans the LAN for Available Raspberrys.
        /// </summary>
        public static List<Raspberry> discoverRaspberrys()
        {
            //TODO implement method
            return new List<Raspberry>();
        }

        

    }
}
