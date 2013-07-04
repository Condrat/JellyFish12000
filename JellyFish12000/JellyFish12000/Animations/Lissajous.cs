using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class Lissajous : Animation
	{
		double m_XMax = 8.0f;
		double m_X = 1.0f;
		bool m_XIncreasing = true;

		public Lissajous()
		{
			UpdatePeriod = 0.035f;
			m_Name = "Lissajous";
		}

		public override void GenerateNewFrame(float dt)
		{
			//AnimationFrame output = new AnimationFrame();
            Reduce(.65f);
			Random random = new Random();

			double xFactor = Dome.LEDS_PER_RIB / Math.Sqrt(2.0f);
			double yFactor = Dome.LEDS_PER_RIB / Math.Sqrt(2.0f);

			if (m_XIncreasing)
			{
				m_X += 1.0f * m_UpdatePeriod;
				if (m_X >= m_XMax)
				{
					m_X = m_XMax;
					m_XIncreasing = false;
				}
			}
			else
			{
                m_X -= 1.0f * m_UpdatePeriod;
				if (m_X <= -m_XMax)
				{
					m_X = -m_XMax;
					m_XIncreasing = true;
				}
			}

			double wX = m_X;
			double wY = m_X + 4.0f;

			// change this to change the parameterization of the inner circle
			int thetaSteps = 360; // 36
			double radiansPerStep = (double)(2.0f * Math.PI / thetaSteps);

			// not sure if we want to really modulate this at all
			double c = dt;
			for (int i = 0; i < thetaSteps; i++)
			{
				double theta = (double)i * radiansPerStep;
				double x = xFactor * Math.Sin((wX * theta) + c);
				double y = yFactor * Math.Sin(wY * theta);
				double r = Math.Sqrt(Math.Pow(x, 2.0f) + Math.Pow(y, 2.0f));
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
