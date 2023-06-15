using AutoMapper;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;

namespace Gruppo2.WebApp.Models.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            CreateMap<Device, DeviceDto>();
            CreateMap<DeviceDto, Device>();

        }
    }
}
