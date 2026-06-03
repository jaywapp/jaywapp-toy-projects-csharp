using FMRookieScouter.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FMRookieScouter.Model
{
    public class Sesson : IXElementSerializable
    {
        #region Internal Field
        private List<Player> _players = new List<Player>();
        private Dictionary<string, Player> _playersDic = new Dictionary<string, Player>();
        #endregion

        #region Properties
        public int Year { get; set; }
        public List<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                _playersDic = _players.ToDictionary(p => p.Common.Name);
            }
        }
        #endregion

        #region Functions
        public Player GetPlayer(string name)
        {
            if (!_playersDic.TryGetValue(name, out Player player))
                return null;

            return player;
        }

        public void Load(XElement element)
        {
            if (element.Name != nameof(Sesson))
                return;

            if (element.TryGetAttributeIntValue(nameof(Year), out int year))
                Year = year;

            var children = element.Elements().ToList();
            var players = new List<Player>();

            foreach (var child in children)
            {
                var player = new Player();

                player.Load(child);
                players.Add(player);
            }

            Players = players
                .GroupBy(p => p.GetID())
                .Select(g => g.FirstOrDefault())
                .ToList();
        }

        public XElement Save()
        {
            var element = new XElement(nameof(Sesson));

            element.Add(new XAttribute(nameof(Year), Year));

            foreach (var player in Players)
                element.Add(player.Save());

            return element;
        }
        #endregion
    }
}
