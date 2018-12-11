using DriveCentric.Core.Models;
using FluentValidation;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class TaskValidator : AbstractValidator<UserTask>
    {
        public TaskValidator()
        {
            RuleSet("Insert", () =>
            {
                RuleFor(x => x.ActionType).NotNull();
                RuleFor(x => x.DateDue).NotNull();
                RuleFor(x => x.UserId).NotNull();
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull();
                RuleFor(x => x.ActionType).NotNull();
                RuleFor(x => x.DateDue).NotNull();
                RuleFor(x => x.UserId).NotNull();
            });

            RuleSet("Delete", () =>
            {
                RuleFor(x => x.Id).NotEqual(0);
            });
        }
    }
}