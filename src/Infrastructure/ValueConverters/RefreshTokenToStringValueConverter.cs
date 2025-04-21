using Domain.Common;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.ValueConverters;

public sealed class RefreshTokenToStringValueConverter() : ValueConverter<RefreshToken, string>
(
    to => to.RefreshTokenToString(),
    from => from.StringToRefreshToken()
);