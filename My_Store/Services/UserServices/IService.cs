namespace My_Store.Services.UserServices
{
    public interface IService <in T> where T : class
    {
        void Create(T entity);  
        void Update(T entity);
      
    }
}
