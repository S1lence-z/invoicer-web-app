using Application.RepositoryInterfaces;
using Domain.Models;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite.Query.Internal;

namespace Infrastructure.Repositories
{
	public class NumberingSchemeRepository(ApplicationDbContext context) : INumberingSchemeRepository
	{
		public async Task<NumberingScheme> CreateAsync(NumberingScheme numberingScheme)
		{
			await context.NumberingScheme.AddAsync(numberingScheme);
			return numberingScheme;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			NumberingScheme numberingScheme = await GetByIdAsync(id, false);
			context.NumberingScheme.Remove(numberingScheme);
			return true;
		}

		public async Task<IEnumerable<NumberingScheme>> GetAllAsync()
		{
			return await context.NumberingScheme
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<NumberingScheme> GetByIdAsync(int id, bool isReadonly)
		{
			if (isReadonly)
				return await context.NumberingScheme
					.Include(ins => ins.EntitiesUsingScheme)
					.Include(ins => ins.InvoicesGeneratedWithScheme)
					.AsNoTracking()
					.FirstOrDefaultAsync(ins => ins.Id == id) ?? throw new KeyNotFoundException($"Numbering scheme with id {id} not found.");
			else
				return await context.NumberingScheme
					.Include(ins => ins.EntitiesUsingScheme)
					.Include(ins => ins.InvoicesGeneratedWithScheme)
					.FirstOrDefaultAsync(ins => ins.Id == id) ?? throw new KeyNotFoundException($"Numbering scheme with id {id} not found.");
		}

		public async Task<NumberingScheme> GetDefaultSchemeAsync(bool isReadonly)
		{
			if (isReadonly)
				return await context.NumberingScheme
					.AsNoTracking()
					.FirstOrDefaultAsync(ins => ins.IsDefault) ?? throw new KeyNotFoundException("Default numbering scheme not found.");
			else
				return await context.NumberingScheme
					.FirstOrDefaultAsync(ins => ins.IsDefault) ?? throw new KeyNotFoundException("Default numbering scheme not found.");
		}

		public Task<bool> IsInUseByEntity(NumberingScheme numberingScheme)
		{
			return context.Entity
				.AsNoTracking()
				.AnyAsync(e => e.CurrentNumberingSchemeId == numberingScheme.Id);
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync();
		}

		public async Task SetDefaultSchemeAsync(NumberingScheme newDefaultScheme)
		{
			IEnumerable<NumberingScheme> allDefaultSchemes = await context.NumberingScheme
				.Where(s => s.IsDefault)
				.ToListAsync();
			foreach (NumberingScheme scheme in allDefaultSchemes)
			{
				if (scheme.Id != newDefaultScheme.Id)
				{
					scheme.IsDefault = false;
					context.NumberingScheme.Update(scheme);
				}
			}
			newDefaultScheme.IsDefault = true;
		}

		public NumberingScheme Update(NumberingScheme updatedNumberingScheme)
		{
			context.NumberingScheme.Update(updatedNumberingScheme);
			return updatedNumberingScheme;
		}
	}
}
