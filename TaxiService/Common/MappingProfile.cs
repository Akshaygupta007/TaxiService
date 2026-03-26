using AutoMapper;
using TaxiService.DTOs.Requests;
using TaxiService.DTOs.Responses;
using TaxiService.Entities;

namespace TaxiService.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ===== USER MAPPINGS =====
            CreateMap<User, UserResponse>();
            CreateMap<RegisterRequest, User>();
            CreateMap<LoginRequest, User>();

            // ===== DRIVER MAPPINGS =====
            CreateMap<Driver, DriverResponse>();
            CreateMap<CreateDriverRequest, Driver>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateDriverRequest, Driver>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ===== VEHICLE MAPPINGS =====
            CreateMap<Vehicle, VehicleResponse>()
                .ForMember(dest => dest.CabTypeID, opt => opt.MapFrom(src => src.CabTypeID));
            CreateMap<CreateVehicleRequest, Vehicle>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateVehicleRequest, Vehicle>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ===== CABTYPE MAPPINGS =====
            CreateMap<CabType, CabTypeResponse>();
            CreateMap<CreateCabTypeRequest, CabType>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateCabTypeRequest, CabType>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // ===== DRIVER VEHICLE MAPPINGS =====
            CreateMap<DriverVehicle, DriverVehicleResponse>()
                .ForMember(dest => dest.DriverName, opt => opt.MapFrom(src => src.Driver.Name))
                .ForMember(dest => dest.VehicleNumber, opt => opt.MapFrom(src => src.Vehicle.VehicleNumber));
/*            CreateMap<CreateDriverVehicleRequest, DriverVehicle>()
                .ForMember(dest => dest.AssignmentDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
            CreateMap<UpdateDriverVehicleRequest, DriverVehicle>();*/
        }
    }
}
