using AutoMapper;
using ProjectPRN231.Dtos;
using ProjectPRN231.Models;
namespace ProjectPRN231.Dtos
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap <ProductDto, Product>();
            CreateMap <AccountDto, Account>();
            CreateMap <Account, AccountDto>();
        }
    }
}
