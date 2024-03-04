using GameOfLife.Context;
using GameOfLife.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Repository
{
    public class UserRepository : IRepository<User>
    {
        public GameOfLifeContext GameContext { get; set; }

        public UserRepository(GameOfLifeContext context) 
        {
            this.GameContext = context;
        }

        public void Delete(User entity)
        {
            GameContext.Remove(entity);
        }

        public User FindByID(int id)
        {
            return GameContext.Users.Find(id);
        }

        public User FindByName(string name)
        {
            return GameContext.Users.FirstOrDefault(u => u.Name.Equals(name));
        }

        public IList<User> GetAll()
        {
            return GameContext.Users.ToList();
        }

        public void Insert(User entity)
        {
            GameContext.Users.Add(entity);
        }

        public void Update(User entity)
        {
            GameContext.Users.Update(entity);
        }
    }
}
