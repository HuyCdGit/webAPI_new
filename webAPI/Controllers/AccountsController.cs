using AutoMapper;
using webAPI.DataService.IConfiguration;
using Microsoft.AspNetCore.Identity;
using webAPI.Authentication.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using webAPI.Authentication.Model.Dto.Incoming;
using webAPI.Authentication.Model.Dto.Outgoing;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using webAPI.Entites.Dbset;
using webAPI.Authentication.Model.Dto.Generic;

namespace webAPI.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly UserManager<IdentityUser> _UserManager;
        private readonly TokenValidationParameters _tokenValidationPrameters;
        //private readonly IUnitOfWork _unitOfWork;
        private readonly JwtConfig _JwtConfig;

        public AccountsController(
            IUnitOfWork unitOfWork,
            UserManager<IdentityUser> UserManager,
            TokenValidationParameters tokenValidationParameters,
            IOptionsMonitor<JwtConfig> OptionsMonitor,
            IMapper mapper
        ) : base(unitOfWork, mapper)
        {
            _JwtConfig = OptionsMonitor.CurrentValue;
            _UserManager = UserManager;
            _tokenValidationPrameters = tokenValidationParameters;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegistrationRequestDto UserRequest)
        {
            if (ModelState.IsValid)
            {
                // Check email is exist
                var userExist = await _UserManager.FindByEmailAsync(UserRequest.Email);
                if (userExist != null)
                {
                    return BadRequest(new UserRegistrationResponseDto
                    {
                        Success = false,
                        Error = new List<string>(){
                            "Email already in use"
                        }
                    });
                }

                //add the user
                var newUser = new IdentityUser
                {
                    Email = UserRequest.Email,
                    UserName = UserRequest.Name,
                    EmailConfirmed = true,
                };

                var isCreated = await _UserManager.CreateAsync(newUser, UserRequest.Password);
                if (!isCreated.Succeeded)
                {
                    return BadRequest(new UserRegistrationResponseDto
                    {
                        Success = false,
                        Error = isCreated.Errors.Select(x => x.Description).ToList()
                    });
                }
                var _hero = new SuperHero();
                _hero.Identity = new Guid(newUser.Id);
                _hero.Name = UserRequest.Name;
                _hero.FirstName = UserRequest.FirstName;
                _hero.LastName = UserRequest.LastName;
                _hero.Places = "";
                _hero.Email = UserRequest.Email;

                await _unitOfWork.Heroes.Add(_hero);
                await _unitOfWork.CompleteAsnyc();
                //Create a jwt Token 
                var token = await GenergateJwtToken(newUser);
                //return back the user
                return Ok(new UserRegistrationResponseDto
                {
                    Success = true,
                    Token = token.JwtToken,
                    RefreshToken = token.RefreshToken
                });
            }
            else
            {
                return BadRequest(new UserRegistrationResponseDto
                {
                    Success = false,
                    Error = new List<string>(){
                        "Invaild payload"
                    }
                });

            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                // Check email is exist 
                var userExist = await _UserManager.FindByEmailAsync(loginRequest.Email);
                if (userExist == null)
                {
                    return BadRequest(new UserLoginResponeDto()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }

                //check if the user has a valid password
                var isCorrect = await _UserManager.CheckPasswordAsync(userExist, loginRequest.Password);

                if (isCorrect)
                {
                    //we need to gennerate a jwt Token
                    var jwtToken = await GenergateJwtToken(userExist);
                    return Ok(new UserLoginResponeDto()
                    {
                        Success = true,
                        Token = jwtToken.JwtToken,
                        RefreshToken = jwtToken.RefreshToken
                    });
                }
                else
                {
                    // password doesn't match
                    return BadRequest(new UserLoginResponeDto()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                    });
                }
            }
            else
            {
                return BadRequest(new UserLoginResponeDto
                {
                    Success = false,
                    Error = new List<string>(){
                            "Invalid payload"
                        }
                });
            }
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenRequestDto)
        {
            if (ModelState.IsValid)
            {
                //check if the toke is invalid
                var result = await VerifyToken(tokenRequestDto);

                if (result == null)
                {
                    return BadRequest(new UserLoginResponeDto()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Token Validation failed"
                        }
                    });
                }
                return Ok(result);
            }
            else
            {
                // password doesn't match
                return BadRequest(new UserLoginResponeDto()
                {
                    Success = false,
                    Error = new List<string>()
                        {
                            "Invalid authentication request"
                        }
                });
            }
        }

        private async Task<AuthResult> VerifyToken(TokenRequestDto tokenRequestDto)
        {
            var tokenHandle = new JwtSecurityTokenHandler();

            try
            {
                // we need to check the validity of the token
                var principal = tokenHandle.ValidateToken(tokenRequestDto.Token, _tokenValidationPrameters, out var validatedToken);

                //we need to validate the result that has been generated for us
                //validate if the string is an actual JWT token not a ramdom string 
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    // check if the jwt token is created with the same algorithms as our jwt token
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);

                    if (!result)
                        return null;

                }

                //we need to check the expiry date of the token
                var uctExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                //convert to date to check
                var expDate = UnixTimeStampToDateTime(uctExpiryDate);

                if (expDate > DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Jwt token has not expired "
                        }
                    };
                }

                //check if the refresh token exist
                var refreshTokenExist = await _unitOfWork.RefreshToken.GetByRefreshToken(tokenRequestDto.RefreshToken);

                if (refreshTokenExist == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Invalid refresh token"
                        }
                    };
                }

                //check the expiry date of a refresh token
                if (refreshTokenExist.ExpiryDate < DateTime.UtcNow)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Refresh token has expired, please login again"
                        }
                    };
                }

                //check if refresh token has been used or not 
                if (refreshTokenExist.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Refresh token has been used, it cannot be reused"
                        }
                    };
                }

                //check refresh token if it has been revoked
                if (refreshTokenExist.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Refresh token has been revoked, it cannot be used"
                        }
                    };
                }

                var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                if (refreshTokenExist.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Error = new List<string>()
                        {
                            "Refresh token reference does not match the jwt token"
                        }
                    };
                }

                //start processing and get a new token
                refreshTokenExist.IsUsed = true;

                var updateResult = await _unitOfWork.RefreshToken.MarkRefreshTokenAsUsed(refreshTokenExist);
                if (updateResult)
                {
                    await _unitOfWork.CompleteAsnyc();
                    //Get the user to generate a new jwt token
                    var dbUser = await _UserManager.FindByIdAsync(refreshTokenExist.UserId);

                    if (dbUser == null)
                    {
                        return new AuthResult()
                        {
                            Success = false,
                            Error = new List<string>()
                        {
                            "Error Processing request"
                        }
                        };
                    }

                    //generate a jwt token
                    var tokens = await GenergateJwtToken(dbUser);

                    return new AuthResult
                    {
                        Token = tokens.JwtToken,
                        Success = true,
                        RefreshToken = tokens.RefreshToken
                    };

                }
                return new AuthResult()
                {
                    Success = false,
                    Error = new List<string>()
                        {
                            "Error Processing request"
                        }
                };

            }
            catch (Exception ex)
            {
                //
                return null;
            }
        }

        private DateTime UnixTimeStampToDateTime(long uctDate)
        {
            //set date time to 1, Jan, 1970
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(uctDate).ToUniversalTime();
            return dateTime;
        }
        private async Task<TokenData> GenergateJwtToken(IdentityUser user)
        {
            // the handle is going to be reposible for create the token
            var jwtHandler = new JwtSecurityTokenHandler();

            //get the sucurity Token 
            var key = Encoding.ASCII.GetBytes(_JwtConfig.Secret);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email), // unique id
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())// used by the refresh token 
                }),
                Expires = DateTime.UtcNow.Add(_JwtConfig.ExpiryTimeFrame), //Todo update the expiration time to minutes
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };
            //generate the security obj token
            var token = jwtHandler.CreateToken(TokenDescriptor);
            //convert the security obj token into a string
            var jwtToken = jwtHandler.WriteToken(token);

            //Generate a refresh token
            var refreshToken = new RefreshToken
            {
                Token = $"{RandomStringGenerator(25)}_{Guid.NewGuid()}",
                UserId = user.Id,
                IsRevoked = false,
                IsUsed = false,
                JwtId = token.Id,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await _unitOfWork.RefreshToken.Add(refreshToken);
            await _unitOfWork.CompleteAsnyc();

            var tokenData = new TokenData
            {
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token
            };

            return tokenData;
        }

        private string RandomStringGenerator(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

}