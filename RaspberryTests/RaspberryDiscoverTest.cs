using System;
using System.Threading;
using NUnit.Framework;
using syncsoft;


namespace RaspberryTests
{
    [TestFixture]
    public class RaspberryDiscoverTest
    {
        [Test]
        public void DiscoverRaspberry()
        {      
            System.Threading.Thread SendBroadcast = new System.Threading.Thread(Raspberry.SendBroadcastDiscoverRaspberrys);
            //System.Threading.Thread ReceiveBroadcast = new System.Threading.Thread(Raspberry.ReceiveBroadcastDiscoverRaspberrys);
            //TODO: Start threads, test if Receive receives data from Send
        }
    }
}
