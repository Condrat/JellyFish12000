using System;
using System.Collections.Generic;


namespace JellyFish12000.Animations
{
    class RowState
    {
        public double offset;        
        public int row;      

        public RowState(int row, double offset)
        {
            this.offset = offset;            
            this.row = row;
        }
    }

    class SpinningRainbow : Animation
    {
        private List<RowState> m_Rows = new List<RowState>();
        Random m_Random = new Random();

        public SpinningRainbow()
        {
            m_Name = "Spinning Rings";
            UpdatePeriod = 0.0f;

            double offset = 0.0f;
            double delta = 1.0 / Dome.NUM_RIBS;
            for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
            {                
                m_Rows.Add(new RowState(row, offset));
                offset += delta;
            }
        }

        public override void GenerateNewFrame(float dt)
        {
            foreach (RowState rState in m_Rows)
            {                
                double delta = 1.0 / Dome.NUM_RIBS;
                double test = rState.offset;

                for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
                {
                    double v = 0.0;
                    test += delta;
                    v = test - Math.Truncate(test);
                    
                    m_CurrentFrame.SetDomeLEDColor(rib, rState.row, ColorManager.GetColor(v));
                }

                rState.offset += dt * 0.5f;
            }            
        }
    }
}
