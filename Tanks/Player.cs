using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tanks
{
    public class Player : GameObject
    {
        private TurnManager turnManager;
        protected int health;
        protected int maxHealth;
        protected int shield;
        private Texture2D playerOneTexture;
        private Texture2D playerTwoTexture;
        private bool isPlayerOne;
        private new int speed;
        private HealthBar healthBar;

        private KeyboardState prevKeyboardState;

        private SoundEffect cannonSound;
        private SoundEffectInstance cannonSoundInstance;
        private float soundEffectVolume = 0.03f;

        private SoundEffect driving;
        private SoundEffectInstance drivingInstance;
        // Kanon
        private Cannon cannon;
        // Indlæs kanonens sprite i `LoadContent`:
        private Texture2D cannonTexture;

        public Player(Vector2 startPosition, bool isPlayerOne, TurnManager turnManager)
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
            prevKeyboardState = Keyboard.GetState();
            this.turnManager = turnManager;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            playerOneTexture = contentManager.Load<Texture2D>("tank_model_1_1_b");
            playerTwoTexture = contentManager.Load<Texture2D>("tank_model_2_1_b");

            //Spites for spillere
            Sprite = isPlayerOne ? playerOneTexture : playerTwoTexture;

            cannonSound = contentManager.Load<SoundEffect>("tankcannon"); // loads bullet sound
            cannonSoundInstance = cannonSound.CreateInstance(); // creates instance for playback

            driving = contentManager.Load<SoundEffect>("tankdriving"); // loads walking sound
            drivingInstance = driving.CreateInstance(); // creates instance for playback during player movement
            // Tilføj kanonens sprite
            cannonTexture = contentManager.Load<Texture2D>("Cannon");
            cannon = new Cannon(cannonTexture, position); // Initialiser kanonen
        }

        public override void Update(GameTime gameTime)
        {
            healthBar.CurrentHealth = health;
            healthBar.Position = position - new Vector2(0, 1) * 18; //Healthbar is 18 pixels above the tank

            // Beregn tankens centerpunkt
            Vector2 tankCenter = position + new Vector2(Sprite.Width / 2 * scale, Sprite.Height / 2 * scale);

            bool isFlipped = false;
            // Opdater kanonens position og rotation
            cannon.Update(tankCenter, rotation, isFlipped);

            if (!turnManager.IsPlayerTurn(this))
                return;

            var keyboardState = Keyboard.GetState();

            // Bevæg tanken
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                position.X -= speed;
                spriteEffects = SpriteEffects.FlipHorizontally;
                
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                position.X += speed;
                spriteEffects = SpriteEffects.None;
                
            }

            // Roter kanonen med W og S
            if (keyboardState.IsKeyDown(Keys.W)) cannon.Rotate(-0.05f);
            if (keyboardState.IsKeyDown(Keys.S)) cannon.Rotate(0.05f);
            if (keyboardState.IsKeyDown(Keys.Space) && !prevKeyboardState.IsKeyDown(Keys.Space))
            {
                //var bulletDirection = new Vector2(1, -1);
                Vector2 direction = new Vector2((float)Math.Cos(cannon.Rotation), (float)Math.Sin(cannon.Rotation));
                direction.Normalize();

                // direction now contains the unit vector representing the direction

                //Spawns the bullet 30 pixels ahead so the player doesn't get hit by its own bullet
                //Alternative solution could be to disable to bullet collision for a few milliseconds, but this is simpler
                var bulletPosition = position + direction * 50;

                Game1.InstantiateGameobject(new Bullet(bulletPosition, direction * 2));
                turnManager.EndTurn();

                cannonSound.Play(soundEffectVolume, 0.0f, 0.0f);
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.D))
            {
                if (drivingInstance.State != SoundState.Playing)
                {
                    drivingInstance.Volume = soundEffectVolume;
                    drivingInstance.Play();
                }
            }
            else
            {
                if (drivingInstance.State == SoundState.Playing)
                {
                    drivingInstance.Stop();
                }
            }



            prevKeyboardState = keyboardState;

            // Roter kanonen med pil op og pil ned
            if (keyboardState.IsKeyDown(Keys.Up)) cannon.Rotate(-0.05f);
            if (keyboardState.IsKeyDown(Keys.Down)) cannon.Rotate(0.05f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            healthBar.Draw(spriteBatch);
            // Tegn tanken først
            spriteBatch.Draw(Sprite, position, null, Color.White, rotation,
                new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, spriteEffects, 0);

            // Tegn kanonen ovenpå tanken
            cannon.Draw(spriteBatch, spriteEffects);

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
            if (other is Bullet)
            {
                health -= 70;
                Debug.WriteLine($"Tank got hit and now has {health} health left");
                Game1.AddGameobjectToRemove(other);
                if (health <= 0)
                {
                    Game1.AddGameobjectToRemove(this);
                }
            }
        }

    }
}