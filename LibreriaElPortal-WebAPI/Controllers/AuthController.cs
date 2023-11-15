using Shared.DTOs;
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
        private readonly JwtTokenManager _jwtTokenManager;
        private readonly IAuthRepository _authRepository;

        public AuthController(IConfiguration config, JwtTokenManager jwtTokenManager,IAuthRepository authRepository)
        {
            _config = config;
            _jwtTokenManager = jwtTokenManager;
            _authRepository = authRepository;
        }

        /// <summary>
        /// Elija un nombre de usuario y una contraseña para registrarse. La contraseña debe ser de al menos 8 caracteres y tener al menos 1 dígito numérico, 1 mayúscula, 1 minúscula y 1 caracter especial.
        /// </summary>
        /// <param name="newUser"></param>
        /// <response code="201">Created. Registro efectuado con éxito.</response>
        /// <response code="400">BadRequest. El formato de la solicitud no es válido o ya existe un usuario con el nombre ingresado.</response>
        /// <response code="500">InternalServerError. Hubo un error al procesar la solicitud.</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDto newUser)
        {
            var existe = await _authRepository.ExisteUsuarioByNameAsync(newUser.Username);
            if (existe)
            {
                return BadRequest($"Ya existe un usuario con el nombre {newUser.Username}");
            }
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

       
        [HttpPost("validate-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
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
