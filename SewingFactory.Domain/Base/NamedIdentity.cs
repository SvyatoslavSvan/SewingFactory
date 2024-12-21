using SewingFactory.Common.Domain.Exceptions;

namespace SewingFactory.Common.Domain.Base
{
    /// <summary>
    /// NamedIdentity dictionary for selector
    /// </summary>
    public abstract class NamedIdentity : Identity
    {
        private string _name = null!;

        protected NamedIdentity(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Entity name
        /// </summary>
        public virtual string Name
        {
            get => _name;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new BizSuiteArgumentNullException(nameof(value));
                }
                _name = value;
            } 
        }
    }
}