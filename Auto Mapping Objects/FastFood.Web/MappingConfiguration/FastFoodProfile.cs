﻿namespace FastFood.Web.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Models.Enums;
    using FastFood.Web.ViewModels.Categories;
    using FastFood.Web.ViewModels.Employees;
    using FastFood.Web.ViewModels.Items;
    using FastFood.Web.ViewModels.Orders;
    using Models;
    using System;
    using System.Globalization;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));

            this.CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionId, 
                           y => y.MapFrom(x => x.Id));

            //Employee
            this.CreateMap<RegisterEmployeeInputModel, Employee>();

            this.CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(x => x.Position, 
                           y => y.MapFrom(x => x.Position.Name));

            //Categories
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name, y => y
                               .MapFrom(x => x.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(x => x.Id));

            //Items

            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(x => x.CategoryId, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.CategoryName, y => y.MapFrom(x => x.Name));

            this.CreateMap<CreateItemInputModel, Item>();

            this.CreateMap<Item, ItemsAllViewModels>()
                .ForMember(x => x.Category, y => y.MapFrom(x => x.Category.Name));

            //Orders

            this.CreateMap<Item, CreateOrderItemViewModel>()
                .ForMember(x => x.ItemId, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.ItemName, y => y.MapFrom(x => x.Name));

            this.CreateMap<Employee, CreateOrderEmployeeViewModel>()
                .ForMember(x => x.EmployeeId, y => y.MapFrom(x => x.Id))
                .ForMember(x => x.EmployeeName, y => y.MapFrom(x => x.Name));

            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x => x.DateTime, y => y.MapFrom(x => DateTime.UtcNow))
                .ForMember(x => x.Type, y => y.MapFrom(x => OrderType.ToGo));

            this.CreateMap<CreateOrderInputModel, OrderItem>()
                 .ForMember(x => x.Quantity, y => y.MapFrom(x => x.Quantity))
                .ForMember(x => x.ItemId, y => y.MapFrom(x => x.ItemId));

            this.CreateMap<Order, OrderAllViewModel>()
                .ForMember(x => x.Employee, y => y.MapFrom(x => x.Employee.Name))
                .ForMember(x => x.DateTime, y => y.MapFrom(x => x.DateTime.ToString("D", CultureInfo.InvariantCulture)))
                .ForMember(x => x.OrderId, y => y.MapFrom(x => x.Id));

        }
    }
}
