using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Test_AllBlue : Animation
    {   
        public Test_AllBlue()
        {
			m_Duration = 2.0f;
            m_Name = "All Blue";
            UpdatePeriod = float.MaxValue;
            m_CurrentFrame.SetFrameColor(Color.Blue);
        }
    }
}
