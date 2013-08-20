using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class Hypocycloid : Animation
	{
		double m_sinWaveIncrement = 0.02;
		double m_currentSinParameter = 0.0f;

		public Hypocycloid()
		{
			UpdatePeriod = 0.03f;
			m_Name = "Hypocycloid";
		}
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_GLITTER, 0, 0, 0, 0, 0, 0);
        }

		public override void GenerateNewFrame(float dt)
		{
			//AnimationFrame output = new AnimationFrame();
            Reduce(.75f);
			Random random = new Random();

			m_currentSinParameter += m_sinWaveIncrement;

			double a = (float)Dome.LEDS_PER_RIB;
			double currentSin = Math.Sin(m_currentSinParameter);
			currentSin += 1.0f;
			currentSin *= 0.5f;
			double b = (a * currentSin);

			// change this to change the parameterization of the inner circle
			int thetaSteps = 360; // 36
			double radiansPerStep = (double)(2.0f * Math.PI / thetaSteps);

			for (int i = 0; i < thetaSteps; i++)
			{
				double theta = (double)i * radiansPerStep;
				double x = ((a - b) * Math.Cos(theta)) + (b * Math.Cos((theta * (a - b)) / b));
				double y = ((a - b) * Math.Sin(theta)) - (b * Math.Sin((theta * (a - b)) / b));
				double r = Math.Sqrt(Math.Pow(x, 2.0f) + Math.Pow(y, 2.0f)) - 1;
				double drawTheta = Math.Atan2(y, x);

				// ATan2 returns values from of -π≤θ≤π
				drawTheta += Math.PI;
				int rib = Dome.GetNearestRibByRadians(drawTheta);

                Color newColor = ColorManager.RandomColor();
                m_CurrentFrame.SetDomeLEDColor(rib, (int)r, newColor);
			}

			//m_CurrentFrame = output;
		}
	}
}
