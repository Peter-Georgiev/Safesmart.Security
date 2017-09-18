using System;
using System.Collections.Generic;
using System.Data;

class EventRecord
{
    private List<int> _cardLow { get; set; }

    public List<byte> EventType { get; set; }

    public List<int> CtrlID { get; set; }

    public List<DateTime> AriseTime { get; set; }

    public List<int> CardLow
    {
        set
        {
            this._cardLow = value;
        }

        get
        {
            return _cardLow;
        }
    }
}