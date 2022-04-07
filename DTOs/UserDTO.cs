using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace TODOLIST.DTOs;



public record UserDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

    public long Mobile { get; set; }
    public List<TodoDTO> MyTodos { get; set; }


}






public record UserCreateDTO
{
    [JsonPropertyName("user_name")]
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string UserName { get; set; }

    [JsonPropertyName("email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [JsonPropertyName("passcode")]
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string Passcode { get; set; }

    [JsonPropertyName("mobile")]
    [Required]
    public long Mobile { get; set; }

}

public record UserUpdateDTO
{

    [JsonPropertyName("user_name")]
    [Required]
    public string UserName { get; set; }


}