using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketBackend.Repository
{
    public interface IRepository<T>
    {
        /// <summary>
        /// Retrieve all entity from a table
        /// </summary>
        /// <returns>A list of entity</returns>
        IList<T> GetAll();

        /// <summary>
        /// Retrieve one entity by a specific ID
        /// </summary>
        /// <param name="id">ID of the entity</param>
        /// <returns>An entity</returns>
        T FindByID(int id);

        /// <summary>
        /// Retrieve a entity by a specific name
        /// </summary>
        /// <param name="name">Name of the entity</param>
        /// <returns>An entity</returns>
        T FindByName(string name);

        /// <summary>
        /// Allow to insert a new entity to the table
        /// </summary>
        /// <param name="entity">The new entity to insert in the database</param>
        void Insert(T entity);

        /// <summary>
        /// An entity which already exist but we modify some data about this entity
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Will remove an entity from the table
        /// </summary>
        /// <param name="entity">Entity to remove</param>
        void Delete(T entity);
    }
}
