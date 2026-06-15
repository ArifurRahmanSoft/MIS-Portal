using System;
using System.Web.Mvc;

namespace CTGroup.Web.Utility.Infrastructure.Mvc
{
    public class Select2Attribute : Attribute, IMetadataAware
    {
        private readonly string DataPlaceholder = string.Empty;
        private readonly string DataOption = string.Empty;

        public Select2Attribute(string dataPlaceholder, string dataOption = "")
        {
            this.DataPlaceholder = dataPlaceholder;
            this.DataOption = dataOption;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues["data-placeholder"] = this.DataPlaceholder;
            if (!string.IsNullOrEmpty(this.DataOption))
            {
                metadata.AdditionalValues["data-option"] = this.DataOption;
            }
        }
    }
}
