using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace JellyFish12000
{
    class Core
    {
        // Main engine components
        protected static GraphicsDeviceManager m_Graphics = null;
        protected static ContentManager m_Content = null;
        protected static IGraphicsDeviceService m_DeviceService = null;
        protected static Game m_Game = null;

        // FrameRate
        private static float m_fpsCalcWait = 1.0f;
        private static float m_FrameCount = 0.0f;
        private static float m_FPS = 0.0f;
        public static float FrameRate
        {
            get { return m_FPS; }
        }

        public static Game GetGame()
        {
            return m_Game;
        }

        public static ContentManager GetContent()
        {
            return m_Content;
        }

        public static GraphicsDevice GetDevice()
        {
            return m_DeviceService.GraphicsDevice;
        }

        public static void Init(int width, int height)
        {
            m_Game = new Game();
            m_Content = m_Game.Content;
            m_Content.RootDirectory = "Content";

            m_Graphics = new GraphicsDeviceManager(m_Game);
            m_Graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            m_Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            m_Graphics.PreferredBackBufferWidth = width;
            m_Graphics.PreferredBackBufferHeight = height;
            m_Graphics.SynchronizeWithVerticalRetrace = false;
            m_Graphics.PreferMultiSampling = false;
            m_Graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(PreparingDeviceSettings);

            m_Graphics.ApplyChanges();

            if (!m_Graphics.SynchronizeWithVerticalRetrace)
                m_Game.IsFixedTimeStep = false;

            m_DeviceService = (IGraphicsDeviceService)m_Game.Services.GetService(typeof(IGraphicsDeviceService));

            // just to preload the asset. m_Content will return a cached copy later on
            Effect domeFX = m_Content.Load<Effect>("Dome");
            Texture2D texture = m_Content.Load<Texture2D>("Sprite");

            domeFX.Parameters["ParticleSize"].SetValue(2.5f);
            domeFX.Parameters["Texture"].SetValue(texture);
        }

        public static void Update(float dt)
        {
            // FPS Calculations
            ++m_FrameCount;
            m_fpsCalcWait -= dt;
            if (m_fpsCalcWait <= 0)
            {
                m_FPS = m_FrameCount;
                m_FrameCount = 0;
                m_fpsCalcWait += 1.0f;

                Console.WriteLine(String.Format("FPS: {0}", m_FPS));
            }
        }

        protected static void PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.Depth24Stencil8;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = SurfaceFormat.Color;
        }

        public static void Destroy()
        {
        }

        public static void ResetDevice(int width, int height)
        {
            m_Graphics.PreferredBackBufferWidth = Math.Max(width, m_Graphics.PreferredBackBufferWidth);
            m_Graphics.PreferredBackBufferHeight = Math.Max(height, m_Graphics.PreferredBackBufferHeight);
            m_Graphics.ApplyChanges();
        }
    }
}
