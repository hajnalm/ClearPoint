using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Dtos
{
    /// <summary>
    /// The DTO class used to represent data to be written to a new or existing TodoItem.
    /// </summary>
    public class TodoItemWriteDto
    {
        public Guid? Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsCompleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TodoItemWriteDto dto &&
                   EqualityComparer<Guid?>.Default.Equals(Id, dto.Id) &&
                   Description == dto.Description &&
                   IsCompleted == dto.IsCompleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description, IsCompleted);
        }
    }
}
