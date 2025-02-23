namespace Domain.Interfaces
{
	public interface IService<TKey, TType>
	{
		Task<TType?> GetByIdAsync(TKey id);
		Task<IList<TType>> GetAllAsync();
		Task<TType> CreateAsync(TType obj);
		Task<TType> UpdateAsync(TKey id, TType obj);
		Task<bool> DeleteAsync(TKey id);
	}
}
