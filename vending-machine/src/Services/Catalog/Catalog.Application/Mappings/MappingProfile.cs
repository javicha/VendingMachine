﻿using AutoMapper;
using Vending.Application.Features.Catalog.Commands.ReturnCoins;
using Vending.Application.Features.Catalog.Queries.GetProductList;
using Vending.Domain.Entities;

namespace Vending.Application.Mappings
{
    /// <summary>
    /// Class to manage object mapping
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                .ForMember(d => d.Price, opts => opts.MapFrom(s => s.Price))
                ;

            CreateMap<Coin, CoinDTO>()
                .ForMember(d => d.Amount, opts => opts.MapFrom(s => s.Amount))
                ;
        }
    }
}
