using AutoMapper;
using AzureTest.Core.Entities;
using AzureTest.Core.ViewModels;

namespace AzureTest.Core.Mapping
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<ApplicationUser, UserViewModel>();
		}
	}
}
