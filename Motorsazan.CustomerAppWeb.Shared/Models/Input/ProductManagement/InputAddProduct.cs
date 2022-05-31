namespace Motorsazan.CustomerAppWeb.Shared.Models.Input.ProductManagement
{
    public class InputAddProduct
    {
        public long ProductionCategoryId { get; set; }

        public string EngineType { get; set; }

        public long NumberOfCylinder { get; set; }

        public decimal ApproximateWeight { get; set; }

        public string MaxPower { get; set; }

        public string MaxTorque { get; set; }

        public decimal Length { get; set; }

        public decimal Width { get; set; }

        public decimal Height { get; set; }
    }
}