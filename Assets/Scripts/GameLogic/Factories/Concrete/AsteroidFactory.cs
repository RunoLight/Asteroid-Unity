using Asteroid.Presentation.Entity.Abstractions;

namespace Asteroid.GameLogic.Factories.Concrete
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