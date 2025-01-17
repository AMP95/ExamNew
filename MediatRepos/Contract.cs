﻿using DTOs;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdContractService : GetIdModelService<Contract>
    {
        public GetIdContractService(IRepository repository, ILogger<GetIdModelService<Contract>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(t => t.Id == id, null, "Carrier,Client,Driver,Vehicle,LoadingPoint,UnloadingPoints,Documents");
            ContractDto dto = null;
            if (contracts.Any())
            {
                Contract constract = contracts.First();
                dto = new ContractDto()
                {
                    Id = constract.Id,
                    Number = constract.Number,
                    CreationDate = constract.CreationDate,
                    Status  = (ContractStatus)constract.Status,
                    LoadPoint = new RoutePointDto() 
                    { 
                        Id = constract.LoadingPoint.Id,
                        Company = constract.LoadingPoint.Company,
                        Route = constract.LoadingPoint.Route,
                        DateAndTime = constract.LoadingPoint.DateAndTime,
                        Address = constract.LoadingPoint.Address,
                        Side = (LoadingSide)constract.LoadingPoint.Side,
                        Phones = constract.LoadingPoint.Phones.Split(';').ToList()
                    },
                    Volume = constract.Volume,
                    Weight = constract.Weight,
                    Payment = constract.CarrierPayment,
                    Prepayment = constract.CarrierPrepayment,
                    ClientPayment = constract.ClientPayment,
                    PayPriority = (PaymentPriority)constract.CarrierPayPriority,
                    PaymentCondition = (RecievingType)constract.CarrierPaymentCondition,
                    Carrier = new CarrierDto() 
                    { 
                        Id = constract.Carrier.Id,
                        Name = constract.Carrier.Name,
                        Vat = (VAT)constract.Carrier.Vat
                    },
                    Client = new ClientDto() 
                    { 
                        Id = constract.Client.Id,
                        Name = constract.Client.Name,
                    },
                    Driver = new DriverDto() 
                    { 
                        Id=constract.Driver.Id,
                        Name = $"{constract.Driver.FamilyName} {constract.Driver.Name} {constract.Driver.FatherName}",
                        Phones = constract.Driver.Phones.Split(';').ToList(),
                        PassportSerial = constract.Driver.PassportSerial,
                        PassportIssuer = constract.Driver.PassportIssuer,
                        PassportDateOfIssue = constract.Driver.PassportDateOfIssue,
                        BirthDate = constract.Driver.DateOfBirth
                    },
                    Vehicle = new VehicleDto() 
                    { 
                        Id = constract.Vehicle.Id,
                        TruckModel = constract.Vehicle.TruckModel,
                        TruckNumber = constract.Vehicle.TruckNumber,
                        TrailerNumber = constract.Vehicle.TrailerNumber,
                        TrailerModel = constract.Vehicle.TrailerModel,
                    },
                    UnloadPoints = new List<RoutePointDto>(),
                    Documents = new List<DocumentDto>()
                };

                foreach (RoutePoint point in constract.UnloadingPoints) 
                {
                    dto.UnloadPoints.Add(new RoutePointDto()
                    {
                        Id = point.Id,
                        Address = point.Address,
                        Company = point.Company,
                        DateAndTime = point.DateAndTime,
                        Route = point.Route,
                        Side = (LoadingSide)point.Side,
                        Type = (LoadPointType)point.Type,
                        Phones = point.Phones.Split(';').ToList(),
                    });
                }
            }
            return dto;
        }
    }

    public class SearchContractService : SearchModelService<Contract>
    {
        public SearchContractService(IRepository repository, ILogger<SearchModelService<Contract>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(string name)
        {
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(c => c.Number.ToString().Contains(name), q => q.OrderBy(c => c.CreationDate).ThenBy(c => c.Number), "Carrier,Client,Driver,Vehicle,LoadingPoint,UnloadingPoints");
            List<ContractDto> dtos = new List<ContractDto>();

            foreach (var contract in contracts)
            {
                ContractDto dto = new ContractDto()
                {
                    Id = contract.Id,
                    Number = contract.Number,
                    CreationDate = contract.CreationDate,
                    LoadPoint = new RoutePointDto()
                    {
                        Id = contract.LoadingPoint.Id,
                        Route = contract.LoadingPoint.Route,
                    },
                    Status = (ContractStatus)contract.Status,
                    Payment = contract.CarrierPayment,
                    Prepayment = contract.CarrierPrepayment,
                    ClientPayment = contract.ClientPayment,
                    Carrier = new CarrierDto()
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Carrier.Name,
                        Vat = (VAT)contract.Carrier.Vat,
                    },
                    Client = new ClientDto() 
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Client.Name,
                    },
                    Driver = new DriverDto()
                    {
                        Id = contract.Driver.Id,
                        Name = $"{contract.Driver.FamilyName} {contract.Driver.Name} {contract.Driver.FatherName}",
                        Phones = contract.Driver.Phones.Split(';').ToList(),
                    },
                    Vehicle = new VehicleDto()
                    {
                        Id = contract.Vehicle.Id,
                        TruckModel = contract.Vehicle.TruckModel,
                        TruckNumber = contract.Vehicle.TruckNumber,
                        TrailerModel = contract.Vehicle.TrailerModel,
                        TrailerNumber = contract.Vehicle.TrailerNumber,
                    },
                    UnloadPoints = contract.UnloadingPoints.Select(s => new RoutePointDto() { Id = s.Id, Route = s.Route}).ToList(),
                    Documents = new List<DocumentDto>()
                };

                dtos.Add(dto);
            }

            return dtos;
        }
    }

    public class SetContractStatusService : IRequestHandler<SetContractStatus, bool>
    {
        protected IRepository _repository;
        protected ILogger<SetContractStatusService> _logger;

        public SetContractStatusService(IRepository repository, ILogger<SetContractStatusService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<bool> Handle(SetContractStatus request, CancellationToken cancellationToken)
        {
            Contract contract = await _repository.GetById<Contract>(request.ContractId);
            if (contract != null)
            {
                contract.Status = (short)request.ContractStatus;
                return await _repository.Update(contract);
            }
            return false;
        }
    }

    public class GetFilteredContractService : IRequestHandler<ContractFilter, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilteredContractService> _logger;
        public GetFilteredContractService(IRepository repository, ILogger<GetFilteredContractService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<object> Handle(ContractFilter request, CancellationToken cancellationToken)
        {
            Expression<Func<Contract, bool>> filter = null;
            switch (request.FilterName) 
            {
                case ContractFilterProperty.Date:
                    DateTime.TryParse(request.Params[0].ToString(), out DateTime start);
                    DateTime.TryParse(request.Params[1].ToString(), out DateTime end);
                    filter = c => c.CreationDate >= start && c.CreationDate <= end;
                    break;
                case ContractFilterProperty.Route:
                    string route = request.Params[0].ToString();
                    filter = c => c.LoadingPoint.Route.Contains(route) || c.UnloadingPoints.Any(p => p.Route.Contains(route));
                    break;
                case ContractFilterProperty.Carrier:
                    string carrier = request.Params[0].ToString();
                    filter = c => c.Carrier.Name.Contains(carrier);
                    break;
                case ContractFilterProperty.Driver:
                    string driver = request.Params[0].ToString();
                    filter = c => $"{c.Driver.FamilyName} {c.Driver.Name} {c.Driver.FatherName}".Contains(driver);
                    break;
                case ContractFilterProperty.Status:
                    ContractStatus status = (ContractStatus)Enum.Parse(typeof(ContractStatus),request.Params[0].ToString());
                    filter = c => c.Status == (int)status;
                    break;
                case ContractFilterProperty.Client:
                    string client = request.Params[0].ToString();
                    filter = c => c.Client.Name.Contains(client);
                    break;
                default: 
                    filter = c => true; 
                    break;
            }

            IEnumerable<Contract> contracts = await _repository.Get(filter, q => q.OrderBy(c => c.CreationDate).ThenBy(c => c.Number), "Carrier,Client,Driver,Vehicle,LoadingPoint,UnloadingPoints");
            List<ContractDto> dtos = new List<ContractDto>();

            foreach (var contract in contracts)
            {
                ContractDto dto = new ContractDto()
                {
                    Id = contract.Id,
                    Number = contract.Number,
                    CreationDate = contract.CreationDate,
                    LoadPoint = new RoutePointDto()
                    {
                        Id = contract.LoadingPoint.Id,
                        Route = contract.LoadingPoint.Route,
                    },
                    Status = (ContractStatus)contract.Status,
                    Payment = contract.CarrierPayment,
                    Prepayment = contract.CarrierPrepayment,
                    ClientPayment = contract.ClientPayment,
                    Carrier = new CarrierDto()
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Carrier.Name,
                        Vat = (VAT)contract.Carrier.Vat,
                    },
                    Client = new ClientDto() 
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Client.Name,
                    },
                    Driver = new DriverDto()
                    {
                        Id = contract.Driver.Id,
                        Name = $"{contract.Driver.FamilyName} {contract.Driver.Name} {contract.Driver.FatherName}",
                        Phones = contract.Driver.Phones.Split(';').ToList(),
                    },
                    Vehicle = new VehicleDto()
                    {
                        Id = contract.Vehicle.Id,
                        TruckModel = contract.Vehicle.TruckModel,
                        TruckNumber = contract.Vehicle.TruckNumber,
                        TrailerModel = contract.Vehicle.TrailerModel,
                        TrailerNumber = contract.Vehicle.TrailerNumber,
                    },
                    UnloadPoints = contract.UnloadingPoints.Select(s => new RoutePointDto() { Id = s.Id, Route = s.Route }).ToList()
                };

                dtos.Add(dto);
            }

            return dtos;

        }
    }

    public class DeleteContractService : DeleteModelService<Contract>
    {
        public DeleteContractService(IRepository repository) : base(repository)
        {
        }
    }

    public class AddContractService : AddModelService<ContractDto>
    {
        public AddContractService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(ContractDto dto)
        {
            Contract contract = new Contract()
            {
                Number = dto.Number,
                CreationDate = dto.CreationDate,
                CarrierPayment = dto.Payment,
                CarrierPrepayment = dto.Prepayment,
                ClientPayment = dto.ClientPayment,
                CarrierPaymentCondition = (short)dto.PaymentCondition,
                CarrierPayPriority = (short)dto.PayPriority,
                Status = (short)ContractStatus.Created,
                Weight = dto.Weight,
                Volume = dto.Volume,
                LoadingPoint = new RoutePoint()
                {
                    Address = dto.LoadPoint.Address,
                    Company = dto.LoadPoint.Company,
                    DateAndTime = dto.LoadPoint.DateAndTime,
                    Route = dto.LoadPoint.Route,
                    Phones = string.Join(";", dto.LoadPoint.Phones),
                    Type = (short)LoadPointType.Upload,
                    Side = (short)dto.LoadPoint.Side
                },
                UnloadingPoints = new List<RoutePoint>()
            };

            Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
            contract.Carrier = carrier;

            Client client = await _repository.GetById<Client>(dto.Client.Id);
            contract.Client = client;

            Driver driver = await _repository.GetById<Driver>(dto.Driver.Id);
            contract.Driver = driver;

            Vehicle truck = await _repository.GetById<Vehicle>(dto.Vehicle.Id);
            contract.Vehicle = truck;

            foreach (RoutePointDto pointDto in dto.UnloadPoints) 
            {
                contract.UnloadingPoints.Add(new RoutePoint() 
                { 
                    Address = pointDto.Address,
                    Company = pointDto.Company,
                    DateAndTime = pointDto.DateAndTime,
                    Route = pointDto.Route,
                    Side = (short)pointDto.Side,
                    Type = (short)pointDto.Type,
                    Phones = string.Join(";", pointDto.Phones),
                });
            }


            return await _repository.Update(contract);
        }
    }

    public class UpdateContractService : UpdateModelService<ContractDto>
    {
        public UpdateContractService(IRepository repository) : base(repository)
        {
        }

        protected override async Task<bool> Update(ContractDto dto)
        {
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(c=>c.Id == dto.Id, null, "UnloadingPoints");

            if (contracts.Any()) 
            {
                Contract contract = contracts.First();
                contract.CarrierPayment = dto.Payment;
                contract.CarrierPrepayment = dto.Prepayment;
                contract.ClientPayment = dto.ClientPayment;
                contract.CarrierPaymentCondition = (short)dto.PaymentCondition;
                contract.CarrierPayPriority = (short)dto.PayPriority;
                contract.Status = (short)dto.Status;
                contract.Weight = dto.Weight;
                contract.Volume = dto.Volume;
                contract.UnloadingPoints.Clear();

                Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
                contract.Carrier = carrier;

                Client client = await _repository.GetById<Client>(dto.Client.Id);
                contract.Client = client;

                Driver driver = await _repository.GetById<Driver>(dto.Driver.Id);
                contract.Driver = driver;

                Vehicle vehicle = await _repository.GetById<Vehicle>(dto.Vehicle.Id);
                contract.Vehicle = vehicle;

                IEnumerable<RoutePoint> loads = await _repository.Get<RoutePoint>(p => p.Address == dto.LoadPoint.Address && p.Side == (short)dto.LoadPoint.Side);

                if (loads.Any())
                {
                    contract.LoadingPoint = loads.First();
                }
                else
                {
                    contract.LoadingPoint = new RoutePoint()
                    {
                        Address = dto.LoadPoint.Address,
                        Company = dto.LoadPoint.Company,
                        DateAndTime = dto.LoadPoint.DateAndTime,
                        Route = dto.LoadPoint.Route,
                        Phones = string.Join(";", dto.LoadPoint.Phones),
                        Type = (short)LoadPointType.Upload,
                        Side = (short)dto.LoadPoint.Side
                    };
                }

                foreach (RoutePointDto pointDto in dto.UnloadPoints)
                {
                    contract.UnloadingPoints.Add(new RoutePoint()
                    {
                        Address = pointDto.Address,
                        Company = pointDto.Company,
                        DateAndTime = pointDto.DateAndTime,
                        Route = pointDto.Route,
                        Side = (short)pointDto.Side,
                        Type = (short)pointDto.Type,
                        Phones = string.Join(";", pointDto.Phones),
                    });
                }

                return await _repository.Update(contract);
            }
            return false;
        }
    }
}
