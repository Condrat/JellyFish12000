using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Spiral : Animation
    {
        private int m_CurrentRowIndex = 0;
        private int m_CurrentRibIndex = 0;        
        private Color m_Color = Color.White;        

        public Spiral()
        {
            m_Duration = 15.0f;
            m_Name = "Spiral";
            UpdatePeriod = 0.015f;
        }

        public override void Start()
        {
            base.Start();
            Random random = new Random(Environment.TickCount);
            m_Color = Color.FromNonPremultiplied(random.Next(128) + 128, random.Next(128) + 128, random.Next(128) + 128, 255);
        }

        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_TRAFFIC, 0, 0, 0, 255, 255, 255);
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.95f);
            if (m_CurrentRibIndex % (Dome.NUM_RIBS * 3) == 0)
            {
                Random random = new Random(Environment.TickCount);
                m_Color = Color.FromNonPremultiplied(random.Next(128) + 128, random.Next(128) + 128, random.Next(128) + 128, 255);
            }

            m_CurrentRibIndex++;

            // see if we've gone around this row one time.
            if (m_CurrentRibIndex >= (Dome.NUM_RIBS))
            {
                // wrap around the rib
                m_CurrentRibIndex = 0;
                m_CurrentRowIndex+=3;

                if (m_CurrentRowIndex >= (Dome.LEDS_PER_RIB- 1))
                {
                    // wrap back up to the top row
                    m_CurrentRowIndex = 0;
                    m_CurrentFrame.SetFrameColor(Color.Black);
                }
            }

            m_CurrentFrame.SetDomeLEDColor(m_CurrentRibIndex, m_CurrentRowIndex, m_Color);            
        }
    }
}
