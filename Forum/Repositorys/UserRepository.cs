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

        public async Task<User> GetById(Guid id)
        {
            var user = await Users.FirstOrDefaultAsync(user => user.Id == id);
            if(user is null)
            {
                throw new ArgumentNullException();
            }
            return user;
        }

        
        public async Task<List<User>> GetAll()
        {
            if(Users == null)
            {
                throw new NullReferenceException("User dbSet is null");
            }
            return await Users.ToListAsync();
        }

        public async Task<User> Update(User newObject)
        {
            try
            {
                var existingUser = await Users.FindAsync(newObject.Id);

                if (existingUser == null)
                {
                    throw new ArgumentNullException();
                }

                Entry(existingUser).CurrentValues.SetValues(newObject);

                await SaveChangesAsync();

                return existingUser;
            }
            catch(Exception)
            {
                Console.WriteLine("somthing went wrong updating user " + newObject.Id);
                throw;
            }
            
        }

        public async Task<User> ValidUser(LoginModel loginModel)
        {
            try
            {
                if (loginModel == null)
                {
                    throw new ArgumentNullException(nameof(loginModel), "Login model cannot be null");
                }

                if (Users == null)
                {
                    throw new InvalidOperationException("Users DbSet is not initialized");
                }

                var existingUser = await Users.FirstOrDefaultAsync(user =>
                    user.Pseudonyme == loginModel.Username && user.MotDePasse == loginModel.Password);

                return existingUser;
            }
            catch (Exception)
            {
                throw;
            }

        }


        private bool UserExists(Guid id)
        {
            return Users.Any(u => u.Id == id);
        }

    }
}
