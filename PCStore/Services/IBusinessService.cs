namespace PCStore.Services
{
    public interface IBusinessService
    {
        Task<decimal> GetTotalInventoryValueAsync();
        Task<int> GetTotalProductsCountAsync();
    }
}
