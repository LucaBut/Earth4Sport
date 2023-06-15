using AutoMapper;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;

namespace Gruppo2.WebApp.Models.Profiles
{
    public class ActivityContentProfile : Profile
    {
        public ActivityContentProfile()
        {
            CreateMap<ActivityContent, ActivityContentDto>();
            CreateMap<ActivityContentDto, ActivityContent>();
        }
    }
}
