using System;
using System.Collections.Generic;
using Asteroid.GameLogic.ConcreteFactory;
using Asteroid.GameLogic.EntityManagement.Manager;
using Asteroid.GameLogic.Ship;
using Asteroid.GameLogic.SpawnStrategy;
using Asteroid.GameLogic.UseCases;
using Asteroid.Presentation.Abstractions;
using Asteroid.Presentation.Entity.Abstractions;
using Asteroid.Presentation.Entity.Ship.Abstractions;
using Asteroid.Presentation.Ui.Abstractions;
using Factory;
using UnityEngine;

namespace Asteroid.GameLogic
{
    public class GameController : IDisposable
    {
        private readonly ShipController ship;
        private readonly ShipGunController shipGun;
        private readonly ShipLaserGunController shipLaserGun;

        private readonly AsteroidManager asteroidManager;
        private readonly UfoManager ufoManager;
        private readonly BulletManager bulletManager;

        private readonly TeleportWhenOutOfScreenUseCase teleportWhenOutOfScreenUseCase;

        private readonly IGameplayUiScreen gameplayUiScreen;
        private readonly IUiScreen waitingToStartUi;
        private readonly IGameOverUiScreen gameOverUi;

        private GameStates state;

        private int score;

        private IEnumerable<IUiScreen> AllUiScreens => new List<IUiScreen>
            { gameplayUiScreen, waitingToStartUi, gameOverUi };

        public GameController(
            IGameplayUiScreen gameplayUiScreen, IUiScreen waitingToStartUi, IGameOverUiScreen gameOverUi,
            Camera camera,
            IShipPresentation shipPresentation,
            IShipGun shipGunPresentation, IShipLaserGun shipLaserGunPresentation,
            IFactory<IShipExplosion> explosionFactory, IFactory<IBrokenShip> brokenShipFactory,
            IFactory<IAsteroidPresentation> asteroidPresentationFactory,
            IFactory<IBulletPresentation> bulletPresentationFactory,
            IMemoryPool bulletPresentationMemoryPool,
            IFactory<IUfoPresentation> ufoPresentationFactory,
            RandomizedEntityFactorySettings asteroidEntityFactorySettings,
            AutoSpawnStrategySettings asteroidAutoSpawnStrategySettings,
            RandomizedEntityFactorySettings ufoEntityFactorySettings,
            AutoSpawnStrategySettings ufoAutoSpawnStrategySettings,
            IPlayerInput playerInput
            )
        {
            this.gameplayUiScreen = gameplayUiScreen;
            this.waitingToStartUi = waitingToStartUi;
            this.gameOverUi = gameOverUi;

            var levelHelper = new LevelHelper(camera);

            teleportWhenOutOfScreenUseCase = new TeleportWhenOutOfScreenUseCase(levelHelper);

            shipGun = new ShipGunController(shipGunPresentation);
            shipLaserGun = new ShipLaserGunController(shipLaserGunPresentation);

            ship = new ShipController(
                shipPresentation,
                shipGun, shipLaserGun,
                explosionFactory, brokenShipFactory,
                playerInput
            );

            ship.ShipStatusUpdate += OnShipStatusUpdate;

            shipGun.Setup(newRotationAdapter: ship);

            asteroidManager = new AsteroidManager(
                asteroidAutoSpawnStrategySettings,
                new AsteroidFactory(levelHelper, asteroidEntityFactorySettings, asteroidPresentationFactory)
            );

            ufoManager = new UfoManager(
                ufoAutoSpawnStrategySettings,
                new UfoFactory(shipPresentation, levelHelper, ufoEntityFactorySettings, ufoPresentationFactory)
            );
            bulletManager = new BulletManager(
                new BulletFactory(bulletPresentationFactory), bulletPresentationMemoryPool
            );

            ship.ChangeState(ShipStates.WaitingToStart);
        }

        private void OnShipStatusUpdate(Vector2 position, Quaternion rotation, Vector2 velocity)
        {
            gameplayUiScreen.SetInfo(position, rotation, velocity);
        }

