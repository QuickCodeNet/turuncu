namespace QuickCode.Turuncu.EmailManagerModule.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class DynamicContractResolver : DefaultContractResolver
    {

        private Type _typeToIgnore;
        public DynamicContractResolver(Type typeToIgnore)
        {
            _typeToIgnore = typeToIgnore;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            properties = properties.Where(p => p.PropertyType != _typeToIgnore).ToList();

            return properties;
        }
    }
}
