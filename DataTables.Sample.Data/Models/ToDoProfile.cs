using AutoMapper;
using Datatables.Sample.Domain.Models;
using Datatables.Samples.Models;

namespace DataTables.Sample.Data.Models
{
    public class ToDoProfile : Profile
    {
        public ToDoProfile()
        {
            CreateMap<ToDo, ToDoModel>();

            CreateMap<Person, UserModel>();
        }
    }
}
