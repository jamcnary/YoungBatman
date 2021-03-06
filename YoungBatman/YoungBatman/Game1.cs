using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace YoungBatman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //KeyboardState keyState;
        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        Enemy[] Enemies = new Enemy[iTotalMaxEnemies];
        Pow[] Bams = new Pow[iTotalBams];
        SpriteFont spriteFont;
        int iScore = 0;
        int iHighScore = 0;
        int iGameStarted = 0;
        int iStartButtonState = 0;
        int iMaxEnemies = 20;
        int iActiveEnemies = 0;
        int iEnemiesPerWave = 1;
        int iEnemyWaveCount = 0;
        static int iMaxBatarangs = 40;
        static int iTotalMaxEnemies = 100;
        static int iTotalBams = 8;

        float fTimeToNewWave = 1f;
        float fWaveTimeElapsed = 0f;

        Vector2 v2MousePosition;
        Vector2 v2BatManPosition;
        Vector2 v2BatManCenter;
        Vector2 v2BatarangOrigin;
        Vector2 v2Target;
        Vector2 v2StartButtonPosition;
        Vector2 v2BatarangDestination;
        Vector2 vScoreTextLoc = new Vector2(1075, 10);
        Vector2 vMenuScoreTextLoc = new Vector2(265, 525);
        Vector2 vMenuHighScoreTextLoc = new Vector2(265, 605);

        Texture2D t2dHud;
        Texture2D t2dBackground;
        Texture2D t2dCrosshair;
        Texture2D t2dBatman;
        Texture2D t2dTitleScreen;
        Texture2D t2dStartButton;
        Texture2D t2dStartButtonHover;
        Texture2D t2dStartButtonPressed;
        Texture2D t2dVillan;
        Texture2D t2dPow;

        Rectangle rStartButtonBox;
        Rectangle rMouseBox;
        Rectangle rBatmanBoundingBox;
        SoundEffect seWiff;
        SoundEffect seThrow;
        SoundEffect seHit;
        SoundEffect seLose;

        Batarang[] batarangs = new Batarang[iMaxBatarangs];


        //static Random rRand = new Random();

        #region Helper Functions


        protected void StartNewGame()
        {
            for (int i = 0; i < iTotalMaxEnemies; i++)
            {
                Enemies[i].IsActive = false;
                Enemies[i].EnemyPosition = Vector2.Zero;

            }
            iGameStarted = 1;
            iScore = 0;
            iEnemyWaveCount = 0;
            fTimeToNewWave = 5f;
            iActiveEnemies = 0;
            iEnemiesPerWave = 1;
        }

        protected void GenerateEnemies()
        {
            for (int x = 0; x < iMaxEnemies; x++)
            {
                if (!Enemies[x].IsActive)
                {
                    Enemies[x].Generate();
                    iActiveEnemies++;
                    
                }
                if (iActiveEnemies > iEnemiesPerWave)
                {
                    break;
                }
            }


        }
        protected void GeneratePow(Vector2 Position)
        {
            for (int x = 0; x < iTotalBams; x++)
            {
                if (!Bams[x].IsActive)
                {
                    Bams[x].Generate(Position);
                    break;
                }

            }
            
        }

        protected void UpdateBatarangs(GameTime gameTime)
        {
            // Updates the location of all of thell active player bullets. 
            for (int x = 0; x < iMaxBatarangs; x++)
            {
                if (batarangs[x].IsActive)
                    batarangs[x].Update(gameTime);
            }
        }

        protected void FireBullet(Vector2 BatarangDestination)
        {
            // Find and fire a free bullet
            for (int x = 0; x < iMaxBatarangs; x++)
            {
                if (!batarangs[x].IsActive)
                {
                    batarangs[x].Fire(BatarangDestination.X, BatarangDestination.Y);
                    seThrow.Play();
                    break;

                }
            }
        }

        protected bool Intersects(Rectangle rectA, Rectangle rectB)
        {
            // Returns True if rectA and rectB contain any overlapping points
            return (rectA.Right > rectB.Left && rectA.Left < rectB.Right &&
                    rectA.Bottom > rectB.Top && rectA.Top < rectB.Bottom);
        }

        protected void DestroyEnemy(int iEnemy)
        {
            Enemies[iEnemy].Deactivate();
            iActiveEnemies--;
        }

        protected void RemoveBullet(int iBatarang)
        {
            batarangs[iBatarang].IsActive = false;
        }

        protected void CheckBatarangCollision()
        {
            // Check to see of any of the players bullets have 
            // impacted any of the enemies.
            for (int i = 0; i < iMaxBatarangs; i++)
            {
                if (batarangs[i].IsActive)
                    for (int x = 0; x < iTotalMaxEnemies; x++)
                        if (Enemies[x].IsActive)
                            if (Intersects(batarangs[i].BoundingBox,
                                           Enemies[x].BoundingBox))
                            {
                                DestroyEnemy(x);
                                GeneratePow(batarangs[i].v2BatarangPosition);
                                seHit.Play();
                                RemoveBullet(i);

                                iScore++;
                            }
            }
        }

        protected void CheckBatmanCollision()
        {
            for (int i = 0; i < iMaxEnemies; i++)
            {
                if (Intersects(rBatmanBoundingBox, Enemies[i].BoundingBox))
                {
                    if (iScore > iHighScore)
                    {
                        iHighScore = iScore;
                    
                    }
                    seLose.Play();
                    iGameStarted = 0;
                }

            }

        }

        protected void CheckBatarangInbounds()
        {
            for (int i = 0; i < iMaxBatarangs; i++)
            {
                if (batarangs[i].IsActive)
                {
                    if ((batarangs[i].X > 1300) || (batarangs[i].X < -20))
                    {
                        batarangs[i].IsActive = false;
                        seWiff.Play();
                        iScore--;
                    }
                    if ((batarangs[i].Y > 740) || (batarangs[i].Y < -20))
                    {
                        batarangs[i].IsActive = false;
                        seWiff.Play();
                        iScore--;
                    }

                }

            }
        }


        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            v2MousePosition = Vector2.Zero;
            v2Target = Vector2.Zero;
           
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>(@"Fonts\Pericles");
            seWiff = Content.Load<SoundEffect>(@"Audio\woosh");
            seThrow = Content.Load<SoundEffect>(@"Audio\thow");
            seHit = Content.Load<SoundEffect>(@"Audio\punch");
            seLose = Content.Load<SoundEffect>(@"Audio\sad");

            t2dCrosshair = Content.Load<Texture2D>(@"Textures\crosshair");
            t2dBackground = Content.Load<Texture2D>(@"Textures\whiteGotham");
            t2dBatman = Content.Load<Texture2D>(@"Textures\image001");
            t2dHud = Content.Load<Texture2D>(@"Textures\hud");
            t2dTitleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            t2dStartButton = Content.Load<Texture2D>(@"Textures\start");
            t2dStartButtonHover = Content.Load<Texture2D>(@"Textures\starthover");
            t2dStartButtonPressed = Content.Load<Texture2D>(@"Textures\startpressed");
            t2dVillan = Content.Load<Texture2D>(@"Textures\batman villans");
            t2dPow = Content.Load<Texture2D>(@"Textures\pow");

            v2StartButtonPosition = new Vector2(v2StartButtonPosition.X = 90, v2StartButtonPosition.Y = 400);
            v2BatManPosition = new Vector2(v2BatManPosition.X = 600, v2BatManPosition.Y = 275);
            v2BatManCenter = new Vector2(v2BatManPosition.X + (t2dBatman.Width / 2), v2BatManPosition.Y + (t2dBatman.Height / 2));
            v2BatarangOrigin = new Vector2(v2BatarangOrigin.X = 610, v2BatarangOrigin.Y = 285);

            rStartButtonBox = new Rectangle((int)v2StartButtonPosition.X + 11, (int)v2StartButtonPosition.Y + 12, 299, 60);
            rMouseBox = new Rectangle((int)v2MousePosition.X, (int)v2MousePosition.Y, 1, 1);
            rBatmanBoundingBox = new Rectangle((int)v2BatManPosition.X + (t2dBatman.Width / 2), (int)v2BatManPosition.Y + (t2dBatman.Height / 2), 1, 1);

            for (int x = 0; x < iMaxBatarangs; x++)
                batarangs[x] = new Batarang(Content.Load<Texture2D>(@"Textures\batarang"));

            for (int x = 0; x < iTotalBams; x++)
                Bams[x] = new Pow(t2dPow);

            for (int i = 0; i < iTotalMaxEnemies; i++)
            {
                Enemies[i] = new Enemy(t2dVillan, 0, 0, i); // Pass i as the frame number selecting which bad guy on sprite sheet
            }
            // TODO: use this.Content to load your game content here
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

       
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            
            KeyboardState keyState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            
            v2MousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            v2Target = new Vector2(v2MousePosition.X + (t2dCrosshair.Width / 2), v2MousePosition.Y + (t2dCrosshair.Height / 2));
            rMouseBox = new Rectangle((int)v2Target.X, (int)v2Target.Y, 1, 1); // 1 pixel mouse collision box.

            if (iGameStarted == 0)
            {
                if (rMouseBox.Intersects(rStartButtonBox))
                {
                    iStartButtonState = 1;
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        iStartButtonState = 2;
                    }
                    if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                    {
                        StartNewGame();
                    }


                }
                else
                {
                    iStartButtonState = 0;
                }
                
                if (keyState.IsKeyDown(Keys.Space))
                {
                    StartNewGame();
                }
            }

            if (iGameStarted == 1)
            {

                CheckBatarangCollision();
                CheckBatmanCollision();
                CheckBatarangInbounds();
                if (iScore <= 0)
                    iScore = 0;
                v2BatarangDestination = v2MousePosition;
                fWaveTimeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if ((mouseState.LeftButton == ButtonState.Pressed) && (lastMouseState.LeftButton == ButtonState.Released))
                {
                    FireBullet(v2BatarangDestination);
                }

                if (fWaveTimeElapsed >= fTimeToNewWave)  
                {
                    fWaveTimeElapsed = 0f;
                    iEnemyWaveCount++;
                    GenerateEnemies();
                    iEnemiesPerWave +=2;
                    if (iEnemiesPerWave >= 20)
                        iEnemiesPerWave += 10;
                    //fTimeToNewWave -= fWaveTimeIncriment;
                    if (fTimeToNewWave <= 0)
                        fTimeToNewWave = 0;

                }

                for (int i = 0; i < iTotalMaxEnemies; i++)
                {
                    if (Enemies[i].IsActive)
                        Enemies[i].Update(gameTime);
                }
                for (int y = 0; y < iTotalBams; y++)
                {
                    if (Bams[y].IsActive)
                        Bams[y].Update(gameTime);
                }
                
                UpdateBatarangs(gameTime);


            }
            // TODO: Add your update logic here
            lastKeyboardState = keyState;
            lastMouseState = mouseState;
            base.Update(gameTime);
        }

        
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (iGameStarted == 1)
            {
                spriteBatch.Draw(t2dBackground, new Rectangle(0, 0, 1280, 720), Color.White);
                spriteBatch.Draw(t2dBatman, v2BatManPosition, Color.White);

                // Draw any active player bullets on the screen
                for (int i = 0; i < iMaxBatarangs; i++)
                {
                    // Only draw active bullets
                    if (batarangs[i].IsActive)
                    {
                        batarangs[i].Draw(spriteBatch);
                    }
                }

                for (int i = 0; i < iTotalMaxEnemies; i++)
                {
                    if (Enemies[i].IsActive)
                        Enemies[i].Draw(spriteBatch);
                }

                for (int i = 0; i < iTotalBams; i++)
                {
                    // Only draw active bullets
                    if (Bams[i].IsActive)
                    {
                        Bams[i].Draw(spriteBatch);
                    }
                }

                spriteBatch.Draw(t2dCrosshair, v2MousePosition, Color.White);
                spriteBatch.Draw(t2dHud, new Rectangle(0, 0, 1280, 720), Color.White);
                spriteBatch.DrawString(spriteFont, iScore.ToString(), vScoreTextLoc, Color.Black);
            }

            if (iGameStarted == 0)
            {
                spriteBatch.Draw(t2dTitleScreen, new Rectangle(0, 0, 1280, 720), Color.White);

                if (iStartButtonState == 0)
                {
                    spriteBatch.Draw(t2dStartButton, v2StartButtonPosition, Color.White);
                }
                if (iStartButtonState == 1)
                {
                    spriteBatch.Draw(t2dStartButtonHover, v2StartButtonPosition, Color.White);
                }
                if (iStartButtonState == 2)
                {
                    spriteBatch.Draw(t2dStartButtonPressed, v2StartButtonPosition, Color.White);
                }
                spriteBatch.DrawString(spriteFont, iScore.ToString(), vMenuScoreTextLoc, Color.White);
                spriteBatch.DrawString(spriteFont, iHighScore.ToString(), vMenuHighScoreTextLoc, Color.White);
                spriteBatch.Draw(t2dCrosshair, v2MousePosition, Color.White);
            }
            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
