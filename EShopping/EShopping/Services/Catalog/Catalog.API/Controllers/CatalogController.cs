using Asp.Versioning;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers;

[ApiVersion("1")]
public class CatalogController(IMediator mediator, ILogger<CatalogController> logger)
    : ApiController
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<CatalogController> _logger = logger;

    [HttpGet]
    [Route("[action]/{productId}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> GetProductById(string productId)
    {
        GetProductByIdQuery query = new(productId);
        ProductResponse result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{productName}", Name = "GetProductByName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByName(string productName)
    {
        GetProductByNameQuery query = new(productName);
        IList<ProductResponse> result = await _mediator.Send(query);
        _logger.LogInformation("Product with {ProductName} fetched", productName);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllProducts")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllProducts(
        [FromQuery] CatalogSpecParams catalogSpecParams
    )
    {
        GetAllProductsQuery query = new(catalogSpecParams);
        Pagination<ProductResponse> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllBrands")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllBrands()
    {
        GetAllBrandsQuery query = new();
        IList<BrandResponse> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("GetAllTypes")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetAllTypes()
    {
        GetAllTypesQuery query = new();
        IList<TypeResponse> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [Route("[action]/{brandName}", Name = "GetProductByBrandName")]
    [ProducesResponseType(typeof(IList<ProductResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IList<ProductResponse>>> GetProductByBrandName(string brandName)
    {
        GetProductByBrandQuery query = new(brandName);
        IList<ProductResponse> result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    [Route("CreateProduct")]
    [ProducesResponseType(typeof(ProductResponse), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ProductResponse>> CreateProduct(
        [FromBody] CreateProductCommand productCommand
    )
    {
        ProductResponse result = await _mediator.Send(productCommand);
        return Ok(result);
    }

    [HttpPut]
    [Route("UpdateProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> UpdateProduct([FromBody] UpdateProductCommand productCommand)
    {
        bool result = await _mediator.Send(productCommand);
        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        DeleteProductByIdCommand query = new(id);
        bool result = await _mediator.Send(query);
        return Ok(result);
    }
}
