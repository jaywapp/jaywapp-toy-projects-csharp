using FMRookieScouter.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace FMRookieScouter.Control.ListBox
{
    /// <summary>
    /// PlayerInformationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerInformationView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private string _displayName;
        #endregion

        #region Properties
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PlayerInformationView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is Player player))
                return;

            DisplayName = $"{player.Common.Name} ({player.Common.Age})";
        }
        #endregion
    }
}
