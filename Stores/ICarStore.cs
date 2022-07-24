using DecoratorAndCaching.Models;

namespace DecoratorAndCaching.Stores;

public interface ICarStore
{
    CarDto List();
    CarDto Get(int id);
}