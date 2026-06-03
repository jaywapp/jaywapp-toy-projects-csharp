using FMRookieScouter.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FMRookieScouter.Service.Filter
{
    public class PlayerFilter
    {
        #region Internal Field
        private string _namePattern;
        private List<ePosition> _positionList = new List<ePosition>();
        private List<eRole> _roleList = new List<eRole>();
        #endregion

        #region Properties
        public string NamePattern
        {
            get => _namePattern;
            set
            {
                _namePattern = value;
                ConditionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public List<ePosition> Positions
        {
            get => _positionList;
            set
            {
                _positionList = value;
                ConditionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public List<eRole> Roles
        {
            get => _roleList;
            set
            {
                _roleList = value;
                ConditionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Event
        public event EventHandler ConditionChanged;
        #endregion

        #region Functions
        public IEnumerable<Player> Filtering(IEnumerable<Player> sources)
        {
            var nameFiltered = FilteringNames(sources);
            var roleFiltered = FilteringRoles(sources);
            var positionFiltered = FilteringPositions(sources);

            return nameFiltered.Intersect(roleFiltered).Intersect(positionFiltered);
        }

        private IEnumerable<Player> FilteringNames(IEnumerable<Player> sources)
        {
            var result = sources.ToList();

            if (!string.IsNullOrEmpty(NamePattern))
            {
                result = result
                    .Where(s => s.Common.Name.Contains(NamePattern))
                    .ToList();
            }

            return result;
        }

        private IEnumerable<Player> FilteringRoles(IEnumerable<Player> sources)
        {
            var result = sources.ToList();

            if (Roles != null && Roles.Any())
            {
                result = result
                    .Where(s =>
                    {
                        var roles = s.Part.Roles.Select(r => r.Type).ToList();
                        return roles.Intersect(Roles).Any();
                    })
                    .ToList();
            }

            return result;
        }

        private IEnumerable<Player> FilteringPositions(IEnumerable<Player> sources)
        {
            var result = sources.ToList();

            if (Positions != null && Positions.Any())
            {
                result = result
                    .Where(s => s.Part.Positions.Intersect(Positions).Any())
                    .ToList();
            }

            return result;
        }
        #endregion
    }
}