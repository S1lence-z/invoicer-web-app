using Domain.Models;
using Application.Mappers;
using Application.ServiceInterfaces;
using Application.RepositoryInterfaces;
using Application.DTOs;

namespace Backend.Services
{
	public class AddressService(IAddressRepository addressRepository) : IAddressService
	{
		public async Task<AddressDto> GetByIdAsync(int id)
		{
			Address foundAddress = await addressRepository.GetByIdAsync(id, true);
			return AddressMapper.MapToDto(foundAddress);
		}
		 
		public async Task<IList<AddressDto>> GetAllAsync()
		{
			IEnumerable<Address> allAdresses = await addressRepository.GetAllAsync();
			return allAdresses.Select(AddressMapper.MapToDto).ToList();
		}

		public async Task<AddressDto> CreateAsync(AddressDto newAddress)
		{
			Address address = AddressMapper.MapToDomain(newAddress);
			await addressRepository.CreateAsync(address);
			await addressRepository.SaveChangesAsync();
			return AddressMapper.MapToDto(address);
		}

		public async Task<AddressDto> UpdateAsync(int id, AddressDto updatedAddress)
		{
			Address existingAddress = await addressRepository.GetByIdAsync(id, false);
			existingAddress.Street = updatedAddress.Street;
			existingAddress.City = updatedAddress.City;
			existingAddress.ZipCode = updatedAddress.ZipCode;
			existingAddress.Country = updatedAddress.Country;

			addressRepository.Update(existingAddress);
			await addressRepository.SaveChangesAsync();
			return AddressMapper.MapToDto(existingAddress);
		}

		public async Task<bool> DeleteAsync(int id)
		{
			await addressRepository.DeleteAsync(id);
			await addressRepository.SaveChangesAsync();
			return true;
		}
	}
}
