using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Information
{
    public class Common : IXElementSerializable
    {
        #region Properties
        public string Name { get; set; } = "";
        public int Age { get; set; } = 0;
        public double Length { get; set; } = 0;
        public double Weight { get; set; } = 0;
        public eFoot Foot { get; set; }
        #endregion

        #region Functions
        public override string ToString() => $"Age : {Age} / L : {Length} / W : {Weight} / Foot : {Foot}";

        public XElement Save()
        {
            var element = new XElement(nameof(Common));

            element.Add(
                new XAttribute(nameof(Name), Name ?? ""),
                new XAttribute(nameof(Age), Age),
                new XAttribute(nameof(Length), Length),
                new XAttribute(nameof(Weight), Weight),
                new XAttribute(nameof(Foot), Foot));

            return element;
        }

        public void Load(XElement element)
        {
            if (element.Name != nameof(Common))
                return;

            if (element.TryGetAttributeValue(nameof(Name), out string name))
                Name = name;
            if (element.TryGetAttributeIntValue(nameof(Age), out int age))
                Age = age;
            if (element.TryGetAttributeDoubleValue(nameof(Length), out double length))
                Length = length;
            if (element.TryGetAttributeDoubleValue(nameof(Weight), out double weight))
                Weight = weight;
            if (element.TryGetAttributeEnumValue(nameof(Foot), out eFoot foot))
                Foot = foot;
        }
        #endregion
    }
}
