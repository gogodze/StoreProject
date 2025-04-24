using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("/api/[controller]")]
[Produces("application/json")]
public abstract class ApiController : ControllerBase;