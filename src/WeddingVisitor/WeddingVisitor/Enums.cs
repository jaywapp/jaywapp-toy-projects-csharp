using System.ComponentModel;

namespace WeddingVisitor
{
    public enum eHost
    {
        [Description("모름")]
        None,
        [Description("신랑")]
        Groom,
        [Description("신부")]
        Bride,
    }

    public enum eRelationship
    {
        [Description("모름")]
        None,
        [Description("가족")]
        Family,
        [Description("친구")]
        Friend,
        [Description("직장 동료")]
        Colleagues,
        [Description("지인")]
        Acquaintance

    }
}
