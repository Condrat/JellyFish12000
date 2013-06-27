using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace JellyFish12000
{
    using Color = Microsoft.Xna.Framework.Color;

    class AnimationFrame
    {
        Color[,] m_Lights = null;
        Color[,] m_Pendants = null;
        // Actual number of pendants

        public AnimationFrame()
        {
            m_Lights = new Color[Dome.NUM_RIBS, Dome.LEDS_PER_RIB];
            m_Lights.Initialize();
            m_Pendants = new Color[Dome.NUM_PENDANTS_MAX, Dome.LEDS_PER_PENDANT_MAX];
            m_Pendants.Initialize();
        }

        public Color SetLedColor(int rib, int led, Color newColor)
        {
            Debug.Assert(rib >= 0 && rib < Dome.NUM_RIBS);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_RIB);

            Color oldColor = m_Lights[rib, led];
            m_Lights[rib, led] = newColor;

            return oldColor;
        }

        public Color GetLedColor(int rib, int led)
        {
            Debug.Assert(rib >= 0 && rib < Dome.NUM_RIBS);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_RIB);

            return m_Lights[rib, led];
        }

        public Color GetPendantLEDColor(int pendant, int led)
        {
            Debug.Assert(pendant >= 0 && pendant < Dome.NUM_PENDANTS_MAX);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_PENDANT_MAX);

            return m_Pendants[pendant, led];
        }

        public void SetRibColor(int rib, Color newColor)
        {
            Debug.Assert(rib >= 0 && rib < Dome.NUM_RIBS);

            for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                m_Lights[rib, row] = newColor;
        }

        public void SetRowColor(int row, Color newColor)
        {
            Debug.Assert(row >= 0 && row < Dome.LEDS_PER_RIB);

            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
                m_Lights[rib, row] = newColor;
        }

        public void SetFrameColor(Color newColor)
        {
            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                    m_Lights[rib, row] = newColor;
        }

        public void Invert()
        {
            for (int rib = 0; rib < Dome.NUM_RIBS; ++rib)
            {
                for (int row = 0; row < Dome.LEDS_PER_RIB; ++row)
                {
                    Color oldColor = m_Lights[rib, row];
                    m_Lights[rib, row] = new Color(255 - oldColor.R, 255 - oldColor.G, 255 - oldColor.B, 255 - oldColor.A);
                }
            }
        }

        // Convenience functions:
        public int NumPendants
        {
            get { return Dome.NumPendants; }
        }

        public int NumPendantLEDs
        {
            get { return Dome.NumPendantLEDs; }
        }

        public Color[,] PendantData
        {
            get { return m_Pendants; }
        }

        // Pendant code:
        public void SetPendantLEDColor(int pendantIndex, int ledIndex, Color newColor)
        {
            Debug.Assert((pendantIndex >= 0) && (pendantIndex < Dome.NUM_PENDANTS_MAX));
            Debug.Assert((ledIndex >= 0) && (ledIndex < Dome.LEDS_PER_PENDANT_MAX));
            m_Pendants[pendantIndex, ledIndex] = newColor;
        }

        public void SetPendantColor(int pendantIndex, Color newColor)
        {
            for (int led = 0; led < this.NumPendantLEDs; led++)
            {
                SetPendantLEDColor(pendantIndex, led, newColor);
            }
        }

        public void SetAllPendantsColor(Color newColor)
        {
            for (int pendant = this.NumPendants; --pendant >= 0; )
            {
                SetPendantColor(pendant, newColor);
            }
        }

        public void ClearAllPendants()
        {
            SetAllPendantsColor(Color.Black);
        }
    }
}
