using Microsoft.EntityFrameworkCore;
using MiniEWallet.Database.Models;
using MiniEWallet.Domains.Response_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniEWallet.Domains.Services;

public class AccServices
{
    protected readonly AppDBContext _db;
    protected readonly AccountResponseModel _responseModel;

    public AccServices()
    {
        _db = new AppDBContext();
        _responseModel = new AccountResponseModel();
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
}