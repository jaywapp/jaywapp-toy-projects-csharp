using FMRookieScouter.Service.Filter;
using Prism.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace FMRookieScouter.View
{
    public class FilterViewModel : ReactiveObject
    {
        #region Internal Field
        private IEnumerable<object> _selectedPositions;
        private IEnumerable<object> _selectedRoles;
        #endregion

        #region Properties
        public PlayerFilter Filter { get; }

        public List<object> Positions { get; set; }
        public List<object> Roles { get; set; }

        public IEnumerable<object> SelectedPositions
        {
            get => _selectedPositions;
            set => this.RaiseAndSetIfChanged(ref _selectedPositions, value);
        }

        public IEnumerable<object> SelectedRoles
        {
            get => _selectedRoles;
            set => this.RaiseAndSetIfChanged(ref _selectedRoles, value);
        }
        #endregion

        #region Event
        public DelegateCommand<TextChangedEventArgs> OnTextChangedCommand { get; }
        #endregion

        #region Constructor
        public FilterViewModel(PlayerFilter filter)
        {
            Filter = filter;
            Positions = GetValues<ePosition>();
            Roles = GetValues<eRole>();

            OnTextChangedCommand = new DelegateCommand<TextChangedEventArgs>(OnTextChanged);

            this.WhenAnyValue(x => x.SelectedPositions)
                .Subscribe(OnSelectedPositionsChanged);

            this.WhenAnyValue(x => x.SelectedRoles)
                .Subscribe(OnSelectedRolesChanged);
        }
        #endregion

        #region Functions
        private void OnSelectedRolesChanged(IEnumerable<object> roles)
        {
            if (roles == null)
                return;

            Filter.Roles = roles.OfType<eRole>().ToList();
        }

        private void OnSelectedPositionsChanged(IEnumerable<object> positions)
        {
            if (positions == null)
                return;

            Filter.Positions = positions.OfType<ePosition>().ToList();
        }

        private void OnTextChanged(TextChangedEventArgs args)
        {
            if (!(args.Source is TextBox textBox))
                return;

            Filter.NamePattern = textBox.Text;
        }

        private static List<object> GetValues<TEnum>()
            where TEnum : struct, IConvertible
        {
            var values = new List<object>();

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
                values.Add(value);

            return values;

        }
        #endregion
    }
}
