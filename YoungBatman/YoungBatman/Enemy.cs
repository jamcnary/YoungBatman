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
        Vector2 v2EnemyPosition;
        double dEnemyChargeAngle;
        float fElapsed = 0f;
        float fUpdateInterval = 0.015f; 
        float fSpeed = 0f;
        int iMaxSpeed = 4;
        int iMinSpeed = 2;
        int iFrame;
        int iApproachSide = 0;
        bool bActive = false;
        bool bDead = false;
        static Random rndGen = new Random();

        public bool IsActive
        {
            get { return bActive; }
            set { bActive = value; }
        }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)v2EnemyPosition.X + 40, (int)v2EnemyPosition.Y + 15, 75, 145); //Rough guess on collision box
            }
        }

        public float Speed
        {
            get { return fSpeed; }
        }

        public int MaxSpeed
        {
            get { return iMaxSpeed; }
            set { iMaxSpeed = value; }
        }

        public int MinSpeed
        {
            get { return iMinSpeed; }
            set { iMinSpeed = value; }
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

        public Vector2 EnemyPosition
        {
            get { return v2EnemyPosition; }
            set { v2EnemyPosition = value; }
        }

        public void Deactivate()
        {
            v2motion.X *= -15;
            v2motion.Y *= -15;
            bDead = true;
        }

        public void Generate()
        {
            bDead = false;
            iApproachSide = rndGen.Next(1, 5); // Determine what side enemy spawns, Then place him random point on that side
            iFrame = rndGen.Next(20);
            if (iApproachSide == 1)
            {
                v2EnemyPosition.X = rndGen.Next(-200,1480);
                v2EnemyPosition.Y = -400;
            }
            if (iApproachSide == 2)
            {
                v2EnemyPosition.X = 1680;
                v2EnemyPosition.Y = rndGen.Next(-400, 1120);
            }
            if (iApproachSide == 3)
            {
                v2EnemyPosition.X = rndGen.Next(-400, 1680);
                v2EnemyPosition.Y = 1120;
            }
            if (iApproachSide == 4 || iApproachSide == 5)
            {
                v2EnemyPosition.X = -400;
                v2EnemyPosition.Y = rndGen.Next(0, 720);
            }
            dEnemyChargeAngle = Math.Atan2((v2BatManCenter.Y) - (v2EnemyPosition.Y + 89), (v2BatManCenter.X) - (v2EnemyPosition.X + 80));
            v2motion = new Vector2((float)Math.Cos(dEnemyChargeAngle), (float)Math.Sin(dEnemyChargeAngle));
            v2motion.Normalize();
            
            fSpeed = ((float)(rndGen.Next(iMinSpeed, iMaxSpeed)) ); // random speed
            bActive = true;
        }

        public Enemy(Texture2D texture, int X, int Y, int Frame)
        {
            asSprite = new AnimatedSprite(texture, 0, 0, 160, 178, 20);
            asSprite.IsAnimating = false;
            iFrame = Frame;
            asSprite.Frame = iFrame;  //pass the frame from constructor to the animated sprite
        }

         
        public void Update(GameTime gameTime)
        {

            fElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fElapsed > fUpdateInterval)
            {
                fElapsed = 0f;
                v2EnemyPosition.X += (fSpeed * v2motion.X);
                v2EnemyPosition.Y += (fSpeed * v2motion.Y);
                asSprite.Frame = iFrame;
                //asSprite.Update(gametime); //Since it is not animating, I don't need this.
                if (bDead)
                {
                    if ((v2EnemyPosition.X > 1300) || (v2EnemyPosition.X < -20))
                    {
                        bActive = false;
                    }
                    if ((v2EnemyPosition.Y > 740) || (v2EnemyPosition.Y < -20))
                    {
                        bActive = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            asSprite.Draw(sb, (int)v2EnemyPosition.X, (int)v2EnemyPosition.Y, false);
        }
    }
}
