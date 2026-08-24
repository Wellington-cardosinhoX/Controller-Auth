using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController(IConfiguration configuration) : ControllerBase
{
    public record LoginRequest(string Usuario, string Senha);

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest loginRequest)
    {
        // 1. Validar usuário e senha (por enquanto, fixo pra testar)
        if (loginRequest.Usuario != "admin" || loginRequest.Senha != "1234")
            return Unauthorized("Usuário ou senha inválidos");

        // 2. Pegar as configurações do JWT
        var jwtKey = configuration["Jwt:Key"];
        var jwtIssuer = configuration["Jwt:Issuer"];
        var jwtAudience = configuration["Jwt:Audience"];

        // 3. Montar as claims (informações que ficam "dentro" do token)
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, loginRequest.Usuario),
            new Claim(ClaimTypes.Role, "Admin")
        };

        // 4. Criar a credencial de assinatura
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 5. Criar o token
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1000),
            signingCredentials: creds
        );

        // 6. Devolver o token pro cliente
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}