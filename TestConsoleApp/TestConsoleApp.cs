﻿using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using System.Data;
using System.ComponentModel;
using System.IO;
using System.Text;

class TestConsoleApp
{
    static void Main()
    {
        Dictionary<int, string> doorCtrlID = new Dictionary<int, string>
        {
            { 4, "ПОРТАЛ ВЪТРЕШЕН" },
            { 3, "ПОРТАЛ" }
        };

        Dictionary<int, string> doorOrdinal = new Dictionary<int, string>
        {
            { 0, "ИЗХОД"},
            { 1, "ВХОД" }
        };
        
        string startSearchData = StartSearchData();
        string endSearchData = EndSearchData();
        string startSearchTime = StartSearchTime();
        string endSearchTime = EndSearchTime();

        /* TEST
        string searchDataTime = "01.09.2017 07:55:00 - 30.09.2017 10:00:00";
        string[] mathesRegex = MatchInput(searchDataTime);
        string startSearchData = mathesRegex[0];
        string endSearchData = mathesRegex[1];
        string startSearchTime = mathesRegex[2];
        string endSearchTime = mathesRegex[3];
        */
        int ordinalOptional = OrdinalOptional();
        int ctrlIDOptional = 3;
        
        int searchDepartment = SearchDepartment();
        
        string searchUserName = "" //формат име фамилия презиме
            .Trim()
            .ToUpper();

        EventRecord eventRecord =
            EventRecordRead(startSearchData, endSearchData, startSearchTime, endSearchTime, ctrlIDOptional, ordinalOptional);

        Dictionary<int, Employee> cardEmployeeDepart = 
            CardEmployeeDepart(searchUserName,searchDepartment);
                
        List<PrintList> print = new List<PrintList>();
        
        int index = 0;
        foreach (var cardLow in eventRecord.CardLow)
        {
            if (!cardEmployeeDepart.ContainsKey(cardLow))
            {
                index++;
                continue;
            }
            
            PrintList prnName = new PrintList
            {
                EmployeeName = cardEmployeeDepart[cardLow].EmployeeName,
                DateTime = eventRecord.AriseTime[index],
                CtrlID = doorCtrlID[eventRecord.CtrlID[index]],
                Ordinal = doorOrdinal[eventRecord.Ordinal[index]],
                DepartmentName = cardEmployeeDepart[cardLow].DepartmentName
            };

            index++;
            print.Add(prnName);
        }
        
        List<PrintList> printSort = print
        .OrderBy(name => name.EmployeeName)
        .ThenBy(dateTime => dateTime.DateTime)
        .ToList();
        
        ExportToExcel(printSort);
        /*
        using (StreamWriter wr = new StreamWriter("myfile.csv", false, Encoding.UTF8))
        {
            foreach (PrintList p in printSort)
            {
                StringBuilder nameList = new StringBuilder();
                nameList.Append(
                    $"Име: {p.EmployeeName}," +
                    $"{p.DateTime}," +
                    $"{p.CtrlID}," +
                    $"{p.Ordinal}," +
                    $"{p.DepartmentName}");
                wr.WriteLine(nameList);
            }
        }
        */
        //Console.Read();
    }


    public static string StartSearchData()
    {
        string startSearchData = String.Empty;
        while (startSearchData == String.Empty)
        {
            Console.Write("Въведи начална дата (дд.мм.гггг): ");
            startSearchData = ReadLineData(Console.ReadLine());
        }
        return startSearchData;
    }

    public static string EndSearchData()
    {
        string endSearchData = String.Empty;
        while (endSearchData == String.Empty)
        {
            Console.Write("Въведи крайна дата (дд.мм.гггг): ");
            endSearchData = ReadLineData(Console.ReadLine());
        }
        return endSearchData;
    }

    public static string StartSearchTime()
    {
        string startSearchTime = String.Empty;
        while (startSearchTime == String.Empty)
        {
            Console.Write("Въведи начален час (час:минути:секунди): ");
            startSearchTime = ReadLineTime(Console.ReadLine());
        }
        return startSearchTime;
    }

    public static string EndSearchTime()
    {
        string endSearchTime = String.Empty;
        while (endSearchTime == String.Empty)
        {
            Console.Write("Въведи краен час (час:минути:секунди): ");
            endSearchTime = ReadLineTime(Console.ReadLine());
        }
        return endSearchTime;
    }

    public static int OrdinalOptional()
    {
        int ordinalOptional = -1;
        while (ordinalOptional == -1)
        {
            Console.Write("Въведи '0' за ИЗХОД или '1' за ВХОД: ");
            string readLineStr = Console.ReadLine();

            if (!int.TryParse(readLineStr, out int readLineDigit))
            {
                Console.WriteLine("Въведи число!");
                continue;
            }

            if (!(readLineDigit > -1 && readLineDigit < 2))
            {
                Console.WriteLine("Некоректно въведено число!");
                continue;
            }
            ordinalOptional = readLineDigit;
        }
        return ordinalOptional;
    }

