using api.premium.calculator.Common.Helpers;
using api.premium.calculator.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace api.premium.calculator.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        User GetById(int id);
    }

    public class UserService : IUserService
    {        
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IFileReader _fileReader;

        public UserService(IJwtTokenHelper jwtTokenHelper, IFileReader fileReader)
        {
            _jwtTokenHelper = jwtTokenHelper;
            _fileReader = fileReader;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var users = _fileReader.LoadJson<List<User>>("Users.StaticData.json");

            if (users == null || users.Count == 0) return null;

            var user = users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            if (user == null) return null;

            var token = _jwtTokenHelper.generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public User GetById(int id)
        {
            var users = _fileReader.LoadJson<List<User>>("Users.StaticData.json");
            return users.FirstOrDefault(x => x.Id == id);
        }
    }
}
