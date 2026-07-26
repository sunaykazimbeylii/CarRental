using CarRental.Application.DTOs.Token;
using CarRental.Application.Interfaces.Services;
using CarRental.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRental.Infrastructure.Implementations.Service
{
    internal class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public TokenResponseDto CreateAccessToken(AppUser user, int minutes)
        {


            JwtSecurityToken securityToken = new JwtSecurityToken(

                issuer: _configuration["JWT:issuer"], //kim yaradib
                audience: _configuration["JWT:audience"], //kim ucun nezerde tutulub
                expires: DateTime.UtcNow.AddMinutes(minutes), //ne qeder aktiv olacaq
                notBefore: DateTime.UtcNow, //hansi muddetden sonra aktiv olacaq
                claims: new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Surname,user.Surname),
                new Claim(ClaimTypes.GivenName,user.Name),

            }, //istifadeci melumatlari
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:secretKey"])), SecurityAlgorithms.HmacSha256)

                );


            return new TokenResponseDto(new JwtSecurityTokenHandler().WriteToken(securityToken), user.UserName, securityToken.ValidTo);
        }
    }
}
