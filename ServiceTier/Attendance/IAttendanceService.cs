using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Attendance
{
    public interface IAttendanceService:IService<RepositoryTier.Entities.Attendance>
    {
    }
}
