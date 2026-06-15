
using RepositoryTier.Coach.Enums;
using RepositoryTier.Entities;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Repositories;
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
using RepositoryTier.User.Results;

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
            bool hasActiveSubscription = await _repo.HasActiveSubscriptionsAsync(Id);
            if (hasActiveSubscription)
                request.IsActive = true;

            //4.Load then Update strategy for efCore Tracking
            var member = await _repo.FindAsync(Id);
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
            return enUpdateMemberStatus.Succeeded;
        }

        public async Task<enUpdateMemberProfileStatus>
            UpdateProfileAsync(int Id, UpdateMemberProfileRequest request)
        {
            //1. Unique Email & Phone
            bool isUniqueEmail = await _userService.IsUniqueEmailAsync(request.Email, Id);
            bool isUniquePhone = await _userService.IsUniquePhoneAsync(request.Phone, Id);

            if (!isUniqueEmail)
                return enUpdateMemberProfileStatus.NotUniqueEmail;

            if (!isUniquePhone)
                return enUpdateMemberProfileStatus.NotUniquePhone;  

            //2.Load then Update strategy for efCore Tracking
            var member = await _repo.FindAsync(Id);
            if (member == null)
                return enUpdateMemberProfileStatus.MemberNotFound;

            member.FitnessGoal = request.FitnessGoal; 
            member.Phone = request.Phone;
            member.Email = request.Email; 
            member.Height = request.Height; 

            EntityState state = _repo.GetEntityState(member);
            if (state == EntityState.Unchanged)
                return enUpdateMemberProfileStatus.DataNotChanged;
            member.UpdatedAt = DateTime.UtcNow;

            //5.Save
            int affectedRow = await _coachRepo.SaveChangesAsync();
            return enUpdateMemberProfileStatus.Succeeded;
        }

        public async Task<GetMemberProfileResopnse?> GetProfileAsync(int Id)
        {
            return await _repo.GetProfileAsync(Id);
        }

        public async Task<GetMemberByIdResopnse?> GetByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(Id);
        }
    }
}
