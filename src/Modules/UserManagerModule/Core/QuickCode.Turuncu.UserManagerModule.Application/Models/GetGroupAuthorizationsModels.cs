using System;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.UserManagerModule.Application.Dtos;

namespace QuickCode.Turuncu.UserManagerModule.Application.Models
{	
    public class PortalPermissionGroupList
    {
        public int PermissionGroupId { get; set; }
        public List<PortalPermissionItem> PortalPermissions { get; set; } = [];
    }

    public class PortalPermissionItem
    {
        public int PortalPermssionId { get; set; }
        public List<PortalPermissionTypeItem> PortalPermissionTypes { get; set; } = [];
    }

    public class PortalPermissionTypeItem
    {
        public int PortalPermssionTypeId { get; set; }
        public bool Value { get; set; }
    }

    public class UpdatePortalPermissionGroupRequest
    {
        public int PermissionGroupId { get; set; }
        public int PortalPermissionId { get; set; }
        public int PortalPermissionTypeId { get; set; }
        public int Value { get; set; }
    }
	
    public class UpdateApiPermissionGroupRequest
    {
        public int PermissionGroupId { get; set; }
        public int ApiMethodDefinitionId { get; set; }
        public int Value { get; set; }
    }
    
    public record ApiMethodDefinitionItem : ApiMethodDefinitionsDto
    {
        public bool Value { get; set; }
    }
	
    public class ApiModulePermissions
    {
        public int PermissionGroupId { get; set; }

        public Dictionary<string, Dictionary<string, List<ApiMethodDefinitionItem>>> ApiModulePermissionList { get; set; } = [];
    }
}