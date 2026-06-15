using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Results
{
    public class RegisterCoachResult
    {
        public RegisterCoachResponse? Resopnse { get; set; }
        public enRegisterCoachStatus Status { get; set; }

        public RegisterCoachResult(enRegisterCoachStatus status, 
            RegisterCoachResponse? response = null)
        {
            this.Resopnse= response;
            this.Status = status;
        }
    }
}
