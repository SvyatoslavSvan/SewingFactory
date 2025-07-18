﻿namespace SewingFactory.Common.Domain.Base;

/// <summary>
///     ViewModelBase for WritableController
/// </summary>
public class ViewModelBase : IViewModel, IHaveId
{
    /// <summary>
    ///     Identifier
    /// </summary>
    public Guid Id { get; set; }
}