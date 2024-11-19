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

        public Player(Vector2 startPosition, bool isPlayerOne)
        {
            this.health = 100;
            this.shield = 100;
            this.position = startPosition;
            this.isPlayerOne = isPlayerOne;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            playerOneTexture = contentManager.Load<Texture2D>("tank_model_1_1_b");
            playerTwoTexture = contentManager.Load<Texture2D>("tank_model_2_1_b");
            Sprite = isPlayerOne ? playerOneTexture : playerTwoTexture;
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();

            if (isPlayerOne)
            {
                if (keyboardState.IsKeyDown(Keys.A)) position.X -= 1;
                if (keyboardState.IsKeyDown(Keys.D)) position.X += 1;
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left)) position.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right)) position.X += 1;
            }
        }

        public override void OnCollision(GameObject other)
        {
            
        }

    }
}