using System.Collections.Generic;
using Asteroid.GameLogic.SpawnStrategy;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.EntityManagement.Manager
{
    public class AsteroidManager : EntityManager<Entity.Asteroid>
    {
        public List<Entity.Asteroid> Asteroids => Entities;

        private const float AsteroidChunkScaleFactor = 0.5f;

        public AsteroidManager(AutoSpawnStrategySettings settings, IFactory<Entity.Asteroid> entityFactory)
            : base(entityFactory, null)
        {
            SpawnStrategy = new AutoSpawnStrategy<Entity.Asteroid>(this, settings);
        }

        protected override void OnEntityDestroyed(Entity.Asteroid destroyedAsteroid)
        {
            base.OnEntityDestroyed(destroyedAsteroid);

            if (destroyedAsteroid.IsAsteroidChunk)
                return;

            var scale = destroyedAsteroid.Scale * AsteroidChunkScaleFactor;
            var offset1 = new Vector2(0.5f, 0.5f);
            var offset2 = new Vector2(-0.5f, -0.5f);

            Spawn().ConstructAsteroidChunk(destroyedAsteroid.Position + offset1, offset1, scale);
            Spawn().ConstructAsteroidChunk(destroyedAsteroid.Position + offset2, offset2, scale);
        }
    }
}