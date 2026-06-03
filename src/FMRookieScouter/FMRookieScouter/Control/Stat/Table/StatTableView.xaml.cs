using FMRookieScouter.Interface;
using FMRookieScouter.Item;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.Stat.Table
{
    /// <summary>
    /// StatTableView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatTableView : UserControl, INotifyPropertyChanged
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
        private ObservableCollection<StatUnitItem> _items = new ObservableCollection<StatUnitItem>();
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

        public ObservableCollection<StatUnitItem> Items
        {
            get => _items;
            set
            {
                _items = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public StatTableView()
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
            Items = new ObservableCollection<StatUnitItem>(stat.GetItems().ToList());
        }
        #endregion
    }
}
