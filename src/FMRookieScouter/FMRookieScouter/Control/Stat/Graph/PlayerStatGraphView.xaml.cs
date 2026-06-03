using FMRookieScouter.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.Stat.Graph
{
    /// <summary>
    /// PlayerStatGraphView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerStatGraphView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private Player _player;

        private Visibility _fieldPlayerVisibility = Visibility.Collapsed;
        private Visibility _goalkeeperVisibility = Visibility.Collapsed;
        #endregion

        #region Properties
        public Player Player
        {
            get => _player;
            set
            {
                _player = value;
                RaisePropertyChanged();
            }
        }

        public Visibility FieldPlayerVisibility
        {
            get => _fieldPlayerVisibility;
            set
            {
                _fieldPlayerVisibility = value;
                RaisePropertyChanged();
            }
        }

        public Visibility GoalkeeperVisibility
        {
            get => _goalkeeperVisibility;
            set
            {
                _goalkeeperVisibility = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PlayerStatGraphView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Player player))
                return;

            Player = player;

            FieldPlayerVisibility = player.Goalkeeping.IsGoalkeepper() ? Visibility.Collapsed : Visibility.Visible;
            GoalkeeperVisibility = player.Goalkeeping.IsGoalkeepper() ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion
    }
}
