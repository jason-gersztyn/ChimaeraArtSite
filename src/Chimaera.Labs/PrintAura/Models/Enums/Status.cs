using System.Runtime.Serialization;

namespace Chimaera.Labs.PrintAura.Models
{
    public enum Status
    {
        Unknown = 0,
        New,
        Processing,
        Pending,
        Canceled,
        Shipped,
        [EnumMember(Value = "On Hold")]
        OnHold
    }
}