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

            // Kanonens rotation sker om følgende punkt
            this.origin = new Vector2(sprite.Width - 32, sprite.Height- 100);
        }

        public void Update(Vector2 tankCenter, float tankRotation, bool isFlipped)
        {
            // Beregn en offset til at flytte kanonen. Dette er koordinater for, hvor den er placeret.
            Vector2 offset = new Vector2(- 21, -30);

            // Roter offset omkring tankens midtpunkt baseret på tankens rotation
            Matrix rotationMatrix = Matrix.CreateRotationZ(tankRotation);
            offset = Vector2.Transform(offset, rotationMatrix);

            // Hvis tank ændrer retning, flip offset for kanonen.
            if (isFlipped)
            {
                offset.X = -offset.X;
            }

            // Placér kanon på tanken vha. offset og tankCenter
            position = tankCenter + offset;

            // Rotation af kanonen skal på baggrund af tankens rotation og kanonens rotation
            rotation = tankRotation + this.rotation;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects spriteEffects)
        {
            spriteBatch.Draw(
            sprite,
            position,
            null,
            Color.White,
            rotation, 
            origin,   
            scale,
            spriteEffects, 
            0
        );
        }

        public void Rotate(float angle)
        {
            // Ændrer kanonens rotation
            rotation += angle; 

            // Begrænser rotation for kanonen til at være parallel med tanken.
            this.rotation = Math.Clamp(this.rotation, -MathHelper.ToRadians(0), MathHelper.ToRadians(180));

            // Tjek for output
            Console.WriteLine("rotating cannon");
        }
    }
}