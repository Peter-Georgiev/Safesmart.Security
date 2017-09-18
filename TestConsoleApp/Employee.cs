using System.Collections.Generic;

class Employee
{
    public List<int> EmployeeID { get; set; }

    public List<string> EmployeeName { get; set; }

    public Employee(List<string> employeeName, List<int> employeeID)
    {
        this.EmployeeName = employeeName;

        this.EmployeeID = employeeID;
    }
}