using System.Diagnostics.CodeAnalysis;

namespace MotorCycleRentail.Infra.Mappers;

[ExcludeFromCodeCoverage]
public class MotorcycleProfile : Profile
{
    public MotorcycleProfile()
    {
        CreateMap<Motorcycle, MotorcycleResponse>();
    }
}
