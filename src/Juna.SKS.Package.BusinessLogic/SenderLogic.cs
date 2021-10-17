﻿using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        public SenderLogic()
        {

        }
        public string SubmitParcel(Parcel parcel)
        {
            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == true)
            {
                return "PYJRB4HZ6";
            }

            return null;
        }
    }
}