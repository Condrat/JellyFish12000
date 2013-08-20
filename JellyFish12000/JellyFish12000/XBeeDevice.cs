using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using MFToolkit.IO;
using MFToolkit.Net.XBee;

namespace JellyFish12000
{
    class XBeeDevice
    {
        protected const int m_BaudRate = 9600;
        protected XBee m_XBee = null;
        //protected XBee m_XBee2 = null;
        protected bool m_IsOpen = false;
        protected bool m_FirstTime = true;

        protected List<NodeDiscover> m_Nodes = null;

        public XBeeDevice()
        {
            m_Nodes = new List<NodeDiscover>();
        }

        public XBeeDevice(string comPort, ApiType apiType = ApiType.EnabledWithEscaped)
        {
            Init(comPort, apiType);
        }

        public void Init(string comPort, ApiType apiType = ApiType.EnabledWithEscaped)
        {
            if (m_FirstTime)
            {
                m_XBee = new XBee(comPort, m_BaudRate, apiType);
                m_XBee.FrameReceived += new FrameReceivedEventHandler(m_XBee_FrameReceived);
                m_FirstTime = false;

                //m_XBee2 = new XBee("COM10", m_BaudRate, apiType);
                //m_XBee2.FrameReceived += new FrameReceivedEventHandler(m_XBee_FrameReceived2);
                //m_XBee2.Open();
            }
        }

        // Open the device with new parameters
        public bool Open(string comPort, ApiType apiType = ApiType.EnabledWithEscaped)
        {
            Init(comPort, apiType);
            if (m_IsOpen)
            {
                m_XBee.Close();
            }

            m_IsOpen = m_XBee.Open(comPort, m_BaudRate);
            return m_IsOpen;
        }

        // Open using pre-set parameters
        public bool Open()
        {
            if (m_FirstTime)
            {
                throw new System.ArgumentException("XBee device needs to be initialized with parameters.");
            }
            if (m_IsOpen)
            {
                m_XBee.Close();
            }

            m_IsOpen = m_XBee.Open();
            return m_IsOpen;
        }

        public void Close()
        {
            if (m_IsOpen)
            {
                m_XBee.Close();
            }
            m_IsOpen = false;
        }

        public bool IsOpen
        {
            get
            {
                return m_IsOpen;
            }
        }

        public List<NodeDiscover> GetNodes()
        {
            return m_Nodes;
        }

        public void PrintNodes()
        {
            int n = 0;
            foreach (NodeDiscover node in m_Nodes)
            {
                //MainForm.ConsoleWriteLine("Node " + n + ": " + node.ShortAddress.ToString());
                MainForm.ConsoleWriteLine("--- Node " + n + ": ");
                MainForm.ConsoleWriteLine("" + node);
                n++;
            }
        }

        public void DiscoverNodes()
        {
            //MainForm.ConsoleWriteLine("XBee: Discovering nodes...");
            AtCommand at = new NodeDiscoverCommand();
            m_Nodes.Clear();
            try
            {
                m_XBee.Execute(at);
            }
            catch (TimeoutException x)
            {
                MainForm.ConsoleErrorWriteLine("XB: Timeout error: " + x.Message);
            }
        }

        void m_XBee_FrameReceived(object sender, FrameReceivedEventArgs e)
        {
            if (e.Response is AtCommandResponse)
            {
                NodeDiscover nd = NodeDiscover.Parse((e.Response as AtCommandResponse));
                if (nd != null && nd.ShortAddress != null)
                {
                    m_Nodes.Add(nd);

                    //MainForm.ConsoleWriteLine("XBee: Discovered Node: " + nd);        
                }
            }

            if (e.Response is RxResponse64)
            {
                MainForm.ConsoleWriteLine("RX: " + ByteUtil.PrintBytes((e.Response as RxResponse64).Value));
                //Console.WriteLine("Recevied Rx64");
                //Console.WriteLine(ByteUtil.PrintBytes((e.Response as RxResponse64).Value));
            }
            if (e.Response is ZNetRxResponse)
            {
                MainForm.ConsoleWriteLine("RX: " + ByteUtil.PrintBytes((e.Response as ZNetRxResponse).Value));
            }

        }
        void m_XBee_FrameReceived2(object sender, FrameReceivedEventArgs e)
        {
            //if (e.Response is RxResponse64)
            //{
            if (e.Response is ZNetRxResponse)
            {
                MainForm.ConsoleWriteLine("RX: " + ByteUtil.PrintBytes((e.Response as ZNetRxResponse).Value));
            }
                //Console.WriteLine("Recevied Rx64");
                //Console.WriteLine(ByteUtil.PrintBytes((e.Response as RxResponse64).Value));
            //}
        }

        
        public void SendData(NodeDiscover node, byte[] payload)
        {
            //TxRequest64 packet = new TxRequest64(node.SerialNumber, payload);
            //TxRequest16 packet = new TxRequest16(node.ShortAddress, payload);
            ZNetTxRequest packet = new ZNetTxRequest(node.SerialNumber, node.ShortAddress, payload);
            //m_XBee.ExecuteNonQuery(packet);
            m_XBee.Execute(packet, 5000);
            Thread.Sleep(10);
        }

    }
}
