﻿using System.Linq;
using System.Threading.Tasks;

namespace VeterinaryClinicGS.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();


        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity);


        Task UpdateAsync(T entity);

        Task DeletAsync(T entity);


        Task<bool> ExistAsync(int id);















    }
}
