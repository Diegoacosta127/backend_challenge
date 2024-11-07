using Core;
using Data;

namespace Services;

public class UserService{
    private readonly UserDbContext _context;
    private readonly PasswordService _passwordService;

    public UserService(UserDbContext context, PasswordService passwordService){
        _context = context;
        _passwordService = passwordService;
    }

    public IEnumerable<User> GetAllUsers(){
        return _context.Users.ToList();
    }

    public User? GetUserById(int id){
        return _context.Users.Find(id);
    }

    public User CreateUser(User user){
        user.Password = _passwordService.HashPassword(user.Password);
        user.IsActive = false;
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? UpdateUser(int id, User updatedUser){
        var existsUser = _context.Users.Find(id);
        if (existsUser == null){
            return null;
        }
        existsUser.Name = updatedUser.Name;
        existsUser.Email = updatedUser.Email;
        existsUser.IsActive = updatedUser.IsActive;
        if (!string.IsNullOrWhiteSpace(updatedUser.Password) && 
            !_passwordService.VerifyPassword(updatedUser.Password, existsUser.Password))
        {
            existsUser.Password = _passwordService.HashPassword(updatedUser.Password);
        }
        _context.SaveChanges();
        return existsUser;
    }

    public bool DeleteUser(int id){
        var existsUser = _context.Users.Find(id);
        if(existsUser == null){
            return false;
        }
        _context.Users.Remove(existsUser);
        _context.SaveChanges();
        return true;
    }

    public User? AuthenticateUser(string name, string password){
        var user = _context.Users.SingleOrDefault(u => u.Name == name);
        if (user == null || !_passwordService.VerifyPassword(password, user.Password)){
            return null;
        }
        return user;
    }

    public User AuthenticateUser(object name, string password)
    {
        throw new NotImplementedException();
    }
}
