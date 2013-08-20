using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class Rain : Animation
    {
        //TODO: this should be a generic particle...

        public class Drop
        {
            public int rib = 0;
            public int row = 0;            

            public Drop(int rib, int row)
            {
                this.rib = rib;
                this.row = row;                
            }

            public void Update(float dt)
            {
                --row;
            }
        }

        private int m_MaxDrops = 100;
        public List<Drop> m_Drops = new List<Drop>();
        Random m_Random = new Random();        

        public Rain()
        {
            m_Name = "Rain";
            UpdatePeriod = 0.025f;

            for (int drop = 0; drop < m_MaxDrops; ++drop)
            {
                int row = m_Random.Next(Dome.LEDS_PER_RIB);
                int rib = m_Random.Next(Dome.NUM_RIBS);                

                m_Drops.Add(new Drop(rib, row));
            }            
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_GLITTER, 64,64,64, 128,128,128);
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.95f);

            foreach (Drop d in m_Drops)
            {
                m_CurrentFrame.SetDomeLEDColor(d.rib, d.row, Color.White);
                d.Update(dt);
                if (d.row < 0)
                {
                    d.rib = m_Random.Next(Dome.NUM_RIBS);
                    d.row = Dome.LEDS_PER_RIB - 1;
                }
            }            
        }
    }
}
