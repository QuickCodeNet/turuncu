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
    public partial interface IPortalPermissionGroupsRepository : IBaseRepository<PortalPermissionGroups>
    {
        Task<DLResponse<PortalPermissionGroups>> GetByPkAsync(int id);
        Task<DLResponse<List<PortalPermissionGroupsGetPortalPermissionGroupsResponseDto>>> PortalPermissionGroupsGetPortalPermissionGroupsAsync(int portalPermissionGroupsPermissionGroupId);
        Task<DLResponse<List<PortalPermissionGroupsGetPortalPermissionGroupResponseDto>>> PortalPermissionGroupsGetPortalPermissionGroupAsync(int portalPermissionGroupsPortalPermissionId, int portalPermissionGroupsPermissionGroupId, int portalPermissionGroupsPortalPermissionTypeId);
    }
}