    public static int SearchDepartment()
    {
        int searchDepartment = 0;
        while (searchDepartment == 0)
        {
            int lastKey = 0;
            foreach (var kvp in Department())
            {
                Console.WriteLine($"Номер: {kvp.Key} за {kvp.Value}");
                lastKey = kvp.Key;
            }
            Console.Write("Избери номер: ");
            string readLineStr = Console.ReadLine();

            if (!int.TryParse(readLineStr, out int readLineDigit))
            {
                Console.Write("Въведи число!");
                continue;
            }

            if (!(readLineDigit > 0 && readLineDigit < lastKey + 1))
            {
                Console.WriteLine("Некоректно въведено число!");
                continue;
            }
            searchDepartment = readLineDigit;
        }
        return searchDepartment;
    }

    public static string ReadLineData(string readLineData)
    {
        string output = String.Empty;

        MatchCollection regex = Regex.Matches(readLineData, "^(\\d{2})\\.(\\d{2})\\.(\\d{4})$");

        foreach (Match r in regex)
        {
            if (r.Groups[1].Value == "" || r.Groups[2].Value == "" || r.Groups[3].Value == "")
            {
                break;
            }

            output = $"{r.Groups[2].Value}/{r.Groups[1].Value}/{r.Groups[3].Value}"
                    .Trim()
                    .ToString();
        }

        if (output == String.Empty)
        {
            Console.WriteLine("НЕКОРЕКТЕН ФОРМАТ НА ДАТАТА!");
            return String.Empty;
        }

        return output;
    }

    public static string ReadLineTime(string readLineTime)
    {
        Match regex = Regex.Match(readLineTime, "^(\\d{2})\\:(\\d{2})\\:(\\d{2})$");
        if (!regex.Success)
        {
            Console.WriteLine("НЕКОРЕКТЕН ФОРМАТ НА ЧАСА!");
            return String.Empty;
        }

        return regex.Value;
    }

    public static Dictionary<int, Employee> CardEmployeeDepart(string searchUserName, int searchDepartment)
    {
        string query = "SELECT c.CardLow AS cardLow, d.DepartmentName AS dName, e.EmployeeName AS eName " +
            "FROM ((`Employee` AS e " +
            "INNER JOIN  `Card` AS c ON e.EmployeeID = c.EmployeeID )" +
            "INNER JOIN `Department` AS d ON e.DepartmentID = d.DepartmentID )" +
            $"WHERE e.DepartmentID={searchDepartment}";

        if (searchUserName.Length > 0)
        {
            query = "SELECT c.CardLow AS cardLow, d.DepartmentName AS dName, e.EmployeeName AS eName " +
            "FROM ((`Employee` AS e " +
            "INNER JOIN  `Card` AS c ON e.EmployeeID = c.EmployeeID )" +
            "INNER JOIN `Department` AS d ON e.DepartmentID = d.DepartmentID )" +
            $"WHERE e.DepartmentID={searchDepartment} WHERE e.EmployeeName={searchUserName}";
        }

        ConnectMDB myDataTable = new ConnectMDB(query);
        Dictionary<int, Employee> cardLowNameDepart = new Dictionary<int, Employee>();

        foreach (var item in myDataTable.ConnectDB().AsEnumerable())
        {
            if (!cardLowNameDepart.ContainsKey(item.Field<int>("cardLow")))
            {
                cardLowNameDepart[item.Field<int>("cardLow")] = new Employee();
            }

            Employee employee = new Employee
            {
                EmployeeName = item.Field<string>("eName"),
                DepartmentName = item.Field<string>("dName")
            };
            cardLowNameDepart[item.Field<int>("cardLow")] = employee;
        }

        return cardLowNameDepart;
    }

    public static EventRecord EventRecordRead(string startSearchData, string endSearchData, string startSearchTime, string endSearchTime, int ctrlIDOptional, int ordinalOptional)
    {
        string query = $"SELECT `AriseTime`, `CardLow`, `CtrlID`, `Ordinal` FROM `EventRecord` " +
            $"WHERE `CtrlID`={ctrlIDOptional} " +
            $"AND `Ordinal`={ordinalOptional} " +
            $"AND AriseTime Between #{startSearchData} 00:00:00# And #{endSearchData} 23:59:59# " +
            $"AND ((TimeValue(AriseTime) Between #{startSearchTime}# And #{endSearchTime}#)) " +
            $"ORDER BY `AriseTime`";
        
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
            Ordinal = myDataTable.ConnectDB()
                .AsEnumerable()
                .Select(r => r.Field<byte>("Ordinal"))
                .ToList()
        };

