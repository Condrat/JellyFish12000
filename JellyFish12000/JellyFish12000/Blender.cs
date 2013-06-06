using System;

namespace JellyFish12000
{
	class Blender
	{
		protected float m_Duration = 2.0f;		
		protected float m_CurBlendValue = 0.0f;
		protected float m_Reciprocal = 1.0f;

		public bool Finished
		{
			get { return m_CurBlendValue >= 1.0f; }
		}

		virtual public void Start()
		{
			m_CurBlendValue = 0.0f;
			m_Reciprocal = 1.0f;
		}

		virtual public AnimationFrame Calculate(float dt, AnimationFrame cur, AnimationFrame next)
		{
			m_CurBlendValue += (dt / m_Duration);
			m_Reciprocal = 1 - m_CurBlendValue;

			return cur;
		}
	}
}
