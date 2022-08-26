using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SalesMine.Core.Bus;
using SalesMine.Core.Controller;
using SalesMine.Core.Identity;
using SalesMine.Core.Messages.Integration;
using SalesMine.Identity.API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SalesMine.Identity.API.Controllers
{
    [Route("api/identity")]
    public class AuthController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IMessageBus _messageBus;

        public AuthController(IOptions<AppSettings> appSettings, SignInManager<IdentityUser> signInManage, UserManager<IdentityUser> userManager, IMessageBus messageBus)
        {
            _appSettings = appSettings.Value;
            _signInManager = signInManage;
            _userManager = userManager;
            _messageBus = messageBus;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginViewModel userLoginViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(userLoginViewModel.Email, userLoginViewModel.Senha, false, true);

            if (result.Succeeded)
                return CustomResponse(await CreateJwt(userLoginViewModel.Email));

            if (result.IsLockedOut)
            {
                AddProcessingError("User temporarily blocked by invalid attempts");
                return CustomResponse();
            }

            AddProcessingError("Invalid user and password");

            return CustomResponse();
        }

        [HttpPost("new-account")]
        public async Task<ActionResult> Register(UserRegisterViewModel userRegisterViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegisterViewModel.Email,
                Email = userRegisterViewModel.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegisterViewModel.Password);

            if (result.Succeeded)
            {
                var customerResult = await RegisterCustomer(userRegisterViewModel);

                if (!customerResult.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);

                    return CustomResponse(customerResult.ValidationResult);
                }

                return CustomResponse(await CreateJwt(userRegisterViewModel.Email));
            }

            foreach (var error in result.Errors)
            {
                AddProcessingError(error.Description);
            }

            return CustomResponse();
        }

        private async Task<ResponseMessage> RegisterCustomer(UserRegisterViewModel userRegister)
        {
            var user = await _userManager.FindByEmailAsync(userRegister.Email);
            var registeredUser = new RegisteredUserIntegrationEvent(Guid.Parse(user.Id), user.UserName, user.Email, userRegister.Cpf);

            try
            {
                return await _messageBus.RequestAsync<RegisteredUserIntegrationEvent, ResponseMessage>(registeredUser
            }
            catch (Exception)
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }

        private async Task<UserLoginResponseViewModel> CreateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = await GetClaimsAsync(user, userRoles);
            var identityClaims = GetIdentityClaims(claims);

            string encodedToken = EncodeToken(identityClaims);

            return GetResponseToken(user, claims, encodedToken);
        }

        private UserLoginResponseViewModel GetResponseToken(IdentityUser user, IList<Claim> claims, string encodedToken)
        {
            return new UserLoginResponseViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };
        }

        private static ClaimsIdentity GetIdentityClaims(IList<Claim> claims)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);
            return identityClaims;
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);
            return encodedToken;
        }

        private async Task<IList<Claim>> GetClaimsAsync(IdentityUser user, IList<string> userRoles)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }

        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
