﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace YoungBatman
{
    class AnimatedSprite
    {

        Texture2D t2dTexture;

        float fFrameRate = 0.02f;
        float fElapsed = 0.0f;
        float fElapsedRotationTime = 0f;
        float fRotationAngle = 0f;

        private Vector2 v2Origin;

        int iFrameOffsetX = 0;
        int iFrameOffsetY = 0;
        int iFrameWidth = 32;
        int iFrameHeight = 32;

        int iFrameCount = 1;
        int iCurrentFrame = 0;
        int iScreenX = 0;
        int iScreenY = 0;
        int iSpinSpeed = 0;

        bool bAnimating = true;
        bool bRotating = false;


        public int X
        {
            get { return iScreenX; }
            set { iScreenX = value; }
        }

        public int Y
        {
            get { return iScreenY; }
            set { iScreenY = value; }
        }

        public int Frame
        {
            get { return iCurrentFrame; }
            set { iCurrentFrame = (int)MathHelper.Clamp(value, 0, iFrameCount); }
        }

        public int SpinSpeed
        {
            get { return iSpinSpeed; }
            set { iSpinSpeed = value; }
        }

        public float FrameLength
        {
            get { return fFrameRate; }
            set { fFrameRate = (float)Math.Max(value, 0f); }
        }

        public bool IsAnimating
        {
            get { return bAnimating; }
            set { bAnimating = value; }
        }

        public bool IsRotating
        {
            get { return bRotating; }
            set { bRotating = value; }
        }


        public AnimatedSprite(Texture2D texture, int FrameOffsetX, int FrameOffsetY, int FrameWidth, int FrameHeight, int FrameCount)
        {
            t2dTexture = texture;
            iFrameOffsetX = FrameOffsetX;
            iFrameOffsetY = FrameOffsetY;
            iFrameWidth = FrameWidth;
            iFrameHeight = FrameHeight;
            iFrameCount = FrameCount;
            v2Origin = new Vector2((texture.Width /2), (texture.Height /2));
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(
            iFrameOffsetX + (iFrameWidth * iCurrentFrame),
            iFrameOffsetY,
            iFrameWidth,
            iFrameHeight);
        }

        public void Update(GameTime gametime)
        {
            if (bAnimating)
            {
                // Accumulate elapsed time...
                fElapsed += (float)gametime.ElapsedGameTime.TotalSeconds;

                // Until it passes our frame length
                if (fElapsed > fFrameRate)
                {
                    // Increment the current frame, wrapping back to 0 at iFrameCount
                    iCurrentFrame = (iCurrentFrame + 1) % iFrameCount;

                    // Reset the elapsed frame time.
                    fElapsed = 0.0f;
                }
            }

            if (bRotating)
            {
                fElapsedRotationTime = (float)gametime.ElapsedGameTime.TotalSeconds;
                fRotationAngle += (fElapsedRotationTime * 20);
                float fcircle = MathHelper.Pi * 2;
                fRotationAngle = fRotationAngle % fcircle;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset, bool NeedBeginEnd)
        {
            if (NeedBeginEnd)
                spriteBatch.Begin();

            spriteBatch.Draw(t2dTexture, new Rectangle(iScreenX + XOffset, iScreenY + YOffset, iFrameWidth, iFrameHeight), GetSourceRect(),Color.White);

            if (NeedBeginEnd)
                spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch, int XOffset, int YOffset)
        {
            if(!bRotating)
                Draw(spriteBatch, XOffset, YOffset, true);

            if (bRotating)
            {
                spriteBatch.Draw(t2dTexture, new Vector2(XOffset,YOffset), null, Color.White, fRotationAngle, v2Origin, 1.0f, SpriteEffects.None, 0f);

            }
        }





    }
}
