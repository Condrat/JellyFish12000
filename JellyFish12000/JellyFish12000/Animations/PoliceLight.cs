using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace JellyFish12000.Animations
{
	class PoliceLight : Animation
	{		
		private float m_RibOffset = 0.0f;
        private Color m_ColorA = ColorManager.RandomColor();
        private Color m_ColorB = ColorManager.RandomColor();
        		
		public PoliceLight()
		{
			m_Name = "Police Light";
			UpdatePeriod = 0.0f;
		}

        public override void Start()
        {
            base.Start();
            m_ColorA = ColorManager.RandomColor();
            m_ColorB = ColorManager.RandomColor();
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_POLICE, m_ColorA.R, m_ColorA.G, m_ColorA.B, m_ColorB.R, m_ColorB.G, m_ColorB.B);
        }

		public override void GenerateNewFrame(float dt)
		{
            //TODO: should this be a constant?
			float radsBetweenRibs = (float)(Math.PI * 2.0 / (double)Dome.NUM_RIBS);

			//paint half the dome red, and half the dome blue
			for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
			{
				float rad = (rib * radsBetweenRibs) + m_RibOffset;

				if (Math.Sin(rad) <= 0)
					m_CurrentFrame.SetRibColor(rib, m_ColorA);
				else
                    m_CurrentFrame.SetRibColor(rib, m_ColorB);
			}

			m_RibOffset += dt * 4f;
		}
	}
}
