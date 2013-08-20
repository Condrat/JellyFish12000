using System;

namespace JellyFish12000.Animations
{
    class RowColorWheel : Animation
    {
        private float m_Current = 1.0f;

        public RowColorWheel()
        {
            m_Name = "Row Color Wheel";
            UpdatePeriod = 0.0f;
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_LINEAR, 0, 0, 0, 1, 3, 128);
        }
        public override void GenerateNewFrame(float dt)
		{
            m_Current += dt * 0.5f;

            double test = m_Current;
            double delta = 1.1 / Dome.LEDS_PER_RIB;
                        
            for(int row = 0; row < Dome.LEDS_PER_RIB; ++row)
            {
                test += delta;
                double v = test - Math.Truncate(test);
                m_CurrentFrame.SetRowColor(row, ColorManager.GetColor(v));
            }            
		}
    }
}
