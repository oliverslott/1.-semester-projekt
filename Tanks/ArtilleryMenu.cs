using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks
{
    public class ArtilleryMenu
    {
        private List<string> ammoTypes;
        private Vector2 position;
        private SpriteFont font;

        public int SelectedAmmo { get; set; }

        public ArtilleryMenu(List<string> ammoTypes, Vector2 position, SpriteFont font)
        {
            this.ammoTypes = ammoTypes;
            this.position = position;
            this.font = font;
            SelectedAmmo = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < ammoTypes.Count; i++)
            {
                Color color = (i == SelectedAmmo) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(font, ammoTypes[i], position + new Vector2(i * 100, 0), color);
            }
        }
    }
}
