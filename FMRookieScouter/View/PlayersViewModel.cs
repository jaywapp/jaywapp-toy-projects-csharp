using FMRookieScouter.Event;
using FMRookieScouter.Model;
using FMRookieScouter.Service.Filter;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace FMRookieScouter.View
{
    public class PlayersViewModel : ReactiveObject
    {
        #region Internal Field
        private Player _selectedPlayer;
        private List<Player> _displayPlayers;
        private PlayerFilter _filter;
        #endregion

        #region Properties
        public List<Player> Players { get; }
        public List<Player> DisplayPlayers
        {
            get => _displayPlayers;
            set => this.RaiseAndSetIfChanged(ref _displayPlayers, value);
        }

        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set => this.RaiseAndSetIfChanged(ref _selectedPlayer, value);
        }

        public PlayerFilter Filter
        {
            get => _filter;
            set => this.RaiseAndSetIfChanged(ref _filter, value);
        }
        #endregion

        #region Event
        public EventHandler<PlayerSelectedEventArgs> PlayerSelected;
        #endregion

        #region Constructor
        public PlayersViewModel(Sesson sesson)
        {
            Players = sesson.Players;

            this.WhenAnyValue(x => x.Filter)
                .Where(filter => filter != null)
                .Subscribe(filter => filter.ConditionChanged += OnFilterChanged);

            this.WhenAnyValue(x => x.Filter)
                .Select(filter => Filtering(filter, Players))
                .BindTo(this, x => x.DisplayPlayers);

            this.WhenAnyValue(x => x.SelectedPlayer)
                .Subscribe(o => PlayerSelected?.Invoke(this, new PlayerSelectedEventArgs(o)));
        }
        #endregion

        #region Functions
        private void OnFilterChanged(object sender, EventArgs e)
        {
            if (!(sender is PlayerFilter filter))
                return;

            DisplayPlayers = filter.Filtering(Players).ToList();
        }

        private static List<Player> Filtering(PlayerFilter filter, IEnumerable<Player> sources)
        {
            return filter?.Filtering(sources)?.ToList() ?? sources.ToList();
        }
        #endregion
    }
}
