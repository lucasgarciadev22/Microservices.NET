using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controler]")]
public class ApiController : ControllerBase { }
