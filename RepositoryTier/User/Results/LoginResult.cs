using RepositoryTier.Authentication.DTOs;
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
        public enLoginStatus Status { get; set; }
        public TokenResponse? TokenResponse { get; set; }
        public LoginResult(enLoginStatus LoginStatus, TokenResponse? tokenResponse=null)
        {
            this.Status = LoginStatus;
            this.TokenResponse = tokenResponse;
        }
    }
}
