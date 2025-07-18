﻿using AutoMapper;
using Calabonga.PagedListCore;

namespace SewingFactory.Backend.IdentityServer.Web.Definitions.Mapping;

/// <summary>
///     Generic converter for IPagedList collections
/// </summary>
/// <typeparam name="TMapFrom"></typeparam>
/// <typeparam name="TMapTo"></typeparam>
public class PagedListConverter<TMapFrom, TMapTo> : ITypeConverter<IPagedList<TMapFrom>, IPagedList<TMapTo>>
{
    /// <summary>Performs conversion from source to destination type</summary>
    /// <param name="source">Source object</param>
    /// <param name="destination">Destination object</param>
    /// <param name="context">Resolution context</param>
    /// <returns>Destination object</returns>
    public IPagedList<TMapTo> Convert(IPagedList<TMapFrom>? source, IPagedList<TMapTo> destination, ResolutionContext context) =>
        source == null
            ? PagedList.Empty<TMapTo>()
            : PagedList.From(source, converter: items => context.Mapper.Map<IEnumerable<TMapTo>>(items)!);
}
