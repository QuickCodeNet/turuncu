using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using QuickCode.Turuncu.Common.Filters;

namespace QuickCode.Turuncu.Common.Controllers;

[ApiExceptionFilter]
[ApiController]
[ApiKey]
[Route("api/[controller]")]
public class QuickCodeBaseApiController : ControllerBase
{

}