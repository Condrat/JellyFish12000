using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class StraightSine01 : Animation
	{
		private float m_Delta = 0.0f;
        private Color m_Color = Color.White;

		public StraightSine01()
		{
			m_Name = "StraightSine01";
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
            return new SatelliteParameters(PATTERN_STROBE, m_Color.R, m_Color.G, m_Color.B, 0, 0, 0);
        }

		public override void GenerateNewFrame(float dt)
		{
			for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
			{
				float rowValue = (float)row + 1.0f;
				float brightness = (float)(Math.Sin(m_Delta * rowValue));

				brightness += 1.0f;
				brightness /= 2.0f;

				m_CurrentFrame.SetRowColor(row, m_Color * brightness);
			}

			m_Delta += m_UpdatePeriod;
		}
	}
}
