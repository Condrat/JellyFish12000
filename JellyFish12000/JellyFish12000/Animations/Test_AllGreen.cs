﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Test_AllGreen: Animation
    {
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_POLICEFADE, 0, 255, 0, 64, 0, 0);
        }
        public Test_AllGreen()
        {
			m_Duration = 2.0f;
            m_Name = "All Green";
            UpdatePeriod = float.MaxValue;
            m_CurrentFrame.SetFrameColor(Color.Green);
        }
    }
}
