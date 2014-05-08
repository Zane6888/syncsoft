using System;
using syncsoft;
using System.Net;
using System.IO;
using NUnit.Framework;

namespace RaspberryTests
{
    [TestFixture]    
    class PCCPHandlerTest
    {
        [Test]
        public void testSync()
        {
            Config.init();
            File.Create("test_1.txt");
            Config.addPath("/text_1.txt", Path.GetFullPath("text_1.txt"));
            PCCPHandler testHandler = new PCCPHandler(IPAddress.Loopback);
        }

    }
}
