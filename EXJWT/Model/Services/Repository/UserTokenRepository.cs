using EXJWT.Model.Context;
using EXJWT.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EXJWT.Model.Services.Repository
{
    public class UserTokenRepository
    {
        private readonly DataBaseContext MyContext;
        public UserTokenRepository(DataBaseContext dataBaseContext)
        {
            MyContext = dataBaseContext;
        }

        public void SaveToken(UserTokenDto userTokenDto)
        {
            UserToken MyUserToken = new UserToken{
                 Id = userTokenDto.Id,
                 ExpTime = userTokenDto.ExpTime ,
                 HashToken = userTokenDto.HashToken,
                 MobilModel=userTokenDto.HashToken,
                 UserId = userTokenDto.UserId               
            };
            MyContext.UserTokens.Add(MyUserToken);
            MyContext.SaveChanges();
                  
        }

    }
}
