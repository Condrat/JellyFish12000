using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class TestAnimation1 : Animation
    {        
        private int m_Offset = 0;        
        
        public TestAnimation1()
        {
            UpdatePeriod = 0.05f;
            m_Name = "Test Animation 1";
        }
            
        public override void GenerateNewFrame(float dt)
        {            
            AnimationFrame output = new AnimationFrame();

            Color color1 = Color.Red;
            Color color2 = Color.Green;
            Color color3 = Color.Blue;
                        
            switch (m_Offset++ % 3)
            {
                case 0:
                    color1 = Color.Red;
                    color2 = Color.Green;
                    color3 = Color.Blue;
                    break;
                case 1:
                    color1 = Color.Blue;
                    color2 = Color.Red;
                    color3 = Color.Green;                    
                    break;
                case 2:
                    color1 = Color.Green;
                    color2 = Color.Blue;
                    color3 = Color.Red;                    
                    break;
            }

            for (int i = 0; i < 12; ++i)
            {
                output.SetRibColor(i * 3 + 0, color1);
                output.SetRibColor(i * 3 + 1, color2);
                output.SetRibColor(i * 3 + 2, color3);
            }

            m_CurrentFrame = output;
        }
    }
}
