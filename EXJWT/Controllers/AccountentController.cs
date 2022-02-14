using EXJWT.Helper;
using EXJWT.Model.Entites;
using EXJWT.Model.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EXJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountentController : ControllerBase
    {
        private readonly UserTokenRepository userTokenRepository;
        private readonly UserRepository userRepository;
        private readonly IConfiguration configuration;
        public AccountentController(UserTokenRepository userToken , UserRepository user , IConfiguration confg )
        {
            userTokenRepository = userToken;
            userRepository = user;
            configuration = confg;
        }


        [HttpPost]
      //  [Authorize]
        public IActionResult post(string UserName , string PassWord)
        {
            SecurityHelper securityHelper = new SecurityHelper();
            if (userRepository.ValidateUser(UserName , PassWord))
            {
                var user = userRepository.Get(1);
                var claims = new List<Claim>
                {
                    new Claim ("Id", user.Id.ToString()),
                    new Claim ("Name",  user.Name),
                };
                string key = configuration["JWtConfig:Key"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenexp = DateTime.Now.AddMinutes(int.Parse(configuration["JWtConfig:expires"]));
                var token = new JwtSecurityToken(
                    issuer: configuration["JWtConfig:issuer"],
                    audience: configuration["JWtConfig:audience"],
                    expires: tokenexp,
                    notBefore: DateTime.Now,
                    claims: claims,
                    signingCredentials: credentials
                    );

                var MyJwt = new JwtSecurityTokenHandler().WriteToken(token);
                userTokenRepository.SaveToken(new UserTokenDto (){
                    MobilModel = "Iphone pro MAx" ,
                    ExpTime = tokenexp ,
                    HashToken = securityHelper.Getsha256Hash(MyJwt) ,
                    UserId = user.Id
                });

                
                return Ok(MyJwt);
            }

            else
            {
                return Unauthorized();
            }
       
        }
    }
}
