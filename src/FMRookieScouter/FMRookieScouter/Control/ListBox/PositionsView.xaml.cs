using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.ListBox
{
    /// <summary>
    /// PositionsView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PositionsView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private string _displayPositions;
        #endregion

        #region Properties
        public string DisplayPositions
        {
            get => _displayPositions;
            set
            {
                _displayPositions = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PositionsView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is List<ePosition> positions))
                return;

            DisplayPositions = string.Join(" / ", positions);
        }
        #endregion
    }
}
