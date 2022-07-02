using System;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using Microsoft.Extensions.Options;
using clickfly.Helpers;
using clickfly.ViewModels;
using PagarmeCoreApi.Standard.Models;
using PagarmeCoreApi.Standard.Controllers;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class CustomerAddressService : BaseService, ICustomerAddressService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAddressRepository _customerAddressRepository;
        private readonly ICustomersController _customersController;

        public CustomerAddressService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            ICustomersController customersController,
            ICustomerRepository customerRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils,
            ICustomerAddressRepository customerAddressRepository
        ) : base(
            appSettings,
            systemLogRepository,
            permissionRepository,
            notificator,
            informer,
            utils
        )
        {
            _customerRepository = customerRepository;
            _customersController = customersController;
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
            string customerId = _informer.GetValue(UserIdTypes.CustomerId);
            bool update = customerAddress.id != "";

            if(update)
            {
                customerAddress = await _customerAddressRepository.Update(customerAddress);
            }
            else
            {
                string customerAddressId = Guid.NewGuid().ToString();
                Customer customer = await _customerRepository.GetById(customerId);

                CreateAddressRequest addressRequest = new CreateAddressRequest();
                addressRequest.Line1 = $"{customerAddress.number}, {customerAddress.street}, {customerAddress.neighborhood}";

                if(customerAddress.complement != null && customerAddress.complement != "")
                {
                    //addressRequest.Line2 = customerAddress.complement;
                }

                Dictionary<string, string> metadata = new Dictionary<string, string>();
                metadata.Add("customer_address_id", customerAddressId);

                addressRequest.ZipCode = customerAddress.zipcode;
                addressRequest.City = customerAddress.city;
                addressRequest.State = customerAddress.state;
                addressRequest.Country = "BR";
                addressRequest.Metadata = metadata;
                
                GetAddressResponse getAddressResponse = await _customersController.CreateAddressAsync(customer.customer_id, addressRequest);

                customerAddress.id = customerAddressId;
                customerAddress.address_id = getAddressResponse.Id;
                customerAddress = await _customerAddressRepository.Create(customerAddress);
            }

            return customerAddress;
        }
    }
}
