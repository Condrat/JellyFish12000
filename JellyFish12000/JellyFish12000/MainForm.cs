using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

/******************************************************************************
 * Potential improvements
 * 
 * *** Prototyping ***
 * Gamma correction so app display more closely resembles actual LED response
 * 
 * Use a MIDI emulator for animation input development.
 * 
 * 
 * *** Animation ***
 * Animation should hold a reference to a ColorGenerator so blending between 
 * animations is seamless. Animations who don't care about having their own 
 * pallete would use the global one(s).
 * 
 * Add mappable inputs to Animation, allowing sub-classed animations to use 
 * or disregard inputs. Ideally these inputs would come from a controller or
 * another input device connected via a socket (smartphone, tablet, etc).
 * 
 * 
 * *** Blender ideas ***
 * Wipes
 * Noise
 * XOR
 * Use animation as blender
 * 
 * 
 * *** Animation ideas ***
 * Serato Video-SL integration
 * Scrolling text
 * Real-time video
 * Sound reactive 
 * PacMan
 * Hearts
 * 
 * 
 * *** Color Input ***
 * Enable a 2D 'surface' so a joystick or touchscreen might be used to map 
 * color to the current animation. 
 * 
 * 
 * *** Input ***
 * Use MIDI.NET to expose animation inputs from the miriad of controllers on
 * the market (including dj controllers!)
 * 
 * 
 * *** UI controls ***
 * Hardware compensation (Animation.Reduce)
 * Gamma correction sliders (r,g, and b)
 * Enable renderer
 * Enable socket connection
 * JellyBrain address(es)
 * 
 * 
 * *** Settings (xml configuration file) ***
 * Address of JellyBrain
 * Gamma correction values
 * Attempt socket connection
 * Renderer enabled
 * 
 * 
 * *** Optimizations / Fixes ***
 * Application_Idle is working acceptably, but a dedicted update/render thread
 * might be better.
 * 
 * Use a rendertarget for the particle colors - would be nice to use the GPU
 * to filter the frame.
 * 
 * Socket should elegantly handle connect, disconnect, and auto-reconnect
 * 
 * 
******************************************************************************/

namespace JellyFish12000
{
    using Timer = System.Windows.Forms.Timer;

	public partial class MainForm : Form
	{
        // Timing 
        private Stopwatch m_Timer = new Stopwatch();
        private Timer m_ColorUpdateTimer = new Timer();
        static private RichTextBox m_DomeConsole = null;
        static private ComboBox m_XBeeComPorts = null;
        static private Button m_XBeeConnectButton = null;
        static private Button m_XBeeFindDevicesButton = null;
        static private Button m_XBeeEnableSatellitesButton = null;

        public MainForm()
		{
			InitializeComponent();
            m_DomeConsole = DomeConsole;
            m_XBeeComPorts = XBeeComPorts;
            m_XBeeConnectButton = XBeeConnectButton;
            m_XBeeFindDevicesButton = XBeeFindDevicesButton;
            m_XBeeEnableSatellitesButton = XBeeEnableSatellitesButton;

            m_XBeeFindDevicesButton.Enabled = false;
            m_XBeeEnableSatellitesButton.Enabled = false;
            SetConnectButtonText(false);
            SetEnableSatellitesButtonText(false);
            Core.Init(domeViewer1.ClientSize.Width, domeViewer1.ClientSize.Height);

			//Dome.Init();
            ColorManager.Init();

            Application.Idle += delegate { Application_Idle(); };
            m_ColorUpdateTimer.Tick += new EventHandler(m_ColorUpdateTimer_Tick);
            m_ColorUpdateTimer.Interval = 10000; // in milliseconds
            m_ColorUpdateTimer.Start();            
		}

        void m_ColorUpdateTimer_Tick(object sender, EventArgs e)
        {
            ColorManager.NextRamp();
        }

        void Application_Idle()
        {
            // Calculate frame time
            m_Timer.Stop();
            float dt = (float)m_Timer.Elapsed.TotalSeconds;
            m_Timer.Reset();
            m_Timer.Start();
            
            Core.Update(dt);
            AnimationManager.Update(dt);
            domeViewer1.Invalidate();    
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ConsoleGoodWriteLine("Instructions: Select COM port of XBee, click connect, click Discover nodes, click again if not all nodes detected.  Click enable satellites to send data out to satellites.");
        }

        private void domeViewer1_Click(object sender, EventArgs e)
        {

        }

