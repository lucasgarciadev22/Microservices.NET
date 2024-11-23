using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ApiController : ControllerBase { }
