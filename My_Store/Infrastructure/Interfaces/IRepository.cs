using My_Store.Models.UserModels;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IRepository<in T> where T : class
    {
        Task<int> Create(T Entity);
        void Update(T Entity);  
        //T GetById(int id);
        //List<T> GetAll();

    }
}
