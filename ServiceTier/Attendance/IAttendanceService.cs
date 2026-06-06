using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier
{
    public interface IAttendanceService:IService<RepositoryTier.Models.Attendance>
    {
    }
}
