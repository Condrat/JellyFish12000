﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;

namespace JellyFish12000
{
    class Dome
    {
        public const int NUM_RIBS = 36;
        public const int LEDS_PER_RIB = 50;
        public const int NUM_PENDANTS_MAX = 32;
        public const int LEDS_PER_PENDANT_MAX = 3;
        public const int NUM_SATELLITES = 2;
        public const int LEDS_PER_SATELLITE = 32;

        public const String JELLYBRAIN_ADDRESS = "10.0.1.29";
        
        public const int TOTAL_LEDS_DOME = NUM_RIBS * LEDS_PER_RIB;

        // These state a maximum number of pendants/LEDs for memory
        // allocation purposes.  There may actually be fewer of these.
        public const int TOTAL_LEDS_PENDANTS = NUM_PENDANTS_MAX * LEDS_PER_PENDANT_MAX;
        public const int TOTAL_LEDS_SATELLITES = NUM_SATELLITES * LEDS_PER_SATELLITE;

        public const int TOTAL_LEDS = TOTAL_LEDS_DOME + TOTAL_LEDS_PENDANTS + TOTAL_LEDS_SATELLITES;

        private static int m_PendantLightIndexOffset = 0;
        private static int m_SatelliteLightIndexOffset = 0;

        // TEMPORARY code until actual PendantController exists:
        // private static PendantController m_PendantController = null;
        private static int m_NumPendants = NUM_PENDANTS_MAX;
        private static int m_NumPendantLEDs = LEDS_PER_PENDANT_MAX;

        // Controller of satellite devices such as pendants or other cars:
                
        private static JellyVertex[] m_Lights = null;
        private static DynamicVertexBuffer m_VB = null;
        private static IndexBuffer m_IB = null;
        private static Effect m_DomeEffect = null;

        private static Socket m_Socket = null;
        private static bool m_AttemptConnect = true;
        private static bool m_RenderEnabled = true;

        // If the color is ~black, show a minimum bright point to indicate where the LED is.
        private static bool m_RequireRenderMinimumBrightness = true;
        private static int m_RenderMinimumBrightness = 24;

        private static Stopwatch m_GlobalTimer;


        static Dome()
        {
            Init();
        }

        public static void Init()
        {
            // set up the renderable assets
            InitIndexBuffer();
            InitVertexBuffer();

            m_GlobalTimer = new Stopwatch();
            m_GlobalTimer.Start();

            MainForm.RefreshXBeeCOMPortList();

            m_DomeEffect = Core.GetContent().Load<Effect>("Dome");
            if (m_Socket == null && m_AttemptConnect)
            {
                MainForm.ConsoleWrite("Connecting to dome...");
                try
                {
                    m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    m_Socket.Connect(JELLYBRAIN_ADDRESS, 80);
                    MainForm.ConsoleGoodWriteLine("Connected!");
                }
                catch (Exception e)
                {
                    MainForm.ConsoleErrorWriteLine("Could not connect!");
                    m_AttemptConnect = false;
                    m_Socket.Dispose();
                    m_Socket = null;
                    e.ToString();
                }
            }
        }

        // Global elapsed seconds for timing that should synchronize
        // across transitions:
        public static long GetElapsedMilliseconds()
        {
            return m_GlobalTimer.ElapsedMilliseconds;
        }

        public static void SetFrame(AnimationFrame nextFrame)
        {
            int bufferIndex = 0;
            byte[] buffer = new byte[TOTAL_LEDS_DOME * 3];

            //channel 0
            for (int rib = 0; rib < Dome.NUM_RIBS / 2; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color newColor = nextFrame.GetDomeLEDColor(rib, row);

                    if (rib % 2 == 0)
                        bufferIndex = (rib * Dome.LEDS_PER_RIB + row) * 3;
                    else
                        bufferIndex = (rib * Dome.LEDS_PER_RIB + (Dome.LEDS_PER_RIB - row - 1)) * 3;

                    buffer[bufferIndex + 0] = newColor.B;
                    buffer[bufferIndex + 1] = newColor.G;
                    buffer[bufferIndex + 2] = newColor.R;
                }
            }

