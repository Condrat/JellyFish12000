using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Microsoft.Xna.Framework;
using MFToolkit.Net.XBee;

// Satellite devices are other vehicles or objects that are controlled
// wirelessly by the "mothership" Jellyfish.  For now, these devices
// are controlled via XBee wireless communications.


namespace JellyFish12000
{
    class SatelliteDevices
    {
        static private XBeeDevice m_XBee = null;

        private const string IDENTIFIER_SATELLITE = "SAT";
        private const string IDENTIFIER_PENDANT = "PEN";
        private const int IDENTIFIER_LENGTH = 3;

        private static bool m_FindingNodes = false;
        private static bool m_IsEnabled = false;


        static private List<NodeDiscover> m_Satellites;
        static private List<NodeDiscover> m_Pendants;

        static private byte[] m_XBeePacketData = null;


        enum DeviceType
        {   None
        ,   Satellite
        ,   Pendant
        };

        static SatelliteDevices()
        {
            m_XBee = new XBeeDevice();
            m_Satellites = new List<NodeDiscover>();
            m_Pendants = new List<NodeDiscover>();
            m_XBeePacketData = new byte[8];
        }

        static public List<string> ListSerialPorts()
        {
            List<string> ports = new List<string>();
            foreach (string str in SerialPort.GetPortNames())
            {
                ports.Add(str);
            }
            return ports;
        }
        static public bool Connect(string port)
        {
            return m_XBee.Open(port);
        }

        static public void Disconnect()
        {
            m_XBee.Close();
        }

        static public bool IsConnected
        {
            get
            {
                return m_XBee.IsOpen;
            }
        }

        static public bool IsEnabled
        {
            get
            {
                return m_IsEnabled;
            }
            set
            {
                m_IsEnabled = value;
            }
        }


        
        static public void FindDevices()
        {
            if (!IsConnected)
            {
                throw new System.InvalidOperationException("XBee not connected!");
            }
            MainForm.ConsoleWriteLine("XB: Discovering Nodes... (waiting 6 seconds)");
            m_FindingNodes = true;
            // Fire off our node discovery.
            Thread thread1 = new Thread(new ThreadStart(m_XBee.DiscoverNodes));
            thread1.IsBackground = true;
            thread1.Start();

            // Launch our node harvesting thread
            Thread thread2 = new Thread(new ThreadStart(HarvestNodes));
            thread2.IsBackground = true;
            thread2.Start();
        }

        static public void HarvestNodes()
        {
            Thread.Sleep(6 * 1000);
            //m_XBee.PrintNodes();
            m_Satellites.Clear();
            m_Pendants.Clear();
            foreach (NodeDiscover node in m_XBee.GetNodes())
            {
                string nodeID = node.NodeIdentifier;
                int index = -1;
                DeviceType deviceType = DeviceType.None;
                if ((index = nodeID.IndexOf(IDENTIFIER_SATELLITE)) != -1)
                {
                    deviceType = DeviceType.Satellite;
                }
                else if ((index = nodeID.IndexOf(IDENTIFIER_PENDANT)) != -1)
                {
                    deviceType = DeviceType.Pendant;
                }

                if (deviceType != DeviceType.None)
                {
                    try
                    {
                        int deviceID = Int32.Parse(nodeID.Substring(index + IDENTIFIER_LENGTH));
                    }
                    catch (FormatException e)
                    {
                        MainForm.ConsoleErrorWriteLine("XB: Malformed device ID on " + nodeID);
                        continue;
                    }
                    switch (deviceType)
                    {
                        case DeviceType.Satellite:
                            MainForm.ConsoleWriteLine("XB: Found Satellite: " + node.NodeIdentifier);
                            m_Satellites.Add(node);
                            break;
                        case DeviceType.Pendant:
                            MainForm.ConsoleWriteLine("XB: Found Pendant: " + node.NodeIdentifier);
                            m_Pendants.Add(node);
                            break;
                        default:
                            break;
                    }
                }
            }

            m_Satellites.Sort(delegate(NodeDiscover a, NodeDiscover b)
            {
                return string.Compare(a.NodeIdentifier, b.NodeIdentifier);
            });

            m_Pendants.Sort(delegate(NodeDiscover a, NodeDiscover b)
            {
                return string.Compare(a.NodeIdentifier, b.NodeIdentifier);
            });
            m_FindingNodes = false;
        }

        // Generate 16-bit 5:6:5 BGR color.  
        // Return as two bytes: <MSB, LSB>
        static private KeyValuePair<byte,byte> GetColorBGR16(Color c)
        {
            //int color = ((c.B & 0xF8) << 8) | ((c.G & 0xFC) << 3) | ((c.R & 0xF8) >> 3);
            int color = ((c.B >> 3) << 11) | ((c.G >> 2) << 5) | (c.R >> 3);

            return new KeyValuePair<byte,byte>((byte)((color >> 8) & 0xFF), (byte)((color) & 0xFF));
        }

