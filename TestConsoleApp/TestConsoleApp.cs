using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data;

class TestConsoleApp
{
    static void Main()
    {
        string startSearchTimeRead = "08:20:00";
        string endSearchTimeRead = "08:20:00";

        string startSearchDataRead = "01.06.2016";
        string endSearchDataRead = "01.07.2016";

        string startSearchData = Convert.ToDateTime(startSearchDataRead.Trim())
            .ToString("dd.MM.yyy HH:mm:ss");
        string endSearchData = Convert.ToDateTime(endSearchDataRead.Trim())
            .ToString("dd.MM.yyy HH:mm:ss");

        string searchUsername = "петър георгиев георгиев"
            .Trim()
            .ToUpper();

        string startSearchTime = Convert.ToDateTime(startSearchTimeRead.Trim())
            .ToString("HH:mm:ss");
        string endSearchTime = Convert.ToDateTime(endSearchTimeRead.Trim())
            .ToString("HH:mm:ss");


        EventRecord eventRecord = EventRecordRead(startSearchTime, endSearchTime);

        Card card = CardRead();

        Employee employee = EmploedRead();

        PrintConsole printResult = new PrintConsole();



        foreach (var itemEven in eventRecord.CardLow)
        {
            if (!card.CardLow.Contains(itemEven))
            {
                continue;
            }

            foreach (var itemCard in card.CardLow)
            {
                if (itemCard != itemEven)
                {
                    continue;
                }

                foreach (var itemEmployeeID in employee.EmployeeID)
                {

                    if (!card.EmployeeID.Contains(itemEmployeeID))
                    {
                        continue;
                    }



                }


            }


            //Console.WriteLine(itemCard);
            Console.WriteLine(card.CardLow);
        }



        Console.WriteLine();
        /*
        var employeeCard = new Dictionary<string, List<Employee>>();

        foreach (var kvp in employee)
        {
            if (!employeeCard.ContainsKey(kvp.Key))
            {
                employeeCard[kvp.Key] = new List<Employee>();
            }
            
            foreach (var kvpCard in card)
            {

                if (kvp.Value == kvpCard.Key)
                {
                    employeeCard[kvp.Key].Add(new Employee
                    {
                        EmployeeID = kvpCard.Value
                    });
                }
            }            
        }
        

        foreach (var kvp in employeeCard)
        {
            Console.WriteLine($"*Име: {kvp.Key}");
            foreach (var item in kvp.Value)
            {
                Console.WriteLine("--Номер на карта: " + string.Join(", ", item.EmployeeID));
            }
        }
        */
    }

    public static EventRecord EventRecordRead(string startSearchTime, string endSearchTime)
    {
        //query = "SELECT AriseTime, CardLow FROM EventRecord WHERE AriseTime Between #03/01/2016 00:00:00# And #04/01/2016 00:00:00#";
        string query = "SELECT `AriseTime`, `CardLow`, `CtrlID`, `EventType` FROM `EventRecord` WHERE ((TimeValue(AriseTime) Between #" + startSearchTime + "# And #" + endSearchTime + "#))";
        ConnectMDB myDataTable = new ConnectMDB(query);

        EventRecord eventRecord = new EventRecord(
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<DateTime>("AriseTime"))
            .ToList(),
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<int>("CardLow"))
            .ToList(),
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<int>("CtrlID"))
            .ToList(),
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<byte>("EventType"))
            .ToList()
            );
        return eventRecord;
    }

    public static Card CardRead()
    {
        string query = "SELECT `EmployeeID`, `CardLow` FROM `Card`";
        ConnectMDB myDataTable = new ConnectMDB(query);

        Card card = new Card(
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<int>("EmployeeID"))
            .ToList(),
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<int>("CardLow"))
            .ToList()
            );
        return card;
    }

    public static Employee EmploedRead()
    {
        string query = "SELECT `EmployeeID`, `EmployeeName` FROM `Employee`";
        ConnectMDB myDataTable = new ConnectMDB(query);

        Employee emploedRead = new Employee(
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<string>("EmployeeName"))
            .ToList(),
            myDataTable.ConnectDB()
            .AsEnumerable()
            .Select(r => r.Field<int>("EmployeeID"))
            .ToList()
            );
        return emploedRead;
    }
}