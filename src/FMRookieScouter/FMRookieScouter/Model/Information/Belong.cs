using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Information
{
    public class Belong : IXElementSerializable
    {
        #region Properties
        public string Club { get; set; } = "";
        public string Nation { get; set; } = "";
        #endregion

        #region Functions
        public void Load(XElement element)
        {
            if (element.Name != nameof(Belong))
                return;

            if(element.TryGetAttributeValue(nameof(Club), out string club))
                Club = club;
            if(element.TryGetAttributeValue(nameof(Nation), out string nation))
                Nation = nation;   
        }

        public XElement Save()
        {
            var element = new XElement(nameof(Belong));

            if (!string.IsNullOrEmpty(Club))
                element.Add(new XAttribute(nameof(Club), Club));
            if (!string.IsNullOrEmpty(Nation))
                element.Add(new XAttribute(nameof(Nation), Nation));

            return element;
        }
        #endregion
    }
}
