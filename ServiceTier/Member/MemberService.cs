
using RepositoryTier.Coach.Enums;
using RepositoryTier.Entities;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Repositories;
using RepositoryTier.Member.Results;
using RepositoryTier.User.Repositories;
using ServiceTier.User;
using RepositoryTier.Member.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryTier.User.Enums;
using System.Threading.Tasks; 
using RepositoryTier.Coach.Repositories;

namespace ServiceTier.Member
{
    public class MemberService: Service<RepositoryTier.Entities.Member>, IMemberService
    {
        private readonly IMemberRepository _repo;
        private readonly IUserService _userService;
        private readonly ICoachRepository _coachRepo;
        public MemberService(
            IMemberRepository repo
            ,IUserService userService,
            ICoachRepository coachRepo) 
            : base(repo) 
        {
            _repo = repo;
            _userService = userService;
            _coachRepo = coachRepo;
        }

         

        public async Task<GetAssignedMembersForCoachResponse>
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request)
        {
            return await _repo.GetAssignedMembersForCoachAsync(request);
        }

        public async Task<GetMembersResopnse> 
            GetMembersAsync(GetMembersRequest request)
        {
            return await _repo.GetMembersAsync(request);
        }

        public async Task<AddMemberResult> AddAsync(AddMemberRequest request)
        {
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone); 

            if (!isUniqueEmail)
                return new AddMemberResult(enAddMemberStatus.NotUniqueEmail); 

            if (!isUniquePhone)
                return new AddMemberResult(enAddMemberStatus.NotUniquePhone);

            bool coachExists = await _coachRepo.ExistsByIdAsync(request.CoachId);
            if(!coachExists)
                return new AddMemberResult(enAddMemberStatus.CoachNotExists);

            bool isActiveCoach = await _coachRepo.IsActiveByIdAsync(request.CoachId);
            if(!isActiveCoach)
                return new AddMemberResult(enAddMemberStatus.CoachInactive);

            var newMember = new RepositoryTier.Entities.Member()
            {
                IsActive = true,
                Address = request.Address.Trim(),
                CreatedAt = DateTime.UtcNow,
                CoachId = request.CoachId,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email.Trim(),
                FitnessGoal = request.FitnessGoal,
                FullName = request.FullName.Trim(),
                Gender = request.Gender,
                Height = request.Height,
                Role = enUserRole.Member,
                Phone = request.Phone.Trim(),
                IsDeleted = false,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };
            await _repo.AddAsync(newMember);
           int affectedRows= await _repo.SaveChangesAsync();
            return affectedRows>0?new AddMemberResult(enAddMemberStatus.Succeeded, newMember.Id) :
                new AddMemberResult(enAddMemberStatus.InternalServerError);
        }
    }
}
