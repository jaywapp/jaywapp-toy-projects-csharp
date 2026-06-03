using FMRookieScouter.Control.Belong;
using FMRookieScouter.Control.Stat;
using FMRookieScouter.Model;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace FMRookieScouter.View
{
    public class PlayerStatViewModel : ReactiveObject
    {
        #region Internal Field
        private readonly Player _player;
        private TabItem _selectedTab;
        #endregion

        #region Properties
        public List<TabItem> Tabs { get; } = new List<TabItem>();

        public TabItem SelectedTab
        {
            get => _selectedTab;
            set => this.RaiseAndSetIfChanged(ref _selectedTab, value);
        }
        #endregion

        #region Constructor
        public PlayerStatViewModel(Player player)
        {
            _player = player;
            Tabs = CreateStatTabItems(player).ToList();
            SelectedTab = Tabs.FirstOrDefault();
        }
        #endregion

        #region Functions
        private static IEnumerable<TabItem> CreateStatTabItems(Player player)
        {
            yield return new TabItem()
            {
                Header = "Role",
                Content = new RolesView() { DataContext = player.Part.Roles },
            };

            if (player.Goalkeeping.IsGoalkeepper())
            {
                yield return new TabItem()
                {
                    Header = nameof(Player.Goalkeeping),
                    Content = new StatView() { DataContext = player.Goalkeeping },
                };
            }

            yield return new TabItem()
            {
                Header = nameof(Player.Mental),
                Content = new StatView() { DataContext = player.Mental },
            };

            yield return new TabItem()
            {
                Header = nameof(Player.Physical),
                Content = new StatView() { DataContext = player.Physical },
            };

            yield return new TabItem()
            {
                Header = nameof(Player.Technical),
                Content = new StatView() { DataContext = player.Technical },
            };
        }
        #endregion
    }
}
