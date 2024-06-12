using Asteroid.GameLogic.EntityManagement.Entity;
using Asteroid.Presentation.Entity.Abstractions;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.ConcreteFactory
{
    public class BulletFactory : BaseEntityFactory<Bullet, IBulletPresentation>
    {
        private const float BulletScale = 0.5f;

        public BulletFactory(IFactory<IBulletPresentation> presentationFactory) : base(presentationFactory)
        {
        }

        public override Bullet Create()
        {
            return Spawn(Vector2.zero, Vector2.zero, BulletScale);
        }
    }
}