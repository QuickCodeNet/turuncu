using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json.Linq;

namespace QuickCode.Turuncu.Common.Extensions;

public static class Extensions
{ 
    public static bool IsRouteMatch(this string routeTemplate, string actualPath)
    {
        var template = TemplateParser.Parse(routeTemplate);
        var matcher = new TemplateMatcher(template, new RouteValueDictionary());
        
        return matcher.TryMatch(actualPath, new RouteValueDictionary());
    }
    
    public static bool IsRouteMatch(this string path, List<string> paths)
    {
        return paths.Exists(item => item.IsRouteMatch(path));
    }
    
    public static string ToJson<T>(this T obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true

        };

        var jsonValue = JsonSerializer.Serialize(obj, options);
        return jsonValue;
    }

    public static bool IsInList(this string obj, params string[] list)
    {
        for (var i = 0; i < list.Length; i++)
        {
            if (list[i] == obj)
            {
                return true;
            }
        }

        return false;
    }

    public static string GetPascalCase(this string name)
    {
        if (name!.IsPascalCase())
        {
            return name;
        }

        name = name.ToLower(CultureInfo.CreateSpecificCulture("en-US"));
        return Regex.Replace(name, @"^\w|_\w",
            (match) => match.Value.Replace("_", "").ToUpper(CultureInfo.CreateSpecificCulture("en-US")));
    }
    
    public static bool IsPascalCase(this string text)
    {
        if (text.ToUpper().Equals(text))
        {
            return false;
        }
            
        var words = text.Split(new[] { ' ', '_' }, StringSplitOptions.RemoveEmptyEntries);
            
        foreach (var word in words)
        {
            if (!char.IsUpper(word[0]))
            {
                return false;
            }
        }

        return true;
    }
    
    public static string GetPropertyTypeName(this Type type, string activeProvider)
    {
        var typeName = type.Name;
        if (type.CustomAttributes.Any(i=>i.AttributeType.Name.Equals("NullableAttribute")))
        {
            typeName = $"{typeName}?";
        }
        
        if (type.FullName!.StartsWith("System.Nullable"))
        {
            typeName = $"{type.GenericTypeArguments[0].Name}?";
        }

        typeName =  typeName.Replace("String", "string")
            .Replace("Int32", "int")
            .Replace("Int64", "int")
            .Replace("DateTime", "datetime")
            .Replace("Int16", "int");
        
        if (activeProvider.Equals("Microsoft.EntityFrameworkCore.SqlServer"))
        {
            typeName = typeName.Replace("Boolean", "bit");
        }

        return typeName;
    }
    
    public static void ParseJsonAsInitialData(this MigrationBuilder migrationBuilder,Type domainEntityType, List<string> fileList)
    {
        foreach (var file in fileList)
        {
            var fileContent = File.ReadAllText(file);
            try
            {
                var jsonObject = JObject.Parse(fileContent);

                var models = jsonObject.ToObject<Dictionary<string, List<Dictionary<string, object>>>>();
                foreach (var tableName in models!.Keys)
                {
                    var entityType =
                        domainEntityType.Assembly.GetType(
                            $"{domainEntityType.Namespace}.{tableName.GetPascalCase()}");
                    if (entityType is null)
                    {
                        continue;
                    }
                    
                    var columnNames = models[tableName].First().Keys.ToArray();

                    var objects = new object[models[tableName].Count, columnNames.Length];

                    var rowCounter = 0;

                    var columnTypes = columnNames.Select(i =>
                        entityType.GetProperty(i.GetPascalCase())!.PropertyType.GetPropertyTypeName(migrationBuilder
                            .ActiveProvider!));
                    
                    foreach (var rows in models[tableName])
                    {
                        for (var index = 0; index < columnNames.Length; index++)
                        {
                            var column = columnNames[index];
                            var colType = entityType.GetProperty(column.GetPascalCase())!;
                            if (colType.PropertyType == typeof(bool))
                            {
                                rows[column] = Convert.ToBoolean(rows[column]);
                            }

                            objects.SetValue(rows[column], rowCounter, index);
                        }

                        rowCounter++;
                    }

                    migrationBuilder.InsertData(
                        table: tableName,
                        columnTypes: columnTypes.ToArray(),
                        columns: columnNames.ToArray(),
                        values: objects);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
    
    public static List<string> GetMigrationDataFiles(this Type type)
    {
        var splitValues = type.Namespace!.Split(".");
        var moduleName = splitValues[2];
        var projectName = splitValues[1];
        var currentDir = Directory.GetCurrentDirectory();
        if (currentDir.Split(Path.DirectorySeparatorChar).Any(i => i.Equals(moduleName)))
        {
            currentDir = currentDir[..currentDir.IndexOf(moduleName)];
        }
        else
        {
            currentDir = $@"/src/Modules/";
        }

        var path = $@"{currentDir}{moduleName}/Infrastructure/QuickCode.{projectName}.{moduleName}.Persistence/Migrations/InitialData";

        if (!Directory.Exists(path))
        {
            path = $"/app/Migrations/InitialData";
        }

        var response = Directory.Exists(path) ? Directory.GetFiles(path, "*.json").ToList() : [];
        return response.OrderBy(i => i).ToList();
    }
    
    public static string GetMigrationDataPath(this Type type)
    {
        var splitValues = type.Namespace!.Split(".");
        var moduleName = splitValues[2];
        var projectName = splitValues[1];
        var currentDir = Directory.GetCurrentDirectory();
        if (currentDir.Split(Path.DirectorySeparatorChar).Any(i => i.Equals(moduleName)))
        {
            currentDir = currentDir[..currentDir.IndexOf(moduleName)];
        }
        else
        {
            currentDir = $@"/src/Modules/";
        }

        var path = $@"{currentDir}{moduleName}/Infrastructure/QuickCode.{projectName}.{moduleName}.Persistence/Migrations/InitialData/{moduleName}Data.json";

        if (!File.Exists(path))
        {
            path = $"/app/Migrations/InitialData/{moduleName}Data.json";
        }

        return path;
    }
}