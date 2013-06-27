using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000
{
    class Animation
    {
        protected AnimationFrame m_CurrentFrame = new AnimationFrame();

		protected float m_Duration = 35.0f; //5 seconds is a good length for testing
		protected float m_RunTime = 0.0f;

		protected float m_UpdatePeriod = 1.0f;
        protected float m_DefaultUpdatePeriod = 1.0f;
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
                GenerateNewFrame(dt);
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
                    Color curColor = m_CurrentFrame.GetLedColor(rib, row);
                    m_CurrentFrame.SetLedColor(rib, row, curColor * percentage);
                }
            }
        }
    }
}
