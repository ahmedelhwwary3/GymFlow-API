using RepositoryTier.Payment.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Payment
{
    public interface IPaymentService:IService<RepositoryTier.Entities.Payment>
    {
        Task<AddPaymentResponse> AddAsync(AddPaymentRequest request);
        Task<GetPaymentByIdResponse?> GetByIdAsync(int Id);
    }
}
