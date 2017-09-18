class Controller
{
    private byte CtrlID { get; set; }

    private string CtrlName { get; set; }

    public Controller(string ctrlName, byte ctrlID)
    {
        this.CtrlName = ctrlName;
        this.CtrlID = ctrlID;
    }
}