        public void Initialize()
        {
            ship.OnDied += OnShipDied;
            ufoManager.OnEntityDied += OnUfoDestroyed;
            asteroidManager.OnEntityDied += OnAsteroidDestroyed;
            shipGun.BulletShotRequested += bulletManager.Shoot;
            shipLaserGun.OnCooldownChanged += OnLaserCooldownUpdated;

            state = GameStates.WaitingToStart;
            EnableUiScreen(waitingToStartUi);
        }

        public void Dispose()
        {
            ship.OnDied -= OnShipDied;
            ufoManager.OnEntityDied -= OnUfoDestroyed;
            asteroidManager.OnEntityDied -= OnAsteroidDestroyed;
            shipGun.BulletShotRequested -= bulletManager.Shoot;
            shipLaserGun.OnCooldownChanged -= OnLaserCooldownUpdated;
        }

        public void Tick(float deltaTime)
        {
            switch (state)
            {
                case GameStates.WaitingToStart:
                {
                    TickStarting(deltaTime);
                    break;
                }
                case GameStates.Playing:
                {
                    TickPlaying(deltaTime);
                    break;
                }
                case GameStates.GameOver:
                {
                    TickGameOver(deltaTime);
                    break;
                }
                default:
                {
                    // Assert.That(false);
                    break;
                }
            }
        }

        public void FixedTick(float fixedDeltaTime)
        {
            switch (state)
            {
                case GameStates.Playing:
                {
                    ship.FixedTick(fixedDeltaTime);
                    ufoManager.FixedTick(fixedDeltaTime);
                    asteroidManager.FixedTick(fixedDeltaTime);
                    bulletManager.FixedTick(fixedDeltaTime);
                    break;
                }
                case GameStates.GameOver:
                {
                    bulletManager.FixedTick(fixedDeltaTime);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        private void EnableUiScreen(IUiScreen screenToEnable)
        {
            foreach (IUiScreen screen in AllUiScreens)
            {
                screen.SetActive(screen == screenToEnable);
            }
        }

        private void OnShipDied()
        {
            state = GameStates.GameOver;

            gameOverUi.SetScore(score);
            EnableUiScreen(gameOverUi);

            ufoManager.Stop();
            asteroidManager.Stop();
        }

        private void OnLaserCooldownUpdated(int charges, float progress)
        {
            if (state != GameStates.Playing)
                return;

            gameplayUiScreen.SetLaserCooldown(charges, progress);
        }

        private void OnAsteroidDestroyed()
        {
            if (state != GameStates.Playing)
                return;

            score += 10;
            gameplayUiScreen.SetScore(score);
        }

        private void OnUfoDestroyed()
        {
            if (state != GameStates.Playing)
                return;

            score += 25;
            gameplayUiScreen.SetScore(score);
        }

        private void TickGameOver(float deltaTime)
        {
            bulletManager.Tick(deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }

        private void TickPlaying(float deltaTime)
        {
            ufoManager.Tick(deltaTime);
            asteroidManager.Tick(deltaTime);
            bulletManager.Tick(deltaTime);
            ship.Tick(deltaTime);

            if (teleportWhenOutOfScreenUseCase.IsNeedToTeleport(1f, ship.Position, out var newShipPosition))
            {
                ship.Position = newShipPosition;
                ship.DisableParticlesForOneFrame();
            }

            foreach (EntityManagement.Entity.Asteroid asteroid in asteroidManager.Asteroids)
            {
                if (teleportWhenOutOfScreenUseCase
                    .IsNeedToTeleport(asteroid.Scale, asteroid.Position, out var newAsteroidPosition)
                   )
                {
                    asteroid.Position = newAsteroidPosition;
                }
            }
        }

        private void TickStarting(float deltaTime)
        {
            ship.Tick(deltaTime);

            if (Input.GetMouseButtonDown(0))
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            score = 0;
            gameplayUiScreen.SetScore(score);

            ufoManager.Start();
            asteroidManager.Start();
            ship.ChangeState(ShipStates.Moving);

            state = GameStates.Playing;

            EnableUiScreen(gameplayUiScreen);
        }

        private enum GameStates
        {
            WaitingToStart,
            Playing,
            GameOver
        }
    }
}