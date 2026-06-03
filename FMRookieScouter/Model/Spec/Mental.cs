using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Spec
{
    public class Mental : IXElementSerializable, IStat
    {
        #region Properties
        public int Aggression { get; set; } = 0;
        public int Anticipation { get; set; } = 0;
        public int Bravery { get; set; } = 0;
        public int Composure { get; set; } = 0;
        public int Concentration { get; set; } = 0;
        public int Decisions { get; set; } = 0;
        public int Determination { get; set; } = 0;
        public int Flair { get; set; } = 0;
        public int Leadership { get; set; } = 0;
        public int OffTheBall { get; set; } = 0;
        public int Positioning { get; set; } = 0;
        public int Teamwork { get; set; } = 0;
        public int Vision { get; set; } = 0;
        public int WorkRate { get; set; } = 0;
        #endregion

        #region Functions
        public void Load(XElement element) => this.LoadSpec(element);
        public XElement Save() => this.SaveSpec();
        #endregion
    }
}
