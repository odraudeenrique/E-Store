using My_Store.Models.UserModels;

namespace My_Store.Infrastructure.Interfaces
{
    public interface IRepository<TInput, TOutput> 
        where TInput : class 
        where TOutput : class
    {
        Task<TOutput> Create(TInput Entity);
        //void Update(T Entity);  
        //T GetById(int id);
        //List<T> GetAll();

    }
}
