using Model.DTO;
using System.Threading.Tasks;

    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResult> LoginAsync(LoginDTO loginDTO);
    }
