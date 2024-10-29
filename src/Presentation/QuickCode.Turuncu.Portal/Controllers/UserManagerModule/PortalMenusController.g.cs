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
using QuickCode.Turuncu.Portal.Models.UserManagerModule;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using UserManagerModuleContracts = QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
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

namespace QuickCode.Turuncu.Portal.Controllers.UserManagerModule
{
    [Permission("UserManagerModulePortalMenus")]
    [Area("UserManagerModule")]
    [Route("UserManagerModulePortalMenus")]
    public partial class PortalMenusController : BaseController
    {
        private int pageSize = 20;
        private readonly UserManagerModuleContracts.IPortalMenusClient pageClient;
        public PortalMenusController(UserManagerModuleContracts.IPortalMenusClient pageClient, UserManagerModuleContracts.ITableComboboxSettingsClient tableComboboxSettingsClient, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache cache) : base(tableComboboxSettingsClient, httpContextAccessor, mapper, cache)
        {
            this.pageClient = pageClient;
        }

        [ResponseCache(VaryByQueryKeys = new[] { "ic" }, Duration = 30)]
        public async Task<IActionResult> GetImage(string ic)
        {
            return await GetImageResult(pageClient, ic);
        }

        [Route("List")]
        public async Task<IActionResult> List()
        {
            var model = GetModel<PortalMenusData>();
            model.PageSize = pageSize;
            model.CurrentPage = 1;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            var listResponse = (await pageClient.PortalMenusGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<PortalMenusObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            return View("List", model);
        }

        [Route("List")]
        [HttpPost]
        public async Task<IActionResult> List(PortalMenusData model)
        {
            ModelBinder(ref model);
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            if (model.CurrentPage == Int32.MaxValue)
            {
                model.CurrentPage = model.TotalPage;
            }

            var listResponse = (await pageClient.PortalMenusGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<PortalMenusObj>>(listResponse.ToList());
            SetModelBinder(ref model);
            model.SelectedItem = new PortalMenusObj();
            return View("List", model);
        }

        [Route("Insert")]
        [HttpPost]
        public async Task<IActionResult> Insert(PortalMenusData model)
        {
            ModelBinder(ref model);
            var selected = mapper.Map<UserManagerModuleContracts.PortalMenusDto>(model.SelectedItem);
            var result = await pageClient.PortalMenusPostAsync(selected);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(PortalMenusData model)
        {
            ModelBinder(ref model);
            var request = mapper.Map<UserManagerModuleContracts.PortalMenusDto>(model.SelectedItem);
            var result = await pageClient.PortalMenusPutAsync(request.Id, request);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(PortalMenusData model)
        {
            ModelBinder(ref model);
            var request = model.SelectedItem;
            var result = await pageClient.PortalMenusDeleteAsync(request.Id);
            SetModelBinder(ref model);
            return Ok(result);
        }

        [Route("InsertItem")]
        public async Task<IActionResult> InsertItem(PortalMenusData model)
        {
            ModelState.Clear();
            ModelBinder(ref model);
            SetModelBinder(ref model);
            model.SelectedItem = new PortalMenusObj();
            return PartialView("Insert", model);
        }

        [Route("DetailItem")]
        public async Task<IActionResult> DetailItem(PortalMenusData model)
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
        public async Task<IActionResult> UpdateItem(PortalMenusData model)
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
        public async Task<IActionResult> DeleteItem(PortalMenusData model)
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

        private async Task<PortalMenusData> FillModel(PortalMenusData model)
        {
            model.PageSize = pageSize;
            model.NumberOfRecord = (await pageClient.CountAsync());
            model.TotalPage = (model.NumberOfRecord / model.PageSize) + (model.NumberOfRecord % model.PageSize == 0 ? 0 : 1);
            var listResponse = (await pageClient.PortalMenusGetAsync(model.CurrentPage, model.PageSize));
            model.List = mapper.Map<List<PortalMenusObj>>(listResponse.ToList());
            return model;
        }
    }
}