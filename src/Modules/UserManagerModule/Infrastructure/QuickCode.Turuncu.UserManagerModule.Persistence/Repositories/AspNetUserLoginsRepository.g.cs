//------------------------------------------------------------------------------ 
// <auto-generated> 
// This code was generated by QuickCode. 
// Runtime Version:1.0
// 
// Changes to this file may cause incorrect behavior and will be lost if 
// the code is regenerated. 
// </auto-generated> 
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using QuickCode.Turuncu.UserManagerModule.Application.Models;
using QuickCode.Turuncu.UserManagerModule.Domain.Entities;
using QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuickCode.Turuncu.UserManagerModule.Persistence.Contexts;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Persistence.Repositories
{
    public partial class AspNetUserLoginsRepository : IAspNetUserLoginsRepository
    {
        private readonly WriteDbContext _writeContext;
        private readonly ReadDbContext _readContext;
        private readonly ILogger<AspNetUserLoginsRepository> _logger;
        public AspNetUserLoginsRepository(ILogger<AspNetUserLoginsRepository> logger, WriteDbContext writeContext, ReadDbContext readContext)
        {
            _writeContext = writeContext;
            _readContext = readContext;
            _logger = logger;
        }

        public async Task<DLResponse<AspNetUserLogins>> InsertAsync(AspNetUserLogins value)
        {
            var returnValue = new DLResponse<AspNetUserLogins>(value, "Success");
            try
            {
                await _writeContext.AspNetUserLogins.AddAsync(value);
                await _writeContext.SaveChangesAsync();
                returnValue = new DLResponse<AspNetUserLogins>(value, "Success");
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "AspNetUserLogins Insert", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                }
                else
                {
                    returnValue.Code = 998;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "AspNetUserLogins Insert", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<bool>> UpdateAsync(AspNetUserLogins value)
        {
            var returnValue = new DLResponse<bool>(false, "Success");
            try
            {
                _writeContext.Set<AspNetUserLogins>().Update(value);
                await _writeContext.SaveChangesAsync();
                returnValue.Value = true;
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "AspNetUserLogins Update", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                }
                else
                {
                    returnValue.Code = 998;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "AspNetUserLogins", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<bool>> DeleteAsync(AspNetUserLogins value)
        {
            var returnValue = new DLResponse<bool>(false, "Success");
            try
            {
                _writeContext.AspNetUserLogins.Remove(value);
                await _writeContext.SaveChangesAsync();
                returnValue.Value = true;
            }
            catch (SqlException ex)
            {
                _logger.LogError("{repoName} SqlException {error}", "AspNetUserLogins Delete", ex.Message);
                if (ex.Number.Equals(2627))
                {
                    returnValue.Code = 999;
                }
                else
                {
                    returnValue.Code = 998;
                }

                returnValue.Message = ex.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "AspNetUserLogins Delete", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<AspNetUserLogins>> GetByPkAsync(string loginProvider, string providerKey)
        {
            var returnValue = new DLResponse<AspNetUserLogins>();
            try
            {
                var result =
                    from asp_net_user_logins in _readContext.AspNetUserLogins
                    where asp_net_user_logins.LoginProvider.Equals(loginProvider) && asp_net_user_logins.ProviderKey.Equals(providerKey)select asp_net_user_logins;
                returnValue.Value = await result.FirstAsync();
                if (returnValue.Value == null)
                {
                    returnValue.Code = 404;
                    returnValue.Message = $"Not found in AspNetUserLogins";
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("{repoName} Exception {error}", "AspNetUserLogins GetByPk", ex.Message);
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<List<AspNetUserLogins>>> ListAsync(int? pageNumber = null, int? pageSize = null)
        {
            var returnValue = new DLResponse<List<AspNetUserLogins>>();
            try
            {
                if (pageNumber < 1)
                {
                    returnValue.Code = 404;
                    returnValue.Message = "Page Number must be greater than 1";
                }
                else
                {
                    if (pageNumber != null)
                    {
                        var skip = ((pageNumber - 1) * pageSize);
                        var take = pageSize;
                        returnValue.Value = await _readContext.AspNetUserLogins.Skip(skip.Value).Take(take.Value).ToListAsync();
                    }
                    else
                    {
                        returnValue.Value = await _readContext.AspNetUserLogins.ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }

        public async Task<DLResponse<int>> CountAsync()
        {
            var returnValue = new DLResponse<int>();
            try
            {
                returnValue.Value = await _readContext.AspNetUserLogins.CountAsync();
            }
            catch (Exception ex)
            {
                returnValue.Code = 404;
                returnValue.Message = ex.ToString();
            }

            return returnValue;
        }
    }
}