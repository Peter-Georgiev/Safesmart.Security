using System;
using System.Collections.Generic;

class Employee
{
    Dictionary<int, string> employe = new Dictionary<int, string>();

    public void SetDict(Dictionary<int, string> dict)
    {
        employe = dict;
    }

    public void Set(int EmployeeID, string EmployeeName)
    {
        if (employe.ContainsKey(EmployeeID))
        {
            employe[EmployeeID] = EmployeeName;
        }
        else
        {
            employe.Add(EmployeeID, EmployeeName);
        }
    }

    public string Get(int EmployeeID)
    {
        string result = String.Empty;

        if (employe.ContainsKey(EmployeeID))
        {
            result = employe[EmployeeID];
        }

        return result;
    }
}