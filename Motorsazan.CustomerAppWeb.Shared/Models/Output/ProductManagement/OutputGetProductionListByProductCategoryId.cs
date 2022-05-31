namespace Motorsazan.CustomerAppWeb.Shared.Models.Output.ProductManagement
{
    public class OutputGetProductionListByProductCategoryId
    {
        public long ProductionId { get; set; }

        public string EngineType { get; set; }

        public string ImageName { get; set; }

        public string FileStream { get; set; }

        public long ProductionCategoryId { get; set; }

        public string CatalogName { get; set; }

        public decimal Height { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public string MaxPower { get; set; }

        public string MaxTorque { get; set; }

        public decimal ApproximateWeight { get; set; }

        public int NumberOfCylinder { get; set; }

        public string ProductionCategoryShowName { get; set; }
    }
}