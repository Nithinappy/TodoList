
using TODOLIST.DTOs;

namespace TODOLIST.Models;
public record Todo
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }



    public TodoDTO asDto => new TodoDTO
    {
        Id = Id,
        Title = Title,
        Description = Description,
        UserID = UserId


    };
}