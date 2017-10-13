using System;
using System.Collections.Generic;

class Employee
{
   // public List<int> EmployeeID { get; set; }

    public string DepartmentName { get; set; }

    public string EmployeeName { get; set; }
    /*

    public void Set(int employeeID, List<string> employeeName)
    {
        if (employe.ContainsKey(employeeID))
        {
            employe[employeeID] = employeeName;
        }
        else
        {
            employe.Add(employeeID, employeeName);
        }
    }

    public List<string> Get(int employeeID)
    {
        List<string> result = new List<string>();

        if (employe.ContainsKey(employeeID))
        {
            result = employe[employeeID];
        }

        return result;
    }

    */







    /*
    Dictionary<int, string> employe = new Dictionary<int, string>();
     
    public void SetDict(Dictionary<int, string> dict)
    {
        employe = dict;
    }

    public void Set(int employeeID, string employeeName)
    {
        if (employe.ContainsKey(employeeID))
        {
            employe[employeeID] = employeeName;
        }
        else
        {
            employe.Add(employeeID, employeeName);
        }
    }

    public string Get(int employeeID)
    {
        string result = String.Empty;

        if (employe.ContainsKey(employeeID))
        {
            result = employe[employeeID];
        }

        return result;
    }
    */
}