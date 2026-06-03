using FMRookieScouter.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace FMRookieScouter.Control.Stat.Table
{
    /// <summary>
    /// PlayerStatTableView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerStatTableView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private UserControl _view;
        #endregion

        #region Properties
        public UserControl View
        {
            get => _view;
            set
            {
                _view = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PlayerStatTableView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Player player))
                return;

            View = GetView(player);
        }

        private static UserControl GetView(Player player)
        {
            if (player.Goalkeeping.IsGoalkeepper())
                return new GoalkeepperPlayerStatTableView() { DataContext = player };
            else
                return new FieldPlayerStatTableView() { DataContext = player };
        }
        #endregion
    }
}
