using System.Runtime.Serialization;

namespace Motorsazan.CustomerAppWeb.Shared.Models.Base
{
    public class Reason
    {
        private string _log;
        private string _user;

        public Reason(string messageIndex, string moreInfo = null)
        {
            Code = messageIndex;
            Log = moreInfo;
        }

        [DataMember]
        public string Log
        {
            set => _log = value;
            get => _log ?? (_log = InitLog());
        }

        [DataMember]
        public string User
        {
            set => _user = value;
            get => _user ?? (_user = InitUser());
        }

        [DataMember] public string Code { set; get; }

        private string InitLog()
        {
            return _log;
        }

        private string InitUser()
        {
            return _user;
        }
    }
}