namespace DotNetCore.Infra.Abstractions
{
    public abstract class ValueObject<T>
        where T : ValueObject<T>
    {

    }
}
