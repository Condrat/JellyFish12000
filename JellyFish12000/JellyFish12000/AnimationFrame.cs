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
        Color[,] m_Satellites = null;
        // Actual number of pendants

        public AnimationFrame()
        {
            m_Lights = new Color[Dome.NUM_RIBS, Dome.LEDS_PER_RIB];
            m_Lights.Initialize();
            m_Pendants = new Color[Dome.NUM_PENDANTS_MAX, Dome.LEDS_PER_PENDANT_MAX];
            m_Pendants.Initialize();
            m_Satellites = new Color[Dome.NUM_SATELLITES, Dome.LEDS_PER_SATELLITE];
            m_Satellites.Initialize();
        }

        public void SetDomeLEDColor(int index, int led, Color newColor)
        {
            Debug.Assert(index >= 0 && index < Dome.NUM_RIBS);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_RIB);

            //Color oldColor = m_Lights[index, led];
            m_Lights[index, led] = newColor;

            //return oldColor;
        }

        public Color GetDomeLEDColor(int rib, int led)
        {
            Debug.Assert(rib >= 0 && rib < Dome.NUM_RIBS);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_RIB);

            return m_Lights[rib, led];
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
            // This utilizes Dome.NumPendants in case the number of pendants 
            // is set to be less than the max.
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

        public Color GetPendantLEDColor(int index, int led)
        {
            Debug.Assert(index >= 0 && index < Dome.NUM_PENDANTS_MAX);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_PENDANT_MAX);

            return m_Pendants[index, led];
        }

        public void SetPendantLEDColor(int index, int ledIndex, Color newColor)
        {
            Debug.Assert((index >= 0) && (index < Dome.NUM_PENDANTS_MAX));
            Debug.Assert((ledIndex >= 0) && (ledIndex < Dome.LEDS_PER_PENDANT_MAX));
            m_Pendants[index, ledIndex] = newColor;
        }

        public void SetPendantColor(int index, Color newColor)
        {
            for (int led = 0; led < this.NumPendantLEDs; led++)
            {
                SetPendantLEDColor(index, led, newColor);
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

        // Satellite code:

        public Color[,] SatelliteData
        {
            get { return m_Satellites; }
        }

        public Color GetSatelliteLEDColor(int index, int led)
        {
            Debug.Assert(index >= 0 && index < Dome.NUM_SATELLITES);
            Debug.Assert(led >= 0 && led < Dome.LEDS_PER_SATELLITE);

            return m_Satellites[index, led];
        }

        public void SetSatelliteLEDColor(int index, int ledIndex, Color newColor)
        {
            Debug.Assert((index >= 0) && (index < Dome.NUM_SATELLITES));
            Debug.Assert((ledIndex >= 0) && (ledIndex < Dome.LEDS_PER_SATELLITE));
            m_Satellites[index, ledIndex] = newColor;
        }


    }
}
