using FMRookieScouter.Interface;
using System;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Information
{
    public class Role : IXElementSerializable
    {
        #region Properties
        public eRole Type { get; set; }
        public double Value { get; set; }
        #endregion

        #region Functions
        public override string ToString() => $"{Type} ({Value})";

        public void Load(XElement element)
        {
            if (element.Name != nameof(Role))
                return;

            if (element.TryGetAttributeEnumValue(nameof(Type), out eRole type))
                Type = type;
            if (element.TryGetAttributeDoubleValue(nameof(Value), out double value))
                Value = value;
        }

        public XElement Save()
        {
            var element = new XElement(nameof(Role));

            element.Add(
                new XAttribute(nameof(Type), Type),
                new XAttribute(nameof(Value), Value));

            return element;
        }
        #endregion
    }
}
