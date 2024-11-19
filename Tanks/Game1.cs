using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tanks
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //private SpriteFont textFont;

        private List<GameObject> gameObjects;

        private static List<GameObject> gameObjectsToRemove;
        private static List<GameObject> gameObjectsToAdd;

        private static Vector2 screenSize;

        private Texture2D collisionTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;


        }

        public static Vector2 GetScreenSize()
        {
            return screenSize;
        }

        protected override void Initialize()
        {
            // Initialiser listerne
            gameObjects = new List<GameObject>();
            gameObjectsToRemove = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();
            
            // Opret to spillere med startpositioner
            var player1 = new Player(new Vector2(200, 360), true);  // Spiller 1 til venstre
            var player2 = new Player(new Vector2(1080, 360), false); // Spiller 2 til højre

            // Tilføj spillerne til spillet
            gameObjects.Add(player1);
            gameObjects.Add(player2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("pixel");

            foreach (GameObject gameobject in gameObjects)
            {
                gameobject.LoadContent(Content);
            }

        }

        protected override void Update(GameTime gameTime)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);

                if (gameObject.CollisionEnabled)
                {
                    foreach (GameObject other in gameObjects)
                    {
                        if (other == gameObject) continue;

                        gameObject.CheckCollision(other);
                    }
                }
            }


            AddGameobjects();
            RemoveGameobjects();

            base.Update(gameTime);
        }

        public static void AddGameobjectToRemove(GameObject gameObject)
        {
            gameObjectsToRemove.Add(gameObject);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach (GameObject gameobject in gameObjects)
            {
                gameobject.Draw(_spriteBatch);

#if DEBUG
                DrawCollisionBox(gameobject);
#endif
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void AddGameobjects()
        {
            foreach (GameObject gameObject in gameObjectsToAdd)
            {
                gameObject.LoadContent(Content);
                gameObjects.Add(gameObject);
                Console.WriteLine($"Spawned object: {gameObject}");
            }

            gameObjectsToAdd.Clear();
        }
        public static void InstantiateGameobject(GameObject gameObject)
        {
            gameObjectsToAdd.Add(gameObject);
        }

        private void RemoveGameobjects()
        {
            foreach (GameObject gameObject in gameObjectsToRemove)
            {
                gameObjects.Remove(gameObject);
            }
            gameObjectsToRemove.Clear();
        }

        private void DrawCollisionBox(GameObject go)
        {
            if (!go.CollisionEnabled) return;

            Rectangle collisionBox = go.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X + 20, collisionBox.Y + 45, collisionBox.Width - 40, 1);

            Rectangle bottomLine = new Rectangle(collisionBox.X + 20, collisionBox.Y + collisionBox.Height - 45, collisionBox.Width - 40, 1);

            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width - 20, collisionBox.Y + 45, 1, collisionBox.Height - 90);

            Rectangle leftLine = new Rectangle(collisionBox.X + 20, collisionBox.Y + 45, 1, collisionBox.Height - 90);

            _spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);

        }

        
    }
}
