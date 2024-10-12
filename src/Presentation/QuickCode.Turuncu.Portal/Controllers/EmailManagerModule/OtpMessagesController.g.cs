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
using QuickCode.Turuncu.Portal.Models.EmailManagerModule;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using UserManagerModuleContracts = QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using EmailManagerModuleContracts = QuickCode.Turuncu.Common.Nswag.Clients.EmailManagerModuleApi.Contracts;
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

namespace QuickCode.Turuncu.Portal.Controllers.EmailManagerModule
{
    [Permission("EmailManagerModuleOtpMessages")]
    [Area("EmailManagerModule")]
    [Route("EmailManagerModuleOtpMessages")]
    public partial class OtpMessagesController : BaseController
    {
        private int pageSize = 20;
        private readonly EmailManagerModuleContracts.IOtpMessagesClient pageClient;
        private readonly EmailManagerModuleContracts.IEmailSendersClient pageEmailSendersClient;
        private readonly EmailManagerModuleContracts.IOtpTypesClient pageOtpTypesClient;
        public OtpMessagesController(EmailManagerModuleContracts.IOtpMessagesClient pageClient, EmailManagerModuleContracts.IEmailSendersClient pageEmailSendersClient, EmailManagerModuleContracts.IOtpTypesClient pageOtpTypesClient, UserManagerModuleContracts.ITableComboboxSettingsClient tableComboboxSettingsClient, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache cache) : base(tableComboboxSettingsClient, httpContextAccessor, mapper, cache)
        {
            this.pageClient = pageClient;
            this.pageEmailSendersClient = pageEmailSendersClient;
            this.pageOtpTypesClient = pageOtpTypesClient;
        }

        [ResponseCache(VaryByQueryKeys = new[] { "ic" }, Duration = 30)]
        public async Task<IActionResult> GetImage(string ic)
        {
            return await GetImageResult(pageClient, ic);
        }

        [Route("List")]
        public async Task<IActionResult> List()
        {
            var model = GetModel<OtpMessagesData>();
            model.PageSize = pageSize;
            model.CurrentPage = 1;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            model.ComboList = await FillPageComboBoxes(model.ComboList);
            var listResponse = (await pageClient.OtpMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<OtpMessagesObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            return View("List", model);
        }

        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> List(OtpMessagesData model)
        {
            ModelBinder(ref model);
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            if (model.CurrentPage == Int32.MaxValue)
            {
                model.CurrentPage = model.TotalPage;
            }

            var listResponse = (await pageClient.OtpMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<OtpMessagesObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            model.SelectedItem = new OtpMessagesObj();
            return View("List", model);
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(OtpMessagesData model)
        {
            ModelBinder(ref model);
            var selected = mapper.Map<EmailManagerModuleContracts.OtpMessagesDto>(model.SelectedItem);
            var result = await pageClient.OtpMessagesPostAsync(selected);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(OtpMessagesData model)
        {
            ModelBinder(ref model);
            var request = mapper.Map<EmailManagerModuleContracts.OtpMessagesDto>(model.SelectedItem);
            var result = await pageClient.OtpMessagesPutAsync(request.Id, request);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(OtpMessagesData model)
        {
            ModelBinder(ref model);
            var request = model.SelectedItem;
            var result = await pageClient.OtpMessagesDeleteAsync(request.Id);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("InsertItem")]
        public async Task<IActionResult> InsertItem(OtpMessagesData model)
        {
            ModelState.Clear();
            ModelBinder(ref model);
            SetModelBinder(ref model);
            model.SelectedItem = new OtpMessagesObj();
            return await Task.FromResult(PartialView("Insert", model));
        }

        [Route("DetailItem")]
        public async Task<IActionResult> DetailItem(OtpMessagesData model)
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
        public async Task<IActionResult> UpdateItem(OtpMessagesData model)
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
        public async Task<IActionResult> DeleteItem(OtpMessagesData model)
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

        private async Task<OtpMessagesData> FillModel(OtpMessagesData model)
        {
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            var listResponse = (await pageClient.OtpMessagesGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<OtpMessagesObj>>(listResponse.ToList());
            return model;
        }

        private async Task<Dictionary<string, IEnumerable<SelectListItem>>> FillPageComboBoxes(Dictionary<string, IEnumerable<SelectListItem>> comboBoxList)
        {
            comboBoxList.Clear();
            comboBoxList.AddRange(await FillComboBoxAsync("EmailSenders", () => pageEmailSendersClient.EmailSendersGetAsync()));
            comboBoxList.AddRange(await FillComboBoxAsync("OtpTypes", () => pageOtpTypesClient.OtpTypesGetAsync()));
            return comboBoxList;
        }
    }
}