using System;
using Microsoft.Xna.Framework;


namespace JellyFish12000.Blenders
{
    using Color = Microsoft.Xna.Framework.Color;

    class Linear : Blender
    {
        public Linear()
        {
            m_Duration = 4.0f;
        }

        public override AnimationFrame Calculate(float dt, AnimationFrame cur, AnimationFrame next)
        {
            AnimationFrame result = new AnimationFrame();
            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color b = cur.GetLedColor(rib, row);
                    Color a = next.GetLedColor(rib, row);

                    Color newColor = new Color();
                    newColor.A = (byte)(a.A * m_CurBlendValue + b.A * m_Reciprocal);
                    newColor.R = (byte)(a.R * m_CurBlendValue + b.R * m_Reciprocal);
                    newColor.G = (byte)(a.G * m_CurBlendValue + b.G * m_Reciprocal);
                    newColor.B = (byte)(a.B * m_CurBlendValue + b.B * m_Reciprocal);

                    result.SetLedColor(rib, row, newColor);
                }
            }

            for
            (int pendant = 0
            ; pendant < Dome.NUM_PENDANTS_MAX
            ; ++pendant
            )
            {
                for (int led = 0; led < Dome.LEDS_PER_PENDANT_MAX; ++led)
                {
                    Color b = cur.GetPendantLEDColor(pendant, led);
                    Color a = next.GetPendantLEDColor(pendant, led);

                    Color newColor = new Color();
                    newColor.A = (byte)(a.A * m_CurBlendValue + b.A * m_Reciprocal);
                    newColor.R = (byte)(a.R * m_CurBlendValue + b.R * m_Reciprocal);
                    newColor.G = (byte)(a.G * m_CurBlendValue + b.G * m_Reciprocal);
                    newColor.B = (byte)(a.B * m_CurBlendValue + b.B * m_Reciprocal);

                    result.SetPendantLEDColor(pendant, led, newColor);
                }
            }


            base.Calculate(dt, cur, next);
            return result;
        }
    }
}
