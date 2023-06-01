using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationErrorController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;

        public NotificationErrorController(WebAppContex context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }



    }

}
