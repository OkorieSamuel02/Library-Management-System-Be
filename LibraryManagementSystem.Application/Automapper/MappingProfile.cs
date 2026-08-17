using AutoMapper;
using LibraryManagementSystem.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Domain.Entity;
using LibraryManagementSystem.Application.Book.DataTransferObject.Response;
using LibraryManagementSystem.Application.Borrowing.DataTransferObject.Response;
using LibraryManagementSystem.Application.Membership.DataTransferObject.Response;

namespace LibraryManagementSystem.Application.Automapper
{
    public class MappingProfile  : Profile
    {
        public MappingProfile()
        {
             CreateMap<LibraryManagementSystem.Domain.Entity.Book, BookResponseModel>().ReverseMap();
            CreateMap<Loan, GetLoanResponseModel>().ReverseMap();
            CreateMap<Member, MemberResponseModel>().ReverseMap();
        }
    }
}
