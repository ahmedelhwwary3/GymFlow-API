
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Authentication.DTOs;

namespace RepositoryTier.User.Results
{
    public class RefreshResult
    {
        public enRefreshStatus RefreshStatus { get; set; }
        public TokenResponse? TokenResponse { get; set; }
        public RefreshResult(enRefreshStatus refreshStatus, TokenResponse? tokenResponse=null)
        {
            this.RefreshStatus= refreshStatus;
            this.TokenResponse= tokenResponse;
        }
    }
}
