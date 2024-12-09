using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tanks
{
    public class Player : GameObject
    {
        protected int health;
        protected int maxHealth;
        protected int shield;
        private Texture2D playerOneTexture;
        private Texture2D playerTwoTexture;
        private bool isPlayerOne;
        private new int speed;
        private HealthBar healthBar;

        private KeyboardState prevKeyboardState;

        public Player(Vector2 startPosition, bool isPlayerOne)
        {
            //Start stats
            this.maxHealth = 100;
            this.health = maxHealth;
            this.shield = 100;

            //Startposition
            this.position = startPosition;

            //Er playerOne bool
            this.isPlayerOne = isPlayerOne;

            //Sprite skala
            this.scale = .15f;

            //Hastighed
            this.speed = 1;

            healthBar = new HealthBar(maxHealth);

        }

        public override void LoadContent(ContentManager contentManager)
        {
            playerOneTexture = contentManager.Load<Texture2D>("tank_model_1_1_b");
            playerTwoTexture = contentManager.Load<Texture2D>("tank_model_2_1_b");

            //Spites for spillere
            Sprite = isPlayerOne ? playerOneTexture : playerTwoTexture;
        }

        public override void Update(GameTime gameTime)
        {
            healthBar.CurrentHealth = health;
            healthBar.Position = position - new Vector2(0, 1) * 18; //Healthbar is 18 pixels above the tank

            var keyboardState = Keyboard.GetState();

            if (isPlayerOne)
            {
                if (keyboardState.IsKeyDown(Keys.A)) 
                {
                    position.X -= speed;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                if (keyboardState.IsKeyDown(Keys.D)) 
                {
                    position.X += speed;
                    spriteEffects = SpriteEffects.None;
                }
                if(keyboardState.IsKeyDown(Keys.Space) && !prevKeyboardState.IsKeyDown(Keys.Space))
                {
                    var bulletDirection = new Vector2(1, -1);

                    //Spawns the bullet 30 pixels ahead so the player doesn't get hit by its own bullet
                    //Alternative solution could be to disable to bullet collision for a few milliseconds, but this is simpler
                    var bulletPosition = position + bulletDirection * 30;

                    Game1.InstantiateGameobject(new Bullet(bulletPosition, bulletDirection));
                }
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left)) 
                {
                    position.X -= speed;
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                if (keyboardState.IsKeyDown(Keys.Right)) 
                {
                    position.X += speed;
                    spriteEffects = SpriteEffects.None;
                }
            }

            prevKeyboardState = keyboardState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            healthBar.Draw(spriteBatch);

            // Tegn spillerne med spriteeffects fra player-input
            spriteBatch.Draw(Sprite, Position, null, Color.White, rotation, 
            new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, spriteEffects, 0);

            if (collisionEnabled)
            {
                // Beregn collision box baseret på den skalerede størrelse
                int scaledWidth = (int)(Sprite.Width * scale);
                int scaledHeight = (int)(Sprite.Height * scale);
                collisionBox = new Rectangle(
                    (int)position.X - scaledWidth / 2,
                    (int)position.Y - scaledHeight / 2,
                    scaledWidth,
                    scaledHeight
                );
            }
        }

        public override void OnCollision(GameObject other)
        {
            if(other is Bullet)
            {
                health -= 70;
                Debug.WriteLine($"Tank got hit and now has {health} health left");
                Game1.AddGameobjectToRemove(other);
            }
        }

    }
}