        return eventRecord;
    }

    public static Dictionary<int, string> Department()
    {
        string query = "SELECT `DepartmentID`, `DepartmentName` FROM `Department` ORDER BY `DepartmentID`";

        ConnectMDB myDataTable = new ConnectMDB(query);

        Dictionary<int, string> department = myDataTable
            .ConnectDB()
            .AsEnumerable()
            .ToDictionary(
            r => r.Field<int>("DepartmentID"),
            r => r.Field<string>("DepartmentName"));
        return department;
    }

    public static void ExportToExcel(List<PrintList> print)
    {
        // Load Excel application
        Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

        // Create empty workbook
        excel.Workbooks.Add();

        // Create Worksheet from active sheet
        Microsoft.Office.Interop.Excel._Worksheet workSheet = excel.ActiveSheet;

        // I created Application and Worksheet objects before try/catch,
        // so that i can close them in finnaly block.
        // It's IMPORTANT to release these COM objects!!
        try
        {
            // ------------------------------------------------
            // Creation of header cells
            // ------------------------------------------------
            workSheet.Cells[1, "A"] = "ИМЕ ПРЕЗИМЕ ФАМИЛИЯ";
            workSheet.Cells[1, "B"] = "ДАТА ЧАС";
            workSheet.Cells[1, "C"] = "ПОРТАЛ";
            workSheet.Cells[1, "D"] = "ВХОД/ИЗХОД";
            workSheet.Cells[1, "E"] = "ОТДЕЛ";

            // ------------------------------------------------
            // Populate sheet with some real data from "cars" list
            // ------------------------------------------------
            int row = 2; // start row (in row 1 are header cells)
            foreach (PrintList prn in print)
            {
                workSheet.Cells[row, "A"] = prn.EmployeeName;
                workSheet.Cells[row, "B"] = prn.DateTime;
                workSheet.Cells[row, "C"] = prn.CtrlID;
                workSheet.Cells[row, "D"] = prn.Ordinal;
                workSheet.Cells[row, "E"] = prn.DepartmentName;

                row++;
            }

            // Apply some predefined styles for data to look nicely :)
            workSheet.Range["A1"].AutoFormat(Microsoft.Office.Interop.Excel.XlRangeAutoFormat.xlRangeAutoFormatClassic1);

            // Define filename
            string fileName = string.Format(@"{0}\ExcelData.xlsx", Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

            // Save this data as a file
            workSheet.SaveAs(fileName);

            // Display SUCCESS message
            Console.WriteLine(string.Format("The file '{0}' is saved successfully!", fileName));
            //MessageBox.Show(string.Format("The file '{0}' is saved successfully!", fileName));
        }
        catch (Exception exception)
        {
            Console.WriteLine("Exception", "There was a PROBLEM saving Excel file!\n" + exception.Message);
            //MessageBox.Show("Exception", "There was a PROBLEM saving Excel file!\n" + exception.Message, MessageBoxButtons.OK, essageBoxIcon.Error);
        }
        finally
        {
            // Quit Excel application
            excel.Quit();

            // Release COM objects (very important!)
            if (excel != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excel);

            if (workSheet != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);

            // Empty variables
            excel = null;
            workSheet = null;

            // Force garbage collector cleaning
            GC.Collect();
        }
    }

    public static string[] MatchInput(string searchDataTime)
    {
        string[] matchInput = new string[4];
        bool isSearchDataTime = false;
        string pattern = "([0-9]{0,2}).([0-9]{0,2}).([0-9]{2,4})\\s+([0-9]{0,2}):([0-9]{0,2}):([0-9]{0,2})";

        while (!isSearchDataTime)
        {

            MatchCollection regex = Regex.Matches(searchDataTime, pattern);

            string startSearchData = String.Empty;
            string endSearchData = String.Empty;
            string startSearchTime = String.Empty;
            string endSearchTime = String.Empty;

            foreach (Match r in regex)
            {
                if (r.Groups[1].Value == "" || r.Groups[2].Value == "" || r.Groups[3].Value == "" ||
                    r.Groups[4].Value == "" || r.Groups[5].Value == "" || r.Groups[6].Value == "")
                {
                    isSearchDataTime = true;
                    break;
                }

                if (startSearchData.Equals(String.Empty))
                {
                    startSearchData = $"{r.Groups[2].Value}/{r.Groups[1].Value}/{r.Groups[3].Value}"
                        .Trim()
                        .ToString();
                }
                else if (endSearchData.Equals(String.Empty) && !startSearchData.Equals(String.Empty))
                {
                    endSearchData = $"{r.Groups[2].Value}/{r.Groups[1].Value}/{r.Groups[3].Value}"
                        .Trim()
                        .ToString();
                }

                if (startSearchTime.Equals(String.Empty))
                {
                    startSearchTime = $"{r.Groups[4].Value}:{r.Groups[5].Value}:{r.Groups[6].Value}"
                        .Trim()
                        .ToString();
                }
                else if (endSearchTime.Equals(String.Empty) && !startSearchTime.Equals(String.Empty))
                {
                    endSearchTime = $"{r.Groups[4].Value}:{r.Groups[5].Value}:{r.Groups[6].Value}"
                        .Trim()
                        .ToString();
                }
            }

            if (isSearchDataTime)
            {
                Console.WriteLine("Error matches regex!");
                Console.WriteLine("Data time format 'from - to': 'DD.MM.YYYY HH:mm:ss - DD.MM.YYYY HH:mm:ss'");
                searchDataTime = Console.ReadLine();
                isSearchDataTime = false;
            }
            else
            {
                matchInput[0] = startSearchData;
                matchInput[1] = endSearchData;
                matchInput[2] = startSearchTime;
                matchInput[3] = endSearchTime;
                break;
            }
        }

        return matchInput;
    }
}