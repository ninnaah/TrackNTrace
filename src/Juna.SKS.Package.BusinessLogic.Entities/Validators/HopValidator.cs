using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities.Validators
{
    [ExcludeFromCodeCoverage]
    public class HopValidator : AbstractValidator<Hop>
    {
        public HopValidator()
        {
            RuleFor(x => x.LocationCoordinates).NotNull();
        }
    }
}
