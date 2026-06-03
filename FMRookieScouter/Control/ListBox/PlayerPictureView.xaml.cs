using FMRookieScouter.Helper;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.ListBox
{
    /// <summary>
    /// PlayerPictureView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PlayerPictureView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private string _imageSource;
        #endregion

        #region Properties
        public string ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public PlayerPictureView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is string name))
                return;

            ImageSource = $"/FMRookieScouter;component/Image/Picture/{name.TrimEnglish()}.png";
        }
        #endregion
    }
}
