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
        int iX;
        int iY;
        public int iSpeed = 12;
        bool bActive;
        float fElapsed = 0f;
        float fUpdateInterval = 0.015f;

        public int X
        {
            get { return iX; }
            set { iX = value; }
        }

        public int Y
        {
            get { return iY; }
            set { iY = value; }
        }

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public int Speed
        {
            get { return iSpeed; }
            set { iSpeed = value; }
        }

        public Rectangle BoundingBox
        {
            get { return new Rectangle(iX, iY, 16, 1); }
        }

        public Batarang(Texture2D texture)
        {
            asBatarang = new AnimatedSprite(texture, 0, 0, 60, 34, 1);
            iX = 0;
            iY = 0;
            bActive = false;
        }

        

        public void Fire(int X, int Y)
        {
            iX = X;
            iY = Y;
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
                    
                    // If the bullet has moved off of the screen, 
                    // set it to inactive
                    if ((iX > 1300) || (iX < -20))
                    {
                        bActive = false;
                    }
                    if ((iY > 740) || (iY < -20))
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
                
                    asBatarang.Draw(sb, iX, iY, false);
                
            }
        }
        

    }
}
