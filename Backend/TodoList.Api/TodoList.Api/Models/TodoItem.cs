using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.Models
{
    /// <summary>
    /// Represents a to-do task.
    /// </summary>
    public class TodoItem
    {
        /// <summary>
        /// The unique identifier of the item.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// The description of the task.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Indicates whether the task has been completed.
        /// </summary>
        [Required]
        public bool IsCompleted { get; set; }

        public override bool Equals(object obj)
        {
            return obj is TodoItem item &&
                   Id.Equals(item.Id) &&
                   Description == item.Description &&
                   IsCompleted == item.IsCompleted;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description, IsCompleted);
        }
    }
}
