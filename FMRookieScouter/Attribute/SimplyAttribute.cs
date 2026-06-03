namespace FMRookieScouter.Attribute
{
    public class SimplyAttribute : System.Attribute
    {
        public string Value { get; }

        public SimplyAttribute(string value)
        {
            Value = value;
        }
    }
}
