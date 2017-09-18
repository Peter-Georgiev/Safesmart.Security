using System;
using System.Collections.Generic;

class EventRecordObj
{
    public List<int> CardLow { get; set; }

    public List<byte> EventType { get; set; }

    public List<int> CtrlID { get; set; }

    public List<DateTime> AriseTime { get; set; }
    /*
    public EventRecordObj(List<DateTime> ariseTime, List<int> ctrlID, List<byte> eventType)
    {
        this.AriseTime = ariseTime;
        this.CtrlID = ctrlID;
        this.EventType = eventType;
    }
    */
    public static implicit operator int(EventRecordObj v)
    {
        throw new NotImplementedException();
    }
}