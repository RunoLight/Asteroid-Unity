using System;
using Asteroid.Presentation.Entity.Abstractions;
using UnityEngine;

namespace Asteroid.GameLogic.EntityManagement
{
    public abstract class GameEntity<TEntity, TPresentation> : IGameEntity<TEntity>
        where TEntity : class
        where TPresentation : IEntityPresentation
    {
        public event Action<TEntity> Destroyed;

        public TPresentation Presentation;
        public object EntityPresentation => Presentation;

        public Vector2 Velocity;

        public Vector2 Position
        {
            get => Presentation.Position;
            set => Presentation.Position = value;
        }

        public float Scale
        {
            get => Presentation.Scale;
            set => Presentation.Scale = value;
        }

        public virtual void Initialize()
        {
        }

        public virtual void FixedTick(float fixedDeltaTime)
        {
        }

        public virtual void Tick(float deltaTime)
        {
        }

        public virtual void Destroy()
        {
            Destroyed?.Invoke(this as TEntity);
        }
    }
}