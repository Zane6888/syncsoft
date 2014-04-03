using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace syncsoft
{
    /// <summary>
    /// Used to handle any connection
    /// </summary>
    public abstract class ConnectionHandler
    {
        public static ConnectionHandler getConnectionHandler(String protocolName)
        {
           
            switch(protocolName)
            {
                case "PCCP":
                    return new PCCPHandler();
                default:
                    throw new ArgumentException("\"" + protocolName + "\" is not a valid protocol name.");
            }
        }

        /// <summary>
        /// Syncronize files  
        /// </summary>
        /// <param name="files">Files and directorys to syncronize, </param>
        /// <returns>Operation Sucessfull</returns>
        public abstract bool sync(List<String> files);
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
