using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webAPI.DataService.IConfiguration;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public IUnitOfWork _unitOfWork;
        //private readonly HeroService _heroService;

        public readonly IMapper _mapper; 
        public BaseController(
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }   
    }
}