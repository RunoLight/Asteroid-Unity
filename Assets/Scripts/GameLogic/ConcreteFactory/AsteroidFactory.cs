using Asteroid.Presentation.Entity.Abstractions;
using Factory;

namespace Asteroid.GameLogic.ConcreteFactory
{
    public class AsteroidFactory : RandomizedEntityFactory<EntityManagement.Entity.Asteroid, IAsteroidPresentation>
    {
        public AsteroidFactory(
            LevelHelper level,
            RandomizedEntityFactorySettings settings,
            IFactory<IAsteroidPresentation> presentationFactory
        ) : base(level, settings, presentationFactory)
        {
        }
    }
}