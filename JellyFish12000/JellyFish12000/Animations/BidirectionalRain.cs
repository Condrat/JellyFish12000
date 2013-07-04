using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class BidirectionalRain : Animation
    {
        //TODO: this should be a generic particle...

        public class Drop
        {
            public int rib = 0;
            public int row = 0;
            public int dir = -1;
            public Color color = ColorManager.RandomColor();

            //TODO: color

            public Drop(int rib, int row, int dir)
            {
                this.rib = rib;
                this.row = row;
                this.dir = dir;
            }

            public void Update(float dt)
            {
                row += dir;
            }
        }

        private int m_MaxDrops = 100;
        public List<Drop> m_Drops = new List<Drop>();
        Random m_Random = new Random();

        public BidirectionalRain()
        {
            m_Name = "Bidirectional Rain";
            UpdatePeriod = 0.035f;

            for (int drop = 0; drop < m_MaxDrops; ++drop)
            {
                int row = m_Random.Next(Dome.LEDS_PER_RIB);
                int rib = m_Random.Next(Dome.NUM_RIBS);
                int dir = m_Random.Next(2) == 1 ? 1 : -1;

                m_Drops.Add(new Drop(rib, row, dir));
            }
        }

        public override void GenerateNewFrame(float dt)
        {
            Reduce(.9f);

            foreach (Drop d in m_Drops)
            {
                m_CurrentFrame.SetDomeLEDColor(d.rib, d.row, d.color);
                d.Update(dt);
                if (d.row < 0)
                {
                    d.rib = m_Random.Next(Dome.NUM_RIBS);
                    d.row = Dome.LEDS_PER_RIB - 1;
                    d.color = ColorManager.RandomColor();
                }
                else if (d.row >= Dome.LEDS_PER_RIB)
                {
                    d.rib = m_Random.Next(Dome.NUM_RIBS);
                    d.row = 0;
                    d.color = ColorManager.RandomColor();
                }
            }
        }
    }
}
