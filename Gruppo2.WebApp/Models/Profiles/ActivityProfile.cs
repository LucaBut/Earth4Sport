using AutoMapper;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;

namespace Gruppo2.WebApp.Models.Profiles
{
    public class ActivityProfile: Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<ActivityDto, Activity>();
        }
    }
}
