using System;

namespace JellyFish12000.Animations
{
    class RibColorWheel : Animation
    {
        private float m_Current = 1.0f;

        public RibColorWheel()
        {
            m_Name = "Rib Color Wheel";
            UpdatePeriod = 0.0f;
        }

        public override void GenerateNewFrame(float dt)
        {
            m_Current += dt * 0.5f;

            double test = m_Current;
            double delta = 1.0 / Dome.NUM_RIBS;

            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                test += delta;
                double v = test - Math.Truncate(test);
                m_CurrentFrame.SetRibColor(rib, ColorManager.GetColor(v));
            }
        }
    }
}
