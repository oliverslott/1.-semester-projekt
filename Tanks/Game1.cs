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

        public static GraphicsDevice GlobalGraphicsDevice { get; private set; }

        //private SpriteFont textFont;

        private List<GameObject> gameObjects;

        private static List<GameObject> gameObjectsToRemove;
        private static List<GameObject> gameObjectsToAdd;

        private static Vector2 screenSize;

        private Texture2D collisionTexture;

        private Texture2D mapSprite;

        private Player[] players = new Player[2]; //Currently game only supports 2 players

        private TurnManager turnManager;

        private Song song;
        private float songVolume = 0.02f;

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
            GlobalGraphicsDevice = GraphicsDevice;
            // Initialiser listerne
            gameObjects = new List<GameObject>();
            gameObjectsToRemove = new List<GameObject>();
            gameObjectsToAdd = new List<GameObject>();

            mapSprite = Content.Load<Texture2D>("tankmap");

            turnManager = new TurnManager(players);

            // Opret to spillere med startpositioner
            var player1 = new Player(new Vector2(200, 405), true, turnManager);  // Spiller 1 til venstre
            var player2 = new Player(new Vector2(1080, 405), false, turnManager); // Spiller 2 til højre

            //Add player to a list so that we can later reference them for changing turns and deciding who won
            players[0] = player1;
            players[1] = player2;

            // Tilføj spillerne til spillet
            gameObjects.Add(player1);
            gameObjects.Add(player2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            collisionTexture = Content.Load<Texture2D>("pixel");

            mapSprite = Content.Load<Texture2D>("tankmap");

            foreach (GameObject gameobject in gameObjects)
            {
                gameobject.LoadContent(Content);
            }

            song = Content.Load<Song>("tankmusic"); // music for the game

            MediaPlayer.IsRepeating = true; // music keeps playing as long as the game is running
            MediaPlayer.Volume = songVolume;
            MediaPlayer.Play(song);
        }

        protected override void Update(GameTime gameTime)
        {
            turnManager.Update(gameTime);

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

            _spriteBatch.Draw(mapSprite, Vector2.Zero, Color.White);

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
                Debug.WriteLine($"Removed object: {gameObject}");
                gameObjects.Remove(gameObject);
            }
            gameObjectsToRemove.Clear();
        }

        private void DrawCollisionBox(GameObject go)
        {
            if (!go.CollisionEnabled) return;

            Rectangle collisionBox = go.CollisionBox;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);

            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);

            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);

            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);

            _spriteBatch.Draw(collisionTexture, topLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, bottomLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, rightLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
            _spriteBatch.Draw(collisionTexture, leftLine, null, Color.Red, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        
    }
}
