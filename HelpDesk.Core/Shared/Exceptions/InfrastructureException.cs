
namespace HelpDesk.Core.Shared.Exceptions;

public class InfrastructureException : Exception
{
    public InfrastructureException()
    {
    }

    public InfrastructureException(string? message) : base(message)
    {
    }
}
