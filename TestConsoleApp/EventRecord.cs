using System;
using System.Collections.Generic;

class EventRecord
{
    public List<byte> EventType { get; set; }

    public List<int> CardLow { get; set; }

    public List<int> CtrlID { get; set; }

    public List<DateTime> AriseTime { get; set; }

    public EventRecord(List<DateTime> ariseTime, List<int> cardLow, List<int> ctrlID, List<byte> eventType)
    {
        this.AriseTime = ariseTime;
        this.CardLow = cardLow;
        this.CtrlID = ctrlID;
        this.EventType = eventType;
    }
}