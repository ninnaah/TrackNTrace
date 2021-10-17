using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities.Validators
{
    public class HopArrivalValidator : AbstractValidator<HopArrival>
    {
		public HopArrivalValidator()
		{
			RuleFor(x => x.Code).Matches(@"^[A-Z]{4}\d{1,4}$");
		}

	}
}
