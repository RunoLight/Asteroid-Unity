using System;
using Asteroid.GameLogic.TheShip.States;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic.TheShip
{
    public class Ship : IObjectRotationAdapter, IDisposable
    {
        public event Action OnDied;
        public event Action<Vector2, Quaternion, Vector2> ShipStatusUpdate;

        private ShipState state;

        private readonly IObjectPositionAdapter position;
        private readonly IObjectRotationAdapter rotation;

        private readonly IShipPresentation shipPresentation;

        private readonly IPlayerInput playerInput;

        public static ShipState StateBeforeGame;
        public static ShipState StateGameplay;
        public static ShipState StateGameOver;

        public Ship(
            IShipPresentation shipPresentation,
            ShipBulletGuns shipBulletGuns,
            ShipLaserGun shipLaserGun,
            IFactory<IShipExplosion> shipExplosion, IFactory<IBrokenShip> brokenShip,
            IPlayerInput playerInput
        )
        {
            this.shipPresentation = shipPresentation;
            position = shipPresentation;
            rotation = shipPresentation;
            this.playerInput = playerInput;

            shipPresentation.OnLethalCollided += OnCollision;

            Position = Vector2.zero;

            StateBeforeGame = new ShipStateWaitingToStart(
                this,
                ShipStateWaitingToStart.Settings.Default
            );
            StateGameplay = new ShipStateGameplay(
                this, shipBulletGuns, shipLaserGun
            );
            StateGameOver = new ShipStateDead(
                this,
                ShipStateDead.Settings.Default,
                shipExplosion,
                brokenShip
            );

            if (StateGameplay is ShipStateGameplay movingState)
            {
                playerInput.Forward += movingState.OnInputForward;
                playerInput.Rotate += movingState.OnInputRotate;
                playerInput.ActivateLaser += movingState.OnInputUseLaser;
            }
        }

        public void Dispose()
        {
            shipPresentation.OnLethalCollided -= OnCollision;

            if (StateGameplay is ShipStateGameplay movingState)
            {
                playerInput.Forward -= movingState.OnInputForward;
                playerInput.Rotate -= movingState.OnInputRotate;
                playerInput.ActivateLaser -= movingState.OnInputUseLaser;
            }

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
            get => position.Value;
            set => position.Value = value;
        }

        public Quaternion Value
        {
            get => rotation.Value;
            set => rotation.Value = value;
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

        public void ChangeState(ShipState shipState)
        {
            state?.Dispose();
            state = shipState;
            state.Start();
        }

        public void Tick(float deltaTime)
        {
            state?.Tick(deltaTime);
            ShipStatusUpdate?.Invoke(Position, Value, Velocity);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            position.Value += Velocity * fixedDeltaTime;
            state?.FixedTick(fixedDeltaTime);
        }

        public void DisableParticlesForOneFrame()
        {
            // Remove trail effect while teleport to another edge of screen
            shipPresentation.DisableParticlesForOneFrame();
        }
    }
}