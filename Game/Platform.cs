using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PlatformerRemaster
{
    public class Platform
    {
        public Vector2 Position { get; set; }
        public int X { get => (int)Position.X; }
        public int Y { get => (int)Position.Y; }
        public int Width { get; }
        public int Height { get; }
        private string _textureMame;
        private Texture2D _texture;

        public Platform(Vector2 position, int width, int height, string textureName)
        {
            Position = position;
            Width = width;
            Height = height;
            _textureMame = textureName;
        }

        public void LoadContent(IServiceProvider serviceProvider)
        {
            ContentManager content = new ContentManager(serviceProvider, "Content");
            _texture = content.Load<Texture2D>(_textureMame);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
        }        
    }
}
