namespace WebApi.UseCases.V1.RegisterCustomer
{
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using Application.Boundaries.RegisterCustomer;
    using Domain.ValueObjects;
    using FluentMediator;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public sealed class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly RegisterCustomerPresenter _presenter;

        public CustomersController(
            IMediator mediator,
            RegisterCustomerPresenter presenter)
        {
            _mediator = mediator;
            _presenter = presenter;
        }

        /// <summary>
        /// Register a customer
        /// </summary>
        /// <response code="201">The registered customer was create successfully.</response>
        /// <response code="400">Bad request.</response>
        /// <response code="500">Error.</response>
        /// <param name="request">The request to register a customer</param>
        /// <returns>The newly registered customer</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RegisterCustomerResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody][Required] RegisterCustomerRequest request)
        {
            var input = new RegisterCustomerInput(
                new SSN(request.SSN),
                new Name(request.Name),
                new Username(request.Username),
                new Password(request.Password)
            );
            await _mediator.PublishAsync(input);
            return _presenter.ViewModel;
        }
    }
}