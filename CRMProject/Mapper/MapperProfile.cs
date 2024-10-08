﻿using AutoMapper;
using CRMProject.DTO;
using CRMProject.Models;

namespace CRMProject.Mapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Customer, CustomerDto>().ReverseMap(); ;
        CreateMap<Inventory, InventoryDto>().ReverseMap();
        CreateMap<AddCustomerToInventoryRequestDto, CustomerInventoryListing>().ReverseMap();
        CreateMap<AddInventoryToCustomerRequestDto, CustomerInventoryListing>().ReverseMap();
        CreateMap<Lead, LeadDto>().ReverseMap();
        CreateMap<MyProfile, MyProfileDto>().ReverseMap();
        CreateMap<MyProfile,UrlDto>().ReverseMap();
        CreateMap<Customer,RefrenceCustomerDto>().ReverseMap();
        CreateMap<Customer,RefrenceCustomerDto>().ReverseMap();
    }

}
