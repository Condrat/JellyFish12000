using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Test_AllBlue : Animation
    {
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_POLICEFADE, 0, 0, 255, 64, 0, 0);
        }
        public Test_AllBlue()
        {
			m_Duration = 2.0f;
            m_Name = "All Blue";
            UpdatePeriod = float.MaxValue;
            m_CurrentFrame.SetFrameColor(Color.Blue);
        }
    }
}
