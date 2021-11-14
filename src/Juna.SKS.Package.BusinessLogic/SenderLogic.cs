using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;

namespace Juna.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        private readonly ILogger<SenderLogic> _logger;
        public SenderLogic(IParcelRepository repo, IMapper mapper, ILogger<SenderLogic> logger)
        {
            _repository = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public string SubmitParcel(Parcel parcel)
        {
            _logger.LogInformation("Trying to submit a parcel");

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == false)
            {
                _logger.LogDebug($"Parcel is invalid - {result}");
                return null;
            }

            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
            _repository.Create(DAparcel);
            _logger.LogInformation("Submitted the parcel");
            return "PYJRB4HZ6";
        }
    }
}
