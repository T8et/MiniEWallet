using System;
using System.Collections.Generic;

namespace MiniEWallet.Database.Models;

public partial class TblTransaction
{
    public int TranId { get; set; }

    public int FrAccId { get; set; }

    public int ToAccId { get; set; }

    public int Amount { get; set; }

    public int TranType { get; set; }

    public DateTime? TimeLog { get; set; }
}
