using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities.Validators
{
    public class HopValidator : AbstractValidator<Hop>
    {
        public HopValidator()
        {
            RuleFor(x => x.LocationCoordinates).NotNull();
        }
    }
}
