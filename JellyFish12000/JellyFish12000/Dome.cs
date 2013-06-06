using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System.Net.Sockets;

namespace JellyFish12000
{
	class Dome
	{
		public const int NUM_RIBS = 36;
		public const int LEDS_PER_RIB = 50;
		public const int TOTAL_LEDS = NUM_RIBS * LEDS_PER_RIB;
		public const String JELLYBRAIN_ADDRESS = "10.0.1.29";

		private static JellyVertex[] m_Lights = null;
		private static DynamicVertexBuffer m_VB = null;
		private static IndexBuffer m_IB = null;		
		private static Effect m_DomeEffect = null;

		private static Socket m_Socket = null;
		private static bool m_AttemptConnect = false;
        private static bool m_RenderEnabled = true;
                
		static Dome()
		{
			Init();
		}
        
		public static void Init()
		{
			// set up the renderable assets
			InitIndexBuffer();
			InitVertexBuffer();

			m_DomeEffect = Core.GetContent().Load<Effect>("Dome");

			if (m_Socket == null && m_AttemptConnect)
			{
				try
				{
					m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
					m_Socket.Connect(JELLYBRAIN_ADDRESS, 80);
				}
				catch (Exception e)
				{
					m_Socket.Dispose();
					m_Socket = null;
					e.ToString();
				}
			}
		}

        public static void SetFrame(AnimationFrame nextFrame)
        {
            int index = 0;
            int bufferIndex = 0;
            byte[] buffer = new byte[TOTAL_LEDS * 3];

            //channel 0
            for (int rib = 0; rib < Dome.NUM_RIBS / 2; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color newColor = nextFrame.GetLedColor(rib, row);

                    if (m_RenderEnabled)
                    {
                        index = (rib * Dome.LEDS_PER_RIB + row) * 4;
                        m_Lights[index + 0].Color = newColor;
                        m_Lights[index + 1].Color = newColor;
                        m_Lights[index + 2].Color = newColor;
                        m_Lights[index + 3].Color = newColor;
                    }

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
                    Color newColor = nextFrame.GetLedColor(rib, row);

                    if (m_RenderEnabled)
                    {
                        index = (rib * Dome.LEDS_PER_RIB + row) * 4;
                        m_Lights[index + 0].Color = newColor;
                        m_Lights[index + 1].Color = newColor;
                        m_Lights[index + 2].Color = newColor;
                        m_Lights[index + 3].Color = newColor;
                    }

                    if (rib % 2 == 1)
                        bufferIndex = ((Dome.NUM_RIBS - rib + 17) * Dome.LEDS_PER_RIB + row) * 3;
                    else
                        bufferIndex = ((Dome.NUM_RIBS - rib + 17) * Dome.LEDS_PER_RIB + (Dome.LEDS_PER_RIB - row - 1)) * 3;

                    buffer[bufferIndex + 0] = newColor.B;
                    buffer[bufferIndex + 1] = newColor.G;
                    buffer[bufferIndex + 2] = newColor.R;
                                                           
                }
            }

			if (m_Socket != null)
			{
				byte[] recvBuffer = new byte[16];
				m_Socket.Send(buffer);
				m_Socket.Receive(recvBuffer, 1, 0);
			}

            if(m_RenderEnabled)
            {
                m_VB.SetData(m_Lights);
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
			m_Lights = new JellyVertex[TOTAL_LEDS * 4];

			// corners never change
			for (int i = 0; i < TOTAL_LEDS; i++)
			{
				m_Lights[i * 4 + 0].Corner = new Short2(-1, -1);
				m_Lights[i * 4 + 1].Corner = new Short2(1, -1);
				m_Lights[i * 4 + 2].Corner = new Short2(1, 1);
				m_Lights[i * 4 + 3].Corner = new Short2(-1, 1);
			}

			// create the actual dome structure
			float cos = 1.0f;
			float sin = 1.0f;
			float radiansBetweenRibs= (float)(2 * Math.PI / NUM_RIBS);
			float radiansBetweenRows = (float)((Math.PI / 2) / LEDS_PER_RIB);
			float radius = 250.0f;
			float height = 250.0f;

			int curLight = 0;
			for (int rib = 0; rib < NUM_RIBS; ++rib)
			{
				double ribAngle = rib * radiansBetweenRibs;
                cos = (float)Math.Cos(ribAngle);
                sin = (float)Math.Sin(ribAngle);                

				for (int led = 0; led < LEDS_PER_RIB; ++led)
				{
					float rowRadius = (float)Math.Cos(led * radiansBetweenRows) * radius + 1.0f;
					float rowHeight = (float)Math.Sin(led * radiansBetweenRows) * height;

					Vector3 pos = new Vector3(cos * rowRadius, sin * rowRadius, rowHeight);

                    int index = curLight * 4;
                    m_Lights[index + 0].Color = Color.Black;
					m_Lights[index + 1].Color = Color.Black;
					m_Lights[index + 2].Color = Color.Black;
					m_Lights[index + 3].Color = Color.Black;

                    m_Lights[index + 0].Position = pos;
                    m_Lights[index + 1].Position = pos;
                    m_Lights[index + 2].Position = pos;
                    m_Lights[index + 3].Position = pos;

					++curLight;
				}
			}

			m_VB.SetData(m_Lights);
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

	}
}


