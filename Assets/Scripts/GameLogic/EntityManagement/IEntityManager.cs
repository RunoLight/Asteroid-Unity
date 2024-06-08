namespace Asteroid.GameLogic.EntityManagement
{
    public interface IEntityManager<out TEntity>
    {
        TEntity CreateEntity();
        int EntitiesCount { get; }
    }
}