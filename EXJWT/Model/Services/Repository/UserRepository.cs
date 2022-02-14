using EXJWT.Model.Context;
using EXJWT.Model.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EXJWT.Model.Services.Repository
{
    public class UserRepository
    {
        private readonly DataBaseContext MyContext;
        public UserRepository(DataBaseContext dataBaseContext)
        {
            MyContext = dataBaseContext;
        }

        public UserDto Get(int id)
        {
            var Result = MyContext.Users.FirstOrDefault(p=>p.Id == id);
            List<UserTokenDto> MyUserToken = new List<UserTokenDto>();
            foreach (var item in Result.userTokens)
            {
                MyUserToken.Add(new UserTokenDto
                {
                    Id = item.Id,
                    ExpTime = item.ExpTime,
                    HashToken = item.HashToken,
                    MobilModel = item.MobilModel,
                    UserId = item.UserId
                });
            }
            return new UserDto
            {
                Id = Result.Id,
                IsActive = Result.IsActive,
                Name = Result.Name,
                userTokens = MyUserToken
            };
        }

        public bool ValidateUser(string UserName , string PassWord)
        {
            var uer = MyContext.Users.FirstOrDefault();
            return uer == null ? false : true;
        }
    }
}
