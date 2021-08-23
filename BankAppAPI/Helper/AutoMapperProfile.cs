using AutoMapper;
using BankService.Model;
using BankService.ResourceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankService
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerModel, Customer>()
                .ForMember(x => x.id, opt => opt.Ignore())
                .ForMember(x => x.Loan, opt => opt.Ignore());
            CreateMap<LoanModel, Loan>()
               .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
