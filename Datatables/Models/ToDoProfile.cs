using AutoMapper;
using Datatables.Sample.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Datatables.Sample.Web.Models
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDoModel, ToDoViewModel>()
                .ForMember(viewModel => viewModel.CreatedBy, map => map.MapFrom(model => model.CreatedBy.FullName))
                .ReverseMap();
            
        }
    }
}
