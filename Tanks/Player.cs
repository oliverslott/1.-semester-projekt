using System;
using System.Collections.Generic;
using System.Data;
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
        protected int shield;
        private Texture2D playerOneTexture;
        private Texture2D playerTwoTexture;
        private bool isPlayerOne;
        private new int speed;

        public Player(Vector2 startPosition, bool isPlayerOne)
        {
            //Start stats
            this.health = 100;
            this.shield = 100;

            //Startposition
            this.position = startPosition;

            //Er playerOne bool
            this.isPlayerOne = isPlayerOne;

            //Sprite skala
            this.scale = .5f;

            //Hastighed
            this.speed = 1;

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {            
            // Tegn spilleren
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
            
        }

    }
}