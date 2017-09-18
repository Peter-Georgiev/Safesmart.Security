using System.Collections.Generic;

class Card
{
    private Dictionary<int, int> card = 
        new Dictionary<int, int>();

    public void SetDict(Dictionary<int, int> dict)
    {
        card = dict;
    }

    public void Set(int cardLow, int employeeID)
    {
        if (card.ContainsKey(cardLow))
        {
            card[cardLow] = employeeID;
        }
        else
        {
            card.Add(cardLow, employeeID);
        }
    }
    
    public int Get(int cardLow)
    {
        int result = int.MinValue;

        if (card.ContainsKey(cardLow))
        {
            result = card[cardLow];
        }

        return result;
    }
}