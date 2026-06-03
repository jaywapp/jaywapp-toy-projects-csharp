using FMRookieScouter.Access;
using FMRookieScouter.Event;
using FMRookieScouter.Model;
using FMRookieScouter.Service.Filter;
using FMRookieScouter.View;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace FMRookieScouter
{
    public class MainWindowViewModel : ReactiveObject
    {
        #region Internal Field
        private PlayerFilter _filter;
        private Player _selectedPlayer;
        private ObservableAsPropertyHelper<PlayerStatViewModel> _playerStatViewModel;
        private Dictionary<Player, PlayerStatViewModel> _statViewModelDic = new Dictionary<Player, PlayerStatViewModel>();
        #endregion

        #region Properties
        public PlayerFilter Filter
        {
            get => _filter;
            set => this.RaiseAndSetIfChanged(ref _filter, value);
        }

        public Player SelectedPlayer
        {
            get => _selectedPlayer;
            set => this.RaiseAndSetIfChanged(ref _selectedPlayer, value);
        }

        public DBAccess Access { get; }
        public FilterViewModel FilterViewModel { get; }
        public List<TabItem> PlayerTabs { get; }

        public PlayerStatViewModel PlayerStatViewModel => _playerStatViewModel.Value;
        #endregion

        #region Constructor
        public MainWindowViewModel()
        {
            Access = new DBAccess();
            Filter = new PlayerFilter();

            PlayerTabs = Access.Sessons.Values.Select(s => CreatePlayerTabItem(s, Filter)).ToList();
            FilterViewModel = new FilterViewModel(Filter);

            this.WhenAnyValue(x => x.Filter)
                .Subscribe(filter =>
                {
                    foreach (var tab in PlayerTabs)
                    {
                        if (TryGetViewModel(tab, out PlayersViewModel vm))
                            vm.Filter = filter;
                    }
                });

            this.WhenAnyValue(x => x.SelectedPlayer)
                .Where(p => p != null)
                .Select(p =>
                {
                    if (!_statViewModelDic.ContainsKey(p))
                        _statViewModelDic.Add(p, new PlayerStatViewModel(p));

                    return _statViewModelDic[p];
                })
                .ToProperty(this, x => x.PlayerStatViewModel, out _playerStatViewModel);
        }
        #endregion

        #region Functions
        private TabItem CreatePlayerTabItem(Sesson sesson, PlayerFilter filter)
        {
            var playersViewModel = new PlayersViewModel(sesson);
            playersViewModel.Filter = filter;

            playersViewModel.PlayerSelected += OnPlayerSelected;

            return new TabItem()
            {
                Header = sesson.Year,
                Content = new PlayersView() { DataContext = playersViewModel, },
            };
        }

        private void OnPlayerSelected(object sender, PlayerSelectedEventArgs e)
        {
            if (e.Seleted == null)
                return;

            SelectedPlayer = e.Seleted;
        }

        private static bool TryGetViewModel(TabItem tabItem, out PlayersViewModel vm)
        {
            vm = null;

            if (!(tabItem.Content is PlayersView view))
                return false;

            if (!(view.DataContext is PlayersViewModel viewModel))
                return false;

            vm = viewModel;
            return true;
        }
        #endregion
    }
}
