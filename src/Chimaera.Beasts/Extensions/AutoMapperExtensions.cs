using System.Linq;
using AutoMapper;

namespace Chimaera.Beasts.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            TypeMap existingMaps = Mapper.GetAllTypeMaps().First(x => x.SourceType.Equals(typeof(TSource)) && x.DestinationType.Equals(typeof(TDestination)));

            foreach (string property in existingMaps.GetUnmappedPropertyNames())
                expression.ForMember(property, opt => opt.Ignore());

            return expression;
        }
    }
}