using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace YoungBatman
{
    class Pow
    {
        AnimatedSprite asPow;
        Vector2 v2Position;
        bool bActive;
        int iFrame;
        float fElapsed = 0f;
        static Random rndGen = new Random();

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public int Frame
        {
            get { return iFrame; }
            set { iFrame = value; }
        }

        public Vector2 Position
        {
            get { return v2Position; }
            set { v2Position = value; }
        }

        public void Generate(Vector2 HitCenter)
        {
            v2Position.X = HitCenter.X - 30;
            v2Position.Y = HitCenter.Y - 17;
            fElapsed = 0f;
            bActive = true;
            iFrame = rndGen.Next(1, 4);
            asPow.Frame = iFrame;

        }

        public Pow(Texture2D texture)
        {
            asPow = new AnimatedSprite(texture, 0, 0, 75, 75, 4);
            asPow.IsAnimating = false;
        }

        public void Update(GameTime gameTime)
        {
            if (bActive)
            {
                fElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (fElapsed >= .5f)
                {
                    bActive = false;
                }

            }

        }

        public void Draw(SpriteBatch sb)
        {
            if (bActive)
            {
                asPow.Draw(sb, (int)v2Position.X, (int)v2Position.Y,false);

            }

        }


    }
}
