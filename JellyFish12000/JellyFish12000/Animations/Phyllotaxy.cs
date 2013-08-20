using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class Phyllotaxy : Animation
	{
		double m_sinWaveIncrement = 1.0;
		double m_currentSinParameter = 0.0f;

		int m_maxNodes = 100;
		int m_minNodes = 2;

        Color m_Color = ColorManager.RandomColor();

		public Phyllotaxy()
		{
			UpdatePeriod = 0.0125f;
			m_Name = "Phyllotaxy";
		}

        public override void Start()
        {            
            base.Start();
            m_Color = ColorManager.RandomColor();
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_SEIZURE, m_Color.R, m_Color.G, m_Color.B, 0, 0, 0);
        }

		public override void GenerateNewFrame(float dt)
		{
            Reduce(.85f);
			//AnimationFrame output = new AnimationFrame();
			Random random = new Random();

            m_currentSinParameter += (m_UpdatePeriod * m_sinWaveIncrement);

			//double a = (float)Dome.LEDS_PER_RIB;
			double currentSin = Math.Sin(m_currentSinParameter);
			currentSin += 1.0f;
			currentSin *= 1.5f;
			
			int totalNodes = (int)(currentSin * (float)(m_maxNodes - m_minNodes)); // number of nodes along the structure
			totalNodes += m_minNodes;
			double w = Dome.LEDS_PER_RIB; // radius of circle to fill (movie is 300 pixels wide)

			double phi = (Math.Sqrt(5.0f) + 1.0f) / 2.0f - 1.0f; // the golden ratio
			double ga = phi * 2.0f * Math.PI;    // the golden angle - used for phyllotaxis on many plants

			for (int i = 1; i <= totalNodes; i++)
			{
				double theta = i * ga;            // angle of phyllotaxis
				double r = Math.Log(1.0f + i * (Math.E - 1.0f) / totalNodes); // logarithmic distance to edge of movie (0-1)

				r *= w;
				r--;
				// make sure that this theta get bounded from [0,2π]
				int radians = (int)(theta / (1.0f * Math.PI));
				theta = theta - ((double)radians * (1.0f * Math.PI));

				int rib = Dome.GetNearestRibByRadians(theta);                
                m_CurrentFrame.SetDomeLEDColor(rib, (int)r, m_Color);

				// for some reason, this was only filling in from 0 to PI
				// so, i guess we'll draw the other side?  i dunno... :)
				theta += Math.PI;

				rib = Dome.GetNearestRibByRadians(theta);
                m_CurrentFrame.SetDomeLEDColor(rib, (int)r, m_Color);
			}


			//m_CurrentFrame = output;
		}

	}
}
