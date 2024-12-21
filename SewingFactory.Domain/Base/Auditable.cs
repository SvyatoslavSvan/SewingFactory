namespace SewingFactory.Common.Domain.Base
{
    public abstract class Auditable : Identity, IAuditable
    {
        private DateTime _createdAt;
        private string _createdBy = null!;
        private DateTime? _updatedAt;
        private string? _updatedBy;

        protected Auditable(DateTime createdAt, string createdBy)
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
        }
        /// <summary>
        /// DateTime when entity created.
        /// It's never changed
        /// </summary>
        public DateTime CreatedAt
        {
            get => _createdAt;
            set
            {
                if (value == default)
                {
                    throw new InvalidOperationException("CreatedAt must be set to a valid date.");
                }
                _createdAt = value;
            }
        }

        /// <summary>
        /// User name who created entity.
        /// It's never changed
        /// </summary>
        public string CreatedBy
        {
            get => _createdBy;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("CreatedBy must not be null or empty.");
                }
                _createdBy = value;
            }
        }

        /// <summary>
        /// Last date entity updated
        /// </summary>
        public DateTime? UpdatedAt
        {
            get => _updatedAt;
            set
            {
                if (value.HasValue && value <= CreatedAt)
                {
                    throw new InvalidOperationException("UpdatedAt must be greater than CreatedAt.");
                }
                _updatedAt = value;
            }
        }

        /// <summary>
        /// Author of last updated
        /// </summary>
        public string? UpdatedBy
        {
            get => _updatedBy;
            set
            {
                if (UpdatedAt.HasValue && string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("UpdatedBy must not be null or empty when UpdatedAt is set.");
                }
                _updatedBy = value;
            }
        }

    }

}