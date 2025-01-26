using DTOs;
using DTOs.Dtos;
using MediatorServices.Abstract;
using MediatR;
using MediatRepos;
using Microsoft.Extensions.Logging;
using Models;
using Models.Sub;
using System.Linq.Expressions;

namespace MediatorServices
{
    public class GetIdContractService : GetIdModelService<ContractDto>
    {
        public GetIdContractService(IRepository repository, ILogger<GetIdModelService<ContractDto>> logger) : base(repository, logger)
        {
        }

        protected override async Task<object> Get(Guid id)
        {
            IEnumerable<Contract> contracts = await _repository.Get<Contract>(t => t.Id == id, null, "Logist,Carrier,Client,Driver,Vehicle,LoadingPoint,UnloadingPoints,Documents,Template");
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
                    Logist = new LogistDto() 
                    { 
                        Id=constract.Logist.Id,
                        Login = constract.Logist.Login,
                        Name = constract.Logist.Name,
                    },
                    Carrier = new CarrierDto() 
                    { 
                        Id = constract.Carrier.Id,
                        Name = constract.Carrier.Name,
                        Vat = (VAT)constract.Carrier.Vat,
                        Address = constract.Carrier.Address,
                        Phones = constract.Carrier.Phones.Split(";").ToList(),
                        Emails = constract.Carrier.Emails.Split(";").ToList(),
                        InnKpp = constract.Carrier.InnKpp
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
                    Documents = new List<DocumentDto>(),
                    Template = new DTOs.Dtos.ContractTemplateDto() 
                    { 
                        Id = constract.Template.Id,
                        Name = constract.Template.Name,
                    }
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

    public class GetFilteredContractService : IRequestHandler<GetFilter<ContractDto>, object>
    {
        protected IRepository _repository;
        protected ILogger<GetFilteredContractService> _logger;

        public GetFilteredContractService(IRepository repository, ILogger<GetFilteredContractService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<object> Handle(GetFilter<ContractDto> request, CancellationToken cancellationToken)
        {
            Expression<Func<Contract, bool>> filter = GetFilter(request.PropertyName, request.Params);
            return await Get(filter);
        }

        protected async Task<object> Get(Expression<Func<Contract, bool>> filter)
        {
            IEnumerable<Contract> contracts = await _repository.Get(filter,
                                                                    q => q.OrderBy(c => c.CreationDate).ThenBy(c => c.Number),
                                                                    "Logist,Carrier,Client,Driver,Vehicle,LoadingPoint,UnloadingPoints");
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
                    Logist = new LogistDto() 
                    {
                        Id = contract.Logist.Id,
                        Login = contract.Logist.Login,
                        Name = contract.Logist.Name,
                    },
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
        protected Expression<Func<Contract, bool>> GetFilter(string property, params object[] parameters)
        {
            Expression<Func<Contract, bool>> filter = null;

            try
            {
                switch (property)
                {
                    case nameof(ContractDto.CreationDate):
                        DateTime.TryParse(parameters[0].ToString(), out DateTime start);
                        DateTime.TryParse(parameters[1].ToString(), out DateTime end);
                        filter = c => c.CreationDate >= start && c.CreationDate <= end;
                        break;
                    case nameof(ContractDto.LoadPoint):
                        string route = parameters[0].ToString();
                        filter = c => c.LoadingPoint.Route.Contains(route);
                        break;
                    case nameof(ContractDto.UnloadPoints):
                        string dest = parameters[0].ToString();
                        filter = c => c.UnloadingPoints.Any(p => p.Route.Contains(dest));
                        break;
                    case nameof(ContractDto.Client):
                        string client = parameters[0].ToString().ToLower();
                        filter = c => c.Client.Name.Contains(client);
                        break;
                    case nameof(ContractDto.Carrier):
                        string carname = parameters[0].ToString().ToLower();
                        filter = d => d.Carrier.Name.ToLower().Contains(carname);
                        break;
                    case nameof(ContractDto.Driver):
                        string driver = parameters[0].ToString().ToLower();
                        filter = c => (c.Driver.FamilyName + " " + c.Driver.Name + " " + c.Driver.FatherName).ToLower().Contains(driver);
                        break;
                    case nameof(ContractDto.Vehicle):
                        string formattedName = parameters[0].ToString().Replace(" ", "").Replace("/", "").ToLower();
                        filter = c => (c.Vehicle.TruckNumber + c.Vehicle.TrailerNumber).Replace("/", "").Replace(" ", "").ToLower().Contains(formattedName);
                        break;
                    case nameof(ContractDto.Status):
                        ContractStatus status = (ContractStatus)Enum.Parse(typeof(ContractStatus), parameters[0].ToString());
                        filter = c => c.Status == (int)status;
                        break;
                    case nameof(ContractDto.Number):
                        short number = (short)parameters[0];
                        filter = c => c.Number == number;
                        break;
                }
            }
            catch (Exception ex)
            {
                filter = c => false;
                _logger.LogError(ex, ex.Message);
            }
            return filter;
        }
    }


    public class GetRequiredToPayService : IRequestHandler<GetRequiredToPay, object>
    {
        private IRepository _repository;
        public GetRequiredToPayService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<object> Handle(GetRequiredToPay request, CancellationToken cancellationToken)
        {
            DateTime start = DateTime.Now.Subtract(TimeSpan.FromDays(30));

            List<RequiredToPayContractDto> required = new List<RequiredToPayContractDto>();

            List<Contract> contracts = (await _repository.Get<Contract>(c => c.CreationDate >= start &&
                                                                             c.CreationDate <= DateTime.Now &&
                                                                             c.Status == (short)ContractStatus.Closed, null, "Carrier,Driver,Vehicle,LoadingPoint,Documents,Payments")).ToList();

            foreach (Contract contract in contracts) 
            {
                if (contract.CarrierPrepayment != 0) 
                {
                    if (!contract.Payments.Any(p => p.Summ == contract.CarrierPrepayment)) 
                    {
                        var bils = contract.Documents.Where(d => d.DocumentType == (short)DocumentType.Bill);

                        if (bils.Any(b => b.Summ >= contract.CarrierPrepayment)) 
                        {
                            RequiredToPayContractDto dto = new RequiredToPayContractDto()
                            {
                                Id = contract.Id,
                                Carrier = contract.Carrier.Name,
                                Driver = $"{contract.Driver.FamilyName} {contract.Driver.Name} {contract.Driver.FatherName}",
                                Vehicle = $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber},{contract.Vehicle.TrailerNumber}",
                                CreationDate = contract.CreationDate,
                                Number = contract.Number,
                                Type = PayType.Prepayment,
                                Balance = contract.CarrierPrepayment
                            };

                            DateTime today = DateTime.Now;

                            DateTime loading = contract.LoadingPoint.DateAndTime;

                            int days = (int)(loading - today).TotalDays;

                            if (loading.Hour > 12) 
                            { 
                                days++;
                            }

                            dto.DaysToExpiration = days;

                            required.Add(dto);
                        }
                    }
                }

                double paymentSumm = contract.Payments.Where(p => p.DocumentDirection == (short)DocumentDirection.Outcome).Sum(d => d.Summ);

                if (contract.CarrierPayment > paymentSumm)
                {
                    IEnumerable<Document> income = contract.Documents.Where(d => d.DocumentDirection == (short)DocumentDirection.Income);

                    if (income.Where(i => i.DocumentType == (short)DocumentType.Bill && i.RecieveType == contract.CarrierPaymentCondition).Count() >= 1 &&
                       (income.Where(i => i.DocumentType == (short)DocumentType.UDT && i.RecieveType == contract.CarrierPaymentCondition).Count() >= 1 ||
                        income.Where(i => i.DocumentType == (short)DocumentType.Act && i.RecieveType == contract.CarrierPaymentCondition).Count() >= 1 &&
                        income.Where(i => i.DocumentType == (short)DocumentType.Invoice && i.RecieveType == contract.CarrierPaymentCondition).Count() >= 1) &&
                        income.Where(i => i.DocumentType == (short)DocumentType.TTN && i.RecieveType == contract.CarrierPaymentCondition).Count() >= 1)
                    {
                        RequiredToPayContractDto dto = new RequiredToPayContractDto()
                        {
                            Id = contract.Id,
                            Carrier = contract.Carrier.Name,
                            Driver = $"{contract.Driver.FamilyName} {contract.Driver.Name} {contract.Driver.FatherName}",
                            Vehicle = $"{contract.Vehicle.TruckModel} {contract.Vehicle.TruckNumber},{contract.Vehicle.TrailerNumber}",
                            CreationDate = contract.CreationDate,
                            Number = contract.Number,
                            Type = PayType.Payment,
                            Balance = contract.CarrierPayment - paymentSumm,
                            DaysToExpiration = (int)(income.OrderBy(d => d.RecievingDate).Last().RecievingDate - DateTime.Now).TotalDays
                        };
                    }
                }
            }

            return required;
        }
    }

    public class DeleteContractService : IRequestHandler<Delete<ContractDto>, bool>
    {
        private IRepository _repository;
        private IFileManager _fileManager;

        public DeleteContractService(IRepository repository, IFileManager fileManager)
        {
            _repository = repository;
            _fileManager = fileManager;
        }

        public async Task<bool> Handle(Delete<ContractDto> request, CancellationToken cancellationToken)
        {
            bool result = await _repository.Remove<Contract>(request.Id);

            IEnumerable<Models.Sub.File> files = await _repository.Get<Models.Sub.File>(f => f.EntityType == nameof(Contract) && f.EntityId == request.Id);

            if (files.Any())
            {
                string catalog = Path.GetFileName(Path.GetDirectoryName(files.First().FullFilePath));

                await _fileManager.RemoveAllFiles(nameof(Contract), catalog);
            }

            return result;
        }
    }

    public class AddContractService : AddModelService<ContractDto>
    {
        IContractCreator _contractCreator;

        public AddContractService(IRepository repository, 
                                  IContractCreator contractCreator) : base(repository)
        {
            _contractCreator = contractCreator;
        }

        protected override async Task<Guid> Add(ContractDto dto)
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
                UnloadingPoints = new List<RoutePoint>(),
                CarrierId = dto.Carrier.Id,
                ClientId = dto.Client.Id,
                DriverId = dto.Driver.Id,
                VehicleId = dto.Vehicle.Id,
                TemplateId = dto.Template.Id,
                LogistId = dto.Logist.Id,
            };

         

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


            Guid id =  await _repository.Add(contract);

            if (id != Guid.Empty)
            {
                dto.Id = id;

                var files = await _repository.Get<Models.Sub.File>(f => f.EntityId == contract.TemplateId);

                string filePath = files.FirstOrDefault().FullFilePath;

                string contractFilePath = _contractCreator.CreateContractDocument(dto, filePath);

                string ext = Path.GetExtension(contractFilePath);

                Models.Sub.File contractFile = new Models.Sub.File()
                {
                    ViewNameWithExtencion = $"{contract.Number}{ext}",
                    EntityType = nameof(Contract),
                    EntityId = contract.Id,
                    FullFilePath = contractFilePath,
                };

                await _repository.Add(contractFile);
            }

            return id;
        }
    }

    public class UpdateContractService : UpdateModelService<ContractDto>
    {
        private IFileManager _fileManager;
        private IContractCreator _contractCreator;
        public UpdateContractService(IRepository repository, IFileManager fileManager, IContractCreator contractCreator) : base(repository)
        {
            _fileManager = fileManager;
            _contractCreator = contractCreator;
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
                contract.CarrierId = dto.Carrier.Id;
                contract.ClientId = dto.Client.Id;
                contract.DriverId = dto.Driver.Id;
                contract.VehicleId = dto.Vehicle.Id;
                contract.TemplateId = dto.Template.Id;

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

                bool result = await _repository.Update(contract);

                if (result) 
                {
                    var files = await _repository.Get<Models.Sub.File>(f => f.EntityId == contract.Id);

                    if (files.Any())
                    {
                        Models.Sub.File file = files.First();

                        if (await _repository.Remove<Models.Sub.File>(file.Id))
                        {
                            _fileManager.RemoveFile(file.FullFilePath);
                        }
                    }

                    files = await _repository.Get<Models.Sub.File>(f => f.EntityId == contract.TemplateId);

                    if (files.Any())
                    {

                        string filePath = files.FirstOrDefault().FullFilePath;

                        string contractFilePath = _contractCreator.CreateContractDocument(dto, filePath);

                        string ext = Path.GetExtension(contractFilePath);

                        Models.Sub.File contractFile = new Models.Sub.File()
                        {
                            ViewNameWithExtencion = $"{contract.Number}{ext}",
                            EntityType = nameof(Contract),
                            EntityId = contract.Id,
                            FullFilePath = contractFilePath,
                        };

                        await _repository.Add(contractFile);
                    }
                }

                return result;
            }
            return false;
        }
    }

    public class UpdateContractPropertyService : IRequestHandler<Patch<ContractDto>, bool>
    {
        protected IRepository _repository;
        protected ILogger<UpdateContractPropertyService> _logger;

        public UpdateContractPropertyService(IRepository repository, ILogger<UpdateContractPropertyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<bool> Handle(Patch<ContractDto> request, CancellationToken cancellationToken)
        {
            try
            {
                Contract contract = await _repository.GetById<Contract>(request.Id);

                foreach (var pair in request.Updates) 
                {
                    switch (pair.Key)
                    {
                        case nameof(ContractDto.Status):
                            if (Enum.TryParse<ContractStatus>(pair.Value.ToString(), out ContractStatus newStatus))
                            {
                                contract.Status = (short)newStatus;
                            }
                            break;
                    }
                }

                return await _repository.Update(contract);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex,ex.Message);
            }
            return false;
        }
    }
}
