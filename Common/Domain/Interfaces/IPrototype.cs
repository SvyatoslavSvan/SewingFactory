namespace SewingFactory.Common.Domain.Interfaces;

public interface IPrototype<out T>
{
    T Clone();
}