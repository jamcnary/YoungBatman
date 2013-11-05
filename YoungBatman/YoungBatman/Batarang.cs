using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace YoungBatman
{
    class Batarang
    {
        static AnimatedSprite asBatarang;
        public float fSpeed = 7f;
        bool bActive;
        double dFireAngle;
        float fElapsed = 0f;
        float fUpdateInterval = 0.015f;
        Vector2 v2TargetPosition = new Vector2(0f, 0f);
        Vector2 v2BatManCenter = new Vector2(600, 315);
        Vector2 v2BatarangDirection = new Vector2(0f, 0f);
        Vector2 v2BatarangPosition = new Vector2(0f, 0f);

        

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public float Speed
        {
            get { return fSpeed; }
            set { fSpeed = value; }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)v2BatarangPosition.X, (int)v2BatarangPosition.Y, 16, 1); }
        }

        public Batarang(Texture2D texture)
        {
            asBatarang = new AnimatedSprite(texture, 0, 0, 60, 34, 1);
            bActive = false;
        }

        

        public void Fire(float X, float Y)
        {
            v2BatarangPosition.X = v2BatManCenter.X;
            v2BatarangPosition.Y = v2BatManCenter.Y;
            v2TargetPosition.X = X;
            v2TargetPosition.Y = Y;

            //v2BatarangDirection.X = (v2TargetPosition.X - v2BatManCenter.X)/100;
            //v2BatarangDirection.Y = (v2TargetPosition.Y - v2BatManCenter.Y)/100;
            dFireAngle = Math.Atan2(v2TargetPosition.Y - v2BatManCenter.Y, v2TargetPosition.X - v2BatManCenter.X);

            v2BatarangDirection = new Vector2((float)Math.Cos(dFireAngle), (float)Math.Sin(dFireAngle));
            v2BatarangDirection.Normalize();
            bActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (bActive)
            {
                fElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (fElapsed > fUpdateInterval)
                {
                    fElapsed = 0f;

                    v2BatarangPosition.X += (fSpeed * v2BatarangDirection.X);
                    v2BatarangPosition.Y += (fSpeed * v2BatarangDirection.Y);
                    // If the batarang has moved off of the screen, 
                    // set it to inactive
                    if ((v2BatarangPosition.X > 1300) || (v2BatarangPosition.X < -20))
                    {
                        bActive = false;
                    }
                    if ((v2BatarangPosition.Y > 740) || (v2BatarangPosition.Y < -20))
                    {
                        bActive = false;
                    }


                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (bActive)
            {
                
                    asBatarang.Draw(sb, (int)v2BatarangPosition.X, (int)v2BatarangPosition.Y, false);
                
            }
        }
        

    }
}
