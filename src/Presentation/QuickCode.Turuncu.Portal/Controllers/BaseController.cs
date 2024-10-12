using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Net.Http;
using System.Net;
using QuickCode.Turuncu.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Caching.Memory;
using QuickCode.Turuncu.Portal.Models;

namespace QuickCode.Turuncu.Portal.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMemoryCache cache;
        protected readonly IMapper mapper;
        protected IHttpContextAccessor httpContextAccessor;
        private readonly ITableComboboxSettingsClient tableComboboxSettingsClient;
        
        public BaseController(ITableComboboxSettingsClient tableComboboxSettingsClient, IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache cache)
        {
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.tableComboboxSettingsClient = tableComboboxSettingsClient;
            this.cache = cache;
        }
        
        protected async Task<Dictionary<string, IEnumerable<SelectListItem>>> FillComboBoxAsync<T>(string key, Func<Task<ObservableCollection<T>>> getDataFunc)
        {
            var cacheKey = $"{key}ComboList";
            var comboBoxList = new Dictionary<string, IEnumerable<SelectListItem>>();
            if (!cache.TryGetValue(cacheKey, out List<SelectListItem> cachedComboList))
            {
                var data = await getDataFunc();
                cachedComboList = data.AsMultiSelectList(GetComboBoxList(key)).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                cache.Set(cacheKey, cachedComboList, cacheEntryOptions);
            }

            comboBoxList.Add(key, cachedComboList);
            return comboBoxList;
        }

        public string GetErrorDescription(int errorCode)
        {
            return GetErrorDescription(errorCode.ToString());
        }

        public string GetErrorDescription(object errorCode)
        {
            if (errorCode == null)
            {
                return "Success";
            }

            return GetErrorDescription(errorCode.ToString());
        }

        protected ComboBoxFormatData GetComboBoxList(string tableName)
        {
            var returnValue = new ComboBoxFormatData();
            var comboBoxSettings = tableComboboxSettingsClient.TableComboboxSettingsGetAsync().Result;
            var tableSetting = comboBoxSettings.FirstOrDefault(i => i.TableName == tableName);
            returnValue.StringFormat = tableSetting!.StringFormat;
            returnValue.ValueField = tableSetting.IdColumn;
            returnValue.TextFields = tableSetting.TextColumns.Split('|').Select(i => i).ToArray();
            return returnValue;
        }

        public const string ModelBinderModelName = "model";

        private bool IsRedirectToAction
        {
            get
            {
                if (!TempData.Keys.Contains("IsRedirectToAction"))
                {
                    TempData["IsRedirectToAction"] = false;
                }

                return (bool)TempData["IsRedirectToAction"];
            }
            set
            {
                TempData["IsRedirectToAction"] = value;
            }
        }


        public override LocalRedirectResult LocalRedirectPermanent(string localUrl)
        {
            IsRedirectToAction = true;
            return base.LocalRedirectPermanent(localUrl);
        }

        public override RedirectResult RedirectPermanent(string url)
        {
            IsRedirectToAction = true;
            return base.RedirectPermanent(url);
        }

        public override RedirectToRouteResult RedirectToRoute(string routeName)
        {
            return base.RedirectToRoute(routeName);
        }



        private bool UserIsAuthenticated()
        {
            if (HttpContext.Session.Get<string>("SessionInfo") == null)
            {
                return false;
            }

            return true;
        }

        protected SortedDictionary<DateTime, ActionInfo> actionHistory = null;
        protected SortedDictionary<DateTime, ActionInfo> ActionHistory
        {
            get
            {
                if (actionHistory == null)
                {
                    actionHistory = new SortedDictionary<DateTime, ActionInfo>();
                }

                if (TempData.Get<object>("ActionHistory") == null)
                {
                    TempData.Put("ActionHistory", actionHistory);
                }

                actionHistory = TempData.Get<SortedDictionary<DateTime, ActionInfo>>("ActionHistory");

                return actionHistory;
            }
        }



     

        protected ActionInfo GetLastAction()
        {
            ActionInfo lastAction = null;
            var actionList = (from A in ActionHistory
                              where A.Value.ControllerName != "Error" && !A.Value.IsRedirectToAction
                              orderby A.Key descending
                              select A);

            if (actionList.Count() > 0)
            {
                lastAction = actionList.First().Value;
            }

            TempData.Put("ActionHistory", ActionHistory);
            return lastAction;
        }

        public class ActionInfo
        {
            public string ActionName { get; set; }
            public string ControllerName { get; set; }
            public string HttpMethod { get; set; }

            public string URL { get; set; }
            public bool IsRedirectToAction { get; set; }
        }

  

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (Request.Method == "GET" && !IsRedirectToAction && !Request.Path.Value.ToString().EndsWith("GetImage", StringComparison.InvariantCulture))
            {
                ClearModelBinderData();
            }


            IsRedirectToAction = false;

            //}

            base.OnActionExecuting(context);

        }

        protected bool IsBackAction()
        {
            return Request.Form["submitAction"] == "Back";
        }

        protected async Task<object> GetItemValue(object serviceClient, string[] parameterKey)
        {
            MethodInfo methodInfo = serviceClient.GetType().GetMethod("GetItemAsync");
            var parameters = new List<object>();
            var methodParameters = methodInfo.GetParameters();
            var parameterAsString = parameterKey[0].Split('|');
            for (int i = 0; i < methodInfo.GetParameters().Length; i++)
            {
                if (methodParameters[i].ParameterType != typeof(CancellationToken))
                {
                    Type t = Nullable.GetUnderlyingType(methodParameters[i].ParameterType) ?? methodParameters[i].ParameterType;
                    object safeValue = (parameterAsString[i] == null) ? null : Convert.ChangeType(parameterAsString[i], t);
                    parameters.Add(safeValue);
                }
                else
                {
                    parameters.Add(null);
                }
            }

            dynamic methodReturnValue = methodInfo.Invoke(serviceClient, parameters.ToArray());
            var item = methodReturnValue.Result;
            var propValue = item.GetType().GetProperty(parameterKey[1]);
            var returnValue = propValue.GetValue(item);

            return await Task.FromResult(returnValue);
        }

        protected async Task<IActionResult> GetImageResult(object serviceClient, string ic)
        {
            var imageValue = await GetItemValue(serviceClient, ic.Split("_"));

            if (imageValue == null)
            {
                string path = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\images"}";

                return base.File(path + "\no_image.png", "image/png");
            }

            return File(imageValue as byte[], "image/png");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //İşlem logu atılır tüm actionlar için

            base.OnActionExecuted(context);
        }


        public T GetModel<T>() where T : class
        {
            Type modelType = typeof(T);

            T mainModel = default(T);

            var tempDataValue = TempData.Get<object>(ModelBinderModelName);

            if (tempDataValue != null && modelType == tempDataValue.GetType())
            {
                mainModel = TempData.Get<T>(ModelBinderModelName);
            }

            if (mainModel == null)
            {
                mainModel = Activator.CreateInstance<T>();
            }

            TempData.Put(ModelBinderModelName, mainModel);
            return mainModel;
        }
         

        /// <summary>
        /// remove all controller temp data except the given  key
        /// </summary>
        public void ClearModelBinderData()
        {
            TempData.Remove(ModelBinderModelName);
        }

        public T ModelBinder<T>(ref T requestModel) where T : class
        {
            return ModelBinder<T>(ref requestModel, ModelBinderModelName);
        }

        public T SetModelBinder<T>(ref T requestModel) where T : class
        {
            return SetModelBinder<T>(ref requestModel, ModelBinderModelName);
        }

        public T SetModelBinder<T>(ref T requestModel, string dataKey) where T : class
        {
            TempData.Put(dataKey, requestModel);
            TempData.Keep(dataKey);
            return requestModel;
        }

        public T ModelBinder<T>(ref T requestModel, string dataKey) where T : class
        {
            Type modelType = typeof(T);

            var tempDataValue = TempData.Get<T>(dataKey);

            if (!TempData.Keys.Contains(dataKey) || modelType != tempDataValue.GetType())
            //if (!TempData.Keys.Contains(dataKey) || modelType != tempDataValue.GetType() || Request.ContentType == null)
            {
                TempData.Put(dataKey, requestModel);
            }

            T mainModel = TempData.Get<T>(dataKey);

            if (requestModel != null && Request.ContentType != null)
            {
                foreach (string key in Request.Form.Keys)
                {
                    PropertyInfo pi = modelType.GetProperty(key);
                    if (pi != null)
                    {
                        object propertyValue = pi.GetValue(requestModel, null);
                        pi.SetValue(mainModel, propertyValue, null);
                    }
                    else
                    {
                        object val = GetValue(modelType, key, requestModel);
                        string requestFormKeyValue = null;
                        if (Request.Form.Keys.Contains(key))
                        {
                            requestFormKeyValue = Request.Form[key];
                        }

                        if (val == null && requestFormKeyValue != null)
                        {
                            SetValue(modelType, key, mainModel, requestFormKeyValue);
                        }
                        else
                        {
                            SetValue(modelType, key, mainModel, val);
                        }
                    }
                }

                var hasImageProperty = requestModel.GetValueFromProperty("SelectedItem.ImageColumnNames");
                if (hasImageProperty != null)
                {
                    var imageColumnNames = hasImageProperty as List<string>;

                    foreach (var imageName in imageColumnNames)
                    {
                        var key = $"SelectedItem.{imageName}";
                        if (Request.Form.Files[key] != null)
                        {
                            var fileItem = Request.Form.Files[key];

                            var currentValue = GetValue(modelType, key, mainModel);
                            if (currentValue is byte[])
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    fileItem.CopyTo(memoryStream);
                                    var imageData = memoryStream.ToArray();
                                    //var resizedImageData = CreateThumbnail(1000, 1000, imageData);
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        SetValue(modelType, key, mainModel, imageData);
                                    }
                                }
                            }
                            //else if (currentValue is string)
                            else
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    fileItem.CopyTo(memoryStream);
                                    var imageData = memoryStream.ToArray();
                                    //var resizedImageData = CreateThumbnail(1000, 1000, imageData);
                                    if (imageData != null && imageData.Length > 0)
                                    {
                                        var fileName = UploadImageAsBlob(fileItem, memoryStream);
                                        SetValue(modelType, key, mainModel, fileName);
                                    }
                                }

                            }
                        }
                        else
                        {
                            var tValue = TempData.Get<T>(dataKey);
                            var lastImageValue = tValue.GetValueFromProperty(key);
                            if(lastImageValue!=null && lastImageValue is String)
                            {
                                SetValue(modelType, key, mainModel, lastImageValue);
                            }
                        }
                    }
                }
            }

            TempData.Put(dataKey, mainModel);
            requestModel = mainModel;
            return mainModel;
        }

        private string UploadImageAsBlob(IFormFile file, MemoryStream memoryStream)
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=quickcodeblobstorage;AccountKey=WfSqJBsJMcQZJnqqTlUHTbL/sh/gsZK5d/Ig9zxqno7O+VgEvxkHargRG/27UUE4sSDc1M3HMI2x+AStBHcxSA==;EndpointSuffix=core.windows.net";
            string baseUrl = "https://quickcodeblobstorage.blob.core.windows.net";
            string containerName = "images";
            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerName);
            var isContainerCreated = container.CreateIfNotExistsAsync().Result;
            string newFileName = $"{Guid.NewGuid()}.{file.ContentType.Split('/').Last()}";
   
            BlobClient blob = container.GetBlobClient(newFileName);

            memoryStream.Position = 0;
            var result = blob.UploadAsync(memoryStream, new BlobHttpHeaders() { ContentType = file.ContentType }).Result;
            blob.SetAccessTierAsync(AccessTier.Cool);
            return $"{baseUrl}/{containerName}/{newFileName}";
        }

