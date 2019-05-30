using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AutoMapper;
using GeoFinder.Data.Helpers;
using GeoFinder.Data.Models;
using GeoFinder.IO.Models.BinaryFileModels;

namespace GeoFinder.Data.AutoMapper
{
    public class CommonMapper : ICommonMapper
    {
        public IMapper Mapper { get; set; }

        public CommonMapper()
        {
            MapperConfiguration config = GetConfiguration();
            Mapper = config.CreateMapper();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public T Map<T>(object source) where T : class
        {
            return (T)Mapper.Map(source, source.GetType(), typeof(T));
        }


        private MapperConfiguration GetConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BinGeoModel, GeoModel>().ForMember(dto => dto.Name,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.Name, 0, src.Name.Length)))
                    .ForMember(dto => dto.CreationDate,
                        mpe => mpe.MapFrom(src => DateTimeHelpers.UnixTimeStampToDateTime(src.CreationDate)));

                cfg.CreateMap<BinIpRange, IpRange>();

                cfg.CreateMap<BinLocation, Location>().ForMember(dto => dto.Country,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.Country, 0, src.Country.Length).TrimEnd('\0'))).
                    ForMember(dto => dto.Region,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.Region, 0, src.Region.Length).TrimEnd('\0'))).
                    ForMember(dto => dto.Postal,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.Postal, 0, src.Postal.Length).TrimEnd('\0'))).
                    ForMember(dto => dto.City,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.City, 0, src.City.Length).TrimEnd('\0'))).
                    ForMember(dto => dto.Organization,
                        mpe => mpe.MapFrom(src => Encoding.UTF8.GetString(src.Organization, 0, src.Organization.Length).TrimEnd('\0')));
            });
        }
    }
}
