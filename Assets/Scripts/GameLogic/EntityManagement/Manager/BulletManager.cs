using Asteroid.GameLogic.EntityManagement.Entity;
using Asteroid.GameLogic.SpawnStrategy;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.EntityManagement.Manager
{
    public class BulletManager : EntityManager<Bullet>
    {
        public BulletManager(IFactory<Bullet> entityFactory, IMemoryPool presentationMemoryPool)
            : base(entityFactory, presentationMemoryPool)
        {
            SpawnStrategy = new ManualSpawnStrategy<Bullet>(this);
        }

        public void Shoot(Vector2 bulletSourcePoint, Quaternion lookDirection)
        {
            Spawn().Setup(bulletSourcePoint, lookDirection);
        }
    }
}