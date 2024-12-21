using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Common.Domain.ValueObjects
{
    public class DateRange
    {
        private DateOnly _start;
        private DateOnly _end;

        public DateRange(DateOnly start, DateOnly end)
        {
            if (start > end)
            {
                throw new BizSuiteArgumentException("Start date cannot be later than the end date.");
            }

            _start = start;
            _end = end;
        }

        public DateOnly Start
        {
            get => _start;
            set
            {
                if (value > _end)
                {
                    throw new BizSuiteArgumentException(nameof(Start),"Start date cannot be later than the end date.");
                }
                _start = value;
            }
        }

        public DateOnly End
        {
            get => _end;
            set
            {
                if (value < _start)
                {
                    throw new BizSuiteArgumentException(nameof(End), "End date cannot be earlier than the start date.");
                }
                _end = value;
            }
        }

        public bool Contains(DateOnly date) => date >= _start && date <= _end;
    }

}
