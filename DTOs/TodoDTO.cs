using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TODOLIST.DTOs;

public record TodoDTO
{
    [JsonPropertyName("todo_id")]
    public long Id { get; set; }

    [JsonPropertyName("todo_title")]

    public string Title { get; set; }

    [JsonPropertyName("todo_description")]

    public string Description { get; set; }
    [JsonPropertyName("user_id")]

    public long UserID { get; set; }

}


public record TodoCreateDTO
{

    [JsonPropertyName("todo_title")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]

    public string Title { get; set; }

    [JsonPropertyName("todo_description")]
    [MaxLength(250)]
    [MinLength(3)]

    public string Description { get; set; }

}

public record TodoUpdateDTO
{

    [JsonPropertyName("todo_title")]
    [Required]
    [MaxLength(50)]
    [MinLength(3)]

    public string Title { get; set; }

    [JsonPropertyName("todo_description")]
    [MaxLength(250)]
    [MinLength(3)]

    public string Description { get; set; }



}

