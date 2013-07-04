using System;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
	class Growie : Animation
	{
		private float m_Delta = 0.0f;
        Color m_Color = ColorManager.RandomColor();

		public Growie()
		{
			UpdatePeriod = 0.025f;
			m_Name = "Growie";
		}

        public override void Start()
        {
            base.Start();
            m_Color = ColorManager.RandomColor();
        }

		public override void GenerateNewFrame(float dt)
		{
			AnimationFrame output = new AnimationFrame(); // get a clear frame
			
			for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
			{
				for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
				{
					float ribvalue = (float)rib + 1.0f;
					float rowvalue = (float)row + 1.0f;
					float brightness = (float)(Math.Sin(m_Delta * ribvalue * rowvalue));
					brightness += 1.0f;
					brightness /= 2.0f;

					Color newColor = m_Color * brightness;
					output.SetDomeLEDColor(rib, row, newColor);
				}
			}

			m_Delta += 2.5f * dt;
			m_CurrentFrame = output;
		}


	}
}
