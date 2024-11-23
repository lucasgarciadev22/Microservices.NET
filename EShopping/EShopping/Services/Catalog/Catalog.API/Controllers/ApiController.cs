using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class ApiController : ControllerBase { }
