namespace QuickCode.Turuncu.EmailManagerModule.Api.Helpers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Globalization;
    using System.Data;
    using System.Collections;
    using System.Xml.Linq;
    using System.Linq;
    using System.Linq.Expressions;
    using System.IO.Compression;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.ComponentModel;
    using Common.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using QuickCode.Turuncu.EmailManagerModule.Application.Interfaces.Repositories;
    using QuickCode.Turuncu.EmailManagerModule.Persistence.Contexts;

    public static class ExtensionLibrary
    {
        #region Extension Methods
        public static void AddQuickCodeRepositories(this IServiceCollection services)
        {
            var repoInterfaces = typeof(IBaseRepository<>).Assembly.GetTypes().Where(i => i.Name.EndsWith("Repository") && i.IsInterface);
            foreach (var interfaceType in repoInterfaces)
            {
                var implementationType = typeof(WriteDbContext).Assembly.GetTypes().First(i => i.Name == interfaceType.Name[1..]);
                services.AddScoped(interfaceType, implementationType);
            }
        }
        
        public static string GetParentDirectory(this string path)
        {
            return Directory.GetParent(path)!.FullName;
        }

        public static string UrlSlugify(this string phrase)
        {
            string str = phrase.ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars          
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space  
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim(); // cut and trim it  
            str = Regex.Replace(str, @"\s", "-"); // hyphens  

            return str;
        }

        public static string ReplaceRegEx(this string obj,string pattern, string replacement, RegexOptions regexOptions = RegexOptions.IgnoreCase)
        {
            return Regex.Replace(obj, pattern, replacement, regexOptions);
        }

        public static bool IsNullOrEmpty(this string obj)
        {
            return String.IsNullOrEmpty(obj);
        }
        public static string ToDescriptionString(this Enum val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string ToDescriptionString<T>(this T val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Object -> String : Object null ise string.Empty döner
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>string object</returns>
        public static string AsString(this object obj)
        {
            if (obj == null)
            {
                obj = string.Empty;
            }

            return obj.ToString();
        }

        public static string ClearPhoneText(this string obj)
        {
            return obj.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);
        }

        /// <summary>
        /// Get xmlDocument inner text
        /// </summary>
        /// <param name="obj">XmlDocument object</param>
        /// <param name="path">XMl node path</param>
        /// <returns></returns>
        public static string GetInnerText(this XmlDocument obj, string path)
        {
            if (obj != null)
            {
                return obj.SelectSingleNode(path).InnerText;
            }

            return string.Empty;
        }



        public static IEnumerable<string> SplitOnCapitals(this string text)
        {
            Regex regex = new Regex(@"\p{Lu}\p{Ll}*");
            foreach (Match match in regex.Matches(text))
            {
                yield return match.Value;
            }
        }

        public static string SplitCamelCaseToString(this string source)
        {
            const string pattern = @"[A-Z][a-z]*|[a-z]+|\d+";
            var matches = Regex.Matches(source, pattern);
            return String.Join(" ", matches);
        }

        public static string SplitCamelCaseNoSpace(this string source)
        {
            const string pattern = @"[A-Z][a-z]*|[a-z]+|\d+";
            var matches = Regex.Matches(source, pattern);
            return String.Join("", matches);
        }

        public static IEnumerable<string> SplitCamelCase(this string source)
        {
            const string pattern = @"[A-Z][a-z]*|[a-z]+|\d+";
            var matches = Regex.Matches(source, pattern);
            foreach (Match match in matches)
            {
                yield return match.Value;
            }
        }

        public static string AsSplitCapitalWithUnderline(this string text)
        {
            StringBuilder sb = new StringBuilder();
            Array list=SplitOnCapitals(text).ToArray();

            for (int i = 0; i < list.Length; i++)
            {
                 sb.AppendFormat("{0}{1}", list.GetValue(i).ToString().ToUpper(CultureInfo.InvariantCulture), i != list.Length - 1 ? "_" : string.Empty);
            }

            return  sb.ToString();
        }

        public static string  Check<T>(Expression<Func<T>> expr)
        {
            var body = ((MemberExpression)expr.Body);
            StringBuilder sb = new StringBuilder();
             sb.AppendFormat("Name is: {0}", body.Member.Name);
             sb.AppendFormat("Value is: {0}", ((FieldInfo)body.Member)
           .GetValue(((ConstantExpression)body.Expression).Value));
            return  sb.ToString();
        }

        public static string GetEnumName(this object obj)
        {
            Type objType = obj.GetType();
            if (objType.IsEnum)
            {
                return Enum.GetName(objType, obj);
            }

            return string.Empty;
        }

        public static bool IsInList(this string obj, params string[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == obj)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsInAny<T>(this T obj, params T[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Equals(obj))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsInAll<T>(this T obj, params T[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                if (!list[i].Equals(obj))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Object -> String : Object null ise string.Empty döner
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="defaultValue">default value</param>
        /// <returns>string object</returns>
        public static string AsString(this object obj, string defaultValue)
        {
            if (string.IsNullOrEmpty(obj.AsString()))
            {
                obj = defaultValue;
            }

            return obj.ToString();
        }

        /// <summary>
        /// Object -> Int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>integer object</returns>
        public static int AsInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static string JsonSerializeObject(this object data)
        {
            Type type = data.GetType();
            JsonSerializerSettings serializer = new Newtonsoft.Json.JsonSerializerSettings();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.ContractResolver = new DynamicContractResolver(typeof(byte[]));
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            //serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
            //serializer.TypeNameHandling = TypeNameHandling.None;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            
            serializer.Converters.Add(new StringEnumConverter { AllowIntegerValues = true });
            string jsonData = JsonConvert.SerializeObject(data, type, serializer);
            return jsonData;
        }

        public static object JsonDeserializeObject(this string objectString, Type returnType)
        {
            JsonSerializerSettings serializer = new Newtonsoft.Json.JsonSerializerSettings();
            serializer.Converters.Add(new Newtonsoft.Json.Converters.JavaScriptDateTimeConverter());
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            //serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;

            object obj = JsonConvert.DeserializeObject(objectString, returnType, serializer); ;
            return obj;
        }

        /// <summary>
        /// Object -> Int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static int AsInt32(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsInt32();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }

        /// <summary>
        /// Object -> Int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static Type AsType(this DataRow obj, string objectName)
        {
            try
            {
                return Type.GetType(obj[objectName].AsString());
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }


        public static object GetValue(this DataRow obj, string columnName, object value)
        {
            object returnObject = value;

            if (obj.Table.Columns.Contains(columnName))
            {
                if (value.Equals(-1))
                {
                    returnObject = obj[columnName];
                }
            }

            return returnObject;

        }

        /// <summary>
        /// As Time Ticks
        /// </summary>
        /// <param name="obj">Stopwatch param</param>
        /// <returns>time tick obj</returns>
        public static double AsTimeTicks(this Stopwatch obj)
        {
            return (double)(1000000000 * obj.ElapsedTicks / Stopwatch.Frequency) / 1000000;
        }

        /// <summary>
        /// Object -> Int64
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>returns long</returns>
        public static long AsInt64(this object obj)
        {
            return Convert.ToInt64(obj);
        }



        /// <summary>
        /// Object -> Int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static long AsInt64(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsInt64();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }


        /// <summary>
        /// Object -> Double
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>returns double</returns>
        public static double AsDouble(this object obj)
        {
            double returnValue = 0;
            try
            {
                if (obj.GetType() == typeof(string))
                {
                    string NumberDecimalSeparator = ".";
                    if (obj.AsString().Contains(","))
                    {
                        NumberDecimalSeparator = ",";
                    }

                    NumberFormatInfo info = new NumberFormatInfo();
                    info.NumberDecimalSeparator = NumberDecimalSeparator;
                    returnValue = Convert.ToDouble(obj, info);
                }
                else
                {
                    returnValue = Convert.ToDouble(obj);
                }
            }
            catch
            {

            }

            return returnValue;
        }

        public static Decimal AsDecimal(this double obj)
        {
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// Object -> AsDouble
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static double AsDouble(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsDouble();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }

        /// <summary>
        /// Object -> Date (Saat değeri bulunmuyor)
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>returns date</returns>
        public static DateTime AsDate(this object obj)
        {
            return Convert.ToDateTime(obj).Date;
        }

        public static DataSet AsDataSet(this string obj)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string columnName="COLUMN_1";
            dt.Columns.Add(columnName);

            DataRow dr = dt.NewRow();
            dr[columnName] = obj;
            ds.Tables.Add(dt);

            return ds;
        }

        public static bool IsInt32(this object obj)
        {
            Int32 val = 0;
            bool isInt32 = false;
            try
            {
                isInt32 = Int32.TryParse(obj.ToString(), out val);

            }
            catch
            {
                isInt32 = false;
            }

            return isInt32;
        }

        public static bool IsInt64(this object obj)
        {
            Int64 val = 0;
            bool isInt64 = false;
            try
            {
                isInt64 = Int64.TryParse(obj.ToString(), out val);

            }
            catch
            {
                isInt64 = false;
            }

            return isInt64;
        }

        public static object AsNull(this object obj)
        {
            object t = obj;

            if (t == DBNull.Value)
            {
                t = null;
            }

            return t;

        }

        /// <summary>
        /// Object -> AsDouble
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static DateTime AsDate(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsDate();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }

        /// <summary>
        /// Object -> DateTime
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>returns date time</returns>
        public static DateTime AsDateTime(this object obj)
        {
            return Convert.ToDateTime(obj);
        }


        /// <summary>
        /// Object -> AsDouble
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static DateTime AsDateTime(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsDateTime();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }

        /// <summary>
        /// byte[] to Assembly
        /// </summary>
        /// <param name="obj">Byte array object</param>
        /// <returns>Assembly object</returns>
        public static Assembly AsAssembly(this byte[] obj)
        {
            return Assembly.Load(obj);
        }

        public static StringBuilder SetString(this StringBuilder obj, string text)
        {
            string s = obj.ToString();
            obj.Clear();
            obj.AppendFormat("{0}{1}", s, text);
            return obj;
        }

        public static string GetObjectDetails(this object o)
        {
            StringBuilder returnString = new StringBuilder();

            if (o != null && o.GetType().IsArray)
            {
                if (o.GetType().FullName == "System.Byte[]")
                {
                    returnString.AppendFormat(string.Format("<{0}>/{0}>", "ByteArray"));
                }
                else
                {
                    Array a = (o as Array);
                    for (int i = 0; i < a.Length; i++)
                    {
                        returnString.Append(string.Format("<{0}{1}>{2}</{0}{1}>", "Index", i, GetObjectDetails(a.GetValue(i))));
                    }

                    returnString.Insert(0, string.Format("<{0}>", "Array"));
                    returnString.AppendFormat(string.Format("</{0}>", "Array"));
                }
            }
            else
            {
                string s = (o == null ? "param_null" : o.AsString());
                returnString.AppendFormat("{0}", s);
            }

            return returnString.ToString();
        }

        public static void SaveAssembly(this byte[] obj, string filepath)
        {
            File.WriteAllBytes(filepath, obj);
        }

        /// <summary>
        /// object to Assembly
        /// </summary>
        /// <param name="obj">sender object</param>
        /// <returns>returns assembly</returns>
        public static Assembly AsAssembly(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return Assembly.Load(obj as byte[]);
        }

        public static bool IsContains(this string obj, params string[] list)
        {
            var result = list.Any(i => obj.Contains(i));
            return result;
        }

        public static bool IsAllContains(this string obj, params string[] list)
        {
            var result = list.Where(i => obj.Contains(i));
            return result.Count() == list.Count();
        }

        public static bool IsContains(this string obj, string containString, bool caseSensitive)
        {
            StringComparison compareType = StringComparison.InvariantCultureIgnoreCase;
            if (caseSensitive)
            {
                compareType = StringComparison.InvariantCulture;
            }

            return obj.IndexOf(containString, compareType) >= 0;
        }

        public static bool IsContains(this string obj, string containString)
        {
            return obj.IsContains(containString, false);
        }


       
        /// <summary>
        /// DateTime -> IsoDateTime (string) 
        /// </summary>
        /// <param name="obj">datetime param</param>
        /// <returns>returns iso date time</returns>
        public static string AsIsoDateTime(this DateTime obj)
        {
            return string.Format("{0:s}", obj);
        }

        /// <summary>
        /// string -> int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <returns>returns integer</returns>
        public static int AsInt32(this string obj)
        {
            return Convert.ToInt32(obj);
        }

        public static string AsString(this string obj)
        {
            return obj ?? string.Empty;
        }

        public static string AsString(this StringBuilder obj, int maxLenght)
        {
            if (obj.ToString().Length > 32700)
            {
                return obj.ToString().Substring(0, 32700);
            }

            return obj.ToString();
        }

        public static string AsString(this StringBuilder obj)
        {
            return obj.ToString();
        }

        public static string AsString(this object obj, int maxLenght)
        {
            if (obj == null)
            {
                obj = string.Empty;
            }

            if (obj.ToString().Length > 32700)
            {
                return obj.ToString().Substring(0, 32700);
            }

            return obj.ToString();
        }

        public static string AsStringXml(this DataSet obj)
        {
            StringWriter sw = new StringWriter();
            obj.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            return sw.ToString();
        }
        
        public static string AsString(this int obj)
        {
            return obj.ToString();
        }

        public static string AsString(this double obj)
        {
            return obj.ToString();
        }

        public static string AsString(this float obj)
        {
            return obj.ToString();
        }

        public static bool AsBoolean(this object obj)
        {
            return Convert.ToBoolean(obj);
        }

        public static object AsObject(this object obj)
        {
            return obj == DBNull.Value ? null : obj;
        }

        public static object AsDbObject(this object obj)
        {
            return obj == null ? DBNull.Value : obj;
        }


        /// <summary>
        /// Object -> Int32
        /// </summary>
        /// <param name="obj">object param</param>
        /// <param name="objectName">object name</param>
        /// <returns>integer object</returns>
        public static bool AsBoolean(this DataRow obj, string objectName)
        {
            try
            {
                return obj[objectName].AsBoolean();
            }
            catch (Exception ex)
            {
                string exMessage = string.Format("{0} Could not convert to Type = {1} ObjectValue={2}\nException={3}", objectName, System.Reflection.MethodBase.GetCurrentMethod().Name, obj[objectName], ex.Message);
                throw new Exception(exMessage);
            }
        }

        public static bool AsBoolean(this string obj)
        {
            return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Saat ile birlikte ISO->DateTime çevrimi yapar
        /// </summary>
        /// <param name="dateTimeString">ISO formatta tarih bilgisi</param>
        /// <returns>DateTime tipinde tarih bilgisi</returns>
        public static DateTime ConvertFromISOFormatToDateTimeWithTime(this string dateTimeString)
        {
            return new DateTime(Convert.ToInt32(dateTimeString.Substring(0, 4)), Convert.ToInt32(dateTimeString.Substring(5, 2)), Convert.ToInt32(dateTimeString.Substring(8, 2)), Convert.ToInt32(dateTimeString.Substring(11, 2)), Convert.ToInt32(dateTimeString.Substring(14, 2)), Convert.ToInt32(dateTimeString.Substring(17, 2)));
        }

        /// <summary>
        /// Saat olmaksızın ISO->DateTime çevrimi yapar
        /// </summary>
        /// <param name="dateTimeString">ISO formatta tarih bilgisi</param>
        /// <returns>DateTime tipinde tarih bilgisi</returns>
        public static DateTime ConvertFromISOFormatToDateTimeWithoutTime(this string dateTimeString)
        {
            return new DateTime(Convert.ToInt32(dateTimeString.Substring(0, 4)), Convert.ToInt32(dateTimeString.Substring(5, 2)), Convert.ToInt32(dateTimeString.Substring(8, 2)));
        }

        /// <summary>
        /// Saat ile birlikte DateTime->ISO çevrimi yapar
        /// </summary>
        /// <param name="dateTime">DateTime tipinde tarih bilgisi</param>
        /// <returns>ISO formatta tarih bilgisi</returns>
        public static string ConvertToISOFormatWithTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// Saat olmaksızın DateTime->ISO çevrimi yapar
        /// </summary>
        /// <param name="dateTime">DateTime tipinde tarih bilgisi</param>
        /// <returns>ISO formatta tarih bilgisi</returns>
        public static string ConvertToISOFormatWithoutTime(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-ddT00:00:00");
        }

        public static string GetPropertyNameWithIndex(string[] propertyNameList, int index)
        {
            StringBuilder returnString = new StringBuilder();

            for (int i = index + 1; i < propertyNameList.Length; i++)
            {
                returnString.AppendFormat("{0}{1}", i == index ? string.Empty : ".", propertyNameList[i]);
            }

            return returnString.ToString();
        }

        public static string[] Split(this string str, string splitChar)
        {
            char splitChr = ' ';

            if (splitChar.Length > 0)
            {
                splitChr = splitChar[0];
            }

            return str.Split(splitChr);
        }

        public static string[] Split(this string str, char chr)
        {
            string[] slist = str.Split(new char[] { chr }, StringSplitOptions.RemoveEmptyEntries);
            return slist;
        }

        public static string GetPropertyNameWithIndex(this string propertyName, int index)
        {
            string[] propertyNameList = propertyName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder returnString = new StringBuilder();

            for (int i = index + 1; i < propertyNameList.Length; i++)
            {
                returnString.AppendFormat("{0}{1}", i == index ? string.Empty : ".", propertyNameList[i]);
            }

            return returnString.ToString();
        }

        public static List<string> ToList(this ICollection<string> list)
        {

            List<string> sList = new List<string>();
            foreach (string s in list)
            {
                sList.Add(s);
            }

            sList.Sort();
            return sList;
        }

        public static List<string> ToList(this ICollection list)
        {

            List<string> sList = new List<string>();
            foreach (string s in list)
            {
                sList.Add(s);
            }

            sList.Sort();
            return sList;
        }

        public static void SetValueToProperty(this object obj, string propertyName, Type objType, object propertyValue)
        {
            if (obj == null)
            {
                obj = objType.Assembly.CreateInstance(objType.FullName);
            }

            string[] propList = propertyName.Split('.');

            for (int i = 0; i < propList.Length; i++)
            {
                string part = propList[i];
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    break;
                }

                if (i == propList.Length - 1)
                {
                    info.SetValue(obj, propertyValue, null);
                }
            }
        }

        public static void SetArrayValueToProperty(this object obj, string propertyName, Type objType, object propertyValue, int size, int index)
        {
            Type arrayItemType = null;

            if (!objType.IsArray || index >= size)
            {
                return;
            }

            arrayItemType = objType.GetElementType();

            if (obj == null)
            {
                obj = Array.CreateInstance(arrayItemType, size);
            }

            obj = (obj as Array).GetValue(index);

            if (obj == null)
            {
                obj = objType.Assembly.CreateInstance(arrayItemType.FullName);
            }

            string[] propList = propertyName.Split('.');

            for (int i = 0; i < propList.Length; i++)
            {
                string part = propList[i];
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null)
                {
                    break;
                }

                info.SetValue(obj, propertyValue, null);
            }
        }

        public static bool TryParseXml(this string xml)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.XmlResolver = null;
                xdoc.LoadXml(xml);
                return true;
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public static bool CheckIsValidDataSetType(this Type type, object obj)
        {
            return (type == typeof(DataSet) || type == typeof(XmlNode) || type == typeof(XmlElement) || type == typeof(DataTable) ||
                ((type == typeof(string) && obj.ToString().StartsWith("<?xml version"))
                || (type == typeof(string) && obj.ToString().StartsWith("<message><mti>"))
                || (type == typeof(string) && obj.ToString().StartsWith("<") && (obj.ToString().Contains("</") || obj.ToString().Contains(">")) && obj.ToString().TryParseXml())));
        }

        public static string ByteToHexString(this byte[] input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte s in input)
            {

                 sb.AppendFormat("{0:X} ", Convert.ToInt64(s).ToString().PadLeft(2, '0'));
            }

            return  sb.ToString();
        }

        public static string ByteToIntString(this byte[] input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte s in input)
            {

                 sb.AppendFormat("{0} ", Convert.ToInt64(s).ToString().PadLeft(3, '0'));
            }

            return  sb.ToString();
        }

        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static XDocument ToXDocument(this string value)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(value);
            }
            catch
            {
                value = "<Error>ToXDocument error</Error>";
                xmlDocument.LoadXml(value);
            }

            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }


        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text to compress</param>
        /// <returns>Resturns zip text</returns>
        public static string CompressToString(this StringBuilder text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text.ToString());
            var memoryStream = new MemoryStream();
            using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                getZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var getZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, getZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, getZipBuffer, 0, 4);
            return Convert.ToBase64String(getZipBuffer);
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text to compress</param>
        /// <returns>Resturns zip text</returns>
        public static byte[] CompressToByte(this StringBuilder text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text.ToString());
            var memoryStream = new MemoryStream();
            using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                getZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var getZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, getZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, getZipBuffer, 0, 4);
            return getZipBuffer;
        }

        public static string XmlToJson(this string xmlValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlValue);
            return JsonConvert.SerializeXmlNode(doc);
        }

        public static string XmlToJson(this XmlDocument xmlValue)
        {
            return JsonConvert.SerializeXmlNode(xmlValue);
        }

        public static string JsonToXmlString(this string jsonValue)
        {
            return JsonConvert.DeserializeXmlNode(jsonValue).InnerXml;
        }

        public static XmlDocument JsonToXmlDocument(this string jsonValue)
        {
            return JsonConvert.DeserializeXmlNode(jsonValue);
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text to compress</param>
        /// <returns>Resturns zip text</returns>
        public static byte[] CompressToByte(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                getZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var getZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, getZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, getZipBuffer, 0, 4);
            return getZipBuffer;
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="value">The text to compress</param>
        /// <returns>Resturns zip text</returns>
        public static byte[] Compress(this byte[] value)
        {
            byte[] buffer = value;
            var memoryStream = new MemoryStream();
            using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                getZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var getZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, getZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, getZipBuffer, 0, 4);
            return getZipBuffer;
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>decompressed string</returns>
        public static string DecompressFromString(this StringBuilder compressedText)
        {
            byte[] getZipBuffer = Convert.FromBase64String(compressedText.ToString());
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(getZipBuffer, 0);
                memoryStream.Write(getZipBuffer, 4, getZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    getZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>decompressed string</returns>
        public static string DecompressFromByte(this byte[] compressedText)
        {
            byte[] getZipBuffer = compressedText;
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(getZipBuffer, 0);
                memoryStream.Write(getZipBuffer, 4, getZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    getZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }


        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>decompressed string</returns>
        public static byte[] Decompress(this byte[] compressedText)
        {
            byte[] getZipBuffer = compressedText;
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(getZipBuffer, 0);
                memoryStream.Write(getZipBuffer, 4, getZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    getZipStream.Read(buffer, 0, buffer.Length);
                }

                return buffer;
            }
        }


        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <param name="fileName">file name</param>
        /// <returns>decompressed string</returns>
        public static void DecompressStringToFile(this StringBuilder compressedText, string fileName)
        {
            byte[] getZipBuffer = Convert.FromBase64String(compressedText.ToString());
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(getZipBuffer, 0);
                memoryStream.Write(getZipBuffer, 4, getZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (Stream fd = File.Create(fileName))
                using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {

                    int nRead;
                    while ((nRead = getZipStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        fd.Write(buffer, 0, nRead);
                    }
                }
            }
        }

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text to compress</param>
        /// <returns>Resturns zip text</returns>
        public static string CompressString(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                getZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var getZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, getZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, getZipBuffer, 0, 4);
            return Convert.ToBase64String(getZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns>decompressed string</returns>
        public static string DecompressString(this string compressedText)
        {
            byte[] getZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(getZipBuffer, 0);
                memoryStream.Write(getZipBuffer, 4, getZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var getZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                {
                    getZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        private static object ChangeType(this object value, Type type)
        {
            if (value == null)
            {
                return null;
            }

            if (type == typeof(DateTime) && value.GetType() == typeof(string))
            {
                DateTime dateValue = DateTime.MinValue;

                if (DateTime.TryParseExact(value.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue))
                {
                    return dateValue;
                }
            }

            if (type == typeof(Decimal) && value.GetType() == typeof(string))
            {
                Decimal convertValue = 0;

                if (Decimal.TryParse(value.ToString().Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out convertValue))
                {
                    return convertValue;
                }
            }

            return Convert.ChangeType(value, type);
        }

        private static object GetObjectValue(object value, Type type)
        {
            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (value.GetType() != type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (String.IsNullOrEmpty(value.ToString()))
                    {
                        return null;
                    }
                    else
                    {
                        return value.ChangeType(type.GetGenericArguments()[0]);
                    }
                }

                return value.ChangeType(type);
            }

            return value;
        }
        public static string ToClass(this DataTable dt, string className)
        {
            StringBuilder sb = new StringBuilder();
            List<string> listColumns = new List<string>();
            List<string> listColumnsField = new List<string>();
            Dictionary<string, string> listColumnsFieldType = new Dictionary<string, string>();
             sb.AppendLine("[DataContract]");
             sb.AppendLine(string.Format("public class {0}", className));
             sb.AppendLine("{");

            foreach (DataColumn dc in dt.Columns)
            {
                listColumnsFieldType.Add(dc.ColumnName, dc.DataType.Name);
                listColumns.Add(dc.ColumnName);
            }

            foreach (string columnName in listColumns)
            {
                 sb.AppendLine("[DataMember]");
                 sb.AppendLine(string.Format("public {0} {1} {{ get; set; }} //{2}", listColumnsFieldType[columnName], columnName.GetPascalCase(), columnName));
            }

             sb.AppendLine("}");


            return  sb.ToString();
        }

        public static object[] ToParameters(this object obj, string parameterNames)
        {
            List<object> parameters = new List<object>();
            Type t = obj.GetType();
            foreach (string p in parameterNames.Split("|"))
            {
                object prmValue = t.GetProperty(p).GetValue(obj, null);
                if (t.GetProperty(p).PropertyType == typeof(DateTime) && ((DateTime)prmValue) == DateTime.MinValue)
                {
                    prmValue = DBNull.Value;
                }

                parameters.Add(prmValue);
            }

            return parameters.ToArray();
        }

        public static object[] ToParameters(this object obj)
        {
            List<object> parameters = new List<object>();
            Type t = obj.GetType();
            foreach (var p in t.GetProperties())
            {
                object prmValue = p.GetValue(obj, null);
                if (p.PropertyType == typeof(DateTime) && ((DateTime)prmValue) == DateTime.MinValue)
                {
                    prmValue = DBNull.Value;
                }

                parameters.Add(prmValue);
            }

            return parameters.ToArray();
        }

        public static string GetPascalCase(this string name)
        {
            name = name.AsString().ToLower(CultureInfo.CreateSpecificCulture("en-US"));
            return Regex.Replace(name, @"^\w|_\w",
                (match) => match.Value.Replace("_", "").ToUpper(CultureInfo.CreateSpecificCulture("en-US")));
        }
        public static List<T> ToListObject<T>(this DataTable dt)
        {
            return dt.ToListObject<T>(true);
        }

        public static List<T> ToListObject<T>(this DataTable dt,bool convertColumnNamePascalCase)
        {
            List<T> list = new List<T>();
            List<string> listColumnNames = new List<string>();
            List<string> listPropertyNames = new List<string>();

            foreach (DataColumn dc in dt.Columns)
            {
                listColumnNames.Add(dc.ColumnName);
                if (convertColumnNamePascalCase)
                {
                    listPropertyNames.Add(dc.ColumnName.GetPascalCase());
                }
                else
                {
                    listPropertyNames.Add(dc.ColumnName);
                }
            }

            foreach (DataRow dr in dt.Rows)
            {
                list.Add(dr.ToObject<T>(string.Join("{|}", listPropertyNames.ToArray()), string.Join("{|}", listColumnNames.ToArray())));
            }

            return list;
        }

        public static List<T> ToListObject<T>(this DataRow[] drs)
        {
            return drs.ToListObject<T>(true);
        }


        public static List<T> ToListObject<T>(this DataRow[] drs, bool convertColumnNamePascalCase)
        {
            List<T> list = new List<T>();
            List<string> listColumnNames = new List<string>();
            List<string> listPropertyNames = new List<string>();
            if (drs.Length > 0)
            {
                foreach (DataColumn dc in drs[0].Table.Columns)
                {
                    listColumnNames.Add(dc.ColumnName);
                    if (convertColumnNamePascalCase)
                    {
                        listPropertyNames.Add(dc.ColumnName.GetPascalCase());
                    }
                    else
                    {
                        listPropertyNames.Add(dc.ColumnName);
                    }
                }

                foreach (DataRow dr in drs)
                {
                    list.Add(dr.ToObject<T>(string.Join("{|}", listPropertyNames.ToArray()), string.Join("{|}", listColumnNames.ToArray())));
                }
            }

            return list;
        }

        public static T ToSingleObject<T>(this DataTable dt)
        {
            T singleValue = default(T);
            List<string> listColumnNames = new List<string>();
            List<string> listPropertyNames = new List<string>();

            foreach (DataColumn dc in dt.Columns)
            {
                listColumnNames.Add(dc.ColumnName);
                listPropertyNames.Add(dc.ColumnName.GetPascalCase());
            }

            foreach (DataRow dr in dt.Rows)
            {
                singleValue = dr.ToObject<T>(string.Join("{|}", listPropertyNames.ToArray()), string.Join("{|}", listColumnNames.ToArray()));
                break;
            }

            return singleValue;
        }

        public static T ToObject<T>(this DataRow dr, string propertyNames, string columnNames)
        {
            string[] splitValue = new string[] { "|" };
            if (columnNames.Contains("{|}"))
            {
                splitValue = new string[] { "{|}" };
            }


            string[] propertyList = propertyNames.Split(splitValue, StringSplitOptions.None);
            string[] columnList = columnNames.Split(splitValue, StringSplitOptions.None);
            T targetObject = Activator.CreateInstance<T>();
            

            bool flag = false;
            for (int i = 0; i < columnList.Length; i++)
            {
                if (dr.Table.Columns.Contains(columnList[i]))
                {
                    PropertyInfo pi = targetObject.GetType().GetProperty(propertyList[i]);
                    if (pi != null)
                    {
                        object val = GetObjectValue(dr[columnList[i]], pi.PropertyType);
                        if (val == null)
                        {
                            if (pi.PropertyType != typeof(String) && !(pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                //tabanından gelen sütunlar eğer null değeri alabiliyorsa fakat bizim modelimizdeki değişken null değeri alamıyorsa hata fırlatılıp 
                                //debug modda bu hatayı düzeltmek için developer a bilgi verir
                                //try
                                //{
                                //    throw new Exception(string.Format("Type:{0}, Property Name:{1}, Property Type:{2}", targetObject.GetType().FullName, pi.Name, pi.PropertyType.FullName));
                                //}
                                //catch
                                //{
                                //}
                            }
                        }

                        pi.SetValue(targetObject, val, null);
                        flag = true;
                    }
                }
            }

            if (!flag)
            {
                //Propery isimlerinden hiç bir tanesi eşleşmez ise bir problem vardır. Property isimlerinin kontrol edilmesi gerekir yada Result.ReturnValue null değil demektir.
                try
                {
                    //StringBuilder sb = new StringBuilder();
                    //foreach (DataColumn dc in dr.Table.Columns)
                    //{
                    //     sb.AppendFormat("{0}|", dc.ColumnName);
                    //}

                    //throw new Exception(string.Format("TargetObject Type: {0}, DataRow: {1}", targetObject.GetType().FullName,  sb.ToString()));
                }
                catch
                {
                }
            }

            return targetObject;
        }
        #endregion
    }
}
