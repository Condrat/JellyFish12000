using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JellyFish12000.Animations
{
    class SexWorms : Animation
    {
        class DomePosition
        {
            public int m_ribPos = 0;
            public int m_rowPos = 0;
        }

        class SexWorm
        {
            // current length of the worm
            public int m_currentLength = 0;
            // a max length to which the worm can grow
            public int m_maxLength = 0;
            // number of frames until this sucker can mate again
            public int m_refractoryFrames = 0;

            public Color m_Color = Color.Red;

            // a list of positions
            public List<DomePosition> m_positionList = new List<DomePosition>();

            Random m_random = null;

            public SexWorm(int maxLength, int randomSeed, int refractoryFrames)
            {
                m_random = new Random(randomSeed);
                m_Color = ColorManager.RandomColor();

                // start out with a random position
                DomePosition firstPos = new DomePosition();

                firstPos.m_ribPos = m_random.Next(0, Dome.NUM_RIBS);
                firstPos.m_rowPos = m_random.Next(0, Dome.LEDS_PER_RIB);

                // set the max length
                m_maxLength = maxLength;

                // add in the first position
                m_positionList.Add(firstPos);

                m_refractoryFrames = refractoryFrames;
            }

            public SexWorm(int ribPos, int rowPos, int maxLength, int randomSeed, int refractoryFrames)
            {
                m_random = new Random(randomSeed);
                m_Color = ColorManager.RandomColor();

                // start out with a random position
                DomePosition firstPos = new DomePosition();

                firstPos.m_ribPos = ribPos;
                firstPos.m_rowPos = rowPos;

                // set the max length
                m_maxLength = maxLength;

                // add in the first position
                m_positionList.Add(firstPos);

                m_refractoryFrames = refractoryFrames;
            }

            public void UpdateFrame()
            {
                if (m_positionList.Count >= m_maxLength)
                {
                    // remove from the 'end' which is the beginning of this list
                    m_positionList.RemoveAt(0);
                }

                // let's move the worm in some random position
                DomePosition currentHeadPos = (DomePosition)m_positionList[m_positionList.Count - 1];

                DomePosition newHeadPos = new DomePosition();
                newHeadPos.m_ribPos = currentHeadPos.m_ribPos;
                newHeadPos.m_rowPos = currentHeadPos.m_rowPos;

                // this random looks a bit weird, but C# returns a value greater than or equal to the min
                // but LESS THAN the max
                int ribDelta = m_random.Next(-1, 2);                
                int rowDelta = m_random.Next(-1, 3);

                newHeadPos.m_ribPos += ribDelta;
                newHeadPos.m_rowPos += rowDelta;

                // handle wrapping around from one end of the dome to another
                newHeadPos.m_ribPos += Dome.NUM_RIBS;
                newHeadPos.m_rowPos += Dome.LEDS_PER_RIB;

                // now make sure that the new position is in bounds
                newHeadPos.m_ribPos %= Dome.NUM_RIBS;
                newHeadPos.m_rowPos %= Dome.LEDS_PER_RIB;

                // add this new position to the list
                m_positionList.Add(newHeadPos);
            }
        }
                
        int m_startingWorms = 5;
        int m_maxWorms = 20;
        int m_maxWormLength = 10;
        int m_globalRefractoryFrames = 1000;
        List<SexWorm> m_wormList = new List<SexWorm>();
        Random m_globalRandom = new Random(Environment.TickCount);

        public SexWorms()
        {
            m_Name = "SexWorms";
            UpdatePeriod = 0.02f;
            InitStartingWorms();
        }

        public override void Start()
        {
            base.Start();
            InitStartingWorms();
        }

        private void InitStartingWorms()
        {
            // create a starting number of worms randomized over the dome
            for (int i = 0; i < m_startingWorms; i++)
            {
                SexWorm newWorm = new SexWorm(m_maxWormLength, m_globalRandom.Next(), m_globalRefractoryFrames);
                m_wormList.Add(newWorm);
            }
        }
        public override SatelliteParameters GenerateSatelliteParameters()
        {
            return new SatelliteParameters(PATTERN_GLITTER, 0, 0, 0, 0, 0, 0);
        }

        public override void  GenerateNewFrame(float dt)
        {
            Reduce(.9f);            
            List<SexWorm> newBabies = new List<SexWorm>();

            // go through all the worms, update them and then render them
            foreach (SexWorm worm in m_wormList)
            {
                worm.UpdateFrame();

                // and now that we have updated the worm, let's render it on the dome
                int positionIndex = 0;
                foreach (DomePosition pos in worm.m_positionList)
                {
                    if (worm.m_refractoryFrames > 0)
                    {
                        // getting closer to reproducing again!
                        worm.m_refractoryFrames--;
                    }

                    // let's see if this is the head
                    if (m_CurrentFrame.GetDomeLEDColor(pos.m_ribPos, pos.m_rowPos) != Color.Black &&
                        worm.m_refractoryFrames <= 0 &&                        
                        positionIndex == 0)
                    {
                        // we intersected another worm!
                        // so let's totally make a baby!
                        if (m_wormList.Count + newBabies.Count < m_maxWorms)
                        {
                            SexWorm newWorm = new SexWorm(pos.m_ribPos, pos.m_rowPos, m_maxWormLength, m_globalRandom.Next(), m_globalRefractoryFrames);
                            newBabies.Add(newWorm);
                            worm.m_refractoryFrames = m_globalRefractoryFrames;
                        }
                    }

                    // right now we draw all of the worm at 100%
                    m_CurrentFrame.SetDomeLEDColor(pos.m_ribPos, pos.m_rowPos, worm.m_Color);

                    positionIndex++;
                }
            }

            foreach (SexWorm babyWorm in newBabies)
            {
                m_wormList.Add(babyWorm);
            }

            if (m_wormList.Count > m_maxWorms)
            {
                m_wormList.Clear();
                InitStartingWorms();
            }

            //m_CurrentFrame = output;
        }
    }
}
