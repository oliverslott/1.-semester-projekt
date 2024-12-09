using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Tanks
{
    // Made by Oliver
    public class TurnManager
    {
        private Player[] players;
        private int currentTurnIndex = 0;
        private float switchingTurnTimer = 0; //seconds
        private float switchingTurnTime = 3f; //seconds
        private bool switchingTurn = false;

        public TurnManager(Player[] players)
        {
            this.players = players;
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (switchingTurn)
            {
                switchingTurnTimer += deltaTime;
                if (switchingTurnTimer >= switchingTurnTime)
                {
                    switchingTurnTimer = 0f;
                    switchingTurn = false;
                }
            }
        }

        public bool IsPlayerTurn(Player player)
        {
            return !switchingTurn && player == players[currentTurnIndex];
        }

        public void EndTurn()
        {
            currentTurnIndex++;
            if (currentTurnIndex >= players.Length)
            {
                currentTurnIndex = 0;
            }
            switchingTurn = true;
            Debug.WriteLine($"Turn changed to player index: {currentTurnIndex}");
        }
    }
}
