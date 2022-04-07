
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TODOLIST.DTOs;
using TODOLIST.Models;
using TODOLIST.Repositories;

namespace TODOLIST.Controllers;
[ApiController]
[Route("api/alltodos")]
public class AllTodoController : ControllerBase
{
    private readonly ILogger<TodoController> _logger;
    private readonly ITodoRepository _todo;

    public AllTodoController(ILogger<TodoController> logger, ITodoRepository Todo)
    {
        _logger = logger;

        _todo = Todo;
    }


    [HttpGet]
    public async Task<ActionResult<List<TodoDTO>>> GetAllTodos()
    {
        var TodoList = await _todo.GetAllTodo();
        var dtoList = TodoList.Select(x => x.asDto);
        return Ok(dtoList);
    }



}
