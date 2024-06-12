using System;
using System.Collections.Generic;
using Asteroid.GameLogic.SpawnStrategy;
using Factory;
using JetBrains.Annotations;

namespace Asteroid.GameLogic.EntityManagement
{
    public class EntityManager<TEntity> : IEntityManager<TEntity> where TEntity : IGameEntity<TEntity>
    {
        public event Action OnEntityDied;

        protected ISpawnStrategy<TEntity> SpawnStrategy;
        protected readonly List<TEntity> Entities = new();

        private readonly IFactory<TEntity> entityFactory;
        [CanBeNull] private readonly IMemoryPool presentationMemoryPool;

        public int EntitiesCount => Entities.Count;

        protected EntityManager(IFactory<TEntity> entityFactory, [CanBeNull] IMemoryPool presentationMemoryPool)
        {
            this.entityFactory = entityFactory;
            this.presentationMemoryPool = presentationMemoryPool;
        }

        public void Start()
        {
            ResetAll();
            SpawnStrategy.Enable();
        }

        public void Stop()
        {
            SpawnStrategy.Disable();
        }

        public void Tick(float deltaTime)
        {
            foreach (var asteroid in new List<TEntity>(Entities))
            {
                asteroid.Tick(deltaTime);
            }

            SpawnStrategy.Tick(deltaTime);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            foreach (var asteroid in new List<TEntity>(Entities))
            {
                asteroid.FixedTick(fixedDeltaTime);
            }
        }

        public TEntity CreateEntity()
        {
            var entity = entityFactory.Create();

            entity.Destroyed += OnEntityDestroyed;
            Entities.Add(entity);

            return entity;
        }

        private void ResetAll()
        {
            foreach (var entity in new List<TEntity>(Entities))
            {
                entity.Destroyed -= OnEntityDestroyed;
                entity.Destroy();
            }

            Entities.Clear();
        }

        protected TEntity Spawn()
        {
            return SpawnStrategy.Spawn();
        }

        protected virtual void OnEntityDestroyed(TEntity obj)
        {
            OnEntityDied?.Invoke();
            obj.Destroyed -= OnEntityDestroyed;

            presentationMemoryPool?.Despawn(obj.EntityPresentation);

            Entities.Remove(obj);
        }
    }
}