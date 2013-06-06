using System;
using System.Collections.Generic;


namespace JellyFish12000.Animations
{
	class AccidentalSpiral : Animation
	{
		AccidentalSpiral()
		{
			m_Name = "Accidental Spiral";
		}

		//private int m_CurrentRowIndex = 0;

		public override void  GenerateNewFrame(float dt)
		{
			//for (int rib = 0; rib < JellyApp.g_iTotalRibs; rib++)
			//{
			//    Rib ribObject = keyframe.GetRib(rib);

			//    Rib.RibDirection direction = Rib.RibDirection.DOWN;

			//    ribObject.TrailEffect(m_CurrentRowIndex, 10, direction);
			//    m_CurrentRowIndex++;
			//    // make sure this wraps
			//    m_CurrentRowIndex %= JellyApp.g_iLightsPerRib;
			//}

		}
	}
}
