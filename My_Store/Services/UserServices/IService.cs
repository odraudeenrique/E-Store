namespace My_Store.Services.UserServices
{
    public interface IService <TInput,TOutput> 
        where TInput : class
        where TOutput : class 
    {
        Task<TOutput> Create(TInput entity);  
        //void Update(T entity);
      
    }
}
