using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
namespace syncsoft
{
    /// <summary>
    /// Used to handle any connection
    /// </summary>
    public abstract class ConnectionHandler
    {
        public static ConnectionHandler getConnectionHandler(String protocolName,IPAddress ip)
        {
           
            switch(protocolName)
            {
                case "PCCP":
                    return new PCCPHandler(ip);
                default:
                    throw new ArgumentException("\"" + protocolName + "\" is not a valid protocol name.");
            }
        }
        protected IPAddress ip;

        /// <summary>
        /// Syncronize files  
        /// </summary>
        /// <param name="files">Files and directorys to syncronize, </param>
        /// <returns>Operation Sucessfull</returns>
        public abstract void sync(List<String> files);
        /// <summary>
        /// Reqest List of Client from Server
        /// </summary>
        /// <returns>List of Clients</returns>
        public abstract List<ClientInfo> getClientList();
        /// <summary>
        /// Request List of Files from Server
        /// </summary>
        /// <returns>List of Files</returns>
        public abstract List<String> getFileList();

    }
}
