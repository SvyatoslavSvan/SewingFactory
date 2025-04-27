using AutoMapper;
using Calabonga.PagedListCore;

namespace SewingFactory.Backend.WorkshopManagement.Web.Application.Messaging.Mapping.Converters;

public class PagedListConverter<TSource, TDestination>
    : ITypeConverter<
        PagedList<TSource>,
        IPagedList<TDestination>>
{
    public IPagedList<TDestination> Convert(
        PagedList<TSource> source,
        IPagedList<TDestination> destination,
        ResolutionContext context)
    {

        var sourceList = source.Items as List<TSource>
                         ?? [.. source.Items];


        var mappedItems = sourceList
            .ConvertAll(converter: item => context.Mapper.Map<TDestination>(item));


        return new PagedList<TDestination>(
            mappedItems,
            source.PageIndex,
            source.PageSize,
            source.IndexFrom,
            source.TotalCount
        );
    }
}