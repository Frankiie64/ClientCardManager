using AutoMapper;
using CientCardManager.Core.Application.ViewModels.Cliente;
using CientCardManager.Core.Application.ViewModels.ClienteTarjeta;
using CientCardManager.Core.Application.ViewModels.TipoTarjeta;
using ClientCardManager.Core.Domain.Entidad;

namespace CientCardManager.Core.Application.Mapping
{
    public class GeneralProfile:Profile
    {
        public GeneralProfile()
        {
            CreateMap<ClienteVM, Cliente > ()
                .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<SaveClienteVM, Cliente>()
               .ForMember(x => x.Tarjetas, opt => opt.Ignore())
               .ForMember(x => x.Creado, opt => opt.Ignore())
               .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<SaveClienteVM, ClienteVM>()
              .ForMember(x => x.Creado, opt => opt.Ignore())
              .ForMember(x => x.Tarjetas, opt => opt.Ignore())
              .ReverseMap();


            CreateMap<TipoTarjetaVM, TipoTarjeta>()
               .ReverseMap();

            CreateMap<SaveTipoTarjetaVM, TipoTarjeta>()
               .ForMember(x => x.Tarjetas, opt => opt.Ignore())
               .ForMember(x => x.Creado, opt => opt.Ignore())
               .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<SaveTipoTarjetaVM, TipoTarjetaVM>()
              .ForMember(x => x.Tarjetas, opt => opt.Ignore())
              .ForMember(x => x.Creado, opt => opt.Ignore())
              .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<ClienteTarjetaVM, ClienteTarjeta>()
               .ForMember(x => x.Creado, opt => opt.Ignore())               
               .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<SaveClienteTarjetaVM, ClienteTarjeta>()
               .ForMember(x => x.Cliente, opt => opt.Ignore())
               .ForMember(x => x.Tarjeta, opt => opt.Ignore())
               .ForMember(x => x.Creado, opt => opt.Ignore())               
               .ForMember(x => x.UltimaModificacion, opt => opt.Ignore())
               .ReverseMap()
               .ForMember(x => x.NombreCliente, opt => opt.Ignore());

            CreateMap<SaveClienteTarjetaVM, ClienteTarjetaVM>()
               .ForMember(x => x.Cliente, opt => opt.Ignore())
               .ForMember(x => x.Tarjeta, opt => opt.Ignore())
              .ReverseMap()
              .ForMember(x => x.NombreCliente, opt=>opt.Ignore());

        }
    }
}
