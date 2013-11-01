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
        Vector2 v2BatManCenter = new Vector2(633, 351);
        float fSpeed = 1f;
        int iFrame;
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
            set { bActive = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(iX +40, iY + 15, 75, 145); //Rough guess on collision box
            }
        }

        public float Speed
        {
            get { return fSpeed; }
        }
        public int Frame
        {
            get { return iFrame; }
            set { iFrame = value; }
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
            iApproachSide = rndGen.Next(1, 4); // Determin what side enemy spawns, Then place him random point on that side
            iFrame = rndGen.Next(20);
            if (iApproachSide == 1)
            {
                iX = rndGen.Next(-200,1480);
                iY = -400;
            }
            if (iApproachSide == 2)
            {
                iX = 1680;
                iY = rndGen.Next(-400,1120);
            }
            if (iApproachSide == 3)
            {
                iX = rndGen.Next(-400, 1680);
                iY = 1120;
            }
            if (iApproachSide == 4)
            {
                iX = -400;
                iY = rndGen.Next(-400, 1120);
            }
            v2motion.X = (v2BatManCenter.X - (iX + 89))/100;  //calculate direction based on batman position
            v2motion.Y = (v2BatManCenter.Y - (iY + 80))/100;
            fSpeed = ((float)(rndGen.Next(50, 75)) /100); // random speed
            bActive = true;
        }

        public Enemy(Texture2D texture, int X, int Y, int Frame)
        {
            asSprite = new AnimatedSprite(texture, 0, 0, 160, 178, 20);
            asSprite.IsAnimating = false;
            iFrame = Frame;
            asSprite.Frame = iFrame;  //pass the frame from constructor to the animated sprite
        }

         
        public void Update(GameTime gametime)
        {
            iX += (int)((float)v2motion.X * fSpeed);
            iY += (int)((float)v2motion.Y * fSpeed);
            asSprite.Frame = iFrame;
            //asSprite.Update(gametime); //Since it is not animating, I don't need this.
        }

        public void Draw(SpriteBatch sb)
        {
                asSprite.Draw(sb, iX, iY, false);
        }
    }
}
