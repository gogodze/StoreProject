namespace Domain.Common;

public static class UlidExt
{
    public static Ulid NewUlid(this Ulid ulid)
    {
        return Ulid.NewUlid();
    }

    public static string UlidToString(this Ulid ulid)
    {
        return ulid.ToString();
    }
}