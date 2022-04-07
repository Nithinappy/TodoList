
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOLIST.DTOs;
using TODOLIST.Models;
using TODOLIST.Repositories;

namespace TODOLIST.Controllers;
[ApiController]
[Route("api/todos")]
public class TodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoRepository _todo;

    public TodoController(ILogger<TodoController> logger, ITodoRepository Todo)
    {
        _logger = logger;

        _todo = Todo;
    }


    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TodoDTO>>> GetAllTodos()
    {
        var TodoList = await _todo.GetAllTodo();
        var dtoList = TodoList.Select(x => x.asDto);
        return Ok(dtoList);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<TodoDTO>> CreateTodo([FromBody] TodoCreateDTO Data)
    {
        var UserId = GetCurrentUserId();
        // var Id = int.Parse(UserId);
        var toCreateTodo = new Todo
        {
            Title = Data.Title.Trim(),
            Description = Data.Description.Trim(),
            UserId = Int64.Parse(UserId)
        };
        var createdTodo = await _todo.CreateTodo(toCreateTodo);

        return StatusCode(StatusCodes.Status201Created, createdTodo.asDto);
    }
    [HttpGet("mytodos")]
    [Authorize]
    public async Task<ActionResult> GetTodoByUserId()
    {
        var UserId = GetCurrentUserId();
        var Id = Int64.Parse(UserId);
        var Todo = await _todo.GetUserTodoById(Id);
        if (Todo is null)
            return NotFound("No Todo found with given Todo Id");
        var dtoList = Todo.Select(x => x.asDto);
        return Ok(dtoList);
    }

    [HttpGet("{id}")]
    [Authorize]

    public async Task<ActionResult> GetTodoById(long id)
    {
        var UserId = GetCurrentUserId();
        var Id = Int64.Parse(UserId);
        var Todo = await _todo.GetTodoById(id);
        if (Todo is null)
            return NotFound("No Todo found with given Todo Id");
        var dtoList = Todo.asDto;
        return Ok(dtoList);
    }
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> Update([FromRoute] long id,
   [FromBody] TodoUpdateDTO Data)
    {
        var existing = await _todo.GetTodoById(id);
        if (existing is null)
            return NotFound("No Todo found with given Todo Id");
        var UserId = GetCurrentUserId();
        var Id = Int64.Parse(UserId);
        if (Id != existing.UserId)
            return Unauthorized("Your not authorized to update.");
        if (existing is null)
            return NotFound("No Todo found with given  Id");

        var toUpdateTodo = existing with
        {
            Title = Data.Title?.Trim() ?? existing.Title,
            Description = Data.Description?.Trim() ?? existing.Description,

        };
        var didUpdate = await _todo.UpdateTodo(toUpdateTodo);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteTodo([FromRoute] long id)
    {
        var existing = await _todo.GetTodoById(id);
        if (existing is null)
            return NotFound("No Todo found with given Todo Id");
        var UserId = GetCurrentUserId();
        var Id = Int64.Parse(UserId);
        if (Id != existing.UserId)
            return Unauthorized("Your not authorized to Delete.");
        if (existing is null)
            return NotFound("No Todos found to Delete");

        var didDelete = await _todo.DeleteTodo(id);

        return NoContent();
    }


    private string GetCurrentUserId()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var userClaims = identity.Claims;
        return (userClaims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value);
    }


}
