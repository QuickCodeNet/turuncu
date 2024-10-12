using System;
using System.Collections.Generic;
using System.Linq;
using QuickCode.Turuncu.Portal.Models;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using QuickCode.Turuncu.Portal.Helpers.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using QuickCode.Turuncu.Common.Helpers;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;

namespace QuickCode.Turuncu.Portal.Controllers.UserManagerModule
{
    [Permission("UserManagerModulePortalPermissions")]
    public partial class KafkaEventsController : BaseController
    {
        [Route("GetKafkaEvents")]
        [HttpGet]
        public async Task<IActionResult> GetKafkaEvents()
        {
            var model = GetModel<GetKafkaEventsData>();
            var kafkaEvents = await pageClient.GetKafkaEventsAsync();
            model.Items = new Dictionary<string, Dictionary<string, List<KafkaEventsGetKafkaEventsResponseDto>>>();
            foreach (var item in kafkaEvents)
            {
                var moduleName = item.Path.Split('/')[2].KebabCaseToPascal("");
                if (item.ControllerName.Equals("AuthenticationController"))
                {
                    moduleName = "UserManagerModule";
                }
                model.Items.TryAdd(moduleName, new Dictionary<string, List<KafkaEventsGetKafkaEventsResponseDto>>());
                model.Items[moduleName].TryAdd(item.ControllerName, []);
                model.Items[moduleName][item.ControllerName].Add(item);
            }

            SetModelBinder(ref model);
            return View("KafkaEvents", model);
        }

        [Route("UpdateKafkaEvent")]
        [HttpPost]
        public async Task<JsonResult> UpdateKafkaEvent(UpdateKafkaEvent request)
        {
            var eventData = await pageClient.KafkaEventsGetAsync(request.Id);
            switch (request.EventName)
            {
                case "complete":
                    eventData.OnComplete = request.Value == 1;
                    break;
                case "error":
                    eventData.OnError = request.Value == 1;
                    break;
                case "timeout":
                    eventData.OnTimeout = request.Value == 1;
                    break;
            }

            var result = await pageClient.KafkaEventsPutAsync(request.Id, eventData);
            return Json(result);
        }
    }
}

