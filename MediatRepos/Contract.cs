using DTOs;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;

namespace MediatorServices
{
    public class GetIdContractService : GetIdModelService<Contract>
    {
        public GetIdContractService(IRepository repository, ILogger<GetIdModelService<Contract>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(t => t.Id == id, null, "Carrier,Driver,Vehicle,Documents");
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
                        Route = constract.LoadingPoint.Route,
                        Address = constract.LoadingPoint.Address,
                        Side = (LoadingSide)constract.LoadingPoint.Side,
                        Phones = constract.LoadingPoint.Phones.Split(';').ToList()
                    },
                    Volume = constract.Volume,
                    Weight = constract.Weight,
                    Payment = constract.Payment,
                    Prepayment = constract.Prepayment,
                    PayPriority = (PaymentPriority)constract.PayPriority,
                    PaymentCondition = (RecievingType)constract.PaymentCondition,
                    Carrier = new CarrierDto() 
                    { 
                        Id = constract.Carrier.Id,
                        Name = constract.Carrier.Name,
                        Vat = (VAT)constract.Carrier.Vat
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
                        Route = point.Route,
                        Side = (LoadingSide)point.Side,
                        Type = (LoadPointType)point.Type,
                        Phones = point.Phones.Split(';').ToList(),
                    });
                }

                foreach (Document document in constract.Documents)
                {
                    dto.Documents.Add(new DocumentDto()
                    {
                        Id = document.Id,
                        Type = (DocumentType)document.DocumentType,
                        Summ = document.Summ,
                        CreationDate = document.CreationDate,
                        RecievingDate = document.RecievingDate,
                        Number = document.Number,
                        Direction = (DocumentDirection)document.DocumentDirection,
                        RecieveType = (RecievingType)document.RecieveType
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
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(c => c.Number.ToString().Contains(name), q => q.OrderBy(c => c.CreationDate).ThenBy(c => c.Number), "Carrier,Driver,Truck,Trailer");
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
                    Payment = contract.Payment,
                    Prepayment = contract.Prepayment,
                    Carrier = new CarrierDto()
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Carrier.Name,
                        Vat = (VAT)contract.Carrier.Vat,
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

    public class GetRangeContractService : GetRangeModelService<Contract>
    {
        public GetRangeContractService(IRepository repository, ILogger<GetRangeModelService<Contract>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(int start, int end)
        {
            DateTime startDate = DateTime.FromOADate(start);
            DateTime endDate = DateTime.FromOADate(end);

            IEnumerable<Contract> contracts = await _repository.Get<Contract>(c => c.CreationDate >= startDate && c.CreationDate <= endDate , q => q.OrderBy(c => c.CreationDate).ThenBy(c => c.Number), "Carrier,Driver,Truck,Trailer");
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
                    Payment = contract.Payment,
                    Prepayment = contract.Prepayment,
                    Carrier = new CarrierDto()
                    {
                        Id = contract.Carrier.Id,
                        Name = contract.Carrier.Name,
                        Vat = (VAT)contract.Carrier.Vat,
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
                    UnloadPoints = new List<RoutePointDto>()
                };

                foreach (RoutePoint point in contract.UnloadingPoints)
                {
                    dto.UnloadPoints.Add(new RoutePointDto()
                    {
                        Id = point.Id,
                        Route = point.Route,
                    });
                }

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
                Payment = dto.Payment,
                Prepayment = dto.Prepayment,
                PaymentCondition = (short)dto.PaymentCondition,
                PayPriority = (short)dto.PayPriority,
                Status = (short)ContractStatus.Created,
                Weight = dto.Weight,
                Volume = dto.Volume,
                UnloadingPoints = new List<RoutePoint>()
            };

            Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
            contract.Carrier = carrier;

            Driver driver = await _repository.GetById<Driver>(dto.Driver.Id);
            contract.Driver = driver;

            Vehicle truck = await _repository.GetById<Vehicle>(dto.Vehicle.Id);
            contract.Vehicle = truck;

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
            Contract contract = await _repository.GetById<Contract>(dto.Id);

            contract.Payment = dto.Payment;
            contract.Prepayment = dto.Prepayment;
            contract.PaymentCondition = (short)dto.PaymentCondition;
            contract.PayPriority = (short)dto.PayPriority;
            contract.Status = (short)dto.Status;
            contract.Weight = dto.Weight;
            contract.Volume = dto.Volume;
            contract.UnloadingPoints.Clear();

            Carrier carrier = await _repository.GetById<Carrier>(dto.Carrier.Id);
            contract.Carrier = carrier;

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
                    Id = Guid.NewGuid(),
                    Address = dto.LoadPoint.Address,
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
                    Id = Guid.NewGuid(),
                    Address = pointDto.Address,
                    Route = pointDto.Route,
                    Side = (short)pointDto.Side,
                    Type = (short)pointDto.Type,
                    Phones = string.Join(";", pointDto.Phones),
                });
            }

            return await _repository.Update(contract);
        }
    }
}
