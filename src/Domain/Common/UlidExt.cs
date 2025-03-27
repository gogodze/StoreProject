namespace Domain.Common;

public static class UlidExt
{
    public static Ulid NewUlid(this Ulid ulid)
    {
        return new Ulid();
    }

    public static string UlidToString(this Ulid ulid)
    {
        return ulid.ToString();
    }
}