using System;
using System.Collections.Generic;

namespace MiniEWallet.Database.Models;

public partial class TblAccount
{
    public int AccId { get; set; }

    public string? AccName { get; set; }

    public int? AccPassword { get; set; }

    public string? AccPhone { get; set; }

    public string? AccAddress { get; set; }

    public int? AccBalance { get; set; }

    public bool? AccActive { get; set; }

    public DateTime? TimeLog { get; set; }
}
