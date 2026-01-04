namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string Email { get; set; }
    bool IsAuthenticated { get; set; }
}