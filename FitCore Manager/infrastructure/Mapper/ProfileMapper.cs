using Application.Dto.MembershipPlan;
using Application.Dto.Rating;
using Application.Dto.Traiiner;
using Application.Dto.user;
using Application.Dto.Workout;
using AutoMapper;
using Domain.Model;
using FitCore_Manager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Mapper
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper() 
        {
            CreateMap<User,UserRegistrationDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();

            CreateMap<CreateMembershipPlanDto, MembershipPlans>().ReverseMap();
            CreateMap<UpdateMembershipPlanDto, MembershipPlans>().ReverseMap();

            CreateMap<UserMembership,UserMembershipDto>().ReverseMap();

            CreateMap<WorkoutPlanDayDetails,WorkoutPlanDayDetailsDto>().ReverseMap();
            CreateMap<CreateWorkoutPlanDayDetailsDto, WorkoutPlanDayDetails>();

            CreateMap<CreateTimeSlotDto, TimeSlot>();

            CreateMap<User, TrainerDto>();

            //CreateMap<TrainerTimeSlot, AvailableSlotViewDto>().ReverseMap();
            CreateMap<GymRatingCreateDto, Feedback>();
            CreateMap<Feedback, GymRatingViewDto>();
            CreateMap<GymRatingCreateDto, GymRatingViewDto>();


        }
    }
}
