using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        public SenderLogic(IParcelRepository repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }
        public string SubmitParcel(Parcel parcel)
        {
            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == true)
            {
                DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _repository.Create(DAparcel);
                return "PYJRB4HZ6";
            }

            return null;
        }
    }
}
