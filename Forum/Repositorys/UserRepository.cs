namespace Forum.Repositorys
{
    using Forum.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class UserRepository : DbContext, IRepository<User>
    {
        public UserRepository(DbContextOptions<UserRepository> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }


        public async Task DeleteById(Guid id)
        {
            var user = await Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            Users.Remove(user);
            await SaveChangesAsync();
           
        }

        public Task<User> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        
        public async Task<List<User>> GetAll()
        {
            if(Users == null)
            {
                throw new NullReferenceException("User dbSet is null");
            }
            return await Users.ToListAsync();
        }

        public Task<User> Update(User newObject)
        {
            throw new NotImplementedException();
        }
        // Add more DbSet properties for other models if needed
    }
}
