using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace YoungBatman
{
    class Enemy
    {
        AnimatedSprite asSprite;
        Vector2 v2motion = new Vector2(0f, 0f);
        Vector2 v2BatManPosition = new Vector2(610, 285);
        float fSpeed = 1f;
        int iX = 0;
        int iY = 0;
        int iApproachSide = 0;
        bool bActive = false;
        static Random rndGen = new Random();

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
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(iX +40, iY + 15, 75, 145);
            }
        }

        public float Speed
        {
            get { return fSpeed; }
        }

        public Vector2 Motion
        {
            get { return v2motion; }
            set { v2motion = value; }
        }

        public void Deactivate()
        {
            bActive = false;
        }

        public void Generate()
        {
            iApproachSide = rndGen.Next(0, 3);
            if (iApproachSide == 0)
            {
                iX = rndGen.Next(-400,1680);
                iY = -400;
            }
            if (iApproachSide == 1)
            {
                iX = 1680;
                iY = rndGen.Next(-400,1120);
            }
            if (iApproachSide == 2)
            {
                iX = rndGen.Next(-400, 1680);
                iY = 1120;
            }
            if (iApproachSide == 3)
            {
                iX = -400;
                iY = rndGen.Next(-400, 1120);
            }
            v2motion.X = v2BatManPosition.X - iX;
            v2motion.Y = v2BatManPosition.Y - iY;
            fSpeed = (float)(rndGen.Next(3, 6));
            bActive = true;
        }

        public Enemy(Texture2D texture, int X, int Y, int Frame)
        {
            asSprite = new AnimatedSprite(texture, 0, 0, 175, 178, 20);
        }



        public void Draw(SpriteBatch sb, int iLocation)
        {
            if (bActive)
                asSprite.Draw(sb, iX, iY, false);
        }

        public void Update(GameTime gametime)
        {
            iX += (int)((float)v2motion.X * fSpeed);
            iY += (int)((float)v2motion.Y * fSpeed);

            asSprite.Update(gametime);
        }
    }
}
