using System.Collections.Generic;

class Card
{
    public List<int> EmployeeID { get; set; }

    public List<int> CardLow { get; set; }

    public Card(List<int> employeeID, List<int> cardLow)
    {
        //Dictionary<int, int>() 
        this.EmployeeID = employeeID;

        this.CardLow = cardLow;
    }
}