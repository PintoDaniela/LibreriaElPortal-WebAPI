using LibreriaElPortal_WebAPI.DTOs;
using LibreriaElPortal_WebAPI.Helper;
using LibreriaElPortal_WebAPI.Interfaces;
using LibreriaElPortal_WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibreriaElPortal_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly elportalContext _appDbContext;
        private readonly JwtTokenManager _jwtTokenManager;
        private readonly PasswordHasher _passwordHasher;
        private readonly IAuthRepository _authRepository;

        public AuthController(IConfiguration config, elportalContext appDbContext, JwtTokenManager jwtTokenManager, PasswordHasher passwordHasher, IAuthRepository authRepository)
        {
            _config = config;
            _appDbContext = appDbContext;
            _jwtTokenManager = jwtTokenManager;
            _passwordHasher = passwordHasher;
            _authRepository = authRepository;
        }


        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            var usuarioCreado = await _authRepository.CreateUserAsync(newUser);

            if (usuarioCreado != null)
            {
                return CreatedAtAction("Register", usuarioCreado.Id, usuarioCreado);
            }
            return BadRequest();
        }


        [HttpPost("login")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] UserDto loginUser)
        {
            var (credencialesOk, user) = await _authRepository.CheckUserCredentialsAsync(loginUser);
            if (!credencialesOk || user == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            var token = _jwtTokenManager.GenerateJwtToken(user, _config);
            return Ok(new { Token = token });
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [HttpPost("validate-token")]
        public IActionResult ValidateToken([FromHeader] string token)
        {
            bool tokenValido = _jwtTokenManager.ValidateToken(token, _config);

            if (!tokenValido)
            {
                return Unauthorized("Token inválido. Inicie sesión para generar un nuevo token de acceso.");
            }

            return Ok("Token válido.");
        }
    }
}
