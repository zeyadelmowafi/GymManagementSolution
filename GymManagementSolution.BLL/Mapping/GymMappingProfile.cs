using AutoMapper;
using GymManagementSolution.BLL.ViewModels;
using GymManagementSolution.DAL.Entities;

namespace GymManagementSolution.BLL.Mapping;

public class GymMappingProfile : Profile
{
    public GymMappingProfile()
    {
        CreateMap<Session, SessionViewModel>()
            .ForMember(x => x.TrainerName, o => o.MapFrom(x => x.Trainer!.Name))
            .ForMember(x => x.CategoryName, o => o.MapFrom(x => x.Category!.Name))
            .ForMember(x => x.AvailableSlots, o => o.MapFrom(x => x.Capacity - x.Bookings.Count));
        CreateMap<CreateSessionViewModel, Session>();
        CreateMap<Session, UpdateSessionViewModel>();
        CreateMap<CreateTrainerViewModel, Trainer>()
            .ForMember(x => x.Address, o => o.MapFrom(x => new Address { BuildingNumber = x.BuildingNumber, Street = x.Street, City = x.City }));
        CreateMap<Trainer, TrainerViewModel>()
            .ForMember(x => x.DateOfBirth, o => o.MapFrom(x => x.DateOfBirth.ToString("yyyy-MM-dd")))
            .ForMember(x => x.Gender, o => o.MapFrom(x => x.Gender.ToString()))
            .ForMember(x => x.Address, o => o.MapFrom(x => x.Address.Display));
        CreateMap<Trainer, TrainerToUpdateViewModel>()
            .ForMember(x => x.BuildingNumber, o => o.MapFrom(x => x.Address.BuildingNumber))
            .ForMember(x => x.Street, o => o.MapFrom(x => x.Address.Street))
            .ForMember(x => x.City, o => o.MapFrom(x => x.Address.City));
        CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(x => x.Name, o => o.Ignore())
            .ForMember(x => x.Address, o => o.MapFrom(x => new Address { BuildingNumber = x.BuildingNumber, Street = x.Street, City = x.City }));
        CreateMap<Plan, PlanViewModel>();
        CreateMap<Plan, UpdatePlanViewModel>().ForMember(x => x.PlanName, o => o.MapFrom(x => x.Name));
        CreateMap<UpdatePlanViewModel, Plan>().ForMember(x => x.Name, o => o.Ignore());
        CreateMap<Member, MemberViewModel>().ForMember(x => x.Address, o => o.MapFrom(x => x.Address.Display));
        CreateMap<CreateMemberViewModel, Member>()
            .ForMember(x => x.Address, o => o.MapFrom(x => new Address { BuildingNumber = x.BuildingNumber, Street = x.Street, City = x.City }));
    }
}
