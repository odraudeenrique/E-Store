namespace My_Store.Services.UserServices
{
    public interface IService <in T> where T : class
    {
        Task Create(T entity);  
        void Update(T entity);
      
    }
}