#pragma warning disable CA1416 // Validate platform compatibility
        public byte[] CreateThumbnail(int maxWidth, int maxHeight, byte[] imageData)
        {
            Bitmap image;
            using (var ms = new MemoryStream(imageData))
            {
                image = new Bitmap(ms);
            }

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            Graphics thumbGraph = Graphics.FromImage(newImage);

            thumbGraph.CompositingQuality = CompositingQuality.HighSpeed;
            thumbGraph.SmoothingMode = SmoothingMode.HighSpeed;
            //thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
           
            thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
            image.Dispose();
            return ImageToByte(newImage);
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
#pragma warning restore CA1416 // Validate platform compatibility

        public object GetValue(Type type, object val)
        {
            Type t = Nullable.GetUnderlyingType(type) ?? type;
            if (val != null && typeof(bool) == t && val.ToString() == "true,false")
            {
                return true;
            }

            if (type.IsEnum)
            {
                return Enum.Parse(type, val.ToString());
            }

            if (val.GetType() == typeof(string) && t != typeof(string) && val.ToString() == string.Empty)
            {
                return null;
            }

            return (val == null) ? null : Convert.ChangeType(val, t);
        }

        public object GetValue(object value, string propertyName)
        {
            object obj = value;
            if (obj == null)
            {
                return null;
            }

            string[] subPropertyNames = propertyName.Split(new char[] { '.' });

            foreach (string property in subPropertyNames)
            {

            }

            return obj;
        }


        public object ClearBytes(object obj, Type objType)
        {
            if (obj == null && objType != typeof(byte[]))
            {
                return null;
            }

            if (objType == typeof(byte[]))
            {
                if (obj != null && (obj as byte[]).Length>2)
                {
                    return new byte[2];
                }
                else
                {
                    return new byte[1];
                }
                
            }

            var type = obj.GetType();

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                dynamic enumerable = obj;

                if (enumerable != null)
                {
                    var arrayItem = enumerable.ToArray();
                    for (int i = 0; i < arrayItem.Length; i++)
                    {
                        arrayItem[i] = ClearBytes(arrayItem[i], arrayItem[i].GetType());
                    }
                }
            }
            else if ((type.IsClass || type.IsArray) && type != typeof(string) && !type.ToString().Equals("System.Object"))
            {
                if (type.IsArray)
                {
                    var arrayItem = (object[])obj;
                    for (int i = 0; i < arrayItem.Length; i++)
                    {
                        arrayItem[i] = ClearBytes(arrayItem[i], arrayItem[i].GetType());
                    }
                }
                else
                {
                    var properties = type.GetProperties();
                    foreach (var prop in properties)
                    {
                        prop.SetValue(obj, ClearBytes(prop.GetValue(obj), prop.PropertyType));
                    }
                }
            }

            return obj;
        }



        private T[] GetArray<T>(IList<T> iList) where T : new()
        {
            var result = new T[iList.Count];

            iList.CopyTo(result, 0);

            return result;
        }

        private object SetUserDataPasword(Type type,string key,object keyValue)
        {
            if(type.ToString().EndsWith(".UserData") && key == "SelectedItem.Password")
            {
                return keyValue.ToString().EncryptWithHash();
            }

            return keyValue;
        }
        public void SetValue(Type type, string key, object valueObject, object keyValue)
        {
            string[] subItems = key.Split(new char[] { '.' });
            keyValue = SetUserDataPasword(type, key, keyValue);
            if (subItems.Length > 0 && (key.Contains(".") || key.Contains("[")))
            {
                string item = subItems[0];
                if (item.Contains("["))
                {
                    string propertyName = item.Split(new char[] { '[' })[0];
                    int arrayIndex = Convert.ToInt32(item.Replace(propertyName, string.Empty).Replace("[", string.Empty).Replace("]", string.Empty));
                    PropertyInfo pi = type.GetProperty(propertyName);
                    if (pi != null)
                    {
                        object val = null;
                        Type valueType = pi.PropertyType;
                        Array array;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            var arrayValue = pi.GetValue(valueObject, null);
                            MethodInfo miArray = arrayValue.GetType().GetMethod("ToArray");
                            array = (miArray.Invoke(arrayValue, null) as Array);
                        }
                        else
                        {
                            array = (pi.GetValue(valueObject, null) as Array);
                        }


                        val = array.GetValue(arrayIndex);
                        valueType = array.GetType().GetElementType();
                        if (!key.Contains("."))
                        {
                            array.SetValue(GetValue(valueType, keyValue), arrayIndex);
                            pi.SetValue(valueObject, array.ToList(), null);
                        }

                        SetValue(valueType, key.Replace(item + ".", string.Empty), val, keyValue);
                    }
                }
                else
                {
                    string propertyName = item;
                    PropertyInfo pi = type.GetProperty(propertyName);
                    if (pi != null)
                    {
                        object val = null;

                        val = pi.GetValue(valueObject, null);
                        if (val == null)
                        {
                            val = Activator.CreateInstance(pi.PropertyType);
                            pi.SetValue(valueObject, val, null);
                        }
                        object subValue = GetPropValue(keyValue, key.Replace(item + ".", string.Empty));
                        if (subValue == null)
                        {
                            subValue = keyValue;
                        }
                        if (val != null)
                        {
                            SetValue(val.GetType(), key.Replace(item + ".", string.Empty), val, subValue);
                        }
                    }
                }
            }
            else
            {
                PropertyInfo pi = type.GetProperty(key);

                if ( pi != null)
                {
                    var newValue = GetValue(pi.PropertyType, keyValue);
                    if(!(pi.PropertyType == typeof(byte[]) && newValue == null))
                    {
                        pi.SetValue(valueObject, newValue, null);
                    }
                  
                }
            }

        }



        public static object GetPropValue(object obj, string name)
        {
            foreach (string part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }

            return obj;
        }
        public object GetValue(Type type, string key, object valueObject)
        {
            string[] subItems = key.Split(new char[] { '.' });

            if (subItems.Length > 0 && key.Contains("."))
            {
                string item = subItems[0];
                if (item.Contains("["))
                {
                    string propertyName = item.Split(new char[] { '[' })[0];
                    int arrayIndex = Convert.ToInt32(item.Replace(propertyName, string.Empty).Replace("[", string.Empty).Replace("]", string.Empty));
                    PropertyInfo pi = type.GetProperty(propertyName);
                    if (pi != null)
                    {
                        object val = null;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            val = pi.GetValue(valueObject, null);
                            if (val == null)
                            {
                                return null;
                            }

                            IEnumerable<object> array = val as IEnumerable<object>;

                            if (array.Count() > 0)
                            {
                                val = array.ElementAt(arrayIndex);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            val = (pi.GetValue(valueObject, null) as Array).GetValue(arrayIndex);
                        }

                        return GetValue(val.GetType(), key.Replace(item + ".", string.Empty), val);
                    }
                }
                else
                {
                    PropertyInfo pi = type.GetProperty(item);
                    if (pi != null)
                    {
                        object tempValue = pi.GetValue(valueObject, null);
                        if (tempValue != null)
                        {
                            string newKey = String.Join(".", subItems, 1, subItems.Length - 1);
                            return GetValue(tempValue.GetType(), newKey, tempValue);
                        }
                    }
                }
            }
            else
            {
                PropertyInfo pi = type.GetProperty(key);
                if (pi != null)
                {
                    return pi.GetValue(valueObject, null);
                }
            }

            return null;
        }
        public object GetValue2(Type type, string key, object valueObject)
        {
            string[] subItems = key.Split(new char[] { '.' });

            if (subItems.Length > 0 && key.Contains("."))
            {
                string item = subItems[0];
                if (item.Contains("["))
                {
                    string propertyName = item.Split(new char[] { '[' })[0];
                    int arrayIndex = Convert.ToInt32(item.Replace(propertyName, string.Empty).Replace("[", string.Empty).Replace("]", string.Empty));
                    PropertyInfo pi = type.GetProperty(propertyName);
                    if (pi != null)
                    {
                        object val = null;
                        if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                        {
                            val = pi.GetValue(valueObject, null);
                            if (val == null)
                            {
                                return null;
                            }

                            IEnumerable<object> array = val as IEnumerable<object>;

                            if (array.Count() > 0)
                            {
                                val = array.ElementAt(arrayIndex);
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            val = (pi.GetValue(valueObject, null) as Array).GetValue(arrayIndex);
                        }

                        return GetValue(val.GetType(), key.Replace(item + ".", string.Empty), val);
                    }
                }
                else
                {
                    PropertyInfo pi = type.GetProperty(item);
                    if (pi != null)
                    {
                        object tempValue = pi.GetValue(valueObject, null);
                        if (tempValue != null)
                        {
                            string newKey = String.Join(".", subItems, 1, subItems.Length - 1);
                            return GetValue(tempValue.GetType(), newKey, tempValue);
                        }
                    }
                }
            }
            else
            {
                PropertyInfo pi = type.GetProperty(key);
                if (pi != null)
                {
                    return pi.GetValue(valueObject, null);
                }
            }

 
            return null;
        }

    }
}
