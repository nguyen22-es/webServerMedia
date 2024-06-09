using AutoMapper;
using Common.DTOModels.Admin;
using Common.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;




namespace Common.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {


            CreateMap<TopicType, TopicTypeDTO>().ReverseMap();

            CreateMap<Topic, TopicDTO>()
                .ForMember(d => d.TopicType, a => a.MapFrom(c => c.TopicType.Name))
                .ReverseMap()
                .ForMember(d => d.TopicType, a => a.Ignore());

            CreateMap<Module, ModuleDTO>()
                .ForMember(d => d.Topic, a => a.MapFrom(c => c.Topic.Title))
                .ReverseMap()
                .ForMember(d => d.Topic, a => a.Ignore())
                .ForMember(d => d.Downloads, a => a.Ignore())
                .ForMember(d => d.medias, a => a.Ignore());

            CreateMap<Media, MediaDTO>()
                .ForMember(d => d.Module, a => a.MapFrom(c => c.Module.Title))
                .ForMember(d => d.Topic, a => a.MapFrom(c => c.Topic.Title))
                .ReverseMap()
                .ForMember(d => d.Module, a => a.Ignore())
                .ForMember(d => d.Topic, a => a.Ignore());

            CreateMap<Download, DownloadDTO>()
                .ForMember(d => d.Module, a => a.MapFrom(c => c.Module.Title))
                .ForMember(d => d.Topic, a => a.MapFrom(c => c.Topic.Title))
                .ReverseMap()
                .ForMember(d => d.Module, a => a.Ignore())
                .ForMember(d => d.Topic, a => a.Ignore());

        }
    }
}
