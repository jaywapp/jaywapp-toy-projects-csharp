using FMRookieScouter.Model.Information;
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

namespace FMRookieScouter.Control.Belong
{
    /// <summary>
    /// RolesView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RolesView : UserControl, INotifyPropertyChanged
    {
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        #region Internal Field
        private string _best;
        private string _worst;

        private ISeries[] _series;
        private Axis[] _yAxes;
        #endregion

        #region Properties
        public string Best
        {
            get => _best;
            set
            {
                _best = value;
                RaisePropertyChanged();
            }
        }

        public string Worst
        {
            get => _worst;
            set
            {
                _worst = value;
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

        public Axis[] YAxes
        {
            get => _yAxes;
            set
            {
                _yAxes = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public RolesView()
        {
            InitializeComponent();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(e.NewValue is IEnumerable<Role> roles))
                return;

            var bestValue = roles.Max(r => r.Value);
            var worstValue = roles.Min(r => r.Value);

            var best = roles.FirstOrDefault(r => r.Value == bestValue);
            var worst = roles.FirstOrDefault(r => r.Value == worstValue);

            Best = best.ToString();
            Worst = worst.ToString();

            Series = CreateSeries(roles, bestValue).ToArray();
            YAxes = CreateYAxes(roles, bestValue).ToArray();
        }

        private static IEnumerable<ISeries> CreateSeries(IEnumerable<Role> roles, double best)
        {
            yield return new ColumnSeries<Role>
            {
                Values = roles.ToArray(),
                Stroke = null,
                Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                IgnoresBarPosition = true,
                TooltipLabelFormatter = point => $"{point.Model?.Type} / {point.Model?.Value}",
                Mapping = (role, point) =>
                {
                    point.PrimaryValue = role.Value;
                    point.SecondaryValue = point.Context.Entity.EntityIndex;
                },
            };
        }

        private static IEnumerable<Axis> CreateYAxes(IEnumerable<Role> roles, double best)
        {
            yield return new Axis 
            {
                MinLimit = 0, 
                MaxLimit = best,
            };
        }
    }
}
