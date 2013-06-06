using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Runtime.InteropServices;
using System.IO;
using System.Collections.Generic;


namespace JellyFish12000
{
    using BitmapData = System.Drawing.Imaging.BitmapData;
    using ImageLockMode = System.Drawing.Imaging.ImageLockMode;

    class ColorManager
    {
        static List<Color[]> m_ColorRamps = null;        
        static int m_CurrentRamp = 0;
        static Random m_Random = new Random();

        public static void Init()
        {
            m_ColorRamps = new List<Color[]>();

            if (Directory.Exists("ColorRamps"))
            {
                String[] files = Directory.GetFiles("ColorRamps");
                
                foreach (String file in files)
                {
                    Color[] ramp = InitColorRampFromImage(file);
                    if (null != ramp)
                        m_ColorRamps.Add(ramp);
                }
            }
        }

        private static Color[] InitColorRampFromImage(String imageFile)
        {
            Color[] colorRamp = null;
     
            try
            {
                System.Drawing.Bitmap bm = new System.Drawing.Bitmap(imageFile);
                colorRamp = new Color[bm.Width];

                System.Drawing.Rectangle r = new System.Drawing.Rectangle(0, 0, bm.Width, bm.Height);

                BitmapData bitmapData = bm.LockBits(r, ImageLockMode.ReadOnly, bm.PixelFormat);
                byte[] pixelData = new byte[bitmapData.Stride];

                IntPtr pData = bitmapData.Scan0;
                Marshal.Copy(pData, pixelData, 0, bitmapData.Stride);

                for (int i = 0, rampPos = 0; i < bitmapData.Stride; i += 4, ++rampPos)
                {
                    byte alpha = pixelData[i + 3];
                    byte red = pixelData[i + 2];
                    byte green = pixelData[i + 1];
                    byte blue = pixelData[i + 0];

                    colorRamp[rampPos] = Color.FromNonPremultiplied(red, green, blue, alpha);
                }

                bm.UnlockBits(bitmapData);
            }
            catch (Exception) {}

            return colorRamp;
        }

        public static void NextRamp()
        {
            m_CurrentRamp++;
            if (m_CurrentRamp >= m_ColorRamps.Count)
                m_CurrentRamp = 0;
        }

        public static Color RandomColor()
        {
            double v = m_Random.NextDouble();
            return GetColor(v);                     
        }
        
        public static Color GetColor(double v)
        {
            return m_ColorRamps.Count == 0 ? GetColorProcedural(v) : GetColorFromCurrentRamp(v);
        }

        private static Color GetColorFromCurrentRamp(double v)
        {
            Debug.Assert(v <= 1.0 && v >= 0.0);

            Color [] ramp = m_ColorRamps[m_CurrentRamp];                       
            int pos = (int)((ramp.Length - 1) * v);            
            return ramp[pos];
        }

        public static Color GetColorProcedural(double v)
        {
            Debug.Assert(v <= 1.0 && v >= 0.0);
                        
            double period = 2 * Math.PI;

            double redOffset = 0.5 * Math.PI;
            double greenOffset = redOffset + (2.0 * Math.PI / 3);
            double blueOffset = greenOffset + (2.0 * Math.PI / 3);

            double c = period * v;

            double red = (Math.Sin(c + redOffset) + 1.0f) / 2.0;
            double green = (Math.Sin(c + greenOffset) + 1.0f) / 2.0;
            double blue = (Math.Sin(c + blueOffset) + 1.0f) / 2.0;

            return Color.FromNonPremultiplied(new Vector4((float)red, (float)green, (float)blue, 1.0f));
        }
    }
}
