using FMRookieScouter.Interface;
using FMRookieScouter.Item;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace FMRookieScouter.Control.Stat.Graph
{
    /// <summary>
    /// StatGraphView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StatGraphView : UserControl, INotifyPropertyChanged
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

        private ISeries[] _series;
        private PolarAxis[] _angleAxes;
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

        public ISeries[] Series
        {
            get => _series;
            set
            {
                _series = value;
                RaisePropertyChanged();
            }
        }

        public PolarAxis[] AngleAxes
        {
            get => _angleAxes;
            set
            {
                _angleAxes = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public StatGraphView()
        {
            InitializeComponent();
        }
        #endregion

        #region Functions
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IStat stat))
                return;

            var items = stat.GetItems().ToList();

            Title = stat.GetType().Name;

            Series = CreateSeries(items).ToArray();
            AngleAxes = CreateAxis(items).ToArray();
        }

        private static IEnumerable<ISeries> CreateSeries(List<StatUnitItem> items)
        {
            yield return new PolarLineSeries<int>
            {
                Values = items.Select(i => i.Value).ToArray(),
                LineSmoothness = 0,
                GeometrySize = 0,
                Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(90)),
            };
        }

        private static IEnumerable<PolarAxis> CreateAxis(List<StatUnitItem> items)
        {
            yield return new PolarAxis
            {
                LabelsRotation = LiveCharts.TangentAngle,
                Labels = items.Select(i => i.Name).ToArray(),
            };
        }
        #endregion
    }
}
