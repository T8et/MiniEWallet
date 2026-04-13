using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MiniEWallet.Database.Models;
using MiniEWallet.Domains.Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MiniEWallet.Domains.Services;

public class AccServices
{
    protected readonly AppDBContext _db;
    protected readonly AccountResponseModel _responseModel;
    protected readonly TranResponseModel _tranResponse;

    public AccServices()
    {
        _db = new AppDBContext();
        _responseModel = new AccountResponseModel();
        _tranResponse = new TranResponseModel();
    }

    public async Task<List<TblAccount>> GetAccs()
    {
        var item = await _db.TblAccounts.ToListAsync();
        return item;
    } 

    public async Task<List<TblAccount>> GetAccById(int id)
    {
        var item = await _db.TblAccounts
                             .Where(x => x.AccId == id)
                             .ToListAsync();
        return item;
    }

    public async Task<AccountResponseModel> CreateAccount(TblAccount accdata)
    {
        try
        {
            var item = await _db.TblAccounts.AddAsync(accdata);
            await _db.SaveChangesAsync();
            _responseModel.respose = BaseResponseModel.Success("200", "Success");
            return _responseModel;
        }
        catch (Exception) 
        {
            _responseModel.respose = BaseResponseModel.ValidationError("511", "Fail Data Insert");
            return _responseModel;
        }
    }

    public async Task<TranResponseModel> MakeDeposit(int id, int amount) 
    {
        var accData = await _db.TblAccounts.Where(x => x.AccId == id).FirstOrDefaultAsync();

        if(accData is null)
        {
            _tranResponse.trnRsp = BaseResponseModel.DataNotExist("401", "Data Not Exists");
            return _tranResponse;
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
        return _tranResponse;
    }

    public async Task<BaseResponseModel> CreateTranType(string description)
    {
        try
        {
            TblTranType trantype = new TblTranType();
            trantype.TranDescription = description;
            trantype.TimeLog = DateTime.Now;
            await _db.TblTranTypes.AddAsync(trantype);
            await _db.SaveChangesAsync();

            return BaseResponseModel.Success("200", "Transaction Type Added Successfully");
        }
        catch (Exception ex) 
        {
            return BaseResponseModel.SystemError("502",ex.ToString());
        }
    }

    public async Task<BaseResponseModel> MakeTransfer(int frid,int toid,int pass,int amount)
    {
        try
        {
            var accData = await _db.TblAccounts.Where(x => x.AccId == frid).FirstOrDefaultAsync();
            if (accData is null)
            {
                return BaseResponseModel.DataNotExist("401", "Data Not Exists");
            }
            if (accData.AccPassword == pass && accData.AccBalance > amount)
            {
                accData.AccBalance = accData.AccBalance - amount;
                await _db.SaveChangesAsync();

                var recData = await _db.TblAccounts.Where(x => x.AccId == toid).FirstOrDefaultAsync();
                if(recData is not null)
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
                return BaseResponseModel.Success("200", "Transaction Type Added Successfully");
            }
            else
            {
                return BaseResponseModel.ValidationError("302", "Not Enough Balance!");
            }
        }
        catch (Exception ex) 
        {
            return BaseResponseModel.SystemError("502", ex.ToString());
        }
    }

    public async Task<BaseResponseModel> MakeWithDrawl(int id,int pass,int amount)
    {
        try
        {
            var data = await _db.TblAccounts.Where(x => x.AccId == id).FirstOrDefaultAsync();
            if (data is null)
            {
                return BaseResponseModel.DataNotExist("401", "Data Not Exists");
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
                        return BaseResponseModel.Success("200", "WithDrwal Successfully");
                    }
                    else
                    {
                        return BaseResponseModel.ValidationError("302", "Balance Not Enough!");
                    }
                }
                else
                {
                    return BaseResponseModel.ValidationError("302", "Password Incorrect!");
                }
            }
        }
        catch (Exception ex) { return BaseResponseModel.SystemError("502", ex.ToString()); }
    }
}