
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class FirebaseJwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public FirebaseJwtMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task Invoke(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            await _next(context);
            return;
        }

        var firebaseIdToken = authHeader.Substring("Bearer ".Length).Trim();

        // Step 1: Validate Firebase ID Token (server-side decoding only here for demo purposes)
        var userId = GetUserIdFromFirebaseIdToken(firebaseIdToken);
        if (userId == null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid Firebase ID Token");
            return;
        }

        // Step 2: Issue custom Access/Refresh tokens
        var accessToken = GenerateToken(userId, _config["Jwt:AccessTokenSecret"], 15); // 15 minutes
        var refreshToken = GenerateToken(userId, _config["Jwt:RefreshTokenSecret"], 43200); // 30 days

        context.Items["UserId"] = userId;
        context.Items["AccessToken"] = accessToken;
        context.Items["RefreshToken"] = refreshToken;

        await _next(context);
    }

    private string GetUserIdFromFirebaseIdToken(string token)
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken?.Claims.FirstOrDefault(c => c.Type == "user_id" || c.Type == "sub")?.Value;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateToken(string userId, string secret, int expireMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secret);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("user_id", userId) }),
            Expires = DateTime.UtcNow.AddMinutes(expireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(token);
    }
}
