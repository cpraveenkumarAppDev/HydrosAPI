namespace HydrosApi.Data
{

    using Models.ADWR;

    public class GetBestUsername : Repository<AwUsers>//AW_USERS
    {
        public string UserName;
        public int? Id;

        public GetBestUsername(string user)
        {
            string oracleUserID=null;

            if (user == null)
                return;

            string userName = user.Replace("AZWATER0\\", "");

            if (userName == null)
                return;

            //get Oracle USER_ID if available
            var foundUser = AwUsers.Get(u => u.Email.ToLower().Replace("@azwater.gov", "") == userName);

            if (foundUser != null)
            {
                Id = foundUser.Id;
                oracleUserID = foundUser != null ? foundUser.UserId : null;
            }
            
            UserName=oracleUserID ?? userName; //Set to Oracle ID if possible           
        }
    }
}