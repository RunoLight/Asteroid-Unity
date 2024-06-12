using Asteroid.GameLogic.EntityManagement;
using Asteroid.Presentation.Entity.Abstractions;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.ConcreteFactory
{
    public class RandomizedEntityFactory<TEntity, TPresentation> : BaseEntityFactory<TEntity, TPresentation>
        where TEntity : GameEntity<TEntity, TPresentation>, new()
        where TPresentation : IEntityPresentation
    {
        private readonly LevelHelper level;
        private readonly RandomizedEntityFactorySettings settings;

        protected RandomizedEntityFactory(
            LevelHelper level, RandomizedEntityFactorySettings settings, IFactory<TPresentation> presentationFactory
        ) : base(presentationFactory)
        {
            this.settings = settings;
            this.level = level;
        }

        public override TEntity Create()
        {
            return SpawnRandom();
        }

        private TEntity SpawnRandom()
        {
            var scale = Random.Range(settings.MinScale, settings.MaxScale);

            return Spawn(
                level.GetRandomPositionOutsideOfScreen(scale),
                GetRandomDirection() * Random.Range(settings.MinSpeed, settings.MaxSpeed),
                scale
            );
        }

        private static Vector3 GetRandomDirection()
        {
            var theta = Random.Range(0, Mathf.PI * 2.0f);
            return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
        }
    }
    
    public class RandomizedEntityFactorySettings
    {
        public readonly float MinSpeed;
        public readonly float MaxSpeed;

        public readonly float MinScale;
        public readonly float MaxScale;

        public RandomizedEntityFactorySettings(float minScale, float maxScale, float minSpeed, float maxSpeed)
        {
            MaxScale = maxScale;
            MinScale = minScale;
            MaxSpeed = maxSpeed;
            MinSpeed = minSpeed;
        }
    }
}