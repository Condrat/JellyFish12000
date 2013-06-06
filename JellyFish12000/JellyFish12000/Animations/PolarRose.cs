using System;
using Microsoft.Xna.Framework;


namespace JellyFish12000.Animations
{
    class PolarRose : Animation
    {
        double m_sinWaveIncrement = 0.01;
        double m_currentSinParameter = 0.0f;
        double m_kMax = 6.0f;
        
        public PolarRose()
        {
            UpdatePeriod = 0.01f;
            m_Name = "PolarRose";
        }

        public override void  GenerateNewFrame(float dt)
        {
            Reduce(.9f);
            //AnimationFrame output = new AnimationFrame();
            m_currentSinParameter += m_sinWaveIncrement;

            // change this to change the parameterization of the function
            int thetaSteps = 360; // 36
            double radiansPerStep = (double)(2.0f * Math.PI / thetaSteps);
            double a = (float)Dome.LEDS_PER_RIB;
            double thetaNaught = 0.0f;
            double currentSin = Math.Sin(m_currentSinParameter);
            currentSin += 1.0f;
            currentSin *= 0.5f;

            double k = currentSin * m_kMax;

            for (int i = 0; i < thetaSteps; i++)
            {
                double theta = (double)i * radiansPerStep;
                double radius = (a * Math.Cos((k * theta) + thetaNaught)) - 1;

                if (radius > 49.0)
                    radius = 49.0;
                else if (radius < 0.0)
                    radius = 0.0;

                int rib = Dome.GetNearestRibByRadians(theta);
                m_CurrentFrame.SetLedColor(rib, (int)radius, ColorManager.RandomColor()); 
            }

            //m_CurrentFrame = output;
        }
    }
}
