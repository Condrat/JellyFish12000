using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000
{
    class Animation
    {
        protected AnimationFrame m_CurrentFrame = new AnimationFrame();

        // Duration of the effect:
        protected float m_Duration = 35.0f; //5 seconds is a good length for testing
        protected float m_RunTime = 0.0f;

        // Adjust these to change the update speed:

        protected float m_UpdatePeriod = 1.0f;
        protected float m_DefaultUpdatePeriod = 1.0f;

        private Color m_ColorPink = new Color(255, 160, 248);

        protected float UpdatePeriod
        {
            set
            {
                m_DefaultUpdatePeriod = value;
                m_UpdatePeriod = value;
            }
        }

        protected String m_Name = "NoName";
        public String Name
        {
            get { return m_Name; }
        }

        public bool Finished
        {
            get { return m_RunTime >= m_Duration; }
        }

        virtual public void Start()
        {
            m_RunTime = 0.0f;
        }

        virtual public void Stop()
        {
        }

        public void Update(float dt)
        {
            m_RunTime += dt;

            if (TimeToUpdate(dt))
            {
                GenerateNewFrame(dt);
                GeneratePendantGraphics(dt);
                GenerateSatelliteGraphics(dt);
            }
        }

        protected bool TimeToUpdate(float dt)
        {
            m_UpdatePeriod -= dt;
            if (m_UpdatePeriod <= 0.0f)
            {
                m_UpdatePeriod = m_DefaultUpdatePeriod;
                return true;
            }

            return false;
        }
                
        virtual public void GeneratePendantGraphics(float dt)
        {
            // Remember that the pendants are very bright, and can be irritating
            // to look at close-up at night.  Intensity values of 0.7 will be pretty
            // bright, and can allow you to make bright "pops" with higher values
            // as needed.

            // By using the global timer, the default pattern will sync up
            // across transitions, and does not need to rely on dt:
            float elapsedSeconds = Convert.ToSingle(Dome.GetElapsedMilliseconds() % 1000) / 1000;

            // Default pendant graphics:
            for (int pendant = 0; pendant < Dome.NUM_PENDANTS_MAX; ++pendant)
            {
                double right = elapsedSeconds + (pendant * 0.1f);
                right -= Math.Floor(right); // Gives us the fractional part
                double left = (1 - elapsedSeconds) + (pendant * 0.1f);
                left -= Math.Floor(left); // Gives us the fractional part

                for (int led = 0; led < Dome.LEDS_PER_PENDANT_MAX; ++led)
                {
                    // Blue moving to the right with red pulses to the left:
                    Color color0 = new Color((float)Math.Sin(2 * Math.PI * right), 0.0f, (float)left);
                    // Pink pulses moving left:
                    Color color1 = Color.Lerp(m_ColorPink, Color.Black, (float)right);
                    // Color color2 = new Color((float)Math.Sin(2 * Math.PI * dx0), 0.0f, (float)dx1);
                    GetCurrentFrame().SetPendantLEDColor(pendant, 0, color0);
                    GetCurrentFrame().SetPendantLEDColor(pendant, 1, color1);
                    GetCurrentFrame().SetPendantLEDColor(pendant, 2, color0);
                }
            }
        }

        virtual public void GenerateSatelliteGraphics(float dt)
        {
            // Grabs the graphics from the Dome:
            for (int led = 0; led < Dome.LEDS_PER_SATELLITE; ++led)
            {
                Color color = GetCurrentFrame().GetDomeLEDColor(led % Dome.NUM_RIBS, Dome.LEDS_PER_RIB / 2);
                GetCurrentFrame().SetSatelliteLEDColor(0, led, color);
                GetCurrentFrame().SetSatelliteLEDColor(1, led, color);
            }            
            /*
            float elapsedSeconds = Convert.ToSingle(Dome.GetElapsedMilliseconds() % 1000) / 1000;

            for (int led = 0; led < Dome.LEDS_PER_SATELLITE; ++led)
            {
                float right = ((float)led / (float)Dome.LEDS_PER_SATELLITE) + elapsedSeconds;
                float left = 1.0f - ((float)led / (float)Dome.LEDS_PER_SATELLITE) + elapsedSeconds;

                right -= (float)Math.Floor(right);
                left -= (float)Math.Floor(left);

                // Blue moving to the right with red pulses to the left:
                Color color0 = new Color
                    ((float)Math.Round((float)Math.Sin(8 * Math.PI * left)) * (float)Math.Sin(Math.PI * elapsedSeconds)
                    , (float)Math.Round((float)Math.Sin(6 * Math.PI * right)) * (float)Math.Sin(Math.PI * elapsedSeconds)
                    ,  1 //(float)Math.Sin((3/2) * Math.PI * elapsedSeconds)
                    );
                // Pink pulses moving left:
                Color color1 = new Color
                    (   (float)Math.Round((float)Math.Sin(8 * Math.PI * left))
                    ,   (float)Math.Round((float)Math.Sin(6 * Math.PI * right))// * (float)Math.Sin(8 * Math.PI * elapsedSeconds)
                    ,   (float)Math.Sin(Math.PI * elapsedSeconds)
                    );
                
                // Color color2 = new Color((float)Math.Sin(2 * Math.PI * dx0), 0.0f, (float)dx1);
                GetCurrentFrame().SetSatelliteLEDColor(0, led, color0);
                GetCurrentFrame().SetSatelliteLEDColor(1, led, color1);
            }
             */
        }

        
        virtual public void GenerateNewFrame(float dt)
        {
            //base class is a blank frame
        }

        virtual public AnimationFrame GetCurrentFrame()
        {
            return m_CurrentFrame;
        }

        protected void Reduce(float percentage)
        {
            percentage *= .7f; //TODO: This is a scalar to adjust for the hardware. Should be on a tweaker!

            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color curColor = m_CurrentFrame.GetDomeLEDColor(rib, row);
                    m_CurrentFrame.SetDomeLEDColor(rib, row, curColor * percentage);
                }
            }
        }
    }
}
