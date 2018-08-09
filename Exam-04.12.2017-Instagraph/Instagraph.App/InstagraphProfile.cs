namespace Instagraph.App
{
    using AutoMapper;
    using DataProcessor.DtoModels;
    using Models;


    public class InstagraphProfile : Profile
    {
        public InstagraphProfile()
        {
            CreateMap<Post, UncommentedPostDto>()
                .ForMember(dto => dto.Picture, pp => pp.MapFrom(p => p.Picture.Path))
                .ForMember(dto => dto.User, u => u.MapFrom(p => p.User.Username));

            CreateMap<User, PopularUserDto>()
                .ForMember(dto => dto.Followers, f => f.MapFrom(u => u.Followers.Count));
        }
    }
}