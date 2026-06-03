using FMRookieScouter.Interface;
using FMRookieScouter.Model.Information;
using FMRookieScouter.Model.Spec;
using System.Xml.Linq;

namespace FMRookieScouter.Model
{
    public class Player : IXElementSerializable
    {
        #region Properties
        public Common Common { get; set; } = new Common();
        public Belong Belong { get; set; } = new Belong();
        public Part Part { get; set; } = new Part();
        public Possibility Possibility { get; set; } = new Possibility();
        public Mental Mental { get; set; } = new Mental();
        public Physical Physical { get; set; } = new Physical();
        public Technical Technical { get; set; } = new Technical();
        public Goalkeeping Goalkeeping { get; set; } = new Goalkeeping();
        #endregion

        #region Functions
        public void Load(XElement element)
        {
            if (element.Name != nameof(Player))
                return;

            var properties = typeof(Player).GetProperties();

            foreach (var property in properties)
            {
                var child = element.Element(property.Name);
                var value = property.GetValue(this);

                if (value is IXElementSerializable serializable && child != null)
                    serializable.Load(child);
            }
        }

        public XElement Save()
        {
            var element = new XElement(nameof(Player));
            var properties = typeof(Player).GetProperties();

            foreach(var property in properties)
            {
                var value = property.GetValue(this);

                if (value is IXElementSerializable serializable)
                    element.Add(serializable.Save());
            }

            return element;
        }


        public override string ToString() => $"{Common.Name}";

        public string GetID() => $"{Common.Name} {Belong.Club} {Belong.Nation}";
        #endregion
    }
}
