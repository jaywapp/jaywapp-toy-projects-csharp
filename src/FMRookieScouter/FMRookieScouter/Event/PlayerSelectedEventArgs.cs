using FMRookieScouter.Model;
using System;

namespace FMRookieScouter.Event
{
    public class PlayerSelectedEventArgs : EventArgs
    {
        public Player Seleted { get; }

        public PlayerSelectedEventArgs(Player player)
        {
            Seleted = player;
        }
    }
}
