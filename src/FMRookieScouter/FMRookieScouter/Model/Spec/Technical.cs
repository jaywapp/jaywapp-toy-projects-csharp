using FMRookieScouter.Interface;
using System.Xml.Linq;

namespace FMRookieScouter.Model.Spec
{
    public class Technical : IXElementSerializable, IStat
    {
        #region Properties
        public int Corners { get; set; } = 0;
        public int Crossing { get; set; } = 0;
        public int Dribbling { get; set; } = 0;
        public int Finishing { get; set; } = 0;
        public int FirstTouch { get; set; } = 0;
        public int FreeKickTaking { get; set; } = 0;
        public int Heading { get; set; } = 0;
        public int LongShots { get; set; } = 0;
        public int LongThrows { get; set; } = 0;
        public int Marking { get; set; } = 0;
        public int Passing { get; set; } = 0;
        public int PenaltyTaking { get; set; } = 0;
        public int Tackling { get; set; } = 0;
        public int Technique { get; set; } = 0;
        #endregion

        #region Functions
        public void Load(XElement element) => this.LoadSpec(element);
        public XElement Save() => this.SaveSpec();
        #endregion
    }
}
