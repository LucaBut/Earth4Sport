using AutoMapper;
using Gruppo2.WebApp.Entities;
using Gruppo2.WebApp.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Gruppo2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly WebAppContex _context;

        private readonly IMapper _mapper;
        public UserController(WebAppContex context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersAsync()
        {
            List<User> users = new List<User>();
            try
            {
                users = await _context.User.ToListAsync();
            }
            catch (Exception ex) 
            {
                Console.Write(ex.ToString());
            }
            //users = await _context.User.ToListAsync();

            List<UserDto> usersDto = new List<UserDto>();
            _mapper.Map(users, usersDto);
            Console.WriteLine(users);
            return usersDto;
        }

        //restituisce gli users con di quelli ids
        [HttpGet("GetUsersByIdsSelected/{usersIds}")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByIdsSelected(Guid[] usersIds)
        {
            List<User> users = new List<User>();
            users = await _context.User.Where(x => usersIds.Contains(x.Id)).ToListAsync();
            if (!users.Any())
                return NoContent();


            List<UserDto> usersDto = new List<UserDto>();
            _mapper.Map(users, usersDto);
            return usersDto;
        }


        //restituisce gli users con di quelli ids
        [HttpGet("GetUsersTest")]
        public async Task<ActionResult<IEnumerable<string>>> GetUsersTest()
        {
            List<string> records = new List<string>();
            string record = "";
            User user = new User();
            
            try
            {
                user = await _context.User.FirstAsync();
                record = user.Name;
                records.Add(record);
                return records;

            }
            catch (Exception ex)
            {
                record = "sono un coglione e non ho trovato un cazzo" + ex.Message.ToString();
                records.Add(record);
                return records;
            }
            


            //List<User> users = new List<User>();
            //users = await _context.User.ToListAsync();
            //if (!users.Any())
            //{
            //    record = "non trovo nessun cazzo di utente";
            //    records.Add(record);    
            //    return records;
            //}


            //record = "ho trovato qualche utente pezzo di merda, sei un coglione";
            //records.Add(record);
            //return records;

        }





        [HttpGet("GetUserByMail/{mail}")]
        public async Task<ActionResult<UserDto>> GetUserByEmailAsync(string mail)
        {
            User user = new User();
            try
            {
                user = await _context.User.Where(x => x.Email == mail).FirstAsync();
                UserDto dto = new UserDto();
                _mapper.Map(user, dto);
                return dto;
            }
            catch(Exception ex)
            {
                return NoContent();
            }

        }
    }
}
