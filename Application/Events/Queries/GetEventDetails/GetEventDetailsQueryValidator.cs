using FluentValidation;

namespace Application.Events.Queries.GetEventDetails;

public class GetEventDetailsQueryValidator : AbstractValidator<GetEventDetailsQuery>
{
    public GetEventDetailsQueryValidator()
    {
        RuleFor(x => x.EventId)
            .GreaterThan(0)
            .WithMessage("Event Id must be greater than zero.");
    }
}
