using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class RandomRow : Animation
    {
        Random m_Random = new Random();
        public RandomRow()
        {
            m_Name = "Random Row";
            UpdatePeriod = 0.02f;
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_STROBE, 0, 0, 0, 1, 0, 0);
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.99999f);

            int row = m_Random.Next(Dome.LEDS_PER_RIB);
            Color color = ColorManager.RandomColor();

            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                m_CurrentFrame.SetDomeLEDColor(rib, row, color);
            }            
        }
    }
}
