using MediatR;

namespace Catalog.API.Controllers;

public class CatalogController : ApiController
{
    public CatalogController(IMediator mediator, ILogger<CatalogController> logger)
    {

    }
}