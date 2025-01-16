using SewingFactory.Backend.WorkshopManagement.Domain.Base;
using SewingFactory.Backend.WorkshopManagement.Domain.SalaryReport;
using SewingFactory.Common.Domain.Exceptions;
using SewingFactory.Common.Domain.ValueObjects;

namespace SewingFactory.Backend.WorkshopManagement.Domain.Entities
{
    public class Technologist : Employee
    {
        private int _salaryPercentage;

        public Technologist(string name, string internalId, int salaryPercentage, Department department) : base(name, internalId, department) => SalaryPercentage = salaryPercentage;

        public int SalaryPercentage
        {
            get => _salaryPercentage;
            set
            {
                if (value is < 0 or > 100)
                {
                    throw new SewingFactoryArgumentException(nameof(SalaryPercentage), "SalaryPercentage must be between 0 and 100");
                }
                _salaryPercentage = value;
            }
        }


        public override Salary CalculateSalary(DateRange dateRange) => throw new NotImplementedException();
    }
}
