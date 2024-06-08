using System;
using Asteroid.GameLogic.Abstraction;
using Asteroid.GameLogic.Factories;
using Asteroid.GameLogic.Ship.States;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Ship;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.GameLogic.Ship
{
    public class ShipController : IObjectRotationAdapter, IDisposable
    {
        public event Action OnDied;
        public event Action<Vector2, Quaternion, Vector2> ShipStatusUpdate;

        private ShipState state;

        private readonly IObjectPositionAdapter positionAdapter;
        private readonly IObjectRotationAdapter rotationAdapter;
        private readonly IShipPresentation shipPresentation;
        private readonly ShipGunController shipGunController;
        private readonly ShipLaserGunController shipLaserGunController;

        private readonly IShipExplosion explosion;
        private readonly IBrokenShip brokenShip;
        private readonly IPlayerInput playerInput;

        public ShipController(
            IShipPresentation shipPresentation,
            ShipGunController shipGunController,
            ShipLaserGunController shipLaserGunController,
            IShipExplosion shipExplosion, IBrokenShip brokenShip,
            IPlayerInput playerInput
        )
        {
            this.shipPresentation = shipPresentation;
            this.shipGunController = shipGunController;
            this.shipLaserGunController = shipLaserGunController;
            positionAdapter = shipPresentation;
            rotationAdapter = shipPresentation;
            explosion = shipExplosion;
            this.brokenShip = brokenShip;
            this.playerInput = playerInput;

            shipPresentation.OnLethalCollided += OnCollision;

            Position = Vector2.zero;
        }

        public void Dispose()
        {
            shipPresentation.OnLethalCollided -= OnCollision;

            state?.Dispose();
        }

        private void OnCollision()
        {
            state.OnCollision();
        }

        public bool Enabled
        {
            set => shipPresentation.SetActive(value);
        }

        public Vector2 Position
        {
            get => positionAdapter.Position;
            set => positionAdapter.Position = value;
        }

        public Quaternion Rotation
        {
            get => rotationAdapter.Rotation;
            set => rotationAdapter.Rotation = value;
        }

        public Vector2 Velocity { get; set; } = Vector2.zero;

        public void SetEngineParticles(bool isActive)
        {
            shipPresentation.EnableParticles(isActive);
        }

        public void Die()
        {
            OnDied?.Invoke();
        }

        public void ChangeState(ShipStates shipStates)
        {
            if (state is ShipStateMoving stateMoving)
            {
                playerInput.Forward -= stateMoving.OnInputForward;
                playerInput.Rotate -= stateMoving.OnInputRotate;
                playerInput.ActivateLaser -= stateMoving.OnInputUseLaser;
            }
            
            state?.Dispose();

            // TODO It may be a states factory
            state = shipStates switch
            {
                ShipStates.WaitingToStart => new ShipStateWaitingToStart(this, new ShipStateWaitingToStart.Settings
                {
                    amplitude = 0.5f,
                    frequency = 1f,
                    startOffset = Vector3.zero
                }),
                ShipStates.Moving => new ShipStateMoving(this, shipGunController, shipLaserGunController),
                ShipStates.Dead => new ShipStateDead(
                    new ShipStateDead.Settings { explosionForce = 1f },
                    this,
                    new MonoFactory<ShipExplosion>(explosion as ShipExplosion, true, null),
                    new MonoFactory<BrokenShip>(brokenShip as BrokenShip, true, null)
                ),
                _ => throw new ArgumentOutOfRangeException(nameof(shipStates), shipStates, null)
            };

            if (state is ShipStateMoving movingState)
            {
                playerInput.Forward += movingState.OnInputForward;
                playerInput.Rotate += movingState.OnInputRotate;
                playerInput.ActivateLaser += movingState.OnInputUseLaser;
            }
            
            state.Start();
        }

        public void Tick(float deltaTime)
        {
            state?.Tick(deltaTime);
            ShipStatusUpdate?.Invoke(Position, Rotation, Velocity);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            positionAdapter.Position += Velocity * fixedDeltaTime;
            state?.FixedTick(fixedDeltaTime);
        }

        public void DisableParticlesForOneFrame()
        {
            // Remove trail effect while teleport to another edge of screen
            shipPresentation.DisableParticlesForOneFrame();
        }
    }
}