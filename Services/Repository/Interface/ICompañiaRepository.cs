namespace Services.Repository.Interface
{
    public interface ICompañiaRepository<T> where T : class
    {
        Task<List<T>> ObtenerCompañia_x_Codigo(T clase);
        Task<List<T>> ObtenerCompañia();
        
    }
}
