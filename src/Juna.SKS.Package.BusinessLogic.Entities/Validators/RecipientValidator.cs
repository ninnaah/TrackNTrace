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
	public class RecipientValidator : AbstractValidator<Recipient>
    {
		public RecipientValidator()
		{
			When(x => x.Country == "Austria" || x.Country == "Österreich", () =>{
				RuleFor(x => x.PostalCode).Matches(@"^[A][-]\d{4}$");
				RuleFor(x => x.Street).Matches(@"^[A-Za-zß]+\s{1}[0-9A-Za-z/]+$");
				RuleFor(x => x.Name).Matches(@"^[A-Z]{1}[a-z-\s]+$");
				RuleFor(x => x.City).Matches(@"^[A-Z]{1}[a-z-\s]+$");
			});
		}
	}
}
