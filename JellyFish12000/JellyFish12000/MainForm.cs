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
                
		public MainForm()
		{
			InitializeComponent();			
            Core.Init(domeViewer1.ClientSize.Width, domeViewer1.ClientSize.Height);

			Dome.Init();
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

		/*
		private void TestButton_Click(object sender, EventArgs e)
		{
			AnimationManager.SetCurrentAnimation("All Red");
		}
		 * */
	}
}
