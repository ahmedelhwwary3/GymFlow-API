using RepositoryTier.Authentication;
 
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Results
{
    public class LoginResult
    {
        public enLoginStatus LoginStatus { get; set; }
        public TokenResponse? TokenResponse { get; set; }
        public LoginResult(enLoginStatus LoginStatus, TokenResponse? tokenResponse=null)
        {
            this.LoginStatus = LoginStatus;
            this.TokenResponse = tokenResponse;
        }
    }
}
