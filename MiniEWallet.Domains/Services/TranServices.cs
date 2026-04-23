using Microsoft.EntityFrameworkCore;
using MiniEWallet.Database.Models;
using MiniEWallet.Domains.Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEWallet.Domains.Services
{
    public class TranServices
    {
        protected readonly AppDBContext _db;
        protected readonly AccountResponseModel _responseModel;
        protected readonly TranResponseModel _tranResponse;
        protected ResultModel<TranResponseModel> _model;

        public TranServices()
        {
            _db = new AppDBContext();
            _responseModel = new AccountResponseModel();
            _tranResponse = new TranResponseModel();
            _model = new ResultModel<TranResponseModel>();
        }

        public async Task<TranResponseModel> MakeDeposit(int id, int amount)
        {
            var accData = await _db.TblAccounts.Where(x => x.AccId == id).FirstOrDefaultAsync();

            if (accData is null)
            {
                _tranResponse.trnRsp = BaseResponseModel.DataNotExist("401", "Data Not Exists");
                goto Next;
            }

            accData.AccBalance += amount;
            await _db.SaveChangesAsync();

            TblTransaction tran_data = new TblTransaction();
            tran_data.FrAccId = accData.AccId;
            tran_data.ToAccId = accData.AccId;
            tran_data.Amount = amount;
            tran_data.TranType = 1;
            tran_data.TimeLog = DateTime.Now;
            await _db.TblTransactions.AddAsync(tran_data);
            await _db.SaveChangesAsync();

            _tranResponse.trnRsp = BaseResponseModel.Success("200", "Transaction Added Successfully");
            Next:
            return _tranResponse;
        }

        public async Task<TranResponseModel> CreateTranType(string description)
        {
            try
            {
                TblTranType trantype = new TblTranType();
                trantype.TranDescription = description;
                trantype.TimeLog = DateTime.Now;
                await _db.TblTranTypes.AddAsync(trantype);
                await _db.SaveChangesAsync();

                _tranResponse.trnRsp = BaseResponseModel.Success("200", "Transaction Type Added Successfully");
                goto Next;
            }
            catch (Exception ex)
            {
                _tranResponse.trnRsp = BaseResponseModel.SystemError("502", ex.ToString());
            }
            Next: return _tranResponse;
        }

        public async Task<TranResponseModel> MakeTransfer(int frid, int toid, int pass, int amount)
        {
            try
            {
                var accData = await _db.TblAccounts.Where(x => x.AccId == frid).FirstOrDefaultAsync();
                if (accData is null)
                {
                    _tranResponse.trnRsp = BaseResponseModel.DataNotExist("401", "Data Not Exists");
                    goto State;
                }
                if (accData.AccPassword == pass && accData.AccBalance > amount)
                {
                    accData.AccBalance = accData.AccBalance - amount;
                    await _db.SaveChangesAsync();

                    var recData = await _db.TblAccounts.Where(x => x.AccId == toid).FirstOrDefaultAsync();
                    if (recData is not null)
                    {
                        recData.AccBalance += amount;
                        await _db.SaveChangesAsync();
                    }

                    TblTransaction trnsData = new TblTransaction();
                    trnsData.FrAccId = accData.AccId;
                    trnsData.ToAccId = toid;
                    trnsData.Amount = amount;
                    trnsData.TranType = 2;
                    trnsData.TimeLog = DateTime.Now;
                    await _db.TblTransactions.AddAsync(trnsData);
                    await _db.SaveChangesAsync();
                    _tranResponse.trnRsp = BaseResponseModel.Success("200", "Transfered Successfully"); ;
                    goto State;
                }
                else if(accData.AccBalance < amount)
                { 
                    _tranResponse.trnRsp = BaseResponseModel.ValidationError("302", "Not Enough Balance!"); ;
                    goto State;
                }
                else
                {
                    _tranResponse.trnRsp = BaseResponseModel.ValidationError("302", "Password Incorrect!");
                    goto State;
                }
            }
            catch (Exception ex)
            {
                _tranResponse.trnRsp = BaseResponseModel.SystemError("502", ex.ToString());
                goto State;
            }
            State: return _tranResponse;
        }

        public async Task<ResultModel<TranResponseModel>> MakeTransfer1(int frid, int toid, int pass, int amount)
        {
            try
            {
                var accData = await _db.TblAccounts.Where(x => x.AccId == frid).FirstOrDefaultAsync();
                if (accData is null)
                {
                    _model = ResultModel<TranResponseModel>.DataNotExist("Data Not Exists");
                    goto State;
                }
                if (accData.AccPassword == pass && accData.AccBalance > amount)
                {
                    accData.AccBalance = accData.AccBalance - amount;
                    await _db.SaveChangesAsync();

                    var recData = await _db.TblAccounts.Where(x => x.AccId == toid).FirstOrDefaultAsync();
                    if (recData is not null)
                    {
                        recData.AccBalance += amount;
                        await _db.SaveChangesAsync();
                    }

                    var trnsData = new TblTransaction()
                    {
                        FrAccId = accData.AccId,
                        ToAccId = toid,
                        Amount = amount,
                        TranType = 2,
                        TimeLog = DateTime.Now
                    };
                    
                    await _db.TblTransactions.AddAsync(trnsData);
                    await _db.SaveChangesAsync();

                    TranResponseModel item = new TranResponseModel();
                    item.tran = trnsData;

                    _model = ResultModel<TranResponseModel>.Success(item,"Transfered Successfully"); ;
                    goto State;
                }
                else if (accData.AccBalance < amount)
                {
                    _model = ResultModel<TranResponseModel>.ValidationError("Not Enough Balance!"); ;
                    goto State;
                }
                else
                {
                    _model = ResultModel<TranResponseModel>.ValidationError("Password Incorrect!");
                    goto State;
                }
            }
            catch (Exception ex)
            {
                _model = ResultModel<TranResponseModel>.SystemError(ex.ToString());
                goto State;
            }
        State: return _model;
        }

        public async Task<TranResponseModel> MakeWithDrawl(int id, int pass, int amount)
        {
            try
            {
                var data = await _db.TblAccounts.Where(x => x.AccId == id).FirstOrDefaultAsync();
                if (data is null)
                {
                    _tranResponse.trnRsp = BaseResponseModel.DataNotExist("401", "Data Not Exists");
                    goto State;
                }
                else
                {
                    if (data.AccPassword == pass)
                    {
                        if (data.AccBalance > amount)
                        {
                            data.AccBalance = data.AccBalance - amount;
                            await _db.SaveChangesAsync();

                            TblTransaction trnsData = new TblTransaction();
                            trnsData.FrAccId = id;
                            trnsData.ToAccId = id;
                            trnsData.Amount = amount;
                            trnsData.TranType = 3;
                            trnsData.TimeLog = DateTime.Now;
                            await _db.TblTransactions.AddAsync(trnsData);
                            await _db.SaveChangesAsync();
                            _tranResponse.trnRsp = BaseResponseModel.Success("200", "WithDrwal Successfully");
                            goto State;
                        }
                        else
                        {
                            _tranResponse.trnRsp = BaseResponseModel.ValidationError("302", "Balance Not Enough!");
                            goto State;
                        }
                    }
                    else
                    {
                        _tranResponse.trnRsp = BaseResponseModel.ValidationError("302", "Password Incorrect!");
                        goto State;
                    }
                }
            }
            catch (Exception ex) { _tranResponse.trnRsp = BaseResponseModel.SystemError("502", ex.ToString()); goto State; }
            State: return _tranResponse;
        }   
    }
}
