using System;
using System.Reflection;
using AutoMapper;
using System.Linq;
using QuickCode.Turuncu.Common.Controllers;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using QuickCode.Turuncu.Portal.Models.UserManagerModule;

namespace QuickCode.Turuncu.Portal.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<TopicWorkflowsGetWorkflowsResponseDto, TopicWorkflowsObj>();
            AddServiceModelMappings();
        }

        private void AddServiceModelMappings()
        {
            string modelNamespace = $"{this.GetType().Namespace!.Replace(".Mapper", string.Empty)}.Models";
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(i => i.IsClass && i.Namespace == modelNamespace);
            var objKeyword = "Obj";
            var dtoKeyword = "Dto";

            var contractsNamespace = "QuickCode.Turuncu.Common.Nswag.Clients";
            var contractsTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(i => i.IsClass && i.FullName!.StartsWith(contractsNamespace) &&
                            i.Name.EndsWith(dtoKeyword) &&
                            i.Namespace!.EndsWith("Api.Contracts")).ToList();
            
            var contractsNswagTypes = typeof(QuickCodeBaseApiController).Assembly.GetTypes()
                .Where(i => i.IsClass && i.FullName!.StartsWith(contractsNamespace) &&
                            i.Name.EndsWith(dtoKeyword) &&
                            i.Namespace!.EndsWith("Api.Contracts"));

            contractsTypes.AddRange(contractsNswagTypes);
            foreach (var dtoType in contractsTypes)
            {
                var moduleName = dtoType.FullName!.Replace($"{contractsNamespace}.", string.Empty)
                    .Replace($"Api.Contracts.{dtoType.Name}", string.Empty);
                var objTypeName = $"{modelNamespace}.{moduleName}.{dtoType.Name.Replace(dtoKeyword, objKeyword)}";
                var objType = Type.GetType(objTypeName);

                if (objType != null)
                {
                    CreateMap(dtoType, objType);
                    CreateMap(objType, dtoType);
                }
            }
        }
    }
}