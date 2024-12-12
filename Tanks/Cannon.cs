using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Tanks
{
    public class Cannon
    {
        private Texture2D sprite;
        private Vector2 position; // Position på tanken
        private float rotation;  // Rotation af kanonen
        private Vector2 origin;  // Midtpunkt af kanonen
        private float scale;     // Skalering af kanonen

        public float Rotation { get => rotation; }

        public Cannon(Texture2D sprite, Vector2 position)
        {
            this.sprite = sprite;
            this.position = position;
            this.rotation = 0f;
            this.scale = 0.15f;

            // Centrer origin, så rotation sker omkring kanonens midtpunkt
            this.origin = new Vector2(sprite.Width / 2 - 55, sprite.Height / 2);
        }

        public void Update(Vector2 tankCenter, float tankRotation, bool isFlipped)
        {
            // Beregn en offset for at flytte kanonen op
            Vector2 offset = new Vector2(-18, -30); // Flytter kanonen 20 pixels op (juster efter behov)

            // Roter offset omkring tankens midtpunkt baseret på tankens rotation
            Matrix rotationMatrix = Matrix.CreateRotationZ(tankRotation);
            offset = Vector2.Transform(offset, rotationMatrix);

            // Hvis tanken er flipped, spejl offset vandret
            if (isFlipped)
            {
                offset.X = -offset.X;

                // Justér positionen for flip omkring venstre side
                position = tankCenter + offset - new Vector2(sprite.Width * scale, 0); // Flyt mod venstre
            }

            // Placér kanonen midt på tanken med offset
            position = tankCenter + offset;

            // Rotation af kanonen følger tankens rotation plus den specifikke rotation af kanonen
            //rotation = tankRotation;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
            sprite,
            position,
            null,
            Color.White,
            rotation, // Roter kanonen som normalt
            origin,   // Sørg for, at kanonen roterer omkring sit midtpunkt
            scale,
            spriteEffects, // Flip baseret på tankens retning
            0
        );
        }

        public void Rotate(float angle)
        {
            rotation += angle; // Justér kanonens rotation
            Debug.WriteLine(rotation);
        }
    }
}