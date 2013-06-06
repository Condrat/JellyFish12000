using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class Strobe : Animation
	{
		private bool m_On = false;
        private int m_OffCount = 0;
        private const int OFF_DEFAULT = 150;
        private Color m_Color = Color.White;

		public Strobe()
        {
            m_Duration = 10.0f;
            m_Name = "Strobe";
            UpdatePeriod = 0.01f;
        }        

		public override void GenerateNewFrame(float dt)
		{
            if (m_On)
                m_CurrentFrame.SetFrameColor(m_Color);
            else
            {
                Reduce(.95f);
            }

            if (m_OffCount-- == 0)
            {
                m_On = true;
                m_OffCount = OFF_DEFAULT;
            }
            else
            {
                m_On = false;
                Random random = new Random(Environment.TickCount);
                m_Color = Color.FromNonPremultiplied(random.Next(128) + 128, random.Next(128) + 128, random.Next(128) + 128, 255);
            }
		}

	}
}
