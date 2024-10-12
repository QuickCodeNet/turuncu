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
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;
using System.Threading.Tasks;

namespace QuickCode.Turuncu.UserManagerModule.Application.Interfaces.Repositories
{
    public partial interface IPermissionGroupsRepository : IBaseRepository<PermissionGroups>
    {
        Task<DLResponse<PermissionGroups>> GetByPkAsync(int id);
        Task<DLResponse<List<PermissionGroupsAspNetUsers_RESTResponseDto>>> PermissionGroupsAspNetUsers_RESTAsync(int permissionGroupsId);
        Task<DLResponse<PermissionGroupsAspNetUsers_KEY_RESTResponseDto>> PermissionGroupsAspNetUsers_KEY_RESTAsync(int permissionGroupsId, string aspNetUsersId);
        Task<DLResponse<List<PermissionGroupsPortalPermissionGroups_RESTResponseDto>>> PermissionGroupsPortalPermissionGroups_RESTAsync(int permissionGroupsId);
        Task<DLResponse<PermissionGroupsPortalPermissionGroups_KEY_RESTResponseDto>> PermissionGroupsPortalPermissionGroups_KEY_RESTAsync(int permissionGroupsId, int portalPermissionGroupsId);
        Task<DLResponse<List<PermissionGroupsApiPermissionGroups_RESTResponseDto>>> PermissionGroupsApiPermissionGroups_RESTAsync(int permissionGroupsId);
        Task<DLResponse<PermissionGroupsApiPermissionGroups_KEY_RESTResponseDto>> PermissionGroupsApiPermissionGroups_KEY_RESTAsync(int permissionGroupsId, int apiPermissionGroupsId);
    }
}