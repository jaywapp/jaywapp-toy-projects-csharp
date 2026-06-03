using FMRookieScouter.Interface;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.Stat
{
    /// <summary>
    /// StatView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private string _title;
        #endregion

        #region Properties
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public StatView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IStat stat))
                return;

            Title = stat.GetType().Name;
        }
        #endregion
    }
}
