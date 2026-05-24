using SalvageCore.DTOs.Customer.Request;
using SalvageCore.DTOs.Customer.Response;
using SalvageCore.Helpers;
using SalvageCore.Models;

namespace SalvageCore.Interface;

public interface ICustomerRepository
{
    public Task<ServiceResponse<bool>> TemporaryRegisterCustomer(CustomerRequest customer);
    public Task<ServiceResponse<SystemUser>> RegisterCustomer(CustomerRequest customer);
    public Task<ServiceResponse<bool>> VerifyCustomerAccount(VerifyCodeRequest request);
    public Task<ICollection<CustomerList>> FetchCustomers();
    public Task<CustomerProfile?> FetchCustomerProfile(Guid customerId);
    public Task<CustomerInfo?> FetchCustomerInfo(string email);
    public Task<bool> CustomerExists(Guid customerId);
    public Task<ICollection<IdTypeList>> FetchIdentityTypes();
    public Task<ServiceResponse<bool>> AccountComplete(Guid customerId);
    public Task<bool> CustomerPhoneExists(string phone);
    public Task<bool> CustomerEmailExists(string email);
    public Task<bool> CustomerIdentityExists(string cardNumber);
    public Task<ServiceResponse<Customer>> CompleteRegistration(CompleteRegistrationReq request);
}