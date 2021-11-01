using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;

namespace clickfly.Services
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ICustomerAddressRepository _customerAddressRepository;

        public CustomerAddressService(ICustomerAddressRepository customerAddressRepository)
        {
            _customerAddressRepository = customerAddressRepository;
        }

        public async Task Delete(string id)
        {
            await _customerAddressRepository.Delete(id);
            return;
        }

        public async Task<CustomerAddress> GetById(string id)
        {
            CustomerAddress customerAddress = await _customerAddressRepository.GetById(id);
            return customerAddress;
        }

        public async Task<PaginationResult<CustomerAddress>> Pagination(PaginationFilter filter)
        {
            PaginationResult<CustomerAddress> paginationResult = await _customerAddressRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<CustomerAddress> Save(CustomerAddress customerAddress)
        {
            bool update = customerAddress.id != "";

            if(update)
            {
                customerAddress = await _customerAddressRepository.Update(customerAddress);
            }
            else
            {
                customerAddress = await _customerAddressRepository.Create(customerAddress);
            }

            return customerAddress;
        }
    }
}
