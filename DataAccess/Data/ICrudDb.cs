using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public interface ICrudDb<T, U>
    {
        Task CreateAsync(T item);
        Task<T> ReadAsync(U key);
        Task<ICollection<T>> ReadAllAsync();
        Task UpdateAsync(T item);
        Task DeleteAsync(U key);
    }
}