            //channel 1
            for (int rib = Dome.NUM_RIBS - 1; rib >= Dome.NUM_RIBS / 2; --rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color newColor = nextFrame.GetDomeLEDColor(rib, row);

                    if (rib % 2 == 1)
                        bufferIndex = ((Dome.NUM_RIBS - rib + 17) * Dome.LEDS_PER_RIB + row) * 3;
                    else
                        bufferIndex = ((Dome.NUM_RIBS - rib + 17) * Dome.LEDS_PER_RIB + (Dome.LEDS_PER_RIB - row - 1)) * 3;

                    buffer[bufferIndex + 0] = newColor.B;
                    buffer[bufferIndex + 1] = newColor.G;
                    buffer[bufferIndex + 2] = newColor.R;

                }
            }

            if (m_RenderEnabled)
            {
                RenderLEDs(nextFrame.GetDomeLEDColor, SetRendererDomeLight, Dome.NUM_RIBS, Dome.LEDS_PER_RIB);
                RenderLEDs(nextFrame.GetPendantLEDColor, SetRendererPendantLight, Dome.NUM_PENDANTS_MAX, Dome.LEDS_PER_PENDANT_MAX);
                RenderLEDs(nextFrame.GetSatelliteLEDColor, SetRendererSatelliteLight, Dome.NUM_SATELLITES, Dome.LEDS_PER_SATELLITE);

                // Send frame-light data to the simulator:
                m_VB.SetData(m_Lights);
            }


            // Send frame-light data to the Jellyfish light controller:
            //MainForm.ConsoleWriteLine(".");

            if (m_Socket == null && m_AttemptConnect)
            {
                MainForm.ConsoleWriteLine("Connecting to dome...");
                try
                {
                    m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    m_Socket.Connect(JELLYBRAIN_ADDRESS, 80);
                }
                catch (Exception e)
                { 
                    m_Socket.Dispose();
                    m_Socket = null;
                    m_AttemptConnect = false;
                    e.ToString();
                }
            }
            
            if (m_Socket != null)
            {
                byte[] recvBuffer = new byte[16];
                m_Socket.Send(buffer);
                m_Socket.Receive(recvBuffer, 1, 0);
            }

