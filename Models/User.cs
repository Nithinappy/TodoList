using TODOLIST.DTOs;

namespace TODOLIST.Models;



public record User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public long Mobile { get; set; }
    public string Passcode { get; set; }

    public UserDTO asDto => new UserDTO
    {

        Id = Id,
        UserName = UserName,
        Email = Email,
        Mobile = Mobile,

    };
}