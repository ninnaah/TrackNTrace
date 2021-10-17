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
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(x => x.Description).Matches(@"^[A-Za-z-\s0-9]+$");
            RuleFor(x => x.NextHops).NotNull();
        }
    }
}
