using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaveInv
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D invader, _ship, _missile;
        Rectangle rectShip, rectMissile;
        Rectangle[,] rectinvander;
        string[,] invaderAlive;
        int invaderspeed = 3;
        int rows = 5;
        int cols = 7;
        string direction = "RIGHT";
        string missileVisible = "NO";
        private SpriteFont _fontscore;
        private int score;
        Rectangle sourceRect, destRect;
        float elapsed;
        float delay = 400f;
        int frames;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;  
            graphics.PreferredBackBufferHeight = 720;  
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }
        protected override void Initialize()
        {
            destRect = new Rectangle(rows, cols, 72, 48);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _ship = Content.Load<Texture2D>("Player32");
            rectShip.Width = _ship.Width;
            rectShip.Height = _ship.Height;
            rectShip.X = 0;
            rectShip.Y = 670;
            invader = Content.Load<Texture2D>("AleinO");
            rectinvander = new Rectangle[rows, cols];
            invaderAlive = new string[rows, cols];
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    rectinvander[r, c].Width = 72;
                    rectinvander[r, c].Height = 48;
                    rectinvander[r, c].X = 80 * c;
                    rectinvander[r, c].Y = 50 * r;
                    invaderAlive[r, c] = "YES";
                }
            _missile = Content.Load<Texture2D>("PBullet");
            rectMissile.Width = _missile.Width;
            rectMissile.Height = _missile.Height;
            rectMissile.X = 0;
            rectMissile.Y = 0;
            _fontscore = Content.Load<SpriteFont>("Score");
        } 
        protected override void UnloadContent()
        {
           
        }
        protected override void Update(GameTime gameTime)
        {
            int rightSide = graphics.GraphicsDevice.Viewport.Width;
            int leftSide = 0;
            sourceRect = new Rectangle(72 * frames, 0, 72, 48);

            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(elapsed >= delay)
            {
                if (frames >= 1)
                    frames = 0;
                
                else
                    frames++;

                elapsed = 0;
                
            }

            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (direction.Equals("RIGHT"))
                        rectinvander[r, c].X += invaderspeed;
                    if (direction.Equals("LEFT"))
                        rectinvander[r, c].X -= invaderspeed;
                }
            string changeDirection = "N";
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (invaderAlive[r, c].Equals("YES"))
                    {
                        if (rectinvander[r, c].X + rectinvander[r, c].Width > rightSide)
                        {
                            direction = "LEFT";
                            changeDirection = "Y";
                        }
                        if (rectinvander[r, c].X < leftSide)
                        {
                            direction = "RIGHT";
                            changeDirection = "Y";
                        }
                    }
                }
            if (changeDirection.Equals("Y"))
            {
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        rectinvander[r, c].Y += 13;
            }
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Left))
                rectShip.X -= 3;
            if (kb.IsKeyDown(Keys.Right))
                rectShip.X += 3;
            if (kb.IsKeyDown(Keys.Space) && missileVisible.Equals("NO"))
            {
                missileVisible = "YES";
                rectMissile.X = rectShip.X + 13;
                rectMissile.Y = rectShip.Y - 32;
            }
           if (missileVisible.Equals("YES"))
                rectMissile.Y -= 5;
           if (missileVisible.Equals("YES"))
               for (int r = 0; r < rows; r++)
                   for (int c = 0; c < cols; c++)
                   {
                       if(invaderAlive[r,c].Equals("YES"))
                           if (rectMissile.Intersects(rectinvander[r, c]))
                           {
                                missileVisible = "NO";
                                invaderAlive[r, c] = "NO";
                                score += 10;
                           } 
                   }
            if (rectMissile.Y + rectMissile.Height < 0)
                missileVisible = "NO";

            int count = 0;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (invaderAlive.Equals("YES"))
                    {
                        count += 1;
                    }
            if(count > (rows * cols / 3))
                invaderspeed = 6;

            for (int r = 0; r < rows; r++)
               for (int c = 0; c < cols; c++)
               {
                    if (invaderAlive[r, c].Equals("YES"))
                        if (rectinvander[r, c].Y + rectinvander[r, c].Height > rectShip.Y)
                            this.Exit();
               }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (invaderAlive[r, c].Equals("YES"))
                    {
                        spriteBatch.Draw(invader,rectinvander[r,c] , sourceRect, Color.White);
                    }
                    
                }
            spriteBatch.Draw(_ship, rectShip, Color.White);
            spriteBatch.DrawString(_fontscore, "Score: " + score, Vector2.Zero, Color.White);
            if(missileVisible.Equals("YES"))
                spriteBatch.Draw(_missile, rectMissile, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
