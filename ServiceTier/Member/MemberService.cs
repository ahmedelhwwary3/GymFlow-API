
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
using Microsoft.EntityFrameworkCore;

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
            //1. Unique Email & Phone
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone); 

            if (!isUniqueEmail)
                return new AddMemberResult(enAddMemberStatus.NotUniqueEmail); 

            if (!isUniquePhone)
                return new AddMemberResult(enAddMemberStatus.NotUniquePhone);

            //2.coach exists and Active
            bool coachExists = await _coachRepo.ExistsAsync(request.CoachId);
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
            //3.Add and save
            await _repo.AddAsync(newMember);
           int affectedRows= await _repo.SaveChangesAsync();
            return affectedRows>0?new AddMemberResult(enAddMemberStatus.Succeeded, newMember.Id) :
                new AddMemberResult(enAddMemberStatus.InternalServerError);
        }

        public async Task<enUpdateMemberStatus>
            UpdateAsync(int Id, UpdateMemberRequest request)
        {
            //1. Unique Email & Phone
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email,Id);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone,Id);

            if (!isUniqueEmail)
                return enUpdateMemberStatus.NotUniqueEmail; 

            if (!isUniquePhone)
                return enUpdateMemberStatus.NotUniquePhone;

            //2.if coach updated coach must be existed and Active
            if (request.CoachId.HasValue)
            {
                bool coachExists = await _coachRepo.ExistsAsync(request.CoachId.Value);
                if (!coachExists)
                    return enUpdateMemberStatus.CoachNotExists;

                bool isActiveCoach = await _coachRepo.IsActiveByIdAsync(request.CoachId.Value);
                if (!isActiveCoach)
                    return enUpdateMemberStatus.CoachInactive;
            }

            //3.Update IsActive only if has no active subscription
            bool hasActiveSubscription = await _repo.HasActiveSubscriptionAsync(Id);
            if (hasActiveSubscription)
                request.IsActive = true;

            //4.Load then Update strategy for efCore Tracking
            var member = await _repo.GetByIdAsync(Id);
            if(member==null)
                return enUpdateMemberStatus.MemberNotFound;

            member.FitnessGoal = request.FitnessGoal;
            member.IsActive = request.IsActive;
            member.Address = request.Address;
            member.CoachId = request.CoachId;
            member.DateOfBirth = request.DateOfBirth;
            member.Phone = request.Phone;
            member.Email = request.Email;
            member.Gender = request.Gender;
            member.Height = request.Height; 

            EntityState state = _repo.GetEntityState(member);
            if (state == EntityState.Unchanged)
                return enUpdateMemberStatus.DataNotChanged;
            member.UpdatedAt = DateTime.UtcNow;

            //5.Save
            int affectedRow = await _coachRepo.SaveChangesAsync();
            return affectedRow > 0 ? enUpdateMemberStatus.Succeeded 
                : enUpdateMemberStatus.InternalServerError;
        }

        public Task<enUpdateMemberProfileStatus>
            UpdateProfileAsync(int Id, UpdateMemberProfileRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
