using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Core;
using Microsoft.Extensions.Configuration;
using Data;
namespace Services{
    public class AuthService{
        private readonly string _secretKey;
        private readonly UserDbContext _context;
        public AuthService(IConfiguration configuration, UserDbContext context){
            _secretKey = configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("Invalid operation.");
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public string GenerateJwtToken(User user){
            user.IsActive = true;
            _context.SaveChanges();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity([
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                ]),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public bool Logout(int id){
            var user = _context.Users.Find(id);
            if (user == null) return false;
            user.IsActive = false;
            _context.SaveChanges();
            return true;
        }
    }
}