﻿using Asteroid.GameLogic.EntityManagement;
using Asteroid.Presentation.Entity.Abstractions;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.ConcreteFactory
{
    public abstract class BaseEntityFactory<TEntity, TPresentation> : IFactory<TEntity>
        where TEntity : GameEntity<TEntity, TPresentation>, new()
        where TPresentation : IEntityPresentation
    {
        private readonly IFactory<TPresentation> presentationFactory;

        protected BaseEntityFactory(IFactory<TPresentation> presentationFactory)
        {
            this.presentationFactory = presentationFactory;
        }

        public virtual TEntity Create()
        {
            return Spawn(Vector2.zero, Vector2.zero, 1f);
        }

        protected TEntity Spawn(Vector2 position, Vector2 velocity, float scale)
        {
            var entity = new TEntity
            {
                Presentation = presentationFactory.Create(),
                Scale = scale,
                Position = position,
                Velocity = velocity
            };
            entity.Initialize();

            return entity;
        }
    }
}