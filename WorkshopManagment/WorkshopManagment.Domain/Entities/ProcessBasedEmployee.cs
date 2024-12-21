using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    /// <summary>
    /// Represents an employee in the workshop who is paid based on process outcomes.
    /// </summary>
    public class ProcessBasedEmployee : Employee
    {
        private int _premium;
        private List<WorkshopDocument> _documents= null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessBasedEmployee"/> class.
        /// </summary>
        /// <param name="name">The name of the employee.</param>
        /// <param name="internalId">The unique internal identifier for the employee.</param>
        /// <param name="department"></param>
        /// <param name="documents"></param>
        /// <param name="premium">The premium percentage for the employee. Defaults to 0.</param>
        /// <exception cref="BizSuiteArgumentNullException">Thrown when <paramref name="internalId"/> is null or invalid.</exception>
        public ProcessBasedEmployee(string name, string internalId, Department department, List<WorkshopDocument> documents, int premium = 0) : base(name, internalId, department)
        {
            Premium = premium;
            Documents = documents;
        }

        /// <summary>
        /// Gets or sets the premium percentage for the employee.
        /// </summary>
        /// <exception cref="BizSuiteArgumentException">Thrown when the value is not between 0 and 100.</exception>
        public int Premium
        {
            get => _premium;
            set
            {
                if (value is < 0 or > 100)
                {
                    throw new BizSuiteArgumentException(nameof(value), "The premium must be between 0 and 100.");
                }
                _premium = value;
            }
        }

        public List<WorkshopDocument> Documents
        {
            get => _documents;
            set => _documents = value ?? throw new BizSuiteArgumentNullException(nameof(Documents));
        }


        public override Salary CalculateSalary(DateRange dateRange)
        {
            var documents = _documents.Where(document => dateRange.Contains(document.Date)); //TODO: contains
            var documentsPayment = new Money(0);
            documentsPayment = documents.Aggregate(documentsPayment, (current, workshopDocument) => current + workshopDocument.CalculatePaymentForEmployee(this));
            return new Salary(documentsPayment, documentsPayment * _premium, this);
        }
    }
}
