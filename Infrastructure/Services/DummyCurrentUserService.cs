using Application.Common.Interfaces;

namespace PetFlow.Infrastructure.Services;

public class DummyCurrentUserService : ICurrentUserService
{
    public string Email { get; set; } = "dummy-email@petflow.com";
    public bool IsAuthenticated { get; set; } = true;
}