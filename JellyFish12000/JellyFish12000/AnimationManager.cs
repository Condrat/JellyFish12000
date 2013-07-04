using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using JellyFish12000.Animations;
using JellyFish12000.Blenders;

namespace JellyFish12000
{
    class AnimationManager
    {
        private static List<Animation> m_AnimationList = new List<Animation>();
        //private static List<Blender> m_BlenderList = new List<Blender>();

        private static int m_CurrentAnimationIndex = 0;
        private static bool m_GetRandomAnimation = false;
        private static Animation m_CurrentAnimation = null;
        private static Animation m_NextAnimation = null;
        private static Blender m_Blender = null;
        private static State m_State = State.Animating;
        private static Random m_Random = null;

        enum State
        {
            // might need an init state for more _advanced_ animation types
            Animating = 0,
            Blending = 1,
        };


        static AnimationManager()
        {
            m_Random = new Random(Environment.TickCount);
            m_Blender = new Linear();

            //InitBlenderList();			
            InitAnimationList();

            // assume there is at least two animations
            m_CurrentAnimation = m_AnimationList[0];
            m_NextAnimation = m_AnimationList[1];
            MainForm.ConsoleWriteLine("AM: Loading '" + m_CurrentAnimation.Name + "'");

        }

        private static void InitAnimationList()
        {
            m_AnimationList.Add(new RibWalk());
            m_AnimationList.Add(new BidirectionalRain());
            m_AnimationList.Add(new PolarRose());
            m_AnimationList.Add(new Growie());
            m_AnimationList.Add(new SpinningRainbow());
            m_AnimationList.Add(new Rain());
            m_AnimationList.Add(new BouncingRings());
            m_AnimationList.Add(new RowColorWheel());
            m_AnimationList.Add(new Strobe());
            m_AnimationList.Add(new StraightSine02());
            m_AnimationList.Add(new Test_AllRed());
            m_AnimationList.Add(new RibColorWheel());
            m_AnimationList.Add(new Spiral());
            m_AnimationList.Add(new RandomRow());
            m_AnimationList.Add(new SexWorms());
            m_AnimationList.Add(new RowWalk());
            m_AnimationList.Add(new ColorWheel());
            m_AnimationList.Add(new Phyllotaxy());
            m_AnimationList.Add(new Test_AllBlue());
            m_AnimationList.Add(new Lissajous());
            m_AnimationList.Add(new RandomRib());
            m_AnimationList.Add(new PoliceLight());
            m_AnimationList.Add(new Hypocycloid());
            m_AnimationList.Add(new Test_AllGreen());
            m_AnimationList.Add(new StraightSine01());

            //m_AnimationList.Add(new TestAnimation1());
        }

        public static void Update(float dt)
        {
            AnimationFrame currentFrame = null;
            AnimationFrame resultFrame = null;
            AnimationFrame nextFrame = null;

            switch (m_State)
            {
                case State.Animating:
                    // animating so we only need to worry about the current ani
                    m_CurrentAnimation.Update(dt);
                    resultFrame = m_CurrentAnimation.GetCurrentFrame();

                    if (m_CurrentAnimation.Finished)
                    {
                        // get the blender to use
                        m_Blender = GetNextBlender();
                        m_Blender.Start();

                        // get the next animation we can blend to
                        m_NextAnimation = GetNextAnimation();
                        m_NextAnimation.Start();

                        m_State = State.Blending;
                    }
                    break;

                case State.Blending:
                    // update both anis since we're blending between the two
                    m_CurrentAnimation.Update(dt);
                    m_NextAnimation.Update(dt);
                    currentFrame = m_CurrentAnimation.GetCurrentFrame();
                    nextFrame = m_NextAnimation.GetCurrentFrame();
                    resultFrame = m_Blender.Calculate(dt, currentFrame, nextFrame);

                    if (m_Blender.Finished)
                    {
                        // done blending the current to next. Next now is current.
                        m_State = State.Animating;
                        m_CurrentAnimation.Stop();
                        m_CurrentAnimation = m_NextAnimation;
                    }
                    break;
            }

            // let the dome know about the new frame
            Dome.SetFrame(resultFrame);
        }

        private static Animation GetNextAnimation()
        {

            // once we've cycled through all the animations, randomize
            if (m_GetRandomAnimation)
            {
                m_CurrentAnimationIndex = m_Random.Next(m_AnimationList.Count);
            }
            else if (++m_CurrentAnimationIndex >= m_AnimationList.Count)
            {
                m_CurrentAnimationIndex = 0;
                m_GetRandomAnimation = true;
            }
            Animation currentAnimation = m_AnimationList[m_CurrentAnimationIndex];
            MainForm.ConsoleWriteLine("AM: Loading '" + currentAnimation.Name + "'");
            return currentAnimation;
        }

        private static Blender GetNextBlender()
        {
            //TODO: more blend types!
            return m_Blender;
        }

        //public static void SetCurrentAnimation(String name)
        //{
        //    int index = 0;
        //    foreach (Animation a in m_AnimationList)
        //    {
        //        if (a.Name == name)
        //        {
        //            m_CurAnimIndex = index;
        //            break;
        //        }

        //        ++index;
        //    }
        //}
    }
}
