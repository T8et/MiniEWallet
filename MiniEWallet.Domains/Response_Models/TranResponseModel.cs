using MiniEWallet.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEWallet.Domains.Response_Models
{
    public class TranResponseModel
    {
        public BaseResponseModel? trnRsp {  get; set; }

        public TblTransaction? tran { get; set; }
    }
}
