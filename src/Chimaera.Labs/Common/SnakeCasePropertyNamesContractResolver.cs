using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Chimaera.Labs
{
    public class SnakeCasePropertyNamesContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Regex.Replace(propertyName, "([a-z])([A-Z])", "$1_$2").ToLower();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var propertyAttribute = Attribute.GetCustomAttribute(member, typeof(JsonPropertyAttribute), true) as JsonPropertyAttribute;

            if (propertyAttribute != null && !string.IsNullOrEmpty(propertyAttribute.PropertyName))
            {
                property.PropertyName = propertyAttribute.PropertyName;
            }

            return property;
        }
    }
}