using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace PlatformerRemaster
{
    public class Player
    {
        private Texture2D idleTexture;
        private IServiceProvider serviceProvider;

        private float gravity = 10000f;
        private float maxVelocity = 15000f;
        private float speed = 800f;
        private int jumpCountMax = 6;
        private int jumpCount;
        public Vector2 velocity = new Vector2(0, 0);

        static KeyboardState currentKeyState;
        static KeyboardState previousKeyState;


        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 position;

        public float X { get => position.X; set => position.X = value; }
        public float Y { get => position.Y; set => position.Y = value; }
        public int Width { get; } = 64;
        public int Height { get; } = 64;

        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;

        public Player(Vector2 position, IServiceProvider serviceProvider)
        {
            Position = position;
            this.serviceProvider = serviceProvider;
            jumpCount = jumpCountMax;
        }

        public void LoadContent()
        {
            content = new ContentManager(serviceProvider, "Content");
            idleTexture = Content.Load<Texture2D>("cover-256");
        }

        public void Reset(Vector2 position)
        {
            Position = position;
        }

        public void Move()
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            if (currentKeyState.IsKeyDown(Keys.Space) && !previousKeyState.IsKeyDown(Keys.Space))
                Jump();

            if (currentKeyState.IsKeyDown(Keys.A))
            {
                velocity.X = -speed;
            }

            if (currentKeyState.IsKeyDown(Keys.D))
            {
                velocity.X = +speed;
            }
            if (currentKeyState.IsKeyUp(Keys.A) && previousKeyState.IsKeyDown(Keys.A)
                || currentKeyState.IsKeyUp(Keys.D) && previousKeyState.IsKeyDown(Keys.D))
            {
                velocity.X = 0;
            }
        }

        public void Jump()
        {
            if (jumpCount <= jumpCountMax)
            {
                velocity.Y = -2000f;
                jumpCount++;
            }
        }

        private void OnGround()
        {
            jumpCount = 0;
        }

        public void Collision(Platform platform)
        { 
            if (X + Width > platform.X && X < platform.X + platform.Width)
            {
                //player above
                if (Y + Height > platform.Y && Y + Height < platform.Y + platform.Height && velocity.Y > 0)
                {
                    Y = platform.Y - Height;
                    velocity.Y = 0;
                    OnGround();
                }
                //player below
                if (Y <= platform.Y + platform.Height && Y > platform.Y && velocity.Y < 0)
                {
                    Y = platform.Y + platform.Height;
                    velocity.Y = 0;                    
                }
            }
            if (Y + Height > platform.Y && Y < platform.Y + platform.Height)
            {
                //player left
                if (X + Width > platform.X && X < platform.X + platform.Width && velocity.X > 0)
                {
                    X = platform.X - platform.Width;
                    velocity.X = 0;
                }

                //player right
                if (X < platform.X + platform.Width && X > platform.X && velocity.X < 0)
                {
                    X = platform.X + platform.Width;
                    velocity.X = 0;
                }
            }
        }

        // retarded method
        public void UpdatePosition(GameTime gameTime)
        {
            if (position.Y < 1080 - 64)
            {
                if (velocity.Y < maxVelocity)
                {
                    velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (velocity.Y > maxVelocity)
                {
                    velocity.Y = maxVelocity;
                }
            }
            if (position.Y > 1080 - 64)
            {
                velocity.Y = 0;
                position.Y = 1080 - 64;
                OnGround();
            }

            if (position.X < 0)
            {
                position.X = 0;
            }
            else if (position.X > 1920 - 64)
            {
                position.X = 1920 - 64;
            }

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(idleTexture, new Rectangle((int)Position.X, (int)Position.Y, 64, 64), Color.White);
        }
    }
}
