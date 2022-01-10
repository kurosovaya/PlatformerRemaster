using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformerRemaster
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private Platform[] platforms;

        Texture2D background_1;

        SpriteFont font;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
            //_graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // IDGF
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            background_1 = Content.Load<Texture2D>("wp3263282");
            player = new Player(new Vector2(0, 120), Services);
            platforms = new Platform[] {new Platform(new Vector2(1025, 800), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64, 800), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 2, 800), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 3, 800), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 4, 800), 64, 64, "RockTItle"),

                                        new Platform(new Vector2(1025, 800 - 64 * 1), 64, 64, "RockTItle"),
                                        //new Platform(new Vector2(1025 - 64, 800 - 64 * 2), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 2, 800 - 64 * 3), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 3, 800 - 64 * 4), 64, 64, "RockTItle"),
                                        new Platform(new Vector2(1025 - 64 * 4, 800 - 64 * 5), 64, 64, "RockTItle")};


            player.LoadContent();
            foreach (Platform platform in platforms)
            {
                platform.LoadContent(Services);
            }


            font = Content.Load<SpriteFont>("myFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            player.Move();
            player.UpdatePosition(gameTime);
            foreach (Platform platform in platforms)
            {
                player.Collision(platform);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();
            _spriteBatch.Draw(background_1, new Rectangle(0, 0, 1920, 1080), Color.White);
            player.Draw(_spriteBatch);
            foreach (Platform platform in platforms)
            {
                platform.Draw(_spriteBatch);
            }

            string text = $"X.V: {player.velocity.X} Y.V: {player.velocity.Y}";
            Vector2 textMiddlePoint = font.MeasureString(text) / 2;
            _spriteBatch.DrawString(font, text, new Vector2(0, 0),
                Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.5f);
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
