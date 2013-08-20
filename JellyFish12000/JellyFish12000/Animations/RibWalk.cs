using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class RibWalk : Animation
    {
        private int m_CurrentRib = 0;

        private double m_Current = 0.0;
        private double m_Delta = 0.01;

        public RibWalk()
        {
            m_Name = "Rib Walk";
            UpdatePeriod = 0.05f;
        }

        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_RAINBOWLINES, 0, 0, 0, 1, 2);
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.9f);
            
            Random random = new Random(Environment.TickCount);

            m_Current += m_Delta;
            if(m_Current >= 1.0)
                m_Current = 0;                       
            
            m_CurrentFrame.SetRibColor(m_CurrentRib, ColorManager.GetColor(m_Current));
            m_CurrentRib = m_CurrentRib >= Dome.NUM_RIBS - 1 ? 0 : m_CurrentRib + 1;
        }
    }
}
