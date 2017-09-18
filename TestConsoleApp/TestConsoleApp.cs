using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data;

class TestConsoleApp
{
    static void Main()
    {
        string startSearchTimeRead = "08:00:00";
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

        int count = 0;
        foreach (var key in eventRecord.CardLow)
        {
            Console.WriteLine($"** {card.Get(key)}," +
                $" {eventRecord.AriseTime[count]}," +
                $" {eventRecord.CtrlID[count]}," +
                $" {eventRecord.EventType[count]}");
        }



        PrintConsole printResult = new PrintConsole();



        Console.WriteLine();
    }

    public static EventRecord EventRecordRead(string startSearchTime, string endSearchTime)
    {
        //query = "SELECT AriseTime, CardLow FROM EventRecord WHERE AriseTime Between #03/01/2016 00:00:00# And #04/01/2016 00:00:00#";
        string query = "SELECT `AriseTime`, `CardLow`, `CtrlID`, `EventType` FROM `EventRecord` WHERE AriseTime Between #06/01/2017 00:00:00# And #06/06/2017 23:00:00# AND ((TimeValue(AriseTime) Between #" + startSearchTime + "# And #" + endSearchTime + "#))";
        ConnectMDB myDataTable = new ConnectMDB(query);

        EventRecord eventRecord = new EventRecord
        {
            CardLow = myDataTable.ConnectDB()
                .AsEnumerable()
                .Select(r => r.Field<int>("CardLow"))
                .ToList(),
            AriseTime = myDataTable.ConnectDB()
                .AsEnumerable()
                .Select(r => r.Field<DateTime>("AriseTime"))
                .ToList(),
            CtrlID = myDataTable.ConnectDB()
                .AsEnumerable()
                .Select(r => r.Field<int>("CtrlID"))
                .ToList(),
            EventType = myDataTable.ConnectDB()
                .AsEnumerable()
                .Select(r => r.Field<byte>("EventType"))
                .ToList()
        };

        return eventRecord;
    }

    public static Card CardRead()
    {
        string query = "SELECT `EmployeeID`, `CardLow` FROM `Card`";
        ConnectMDB myDataTable = new ConnectMDB(query);

        Card card = new Card();
        card.SetDict(myDataTable
            .ConnectDB()
            .AsEnumerable()
            .ToDictionary(
            r => r.Field<int>("CardLow"),
            r => r.Field<int>("EmployeeID"))
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