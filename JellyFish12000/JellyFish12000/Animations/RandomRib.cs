using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class RandomRib : Animation
    {      
        Random m_Random = new Random();
        public RandomRib()
        {
            m_Name = "Random Rib";
            UpdatePeriod = 0.02f;            
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_RAINBOWLINES, 0, 0, 0, 0, 0, 0);
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.99999f);

            int rib = m_Random.Next(Dome.NUM_RIBS);
            Color color = ColorManager.RandomColor();
            for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
            {
                m_CurrentFrame.SetDomeLEDColor(rib, row, color);            
            }
        }
    }
}
