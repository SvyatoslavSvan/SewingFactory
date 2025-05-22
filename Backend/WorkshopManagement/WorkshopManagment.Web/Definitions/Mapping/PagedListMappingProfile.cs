using AutoMapper;
using Calabonga.PagedListCore;

namespace SewingFactory.Backend.WorkshopManagement.Web.Definitions.Mapping;

public class PagedListMappingProfile : Profile
{
    public PagedListMappingProfile() =>
        CreateMap(typeof(IPagedList<>), typeof(IPagedList<>))
            .ConvertUsing(typeof(PagedListConverter<,>));
}