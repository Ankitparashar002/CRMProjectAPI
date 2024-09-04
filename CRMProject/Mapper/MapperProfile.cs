using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;

namespace CRMProject.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Customer, CustomerDto>();
        CreateMap<Inventory, InventoryDto>();
    }

}
