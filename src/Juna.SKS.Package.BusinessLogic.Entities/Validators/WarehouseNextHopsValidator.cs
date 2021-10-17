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
    public class WarehouseNextHopsValidator : AbstractValidator<WarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(x => x.Hop).NotNull();
        }
    }
}
