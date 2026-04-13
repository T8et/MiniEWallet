using System;
using System.Collections.Generic;

namespace MiniEWallet.Database.Models;

public partial class TblTranType
{
    public int TranType { get; set; }

    public string? TranDescription { get; set; }

    public DateTime? TimeLog { get; set; }
}
