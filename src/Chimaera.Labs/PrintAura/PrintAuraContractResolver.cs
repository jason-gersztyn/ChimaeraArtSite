using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chimaera.Labs.PrintAura
{
    public class PrintAuraContractResolver : SnakeCasePropertyNamesContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if(property.PropertyType.IsEnum)
            {
                property.ShouldSerialize = x =>
                {
                    var prop = property.DeclaringType.GetProperty(property.UnderlyingName);

                    string value = prop.GetValue(x).ToString();

                    return value.ToLower() != "unknown";
                };
            }

            return property;
        }
    }
}