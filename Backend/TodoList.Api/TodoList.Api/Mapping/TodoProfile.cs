using AutoMapper;
using System;
using TodoList.Api.Dtos;
using TodoList.Api.Models;

namespace TodoList.Api.Mapping
{
    /// <summary>
    /// AutoMapper mapping profile for <see cref="TodoItem"/> conversions.
    /// </summary>
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoItem, TodoItemReadDto>();
            CreateMap<TodoItemWriteDto, TodoItem>()
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id ?? Guid.NewGuid()));
        }
    }
}
