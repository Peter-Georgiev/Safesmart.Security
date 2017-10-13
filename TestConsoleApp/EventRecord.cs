using System;
using System.Collections.Generic;
using System.Data;

class EventRecord
{
    public List<int> CardLow { get; set; }

    public List<byte> Ordinal { get; set; }

    public List<int> CtrlID { get; set; }

    public List<DateTime> AriseTime { get; set; }
}