namespace Asteroid.GameLogic.Factories
{
    public interface IFactory<out TValue>
    {
        TValue Create();
    }
}
    

