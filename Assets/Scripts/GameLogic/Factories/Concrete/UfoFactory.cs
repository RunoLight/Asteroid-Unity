using Asteroid.GameLogic.EntityManagement.Entity;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Abstractions;

namespace Asteroid.GameLogic.Factories.Concrete
{
    public class UfoFactory : RandomizedEntityFactory<Ufo, IUfoPresentation>
    {
        private readonly IObjectPositionAdapter targetToFollow;

        public UfoFactory(
            IObjectPositionAdapter targetToFollow,
            LevelHelper level,
            RandomizedEntityFactorySettings settings,
            IFactory<IUfoPresentation> presentationFactory
        ) : base(level, settings, presentationFactory)
        {
            this.targetToFollow = targetToFollow;
        }

        public override Ufo Create()
        {
            return base.Create().Initialize(targetToFollow);
        }
    }
}