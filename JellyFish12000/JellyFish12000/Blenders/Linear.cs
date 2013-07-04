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

        protected delegate Color GetLEDColor(int index, int led);
        protected delegate void SetLightColor(int index, int led, Color newColor);
        protected void BlendColors(GetLEDColor getA, GetLEDColor getB, SetLightColor set, int numObjects, int numLEDs)
        {
            for (int obj = 0; obj < numObjects; ++obj)
            {
                for (int led = 0; led < numLEDs; ++led)
                {
                    Color b = getA(obj, led);
                    Color a = getB(obj, led);
                    Color newColor = new Color();
                    newColor.A = (byte)(a.A * m_CurBlendValue + b.A * m_Reciprocal);
                    newColor.R = (byte)(a.R * m_CurBlendValue + b.R * m_Reciprocal);
                    newColor.G = (byte)(a.G * m_CurBlendValue + b.G * m_Reciprocal);
                    newColor.B = (byte)(a.B * m_CurBlendValue + b.B * m_Reciprocal);
                    set(obj, led, newColor);
                }
            }
        }

        public override AnimationFrame Calculate(float dt, AnimationFrame cur, AnimationFrame next)
        {
            AnimationFrame result = new AnimationFrame();

            BlendColors(cur.GetDomeLEDColor, next.GetDomeLEDColor, result.SetDomeLEDColor, Dome.NUM_RIBS, Dome.LEDS_PER_RIB);
            BlendColors(cur.GetPendantLEDColor, next.GetPendantLEDColor, result.SetPendantLEDColor, Dome.NUM_PENDANTS_MAX, Dome.LEDS_PER_PENDANT_MAX);
            BlendColors(cur.GetSatelliteLEDColor, next.GetSatelliteLEDColor, result.SetSatelliteLEDColor, Dome.NUM_SATELLITES, Dome.LEDS_PER_SATELLITE);

            /*
            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color b = cur.GetDomeLEDColor(rib, row);
                    Color a = next.GetDomeLEDColor(rib, row);

                    Color newColor = new Color();
                    newColor.A = (byte)(a.A * m_CurBlendValue + b.A * m_Reciprocal);
                    newColor.R = (byte)(a.R * m_CurBlendValue + b.R * m_Reciprocal);
                    newColor.G = (byte)(a.G * m_CurBlendValue + b.G * m_Reciprocal);
                    newColor.B = (byte)(a.B * m_CurBlendValue + b.B * m_Reciprocal);

                    result.SetDomeLEDColor(rib, row, newColor);
                }
            }

            for (int pendant = 0; pendant < Dome.NUM_PENDANTS_MAX; ++pendant)
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
            */

            base.Calculate(dt, cur, next);
            return result;
        }
    }
}
