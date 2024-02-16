using ReportingService.Data.Domain;
using System.Text;

namespace ReportService.Data.Domain
{
    public class ReportFormatter
    {
        private readonly StringBuilder stringBuilder;
        public ReportFormatter()
        {
            stringBuilder = new StringBuilder();
        }

        public void AddNewLine() => stringBuilder.Append(Environment.NewLine);
        public void AddWordLine() => stringBuilder.Append("--------------------------------------------");
        public void AddWordTab() => stringBuilder.Append("         ");
        public void AddEmployeeName(Employee employee) => stringBuilder.Append(employee.Name);
        public void AddSalary(Employee employee) => stringBuilder.Append(employee.Salary + "р");
        public void AddSalarySum(int salarySum) => stringBuilder.Append(salarySum + "р");
        public void AddWord(string word) => stringBuilder.Append(word);

        public string Build() => stringBuilder.ToString();
    }
}
