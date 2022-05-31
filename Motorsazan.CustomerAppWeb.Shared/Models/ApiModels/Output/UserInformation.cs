namespace Motorsazan.CustomerAppWeb.Shared.Models.ApiModels.Output
{
    public class UserInformation
    {
        public Status Status { get; set; }
        public UserInformationParams Params { get; set; }
    }

    public class UserInformationParams
    {
        public User[] List { get; set; }
    }
}