using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class StraightSine02 : Animation
	{
		private float m_Delta = 0.0f;
        private Color m_Color = Color.White;

		public StraightSine02()
		{
			m_Name = "StraightSine02";
			UpdatePeriod = 0.005f;
		}

        public override void Start()
        {
            base.Start();
            Random random = new Random(Environment.TickCount);
            m_Color = Color.FromNonPremultiplied(random.Next(128) + 64, random.Next(128) + 64, random.Next(128) + 64, 255);
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_LINEAR, m_Color.R, m_Color.G, m_Color.B, 1, 3, 0);
        }

		public override void GenerateNewFrame(float dt)
		{
			for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
			{
				float ribValue = (float)rib + 1.0f;
				float brightness = (float)(Math.Sin(m_Delta * ribValue));

				brightness += 1.0f;
				brightness /= 2.0f;

                m_CurrentFrame.SetRibColor(rib, m_Color * brightness);
			}

			m_Delta += dt * .5f;
		}
	}
}
