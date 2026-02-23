using backend.Models;

namespace backend.Servies
{
    public class AuthService
    {
        private readonly CnpmContext _context;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _config;

        public AuthService(CnpmContext context, JwtService jwtService, IConfiguration config)
        {
            _config = config;
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(string accessToken, string refreshToken)> IssueTokensAsync(User user)
        {
            var accessToken = _jwtService.GenerateAccessToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            await _jwtService.SaveRefreshTokenAsync(_context, user, refreshToken);

            return (accessToken, refreshToken);
        }
    }
}

