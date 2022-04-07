using Dapper;
using TODOLIST.Models;
using TODOLIST.Utilities;
using SchoolTask.Repositories;

namespace TODOLIST.Repositories;
public interface ITodoRepository
{


    Task<Todo> CreateTodo(Todo item);
    Task<Todo> GetTodoById(long Id);
    Task<List<Todo>> GetAllTodo();
    Task<List<Todo>> GetUserTodoById(long Id);

    Task<bool> DeleteTodo(long Id);
    Task<bool> UpdateTodo(Todo item);

}

public class TodoRepository : BaseRepository, ITodoRepository
{

    public TodoRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Todo> CreateTodo(Todo item)
    {
        var query = $@"INSERT INTO ""{TableNames.todos}""
        (title,description,user_id)
        VALUES (@Title,@Description,@UserId)  RETURNING *";
        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Todo>(query, item);
            return res;
        }
    }

    public async Task<bool> DeleteTodo(long Id)
    {
        var query = $@"Delete  FROM ""{TableNames.todos}""
        WHERE id = @Id";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { Id });
            return res > 0;
        }

    }

    public async Task<List<Todo>> GetAllTodo()
    {
        // Query
        var query = $@"SELECT * FROM ""{TableNames.todos}""";

        List<Todo> res;
        using (var con = NewConnection)
            res = (await con.QueryAsync<Todo>(query)).AsList(); // Execute the query

        return res;
    }

    public async Task<List<Todo>> GetUserTodoById(long Id)
    {
        var query = $@"SELECT * FROM {TableNames.todos} WHERE user_id = @Id";

        using (var con = NewConnection)
            return (await con.QueryAsync<Todo>(query, new { Id })).AsList();
    }

    public async Task<Todo> GetTodoById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.todos}""
        WHERE id = @Id";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Todo>(query, new { Id });

    }

    public async Task<bool> UpdateTodo(Todo item)
    {
        var query = $@"UPDATE ""{TableNames.todos}"" SET title = @Title,
        description = @Description WHERE id = @Id";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, item);
            return rowCount == 1;
        }
    }
}
