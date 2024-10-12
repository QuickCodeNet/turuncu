using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.Portal.Helpers;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;

namespace QuickCode.Turuncu.Portal.Models
{
	public class GetPortalPermissionGroupData
	{
		public PortalPermissionGroupList Items { get; set; }

		public int SelectedGroupId { get; set; }

		public Dictionary<string, IEnumerable<SelectListItem>> ComboList = new Dictionary<string, IEnumerable<SelectListItem>>();
	}

	
	public class UpdateGroupAuthorizationRequestData : UpdatePortalPermissionGroupRequest
	{
		public string Key
		{
			get
			{
				object[] keyList = new object[] { PermissionGroupId, PortalPermissionId, PortalPermissionTypeId };
				return String.Join("|", keyList.Select(i => i.AsString()));
			}
		}
	}
	
	public class UpdateKafkaEvent
	{
		public int Id { get; set; }
		public string EventName { get; set; }
		public int Value { get; set; }
	}

	public class GetKafkaEventsData
	{
		public Dictionary<string, Dictionary<string, List<KafkaEventsGetKafkaEventsResponseDto>>> Items { get; set; }
	}

	
	public class GetApiPermissionGroupData
	{
		public ApiModulePermissions Items { get; set; }

		public int SelectedGroupId { get; set; }

		public Dictionary<string, IEnumerable<SelectListItem>> ComboList = new Dictionary<string, IEnumerable<SelectListItem>>();
	}

	
	public class UpdateGroupAuthorizationApiRequestData : UpdateApiPermissionGroupRequest
	{
		public string Key
		{
			get
			{
				object[] keyList = new object[] { PermissionGroupId, ApiMethodDefinitionId };
				return String.Join("|", keyList.Select(i => i.AsString()));
			}
		}
	}
}

