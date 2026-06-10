using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;

namespace RepositoryTier.Models;

public partial class Coach:User
{    
    public enCoachSpecialization Specialization { get; set; }

    public DateOnly HireDate { get; set; }

    public decimal Salary { get; set; }  

    public virtual ICollection<Member> Members { get; set; } = new List<Member>(); 

    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

    public Coach()
    {
        base.Role = enUserRole.Coach;
    }
}
