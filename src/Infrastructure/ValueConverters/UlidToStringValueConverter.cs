using System.Linq.Expressions;
using Domain.Common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.ValueConverters;

public sealed class UlidToStringValueConverter() : ValueConverter<Ulid, string>
(
    to => to.UlidToString(),
    from => from.StringToUlid()
);
