using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace JellyFish12000
{
	class DomeViewer : CoreWindow
	{
		protected override void Render(Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
		{
			Dome.Render(m_View, m_Proj);
		}
	}
}
