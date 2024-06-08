using Asteroid.Presentation.Entity.Abstractions;
using Asteroid.Presentation.Marker;

namespace Asteroid.Presentation.Entity
{
    public class BulletPresentation : BaseEntityPresentation<IEnemy>, IDestroysEnemies, IBulletPresentation
    {
    }
}