        public static void ConsoleWrite(String s)
        {
            MethodInvoker action = delegate
            {   
                m_DomeConsole.SelectionColor = Color.Black;
                m_DomeConsole.AppendText(s);
            };
            m_DomeConsole.BeginInvoke(action);
        }
        public static void ConsoleWriteLine(String s)
        {
            MethodInvoker action = delegate
            {   
                m_DomeConsole.SelectionColor = Color.Black;
                m_DomeConsole.AppendText(s + "\n");
            };
            m_DomeConsole.BeginInvoke(action);
        }

        public static void ConsoleGoodWriteLine(String s)
        {
            MethodInvoker action = delegate
            {   
                m_DomeConsole.SelectionColor = Color.DarkGreen;
                m_DomeConsole.AppendText(s + "\n");
            };
            m_DomeConsole.BeginInvoke(action);
        }
        public static void ConsoleErrorWriteLine(String s)
        {
            MethodInvoker action = delegate
            {   
                m_DomeConsole.SelectionColor = Color.DarkRed;
                m_DomeConsole.AppendText(s + "\n");
            };
            m_DomeConsole.BeginInvoke(action);
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void DomeConsole_TextChanged(object sender, EventArgs e)
        {
            // Stay scrolled to bottom
            DomeConsole.SelectionStart = DomeConsole.Text.Length; //Set the current caret position at the end
            DomeConsole.ScrollToCaret(); //Now scroll it automatically
        }

        public static ComboBox.ObjectCollection GetXBeeComPortList()
        {
            return m_XBeeComPorts.Items;
        }

        public static void RefreshXBeeCOMPortList()
        {
            m_XBeeComPorts.Items.Clear();
            foreach (string port in SatelliteDevices.ListSerialPorts())
            {
                m_XBeeComPorts.Items.Add(port);
            }
            if (m_XBeeComPorts.Items.Count > 0)
            {
                m_XBeeComPorts.SelectedIndex = 0;
            }
        }

        public static void SetConnectButtonText(bool connected = true)
        {
            if (connected)
            {
                m_XBeeConnectButton.ForeColor = Color.DarkRed;
                m_XBeeConnectButton.Text = "Disconnect";
            }
            else
            {
                m_XBeeConnectButton.ForeColor = Color.DarkGreen;
                m_XBeeConnectButton.Text = "Connect";
            }
        }

        private void XBeeConnectButton_Click(object sender, EventArgs e)
        {
            if (!SatelliteDevices.IsConnected)
            {
                string port = m_XBeeComPorts.SelectedItem.ToString();
                ConsoleWrite("XB: Connecting to wireless device on " + port + " ... ");

                if (SatelliteDevices.Connect(port))
                {
                    ConsoleGoodWriteLine("CONNECTED!");
                    SetConnectButtonText(true);
                    m_XBeeFindDevicesButton.Enabled = true;
                    m_XBeeEnableSatellitesButton.Enabled = true;
                }
                else
                {
                    ConsoleErrorWriteLine("Error connecting to COM port!");
                }
            }
            else
            {
                ConsoleWrite("XB: Disconnect.");
                SatelliteDevices.Disconnect();
                SetConnectButtonText(false);
                m_XBeeFindDevicesButton.Enabled = false;
                m_XBeeEnableSatellitesButton.Enabled = false;

            }
        }

        private void XBeeFindDevicesButton_Click(object sender, EventArgs e)
        {
            SatelliteDevices.FindDevices();
        }

        public static void SetEnableSatellitesButtonText(bool enabled = true)
        {
            if (enabled)
            {
                m_XBeeEnableSatellitesButton.ForeColor = Color.DarkRed;
                m_XBeeEnableSatellitesButton.Text = "Disable Satellites";
            }
            else
            {
                m_XBeeEnableSatellitesButton.ForeColor = Color.DarkGreen;
                m_XBeeEnableSatellitesButton.Text = "Enable Satellites";
            }
        }


        private void XBeeEnableSatellitesButton_Click(object sender, EventArgs e)
        {
            if (!SatelliteDevices.IsEnabled)
            {
                ConsoleWrite("XB: Enabling Satellites");
                SatelliteDevices.IsEnabled = true;
                SetEnableSatellitesButtonText(true);
                
            }
            else
            {
                ConsoleWrite("XB: Disabling Satellites");
                SatelliteDevices.IsEnabled = false;
                SetEnableSatellitesButtonText(false);
            }
        }

        





		/*
		private void TestButton_Click(object sender, EventArgs e)
		{
			AnimationManager.SetCurrentAnimation("All Red");
		}
		 * */
	}
}
