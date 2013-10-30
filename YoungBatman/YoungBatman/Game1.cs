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
        KeyboardState keyState;
        KeyboardState lastKeyboardState;
        MouseState lastMouseState;
        Enemy[] Enemies = new Enemy[iTotalMaxEnemies];

        int iScore = 0;
        int iGameStarted = 0;
        int iStartButtonState = 0;
        int iMaxEnemies = 9;
        int iActiveEnemies = 9;
        static int iTotalMaxEnemies = 20;

        Vector2 v2MousePosition;
        Vector2 v2BatManPosition;
        Vector2 v2BatarangOrigin;
        Vector2 v2Target;
        Vector2 v2StartButtonPosition;

        Texture2D t2dHud;
        Texture2D t2dBackground;
        Texture2D t2dCrosshair;
        Texture2D t2dBatman;
        Texture2D t2dTitleScreen;
        Texture2D t2dStartButton;
        Texture2D t2dStartButtonHover;
        Texture2D t2dStartButtonPressed;
        Texture2D t2dVillan;

        Rectangle rStartButtonBox;
        Rectangle rMouseBox;

        protected void StartNewGame()
        {
            iGameStarted = 1;
            iScore = 0;
        }

        protected void GenerateEnemies()
        {
            
        }

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
            
            v2StartButtonPosition = new Vector2(v2StartButtonPosition.X = 90, v2StartButtonPosition.Y = 400);
            v2BatManPosition = new Vector2(v2BatManPosition.X = 600, v2BatManPosition.Y = 275);
            v2BatarangOrigin = new Vector2(v2BatarangOrigin.X = 610, v2BatarangOrigin.Y = 285);

            t2dCrosshair = Content.Load<Texture2D>(@"Textures\crosshair");
            t2dBackground = Content.Load<Texture2D>(@"Textures\whiteGotham");
            t2dBatman = Content.Load<Texture2D>(@"Textures\image001");
            t2dHud = Content.Load<Texture2D>(@"Textures\hud");
            t2dTitleScreen = Content.Load<Texture2D>(@"Textures\TitleScreen");
            t2dStartButton = Content.Load<Texture2D>(@"Textures\start");
            t2dStartButtonHover = Content.Load<Texture2D>(@"Textures\starthover");
            t2dStartButtonPressed = Content.Load<Texture2D>(@"Textures\startpressed");
            t2dVillan = Content.Load<Texture2D>(@"Textures\batman villans");

            rStartButtonBox = new Rectangle((int)v2StartButtonPosition.X + 11, (int)v2StartButtonPosition.Y + 12, 299, 60);
            rMouseBox = new Rectangle((int)v2MousePosition.X, (int)v2MousePosition.Y, 1, 1);

           

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
                if (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed)
                {
                    Enemies[0].Generate();

                }

                for (int i = 0; i < iTotalMaxEnemies; i++)
                {
                    if (Enemies[i].IsActive)
                        Enemies[i].Update(gameTime);
                }

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

                for (int i = 0; i < iTotalMaxEnemies; i++)
                {
                    if (Enemies[i].IsActive)
                        Enemies[i].Draw(spriteBatch);
                }

                spriteBatch.Draw(t2dCrosshair, v2MousePosition, Color.White);
                spriteBatch.Draw(t2dHud, new Rectangle(0, 0, 1280, 720), Color.White);
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

                spriteBatch.Draw(t2dCrosshair, v2MousePosition, Color.White);
            }
            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}