        static public void SetSatelliteParameters(Animation.SatelliteParameters parameters)
        {
            m_XBeePacketData[0] = Convert.ToByte('p');
            m_XBeePacketData[1] = parameters.pattern;
            m_XBeePacketData[2] = parameters.r;
            m_XBeePacketData[3] = parameters.g;
            m_XBeePacketData[4] = parameters.b;
            m_XBeePacketData[5] = parameters.param0;
            m_XBeePacketData[6] = parameters.param1;
            m_XBeePacketData[7] = parameters.param2;
        }

        //static public void UpdateSatellites(Color[,] satelliteLEDs, Color[,] pendantLEDs)
        static public void UpdateSatellites()
        {
            if (!IsEnabled || m_FindingNodes || !IsConnected)
            {
                return;
            }
            /*
            byte[] buffer = new byte[8];

            for(int i = 0; i < 8; i++)
            {
                buffer[i] = 0;
            }

            buffer[0] = Convert.ToByte('p');

            const byte PATTERN_LINEARFADE = 1;
            const byte PATTERN_RAINBOW = 2;
            const byte PATTERN_GLITTER = 3;

            switch(m_pattern)
            {
                case 0:
                    buffer[1] = PATTERN_LINEARFADE;
                    buffer[2] = 0;
                    buffer[3] = 255;
                    buffer[4] = 0;
                    buffer[5] = 0; // CCW
                    buffer[6] = 2; // Speed
                    break;
                case 2:
                case 4:
                    buffer[1] = PATTERN_RAINBOW;
                    buffer[2] = 0;
                    buffer[3] = 0;
                    buffer[4] = 0; 
                    buffer[5] = 1; // CCW
                    buffer[6] = 2; // Speed
                    break;
                case 1:
                    buffer[1] = PATTERN_GLITTER;
                    buffer[2] = 255;
                    buffer[3] = 0;
                    buffer[4] = 0;
                    buffer[5] = 0; // CCW
                    buffer[6] = 255; // CCW
                    buffer[7] = 255; // CCW
                    break;
                default:
                    buffer[1] = PATTERN_RAINBOW;
                    buffer[2] = 0;
                    buffer[3] = 0;
                    buffer[4] = 0;                     
                    buffer[5] = 0; // Clockwise
                    buffer[6] = 3; // Speed
                    break;           
            }


            m_pattern++;
            if (m_pattern == 6)
            {
                m_pattern = 0;
            }
            */
            foreach (NodeDiscover node in m_Satellites)
            {
                m_XBee.SendData(node, m_XBeePacketData);
            }

            
            /*


            // Max payload size = 72
            // command + index + offset + (2 * numLEDs)
            // 3 + 2 * 32 LEDs = 67 bytes
            
            // Packet format (set strand):
            // command, numLEDs, offset, [ numLEDs * [LSB, MSB] ]... 

            // In the final packet, we set numLEDs += 128
            // This indicates to the board to load the data after
            // that data frame has been received.

            const byte maxLEDs = 32;
            byte commandSetLED = Convert.ToByte('s');
            byte commandSetStrand = Convert.ToByte('X');
            byte commandDone = Convert.ToByte('d');
            const int headerBytes = 3;
            const int bytesPerLED = 2;

            byte[] buffer = new byte[headerBytes + bytesPerLED * maxLEDs];


            int nodeIndex = 0;
            foreach (NodeDiscover node in m_Satellites)
            {
                //node
                int ledSubIndex = 0;
                byte ledOffset = 0;
                for(int led = 0; led < Dome.LEDS_PER_SATELLITE; ++led, ++ledSubIndex)
                {
                    if (ledSubIndex >= (maxLEDs* 2))
                    {
                        buffer[0] = commandSetStrand;
                        buffer[1] = maxLEDs;
                        buffer[2] = ledOffset;
                        MainForm.ConsoleWrite("Sending ("+ node.SerialNumber + " " + node.ShortAddress + " " + node.NodeIdentifier + ")...");
                        try
                        {
                            m_XBee.SendData(node, buffer);
                            MainForm.ConsoleWriteLine("done.");
                        }
                        catch (TimeoutException e)
                        {
                            MainForm.ConsoleErrorWriteLine("" + e.ToString());
                        }
                        

                        ledOffset += maxLEDs;
                        ledSubIndex = 0;
                    }

                    Color c = satelliteLEDs[nodeIndex, led];
                    
                    KeyValuePair<byte, byte> color16BGR = GetColorBGR16(c);
                    byte msb = color16BGR.Key;
                    byte lsb = color16BGR.Value;

                    int bufferIndex = (bytesPerLED * ledSubIndex) + headerBytes;

                    buffer[bufferIndex] = lsb;
                    buffer[bufferIndex + 1] = msb;
                   
                }

                // Send out last packet:
                buffer[0] = commandSetStrand;
                // Add 128 to indicate that it should be shown:
                buffer[1] = (byte)(128 + ledSubIndex);
                buffer[2] = ledOffset;
                MainForm.ConsoleWrite("Sending final...");
                try
                {
                    m_XBee.SendData(node, buffer);
                    MainForm.ConsoleWriteLine("done.");
                }
                catch (TimeoutException e)
                {
                    MainForm.ConsoleErrorWriteLine("" + e.ToString());
                }


                nodeIndex++;
            }
             */
        }
    }
}
