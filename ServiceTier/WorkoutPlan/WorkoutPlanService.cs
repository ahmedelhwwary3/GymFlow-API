
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Member.Repositories;
using RepositoryTier.WorkoutPlan.DTOs;
using RepositoryTier.WorkoutPlan.Repositories;
using RepositoryTier.WorkoutPlan.Results;
using RepositoryTier.WorkoutPlan.Enums;
using RepositoryTier.WorkoutPlanExercise.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Exercise.Repositories;
using RepositoryTier.User.Repositories;

namespace ServiceTier.WorkoutPlan
{
    public class WorkoutPlanService:  IWorkoutPlanService
    {
        private readonly IWorkoutPlanRepository _repo;
        private readonly ICoachRepository _coachRepo;
        private readonly IUserRepository _userRepo;
        private readonly IExerciseRepository _exerciseRepo;
        public WorkoutPlanService(
            IWorkoutPlanRepository repo,
            ICoachRepository coachRepo,
            IExerciseRepository exerciseRepo,
            IUserRepository userRepo)  
        {
            _repo = repo;
            _coachRepo= coachRepo;
            _exerciseRepo= exerciseRepo;
            _userRepo = userRepo;
        }

        public async Task<GetWorkoutPlansResponse> 
            GetWorkoutPlansAsync(GetWorkoutPlansRequest request, int? memberId)
        {
            return await _repo.GetWorkoutPlansAsync(request,memberId);
        }

        public async Task<AddWorkoutPlanResult> 
            AddFullPlanAsync(AddWorkoutPlanRequest request)
        {
            //1.check coach exists and active
            if (request.CoachId == null)
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.CoachNotFound);

            bool? isCoachActive = await _coachRepo
                .IsActiveAsync(request.CoachId.Value);

            if (isCoachActive == null)
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.CoachNotFound);

            if (!isCoachActive.Value)
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.CoachInactive);

            //2.check unique exercises
            var uniqueExerciseIDs = request.Exercises
                .Select(e => e.ExerciseId)
                .Distinct(); 
             
            var requestExerciseIDs = request.Exercises
                .Select(e => e.ExerciseId);
             
            if (uniqueExerciseIDs.Count() < requestExerciseIDs.Count())
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.ExerciseRepeated);

            //3.check exercises existanse
            bool allExercisesExist = await _exerciseRepo
                .AllExistAsync(uniqueExerciseIDs);

            if (!allExercisesExist)
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.ExerciseNotFound);

            //4.check member exists and get Id
            int? memberId = await _userRepo
                .GetIdByIdentifierAsync(request.MemberIdentifier.Trim());

            if (memberId == null)
                return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.MemberNotFound);

            //5.Manual change detecting before save
            // - Add Plan first then attach exerciese to it through ConnectionTable (M:M)
            // - Deactivate old active plan
            _repo.ChangeAutoDetectChangesStatus(false);
            var newPlan = new RepositoryTier.Entities.WorkoutPlan
            {
                CreatedAt=DateTime.UtcNow,
                IsActive=true,
                CoachId=request.CoachId.Value,
                MemberId=memberId.Value,
                Name=request.PlanName.Trim() 
            };
            await _repo.AddAsync(newPlan);

            var oldPlan = await _repo
                .GetLastByMemberIdAsync(memberId.Value);
            if (oldPlan != null)
                oldPlan.IsActive = false;// No update if it was already Inactive

            foreach (var exercise in request.Exercises)
            {
                var newPlanExercise = new RepositoryTier.Entities.WorkoutPlanExercise
                {
                    ExerciseId= exercise.ExerciseId,
                    Notes= exercise.Notes?.Trim(),
                    Reps=exercise.Reps,
                    Sets= exercise.Sets
                };
                newPlan.WorkoutPlanExercises.Add(newPlanExercise);
            } 
            _repo.ChangeAutoDetectChangesStatus(true);
            int affectedRows= await _userRepo.SaveChangesAsync();
            return new AddWorkoutPlanResult(enAddWorkoutPlanStatus.Succeeded);
        }

        public async Task<GetWorkoutPlanByIdResponse?> GetByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(Id);
        }
    }
}
