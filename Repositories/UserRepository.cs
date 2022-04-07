using Dapper;
using TODOLIST.Models;
using TODOLIST.Utilities;
using SchoolTask.Repositories;

namespace TODOLIST.Repositories;
public interface IUserRepository
{

    Task<User> GetUserById(long Id);
    Task<List<User>> AllUsers();
    Task<User> FindEmail(string email);
    Task<User> UserLogin(string Email, string Passcode);
    Task<User> CreateUser(User item);
    Task<bool> UpdateUser(User item);
    Task<User> DeleteUser(long Id);


}

public class UserRepository : BaseRepository, IUserRepository
{

    public UserRepository(IConfiguration config) : base(config)
    {

    }
    public async Task<User> CreateUser(User item)
    {
        var query = $@"INSERT INTO ""{TableNames.users}""
        (user_name, email,passcode,mobile)
	     VALUES (@UserName,@Email,@Passcode,@Mobile)
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<User>(query, item);
            return res;
        }
    }

    public Task<User> DeleteUser(long Id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<User>> GetAllUsers(int pageNumber, int Limit)
    {

        // Query
        var query = $@"SELECT * FROM ""{TableNames.users}"" ORDER BY Id LIMIT @Limit OFFSET @PageNumber"; ;

        List<User> res;
        using (var con = NewConnection)
            res = (await con.QueryAsync<User>(query, new { @PageNumber = (pageNumber - 1) * Limit, Limit })).AsList();
        // using (var con = NewConnection)
        //     res = (await con.QueryAsync<User>(query))
        //     .Skip((UserParameter.PageNumber - 1) * UserParameter.PageSize)
        //     .Take(UserParameter.PageSize)
        //     .AsList();

        return res;
    }


    public async Task<User> GetUserById(long Id)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
        WHERE id = @Id";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Id });

    }

    public async Task<bool> UpdateUser(User item)
    {
        var query = $@"UPDATE ""{TableNames.users}"" SET class_id = @ClassId,
        last_name = @LastName,parent_contact = @ParentContact WHERE Id = @Id";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, item);
            return rowCount == 1;
        }


    }



    public async Task<User> UserLogin(string Email, string Passcode)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
        WHERE email = @Email AND passcode = @Passcode";
        // SQL-Injection

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { Email, Passcode });
    }

    public async Task<List<User>> AllUsers()
    {
        // Query
        var query = $@"SELECT * FROM ""{TableNames.users}""";

        List<User> res;
        using (var con = NewConnection)
            res = (await con.QueryAsync<User>(query)).AsList();

        return res;
    }

    public async Task<User> FindEmail(string email)
    {
        // Query
        var query = $@"SELECT * FROM ""{TableNames.users}"" WHERE email = @Email";
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<User>(query, new { email });

    }
}

