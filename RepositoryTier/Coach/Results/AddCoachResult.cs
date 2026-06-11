using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.Results
{
    public class AddCoachResult
    {
        public AddCoachResponse? Resopnse { get; set; }
        public enAddCoachStatus Status { get; set; }

        public AddCoachResult(enAddCoachStatus status, 
            AddCoachResponse? response = null)
        {
            this.Resopnse= response;
            this.Status = status;
        }
    }
}
