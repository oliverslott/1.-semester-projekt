using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tanks
{
    public class Cannon
    {
        private Texture2D sprite;
        private Vector2 position; // Position på tanken
        private float rotation;  // Rotation af kanonen
        private Vector2 origin;  // Midtpunkt af kanonen
        private float scale;     // Skalering af kanonen

        public Cannon(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
            this.rotation = 0f;
            this.scale = 0.15f;

            // Centrer origin, så rotation sker omkring kanonens midtpunkt
            this.origin = new Vector2(sprite.Width / 2, sprite.Height / 2);
        }

        public void Update(Vector2 tankCenter, float tankRotation)
        {
            // Beregn en offset for at flytte kanonen op
            Vector2 offset = new Vector2(-8, -30); // Flytter kanonen 20 pixels op (juster efter behov)

            // Roter offset omkring tankens midtpunkt baseret på tankens rotation
            Matrix rotationMatrix = Matrix.CreateRotationZ(tankRotation);
            offset = Vector2.Transform(offset, rotationMatrix);

            // Placér kanonen midt på tanken med offset
            position = tankCenter + offset;

            // Rotation af kanonen følger tankens rotation plus den specifikke rotation af kanonen
            rotation = tankRotation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                sprite, 
                position, 
                null, 
                Color.White, 
                rotation,    // Brug rotation til at dreje kanonen
                origin, 
                scale, 
                SpriteEffects.None, 
                0f
            );
        }

        public void Rotate(float angle)
        {
            rotation += angle; // Justér kanonens rotation
        }
    }
}