            //if (m_ShowOnce)
            //{
            //    m_ShowOnce = false;
            //    //SatelliteDevices.UpdateSatellites(nextFrame.SatelliteData, nextFrame.PendantData);
            //}
            

        }


        
        protected delegate Color GetLEDColor(int index, int led);
        protected delegate void SetLightColor(int index, int led, Color newColor);
        protected static void RenderLEDs(GetLEDColor get, SetLightColor set, int numObjects, int numLEDs)
        {
            for (int obj = 0; obj < numObjects; ++obj)
            {
                for (int led = 0; led < numLEDs; ++led)
                {
                    Color newColor = get(obj, led);
                    set(obj, led, newColor);
                }
            }
        }


        public static void Render(Matrix view, Matrix proj)
        {
            if (!m_RenderEnabled) return;

            GraphicsDevice device = Core.GetDevice();

            m_DomeEffect.Parameters["View"].SetValue(view);
            m_DomeEffect.Parameters["Projection"].SetValue(proj);
            m_DomeEffect.Parameters["ViewportScale"].SetValue(new Vector2(0.5f / device.Viewport.AspectRatio, -0.5f));

            // Restore the vertex buffer contents if the graphics device was lost.
            if (m_VB.IsContentLost)
            {
                m_VB = new DynamicVertexBuffer(device, JellyVertex.VertexDeclaration, TOTAL_LEDS * 4, BufferUsage.WriteOnly);
                m_VB.SetData(m_Lights);
            }

            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.DepthRead;

            // Set the vertex and index buffer.
            device.SetVertexBuffer(m_VB);
            device.Indices = m_IB;

            // Activate the particle effect.
            foreach (EffectPass pass in m_DomeEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, TOTAL_LEDS * 4, 0, TOTAL_LEDS * 2);
            }

            device.DepthStencilState = DepthStencilState.Default;
            device.SetVertexBuffer(null);
        }

        private static void InitIndexBuffer()
        {
            GraphicsDevice device = Core.GetDevice();

            // particle system is drawn using a trilist
            ushort[] indices = new ushort[TOTAL_LEDS * 6];
            for (int i = 0; i < TOTAL_LEDS; i++)
            {
                indices[i * 6 + 0] = (ushort)(i * 4 + 0);
                indices[i * 6 + 1] = (ushort)(i * 4 + 1);
                indices[i * 6 + 2] = (ushort)(i * 4 + 2);
                indices[i * 6 + 3] = (ushort)(i * 4 + 0);
                indices[i * 6 + 4] = (ushort)(i * 4 + 2);
                indices[i * 6 + 5] = (ushort)(i * 4 + 3);
            }

            m_IB = new IndexBuffer(device, typeof(ushort), indices.Length, BufferUsage.WriteOnly);
            m_IB.SetData(indices);
        }

        private static void InitVertexBuffer()
        {
            GraphicsDevice device = Core.GetDevice();
            m_VB = new DynamicVertexBuffer(device, JellyVertex.VertexDeclaration, TOTAL_LEDS * 4, BufferUsage.WriteOnly);

            // 4 verts per quad / particle
            m_Lights = new JellyVertex[4 * TOTAL_LEDS];

            // corners never change
            for (int i = 0; i < TOTAL_LEDS; i++)
            {
                m_Lights[i * 4 + 0].Corner = new Short2(-1, -1);
                m_Lights[i * 4 + 1].Corner = new Short2(1, -1);
                m_Lights[i * 4 + 2].Corner = new Short2(1, 1);
                m_Lights[i * 4 + 3].Corner = new Short2(-1, 1);
            }

            // create the actual dome structure
            //float cos = 1.0f;
            //float sin = 1.0f;
            float radiansBetweenRibs = (float)(2 * Math.PI / NUM_RIBS);
            float radiansBetweenRows = (float)((Math.PI / 2) / LEDS_PER_RIB);
            
            float radiusDome = 250.0f;
            float heightDome = 250.0f;


            int index = 0;


            for (int rib = 0; rib < NUM_RIBS; ++rib)
            {
                double ribAngle = rib * radiansBetweenRibs;
                float cos = (float)Math.Cos(ribAngle);
                float sin = (float)Math.Sin(ribAngle);

                for (int led = 0; led < LEDS_PER_RIB; ++led, index += 4)
                {
                    float rowRadius = (float)Math.Cos(led * radiansBetweenRows) * radiusDome + 1.0f;
                    float rowHeight = (float)Math.Sin(led * radiansBetweenRows) * heightDome;

                    Vector3 pos = new Vector3(cos * rowRadius, sin * rowRadius, rowHeight);
                    m_Lights[index + 0].Color = Color.White;
                    m_Lights[index + 1].Color = Color.White;
                    m_Lights[index + 2].Color = Color.White;
                    m_Lights[index + 3].Color = Color.White;

                    m_Lights[index + 0].Position = pos;
                    m_Lights[index + 1].Position = pos;
                    m_Lights[index + 2].Position = pos;
                    m_Lights[index + 3].Position = pos;
                }
            }

            // Init our pendant indicators

            // H
            float radiusPendants = 280.0f;
            float heightPendants = 16.0f;

            // Angle over which we want to render the pendants:
            float arcPendants = (float)Math.PI / 2;
            float radiansBetweenPendants = (float)(arcPendants / NUM_PENDANTS_MAX);
            // float radiansBetweenPendantLEDs = (float)((Math.PI / 2) / LEDS_PER_PENDANT_MAX);
            
            // The radius between pendant LEDs is spacing between LEDs in a pendant:
            float radiusBetweenPendantLEDs = 8.0f; // 32.0f / LEDS_PER_PENDANT_MAX;

            m_PendantLightIndexOffset = index / 4;
            for (int pendant = 0; pendant < NUM_PENDANTS_MAX; ++pendant)
            {
                // 3D-position: X and Y are on the ground, Z is up towards top of dome
                double angle = ((pendant + 0.5) * radiansBetweenPendants) + (Math.PI / 4);
                float cos = (float)Math.Cos(angle);
                float sin = (float)Math.Sin(angle);
                for (int led = 0; led < LEDS_PER_PENDANT_MAX; ++led, index += 4)
                {
                    float rowRadius = radiusPendants + led * radiusBetweenPendantLEDs; // (float)Math.Cos(led * radiansBetweenPendantLEDs) * radiusPendants + 1.0f;
                    float rowHeight = heightPendants; // *(float)Math.Sin(led * radiansBetweenPendantLEDs);
                    Vector3 pos = new Vector3(cos * rowRadius, sin * rowRadius, rowHeight);
                    m_Lights[index + 0].Color = Color.White;
                    m_Lights[index + 1].Color = Color.White;
                    m_Lights[index + 2].Color = Color.White;
                    m_Lights[index + 3].Color = Color.White;

                    m_Lights[index + 0].Position = pos;
                    m_Lights[index + 1].Position = pos;
                    m_Lights[index + 2].Position = pos;
                    m_Lights[index + 3].Position = pos;
                }
            }

            // Init our satellites:
            float arcSatellites = 2.0f * (float)Math.PI;
            float radiansBetweenSatelliteLEDs = (float)(arcSatellites / LEDS_PER_SATELLITE);
            float radiusSatellites = 64.0f;
            float xOffsetSatellites = 280.0f;
            float yOffsetSatellites = 280.0f;
            float zOffsetSatellites = 0.0f;

            m_SatelliteLightIndexOffset = index / 4;
            for (int satellite = 0; satellite < NUM_SATELLITES; ++satellite)
            {
                // 3D-position: X and Y are on the ground, Z is up towards top of dome

                float xOffset = (satellite == 0) ? xOffsetSatellites : -xOffsetSatellites;

                for (int led = 0; led < LEDS_PER_SATELLITE; ++led, index += 4)
                {
                    double angle = (led * radiansBetweenSatelliteLEDs);// +(Math.PI / 4);
                    float cos = (float)Math.Cos(angle);
                    float sin = (float)Math.Sin(angle);
                    Vector3 pos = new Vector3(xOffset + cos * radiusSatellites, yOffsetSatellites + sin * radiusSatellites, zOffsetSatellites);
                    m_Lights[index + 0].Color = Color.White;
                    m_Lights[index + 1].Color = Color.White;
                    m_Lights[index + 2].Color = Color.White;
                    m_Lights[index + 3].Color = Color.White;

                    m_Lights[index + 0].Position = pos;
                    m_Lights[index + 1].Position = pos;
                    m_Lights[index + 2].Position = pos;
                    m_Lights[index + 3].Position = pos;
                }
            }



            m_VB.SetData(m_Lights);
        }

        public static void SetRendererLightColor(int lightIndex, Color newColor)
        {
            if (m_RenderEnabled)
            {
                int index = lightIndex * 4;
                if(m_RequireRenderMinimumBrightness)
                {
                    // This code ensures that the rendered point has a minimum
                    // brightness to indicate where the LED is located on the dome
                    newColor = new Color
                        (   Math.Max((int)newColor.R, m_RenderMinimumBrightness)
                        ,   Math.Max((int)newColor.G, m_RenderMinimumBrightness)
                        ,   Math.Max((int)newColor.B, m_RenderMinimumBrightness)
                        );
                }
                m_Lights[index + 0].Color = newColor;
                m_Lights[index + 1].Color = newColor;
                m_Lights[index + 2].Color = newColor;
                m_Lights[index + 3].Color = newColor;
            }
        }

        public static void SetRendererDomeLight(int rib, int row, Color newColor)
        {
            if (m_RenderEnabled)
            {
                int index = (rib * LEDS_PER_RIB + row);
                SetRendererLightColor(index, newColor);
            }
        }

        public static void SetRendererPendantLight(int pendant, int led, Color newColor)
        {
            if (m_RenderEnabled)
            {
                int index = m_PendantLightIndexOffset + (pendant * LEDS_PER_PENDANT_MAX + led);
                SetRendererLightColor(index, newColor);
            }
        }

        public static void SetRendererSatelliteLight(int satellite, int led, Color newColor)
        {
            if (m_RenderEnabled)
            {
                int index = m_SatelliteLightIndexOffset + (satellite * LEDS_PER_SATELLITE + led);
                SetRendererLightColor(index, newColor);
            }
        }

        
        public static double GetRibRadians(int ribIndex)
        {
            double radiansPerRib = (double)(2.0f * Math.PI / Dome.NUM_RIBS);
            return (double)ribIndex * radiansPerRib;
        }

        public static int GetNearestRibByRadians(double radians)
        {
            double nearestRadiansDelta = -1.0f;
            int nearestRibIndex = -1;
            for (int i = 0; i < Dome.NUM_RIBS; i++)
            {
                double currentRibRadians = GetRibRadians(i);
                double radiansDelta = Math.Abs(radians - currentRibRadians);

                if (nearestRadiansDelta < 0 || radiansDelta < nearestRadiansDelta)
                {
                    nearestRadiansDelta = radiansDelta;
                    nearestRibIndex = i;
                }
            }

            return nearestRibIndex;
        }

        // Convenience functions:
        public static int NumPendants
        {
            get { return m_NumPendants; }
        }

        public static int NumPendantLEDs
        {
            get { return m_NumPendantLEDs; }
        }

        public static void SetNumPendants(int numPendants)
        {
            m_NumPendants = numPendants;
        }
        public static void SetNumPendantLEDs(int numLEDs)
        {
            m_NumPendantLEDs = numLEDs;
        }

    }
}


