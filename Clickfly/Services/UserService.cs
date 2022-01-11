using System;
using System.Net;
using System.Threading.Tasks;
using clickfly.Models;
using clickfly.Repositories;
using clickfly.Exceptions;
using BCryptNet = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using clickfly.Helpers;
using clickfly.ViewModels;
using System.Collections.Generic;

namespace clickfly.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IPermissionGroupRepository _permissionGroupRepository;
        private readonly IEmailService _emailService;

        public UserService(
            IOptions<AppSettings> appSettings, 
            ISystemLogRepository systemLogRepository,
            IPermissionRepository permissionRepository,
            INotificator notificator, 
            IInformer informer,
            IUtils utils, 
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IPermissionGroupRepository permissionGroupRepository,
            IEmailService emailService
        ) : base(appSettings, systemLogRepository, permissionRepository, notificator, informer, utils)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _permissionGroupRepository = permissionGroupRepository;
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

        public async Task<PaginationResult<User>> Pagination(PaginationFilter filter)
        {
            PaginationResult<User> paginationResult = await _userRepository.Pagination(filter);
            return paginationResult;
        }

        public async Task<User> Save(User user)
        {
            bool update = user.id != "";
            //User authUser = _informer.GetValue<User>(UserTypes.User);

            if(update)
            {
                await _userRepository.Update(user);
            }
            else
            {
                string password = _utils.RandomHexString();

                if((await _userRepository.GetByEmail(user.email)) != null)
                {
                    throw new ConflictException("Já existe um usuário associado a esse email.");
                }

                if((await _userRepository.GetByUsername(user.username)) != null)
                {
                    throw new ConflictException("Já existe um usuário associado a esse nome de usuário.");
                }

                user.password = password;
                //user.created_by = authUser.id;
                user = await _userRepository.Create(user);

                // Criar grupo de permissões
                UserRole userRole = await _userRoleRepository.GetByName(user.role);
                if(userRole == null)
                {
                    throw new UnauthorizedException("Permissão negada.");
                }
                
                List<UserRolePermission> permissions = userRole.permissions;
                PermissionGroup permissionGroup = new PermissionGroup();
                permissionGroup.user_id = user.id;
                permissionGroup.user_role_id = userRole.id;
                permissionGroup.allowed = true;

                Console.WriteLine($"Permissions: {permissions.Count}");

                permissionGroup = await _permissionGroupRepository.Create(permissionGroup);
                foreach(UserRolePermission userRolePermission in permissions)
                {
                    Console.WriteLine(userRolePermission.id);
                    Permission permission = new Permission();
                    permission.permission_group_id = permissionGroup.id;
                    permission.permission_resource_id = userRolePermission.permission_resource_id;
                    permission._create = userRolePermission._create;
                    permission._read = userRolePermission._read;
                    permission._update = userRolePermission._update;
                    permission._delete = userRolePermission._delete;
                    //permission.created_by = authUser.id;

                    permission = await _permissionRepository.Create(permission);
                }

                // Enviar email
                UserLoginCredentials loginCredentials = new UserLoginCredentials();
                loginCredentials.name = user.name;
                loginCredentials.username = user.username;
                loginCredentials.password = password;

                EmailRequest emailRequest = new EmailRequest();
                emailRequest.to = "josuefan56@gmail.com"; // modificar aqui
                emailRequest.from = "noreply@clickfly.app";
                emailRequest.fromName = "ClickFly";
                emailRequest.subject = "Acesso ao sistema ClickFly";
                emailRequest.templateName = "UserLoginCredentials";
                emailRequest.model = loginCredentials;
                emailRequest.modelType = typeof(UserLoginCredentials);

                _emailService.SendEmail(emailRequest);

                SystemLog systemLog = new SystemLog();
                systemLog.resource_id = user.id;
                //systemLog.user_id = authUser.id;
                //systemLog.created_by = authUser.id;
                systemLog.resource = Resources.Users;
                systemLog.user_type = UserTypes.User;
                systemLog.action = Actions.Create;

                systemLog = await _systemLogRepository.Create(systemLog);
            }

            user = await _userRepository.GetById(user.id);
            return user;
        }

        public async Task<User> UpdateRole(UpdateRole updateRole)
        {
            User user = await _userRepository.GetById(updateRole.user_id);
            UserRole currentRole = await _userRoleRepository.GetByUserId(user.id);

            if(user.role != currentRole.name)
            {
                UserRole userRole = await _userRoleRepository.GetByName(updateRole.role);
                if(userRole == null)
                {
                    throw new UnauthorizedException("Permissão negada.");
                }

                // Atualizar grupo
                PermissionGroup permissionGroup = await _permissionGroupRepository.GetByUserId(user.id);
                permissionGroup.user_role_id = userRole.id;

                // Excluir permissões atuais
                foreach(Permission p in permissionGroup.permissions)
                {
                    await _permissionRepository.Delete(p.id);
                }

                permissionGroup = await _permissionGroupRepository.Update(permissionGroup);

                // Atribuir novas permissões
                foreach(UserRolePermission p in userRole.permissions)
                {
                    Permission permission = new Permission();
                    permission.permission_group_id = permissionGroup.id;
                    permission.permission_resource_id = p.permission_resource_id;
                    permission._create = p._create;
                    permission._read = p._read;
                    permission._update = p._update;
                    permission._delete = p._delete;

                    permission = await _permissionRepository.Create(permission);
                }
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
                    new Claim(UserIdTypes.UserId, user.id),
                    new Claim(ClaimTypes.Name, user.name.ToString()),
                    new Claim(ClaimTypes.Role, user.role.ToString()),
                    new Claim(UserTypes.User, JsonConvert.SerializeObject(user))
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var encodedToken = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(encodedToken);
        }
    }
}
