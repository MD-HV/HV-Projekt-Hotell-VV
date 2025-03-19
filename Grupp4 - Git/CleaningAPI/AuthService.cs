using Microsoft.EntityFrameworkCore;

namespace CleaningAPI
{
    public class AuthService
    {
        private readonly CleaningDbContext _context;

        public AuthService(CleaningDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return null;

            // You should hash the password in your real-world applications. 
            // This is just for demonstration.
            if (user.Password != password) // Simple password comparison (use hashed password in production)
                return null;

            return user;
        }
    }
}
