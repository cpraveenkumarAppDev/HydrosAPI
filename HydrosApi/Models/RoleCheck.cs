﻿namespace HydrosApi.Models
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Web;
 

    //GET USER ROLES
    public partial class RoleCheck
    {
        public static bool ThisUser(string role)
        {                        
            List<string> roleList = role.Split(',').ToList();
            
            foreach(var r in roleList)
            {
                if (HttpContext.Current.User.IsInRole(r.ToString()))
                {                    
                    return true;                     
                }                
            }
            return false;
        }         
    }
}