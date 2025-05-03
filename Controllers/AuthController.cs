using Microsoft.AspNetCore.Mvc;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using bloodsociety.Data;
using bloodsociety.Models;
using bloodsociety.DTO;
using System;
using System.Threading.Tasks;

namespace bloodsociety.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BloodSocietyContext _context;
        private readonly IConfiguration _config;

        public AuthController(BloodSocietyContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            // 1. Create user in Auth0
            var domain = _config["Auth0:Domain"];
            var clientId = _config["Auth0:ClientId"];
            var clientSecret = _config["Auth0:ClientSecret"];
            var audience = _config["Auth0:Audience"];

            // Get Management API token
            var authApi = new AuthenticationApiClient($"https://{domain}");
            var tokenResp = await authApi.GetTokenAsync(new ClientCredentialsTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Audience = audience
            });

            var mgmtApi = new ManagementApiClient(tokenResp.AccessToken, domain);
            try
            {
                // Check if user already exists in Auth0
                var existing = await mgmtApi.Users.GetUsersByEmailAsync(req.Email);
                if (existing.Count > 0)
                    return Conflict("User already exists in Auth0");

                // Create Auth0 user
                var userReq = new UserCreateRequest
                {
                    Email = req.Email,
                    Password = req.Password,
                    Connection = "Username-Password-Authentication",
                    EmailVerified = false,
                    FullName = req.Name,
                    UserMetadata = new { phone = req.Phone, role = req.Role }
                };
                var auth0User = await mgmtApi.Users.CreateAsync(userReq);

                // 2. Create user in local DB
                var localUser = new bloodsociety.Models.User
                {
                    Name = req.Name,
                    Email = req.Email,
                    PasswordHash = "Auth0", // Password is managed by Auth0
                    Phone = req.Phone,
                    Role = req.Role,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Users.Add(localUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/auth/login
        [HttpPost("login")]
public async Task<IActionResult> Login([FromBody] LoginRequest req)
{
    var domain = _config["Auth0:Domain"];
    var clientId = _config["Auth0:ClientId"];
    var audience = _config["Auth0:Audience"];

    var authApi = new AuthenticationApiClient($"https://{domain}");
    try
    {
        // 1. Authenticate with Auth0
        var tokenResp = await authApi.GetTokenAsync(new ResourceOwnerTokenRequest
        {
            ClientId = clientId,
            Audience = audience,
            Scope = "openid profile email offline_access", // Request refresh token
            Realm = "Username-Password-Authentication",
            Username = req.Email,
            Password = req.Password
        });

        // 2. Check if user exists in local DB with matching email and password
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == req.Email && u.PasswordHash == req.Password);
        if (user == null)
            return Unauthorized(new { error = "User not found or password does not match in local database" });

        return Ok(new
        {
            access_token = tokenResp.AccessToken,
            id_token = tokenResp.IdToken,
            refresh_token = tokenResp.RefreshToken, // Include refresh token if present
            expires_in = tokenResp.ExpiresIn,
            token_type = tokenResp.TokenType,
            user = new { user.UserId, user.Name, user.Email, user.Role }
        });
    }
    catch (Exception ex)
    {
        return Unauthorized(new { error = ex.Message });
    }
}

        // POST: api/auth/verify-otp
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp()
        {
            // OTP verification can be handled via Auth0 rules or actions (not implemented here)
            return Ok("Verify OTP endpoint (not implemented)");
        }
    }
}
