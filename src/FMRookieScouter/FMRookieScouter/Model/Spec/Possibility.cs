using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Spec
{
    public class Possibility : IXElementSerializable, IStat
    {
        #region Properties
        public int Ability { get; set; } = 0;
        public int Potential { get; set; } = 0;
        #endregion

        #region Functions
        public void Load(XElement element) => this.LoadSpec(element);
        public XElement Save() => this.SaveSpec();
        #endregion
    }
}
