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
using QuickCode.Turuncu.Portal.Models;
using QuickCode.Turuncu.Portal.Models.SmsManagerModule;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using UserManagerModuleContracts = QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using SmsManagerModuleContracts = QuickCode.Turuncu.Common.Nswag.Clients.SmsManagerModuleApi.Contracts;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using QuickCode.Turuncu.Portal.Helpers.Authorization;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using AutoRest.Core.Utilities.Collections;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuickCode.Turuncu.Portal.Controllers.SmsManagerModule
{
    [Permission("SmsManagerModuleInfoMessages")]
    [Area("SmsManagerModule")]
    [Route("SmsManagerModuleInfoMessages")]
    public partial class InfoMessagesController : BaseController
    {
        private int pageSize = 20;
        private readonly SmsManagerModuleContracts.IInfoMessagesClient pageClient;
        private readonly SmsManagerModuleContracts.ISmsSendersClient pageSmsSendersClient;
        private readonly SmsManagerModuleContracts.IInfoTypesClient pageInfoTypesClient;
        public InfoMessagesController(SmsManagerModuleContracts.IInfoMessagesClient pageClient, SmsManagerModuleContracts.ISmsSendersClient pageSmsSendersClient, SmsManagerModuleContracts.IInfoTypesClient pageInfoTypesClient, UserManagerModuleContracts.ITableComboboxSettingsClient tableComboboxSettingsClient, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache cache) : base(tableComboboxSettingsClient, httpContextAccessor, mapper, cache)
        {
            this.pageClient = pageClient;
            this.pageSmsSendersClient = pageSmsSendersClient;
            this.pageInfoTypesClient = pageInfoTypesClient;
        }

        [ResponseCache(VaryByQueryKeys = new[] { "ic" }, Duration = 30)]
        public async Task<IActionResult> GetImage(string ic)
        {
            return await GetImageResult(pageClient, ic);
        }

        [Route("List")]
        public async Task<IActionResult> List()
        {
            var model = GetModel<InfoMessagesData>();
            model.PageSize = pageSize;
            model.CurrentPage = 1;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            model.ComboList = await FillPageComboBoxes(model.ComboList);
            var listResponse = (await pageClient.InfoMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<InfoMessagesObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            return View("List", model);
        }

        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> List(InfoMessagesData model)
        {
            ModelBinder(ref model);
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            if (model.CurrentPage == Int32.MaxValue)
            {
                model.CurrentPage = model.TotalPage;
            }

            var listResponse = (await pageClient.InfoMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<InfoMessagesObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            model.SelectedItem = new InfoMessagesObj();
            return View("List", model);
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(InfoMessagesData model)
        {
            ModelBinder(ref model);
            var selected = mapper.Map<SmsManagerModuleContracts.InfoMessagesDto>(model.SelectedItem);
            var result = await pageClient.InfoMessagesPostAsync(selected);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(InfoMessagesData model)
        {
            ModelBinder(ref model);
            var request = mapper.Map<SmsManagerModuleContracts.InfoMessagesDto>(model.SelectedItem);
            var result = await pageClient.InfoMessagesPutAsync(request.Id, request);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(InfoMessagesData model)
        {
            ModelBinder(ref model);
            var request = model.SelectedItem;
            var result = await pageClient.InfoMessagesDeleteAsync(request.Id);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("InsertItem")]
        public async Task<IActionResult> InsertItem(InfoMessagesData model)
        {
            ModelState.Clear();
            ModelBinder(ref model);
            SetModelBinder(ref model);
            model.SelectedItem = new InfoMessagesObj();
            return PartialView("Insert", model);
        }

        [Route("DetailItem")]
        public async Task<IActionResult> DetailItem(InfoMessagesData model)
        {
            ModelBinder(ref model);
            if (model.List == null)
            {
                model = await FillModel(model);
            }

            model.SelectedItem = model.List.Where(i => i._Key == model.SelectedKey).FirstOrDefault();
            SetModelBinder(ref model);
            return PartialView("Detail", model);
        }

        [Route("UpdateItem")]
        [HttpPost]
        public async Task<IActionResult> UpdateItem(InfoMessagesData model)
        {
            ModelState.Clear();
            ModelBinder(ref model);
            if (model.List == null)
            {
                model = await FillModel(model);
            }

            model.SelectedItem = model.List.Where(i => i._Key == model.SelectedKey).FirstOrDefault();
            SetModelBinder(ref model);
            return PartialView("Update", model);
        }

        [Route("DeleteItem")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem(InfoMessagesData model)
        {
            ModelBinder(ref model);
            if (model.List == null)
            {
                model = await FillModel(model);
            }

            model.SelectedItem = model.List.Where(i => i._Key == model.SelectedKey).FirstOrDefault();
            SetModelBinder(ref model);
            return PartialView("Delete", model);
        }

        private async Task<InfoMessagesData> FillModel(InfoMessagesData model)
        {
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            var listResponse = (await pageClient.InfoMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<InfoMessagesObj>>(listResponse.ToList());
            return model;
        }

        private async Task<Dictionary<string, IEnumerable<SelectListItem>>> FillPageComboBoxes(Dictionary<string, IEnumerable<SelectListItem>> comboBoxList)
        {
            comboBoxList.Clear();
            comboBoxList.AddRange(await FillComboBoxAsync("SmsSenders", () => pageSmsSendersClient.SmsSendersGetAsync()));
            comboBoxList.AddRange(await FillComboBoxAsync("InfoTypes", () => pageInfoTypesClient.InfoTypesGetAsync()));
            return comboBoxList;
        }
    }
}