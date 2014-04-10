using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace syncsoft
{
    /// <summary>
    /// Handles a connection using PCCP
    /// </summary>
    class PCCPHandler : ConnectionHandler
    {
        public const int PORT = 2727;

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

        public override bool sync(List<string> files)
        {
            
            //TODO implement
            throw new NotImplementedException();
        }
    }
}
