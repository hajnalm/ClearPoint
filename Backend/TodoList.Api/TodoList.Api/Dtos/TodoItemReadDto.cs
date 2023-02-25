using System;

namespace TodoList.Api.Dtos
{
    /// <summary>
    /// The DTO class used to represent TodoItem data retrieved from existing items.
    /// </summary>
    public class TodoItemReadDto
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }


        public override bool Equals(object obj)
        {
            return obj is TodoItemReadDto dto &&
                   Id.Equals(dto.Id) &&
                   Description == dto.Description &&
                   IsCompleted == dto.IsCompleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description, IsCompleted);
        }
    }
}
