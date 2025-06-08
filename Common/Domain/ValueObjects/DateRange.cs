using System.Linq.Expressions;
using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Common.Domain.ValueObjects;

public sealed class DateRange
{
    private DateOnly _end;
    private DateOnly _start;

    public DateRange(DateOnly start, DateOnly end)
    {
        if (start > end) throw new SewingFactoryArgumentException("Start date cannot be later than the end date.");

        _start = start;
        _end = end;
    }

    public DateOnly Start
    {
        get => _start;
        set
        {
            if (value > _end)
                throw new SewingFactoryArgumentException(nameof(Start),
                    "Start date cannot be later than the end date.");
            _start = value;
        }
    }

    public DateOnly End
    {
        get => _end;
        set
        {
            if (value < _start)
                throw new SewingFactoryArgumentException(nameof(End),
                    "End date cannot be earlier than the start date.");
            _end = value;
        }
    }

    public bool Contains(DateOnly date)
    {
        return date >= _start && date <= _end;
    }

    public override string ToString() => _start + " - " + _end;
}