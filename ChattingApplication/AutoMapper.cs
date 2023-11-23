using AutoMapper;
using ChattingApplication.Core.Domains;
using ChattingApplication.Dtos;

namespace ChattingApplication
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {

            CreateMap<User, RegisterDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(m => m.Id, opt => opt.Ignore());

            CreateMap<User, GetProfileDto>();
            CreateMap<EditProfileDto, User>()
                .ForMember(m => m.Id, opt => opt.Ignore());

        }
    }
}
