namespace Motorsazan.CustomerAppWeb.Shared.Models.DomainModels
{
    public class Tbl_User
    {
        public long DepartmentId { get; set; }
        public int UnitCode { get; set; }
        public string Title { get; set; }
        public int ParentUnitCode { get; set; }
        public string ChartTitle { get; set; }
        public string PostCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int EID { get; set; }
        public int UserID { get; set; }
        public int EmployeeID { get; set; }
    }
}