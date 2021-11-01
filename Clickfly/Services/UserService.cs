using System;
using System.Net;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.ViewModels;
using clickfly.Repositories;
using clickfly.Exceptions;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;

namespace clickfly.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IOptions<AppSettings> appSettings, IUtils utils, IUserRepository userRepository) : base(appSettings, utils)
        {
            _userRepository = userRepository;
        }

        public async Task<Authenticated> Authenticate(AuthenticateParams authenticateParams)
        {
            User user = null;

            string username = authenticateParams.username;
            string password = authenticateParams.password;

            bool isEmail = _utils.IsValidEmail(username);

            if(isEmail)
            {
                user = await _userRepository.GetByEmail(username);
            }
            else
            {
                user = await _userRepository.GetByUsername(username);
            }

            if(user == null)
            {
                throw new NotFoundException("Usuário ou senha inválidos.");
            }

            bool passwordIsValid = BCryptNet.Verify(password, user.password_hash);
            if(!passwordIsValid)
            {
                throw new NotFoundException("Usuário ou senha inválidos.");
            }

            string token = GenerateToken(user);
            Authenticated authenticated = new Authenticated();

            authenticated.token = token;
            authenticated.auth_user = user;

            return authenticated;
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetById(string id)
        {
            User user = await _userRepository.GetById(id);
            return user;
        }

        public Task<PaginationResult<User>> Pagination(PaginationFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Save(User user)
        {
            bool update = user.id != "";

            if(update)
            {
                //user = await _userRepository.Update(user);
            }
            else
            {
                user = await _userRepository.Create(user);
            }

            return user;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.name.ToString()),
                    new Claim(ClaimTypes.Role, user.role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}
