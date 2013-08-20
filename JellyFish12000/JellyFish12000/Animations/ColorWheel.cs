using System;

namespace JellyFish12000.Animations
{
    class ColorWheel : Animation
	{
        private float m_Current = 1.0f;

        public ColorWheel()
		{
			m_Name = "Color Wheel";
            UpdatePeriod = 0.0f;            
		}
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_LINEAR, 0, 0, 0, 0, 0, 0);
        }
        	
		public override void GenerateNewFrame(float dt)
		{
            m_Current += dt  * 0.125f;

            if (m_Current > 1.0f)
                m_Current = 0.0f;                     

            m_CurrentFrame.SetFrameColor(ColorManager.GetColor(m_Current));
		}
	}
}
