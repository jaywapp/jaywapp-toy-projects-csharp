using FMRookieScouter.Interface;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Information
{
    public class Part : IXElementSerializable
    {
        #region Properties
        public List<ePosition> Positions { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
        #endregion

        #region Functions
        public string GetPositions()
        {
            return string.Join(" / ", Positions);
        }

        public void Load(XElement element)
        {
            if (element.Name != nameof(Part))
                return;

            LoadPositions(element.Element(nameof(Positions)));
            LoadRoles(element.Element(nameof(Roles)));
        }

        public XElement Save()
        {
            var element = new XElement(nameof(Part));

            element.Add(SavePositions());
            element.Add(SaveRoles());

            return element;
        }

        private void LoadPositions(XElement element)
        {
            if (element == null)
                return;
            if (element.Name != nameof(Positions))
                return;

            var positions = new List<ePosition>();
            var children = element.Elements("Position");
            foreach (var child in children)
            {
                if (Enum.TryParse(child.Value, out ePosition position))
                    positions.Add(position);
            }

            Positions = positions;
        }

        private void LoadRoles(XElement element)
        {
            if (element == null)
                return;
            if (element.Name != nameof(Roles))
                return;

            var roles = new List<Role>();

            var children = element.Elements(nameof(Role));
            foreach (var child in children)
            {
                var role = new Role();

                role.Load(child);
                roles.Add(role);
            }

            Roles = roles;
        }

        private XElement SavePositions()
        {
            var element = new XElement(nameof(Positions));

            foreach (var position in Positions)
            {
                var child = new XElement("Position");
                child.Value = position.ToString();

                element.Add(child);
            }

            return element;
        }

        private XElement SaveRoles()
        {
            var element = new XElement(nameof(Roles));

            foreach (var role in Roles)
                element.Add(role.Save());

            return element;
        }
        #endregion
    }
}
