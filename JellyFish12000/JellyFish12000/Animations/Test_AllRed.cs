using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Test_AllRed : Animation
    {       
        public Test_AllRed()
        {
			m_Duration = 2.0f;
            m_Name = "All Red";
            UpdatePeriod = float.MaxValue;
            m_CurrentFrame.SetFrameColor(Color.Red);
        }        
    }
}
