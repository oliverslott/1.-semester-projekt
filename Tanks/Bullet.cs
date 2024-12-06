using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    /// <summary>
    /// Made by Oliver
    /// </summary>
    internal class Bullet : GameObject
    {
        public Bullet(Vector2 pos, Vector2 direction)
        {
            position = pos;
            Sprite = new Texture2D(Game1.GlobalGraphicsDevice, 1, 1);
            scale = 5;
            Sprite.SetData(new[] { Color.Black });
            velocity = direction;
            speed = 300;
        }
        public override void LoadContent(ContentManager contentManager)
        {

        }

        public override void OnCollision(GameObject other)
        {

        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            velocity.Y += 1f * deltaTime;
            Move(gameTime);
        }
    }
}
