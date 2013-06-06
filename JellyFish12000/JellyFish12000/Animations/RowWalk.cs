using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class RowWalk : Animation
    {
        private int m_CurrentRow = 0;

        public RowWalk()
        {
            m_Name = "Row Walk";
            UpdatePeriod = 0.025f;
        }
          
        public override void GenerateNewFrame(float dt)        
        {
            Reduce(.94f);

            Random random = new Random(Environment.TickCount);
            Color color = Color.FromNonPremultiplied(random.Next(128) + 128, random.Next(128) + 128, random.Next(128) + 128, 255);

            m_CurrentFrame.SetRowColor(m_CurrentRow, color);            
            m_CurrentRow = m_CurrentRow >= Dome.LEDS_PER_RIB - 1 ? 0 : m_CurrentRow + 1;
        }
    }
}
