namespace Application.UseCases
{
    using System.Threading.Tasks;
    using Application.Boundaries.Register;
    using Application.Repositories;
    using Application.Services;
    using Domain;
    using Domain.Accounts;
    using Domain.Customers;
    using Domain.ValueObjects;

    public sealed class Register : IUseCase
    {
        private readonly IUserService _userService;
        private readonly IEntityFactory _entityFactory;
        private readonly IOutputPort _outputPort;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public Register(
            IUserService userService,
            IEntityFactory entityFactory,
            IOutputPort outputPort,
            ICustomerRepository customerRepository,
            IAccountRepository accountRepository,
            IUserRepository userRepository,
            IUnitOfWork unityOfWork)
        {
            _userService = userService;
            _entityFactory = entityFactory;
            _outputPort = outputPort;
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _unitOfWork = unityOfWork;
        }

        public async Task Execute(RegisterInput input)
        {
            ICustomer customer;

            if (_userService.GetCustomerId() is CustomerId customerId)
            {
                try
                {
                    customer = await _customerRepository.GetBy(customerId);
                    _outputPort.CustomerAlreadyRegistered($"Customer already exists.");
                    return;
                }
                catch (CustomerNotFoundException)
                {
                }
            }

            customer = _entityFactory.NewCustomer(input.SSN, _userService.GetUserName());

            var account = _entityFactory.NewAccount(customer);

            var credit = account.Deposit(
                _entityFactory,
                input.InitialAmount);

            customer.Register(account);

            var user = _entityFactory.NewUser(
                customer,
                _userService.GetExternalUserId());

            await _customerRepository.Add(customer);
            await _accountRepository.Add(account, credit);
            await _userRepository.Add(user);
            await _unitOfWork.Save();

            BuildOutput(_userService.GetExternalUserId(), customer, account);
        }

        public void BuildOutput(
            ExternalUserId externalUserId,
            ICustomer customer,
            IAccount account)
        {
            var output = new RegisterOutput(
                externalUserId,
                customer,
                account);
            _outputPort.Standard(output);
        }
    }
}