using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class RingState
	{
		public float CurrentRow = -1.0f;
		public float Speed = -1.0f;
		public bool Up = false;
		public Color Color = Color.White;
	}
        
	class BouncingRings : Animation
	{
		private int m_minRings = 4;
		private int m_maxRings = 15;
		private List<RingState> m_Rings = new List<RingState>();

		private float m_sinWaveIncrement = 0.02f;
		private double m_currentSinParameter = 0.0;

		public BouncingRings()
		{
			m_Name = "BouncingRings";
			UpdatePeriod = 0.035f;

			Random random = new Random(Environment.TickCount);

			for (int i = 0; i < m_maxRings; i++)
			{
				RingState newRing = new RingState();
				newRing.CurrentRow = random.Next(0, Dome.LEDS_PER_RIB);
				newRing.Speed = 1.0f;
				newRing.Up = (i % 2 == 0);
                newRing.Color = ColorManager.RandomColor();                	

				m_Rings.Add(newRing);
			}
		}
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_STROBE, 64, 64, 64, 1, 0, 0);
        }
		public override void GenerateNewFrame(float dt)
		{
			m_currentSinParameter += m_sinWaveIncrement;

			double currentSin = Math.Sin(m_currentSinParameter);
			currentSin += 1.0f;
			currentSin *= 0.5f;

			int ringDelta = m_maxRings - m_minRings;
			int peakWithinRange = (int)(currentSin * ringDelta);
			int currentRings = m_minRings + peakWithinRange;

            Reduce(.5f);
			//AnimationFrame output = new AnimationFrame(); // get a clear frame

			for (int i = 0; i < currentRings; i++)
			{
				RingState ring = (RingState)m_Rings[i];

				if (ring.Up)
				{
					ring.CurrentRow -= ring.Speed;
					if (ring.CurrentRow < 0)
					{
						ring.Up = !ring.Up;
						ring.CurrentRow = 0;
					}
				}
				else
				{
					ring.CurrentRow += ring.Speed;
					if (ring.CurrentRow > Dome.LEDS_PER_RIB- 1)
					{
						ring.Up = !ring.Up;
						ring.CurrentRow = Dome.LEDS_PER_RIB - 1;
					}
				}

                m_CurrentFrame.SetRowColor((int)ring.CurrentRow, ring.Color);			
			}

			//m_CurrentFrame = output;
			
		}

	}
}
