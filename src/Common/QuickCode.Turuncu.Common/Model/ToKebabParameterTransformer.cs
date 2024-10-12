using System.Text.RegularExpressions;

namespace QuickCode.Turuncu.Common.Model;

public class ToKebabParameterTransformer : IOutboundParameterTransformer
{
    private Type ModuleType { get; set; }

    public ToKebabParameterTransformer(Type moduleType)
    {
        this.ModuleType = moduleType;
    }
    
    public string TransformOutbound(object? value)
    {
        if (value == null)
        {
            return null!;
        }
        
        //QuickCode.Quickcode.QuickCodeModule.Api
        var items = ModuleType.FullName!.Split(".", StringSplitOptions.RemoveEmptyEntries);
        string moduleName;
        if (items.Length > 3)
        {
            moduleName = ModuleType.FullName!.Split(".", StringSplitOptions.RemoveEmptyEntries)[2];
        }
        else
        {
            moduleName = ModuleType.Assembly.FullName!.Split(".", StringSplitOptions.RemoveEmptyEntries)[2];
        }

        var controllerName = Regex.Replace($"{moduleName}/{value}", "([a-z])([A-Z])", "$1-$2").ToLower();

        return controllerName;
    }
}