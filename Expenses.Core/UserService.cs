using Expenses.Core.CustomExceptions;
using Expenses.Core.DTO;
using Expenses.DB;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Expenses.Core.Utilities;

namespace Expenses.Core
{
    public class UserService : IUserService
    {   
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public string JwtGenerator { get; private set; }

        //constructor
        public UserService(AppDbContext context, IPasswordHasher passwordHasher )
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthenticatedUser> SignUp(User user)
        {
            var checkUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(user.Username));

            if(checkUser != null)
            {
                throw new UsernameAlreadyExistsException("Username Already Exists");
            }

            // hash password
            user.Password = _passwordHasher.HashPassword(user.Password);

            //add user
            await _context.AddAsync(user);
            //save changes 
            await _context.SaveChangesAsync();

            //return the authenticated user 
            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JWTGenerator.GenerateUserToken(user.Username)
            };
        }  

        public async Task<AuthenticatedUser> SignIn(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == user.Username);

            if(dbUser == null || _passwordHasher.VerifyHashedPassword(dbUser.Password, user.Password) == PasswordVerificationResult.Failed)
            {
                throw new InvalidUsernamePasswordException("Invalid Username or Password");

            }

            return new AuthenticatedUser
            {
                Username = user.Username,
                Token = JWTGenerator.GenerateUserToken(user.Username)
            };
        }
    }
}
