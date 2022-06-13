using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BWJ.Web.Core.Repository
{
    public interface IMockRepositoryServiceBase<T>
    {
        Task CreateRecord(T model);
        Task DeleteRecord(Func<T, bool> predicate);
        Task<T> GetRecord(Func<T, bool> predicate);
        Task<IEnumerable<T>> GetRecords();
        Task<IEnumerable<T>> GetRecords(Func<T, bool> predicate);
        Task UpdateRecord(Func<T, bool> predicate, T model);
    }
}