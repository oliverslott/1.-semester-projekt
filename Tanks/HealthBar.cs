using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    //Made by Oliver
    internal class HealthBar
    {
        private Texture2D sprite;
        private Vector2 position;
        private const int healthbar_width = 35;
        private int maxHealth;
        private int currentHealth;
        public Vector2 Position { get => position; set => position = value; }
        public int CurrentHealth { get => currentHealth; set => currentHealth = value; }

        public HealthBar(int maxHealth)
        {
            sprite = new Texture2D(Game1.GlobalGraphicsDevice, 1, 1);
            sprite.SetData(new[] { Color.Red });
            this.maxHealth = maxHealth;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            int healthWidth = (int)(healthPercentage * healthbar_width);
            spriteBatch.Draw(sprite, new Rectangle((int)position.X - healthbar_width / 2, (int)position.Y, healthWidth, 5), Color.White);
        }
    }
}
