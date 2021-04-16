namespace HydrosApi.Data
{

    using Models.ADWR;

    public class GetBestUsername : Repository<AwUsers>//AW_USERS
    {
        public string UserName;

        public GetBestUsername(string user)
        {
            if (user == null)
                return;

            string userName = user.Replace("AZWATER0\\", "");

            if (userName == null)
                return;

            //get Oracle USER_ID if available
            var foundUser = AwUsers.Get(u => u.Email.ToLower().Replace("@azwater.gov", "") == userName);
          
            string oracleUserID = foundUser != null ? foundUser.UserId : null;
            UserName=oracleUserID ?? userName; //Set to Oracle ID if possible
        }
    }
}