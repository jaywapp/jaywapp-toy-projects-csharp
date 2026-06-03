using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Spec
{
    public class Physical : IXElementSerializable, IStat
    {
        #region Properties
        public int Acceleration { get; set; } = 0;
        public int Agility { get; set; } = 0;
        public int Balance { get; set; } = 0;
        public int JumpingReach { get; set; } = 0;
        public int NaturalFitness { get; set; } = 0;
        public int Pace { get; set; } = 0;
        public int Stamina { get; set; } = 0;
        public int Strength { get; set; } = 0;
        #endregion

        #region Functions
        public void Load(XElement element) => this.LoadSpec(element);
        public XElement Save() => this.SaveSpec();
        #endregion
    }
}
