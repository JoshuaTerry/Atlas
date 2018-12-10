using DriveCentric.Core.Models;
using FluentValidation;

namespace DriveCentric.BusinessLogic.Implementation
{
    public class ModuleValidator : AbstractValidator<Module>
    {
        public ModuleValidator()
        {
            RuleSet("Insert", () =>
            {
            });

            RuleSet("Update", () =>
            {
            });

            RuleSet("Delete", () =>
            {
                RuleFor(x => x.Id).NotNull();
            });
        }
    }
}