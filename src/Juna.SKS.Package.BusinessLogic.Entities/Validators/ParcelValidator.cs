﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities.Validators
{
    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            RuleFor(x => x.TrackingId).Matches("^[A-Z0-9]{9}$");
            RuleFor(x => x.Weight).GreaterThan(0);
            RuleFor(x => x.Recipient).NotNull();
            RuleFor(x => x.Sender).NotNull();
            RuleFor(x => x.VisitedHops).NotNull();
            RuleFor(x => x.FutureHops).NotNull();
        }
    }
}