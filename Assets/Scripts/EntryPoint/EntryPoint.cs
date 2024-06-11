using Asteroid.GameLogic;
using Asteroid.GameLogic.Factories;
using Asteroid.GameLogic.Factories.Concrete;
using Asteroid.GameLogic.SpawnStrategy;
using Asteroid.Input;
using Asteroid.Presentation.Entity;
using Asteroid.Presentation.Entity.Ship;
using Asteroid.Presentation.Ui;
using UnityEngine;

namespace Asteroid.EntryPoint
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private ShipPresentation shipPresentation;
        [SerializeField] private ShipGunPresentation shipGunPresentation;
        [SerializeField] private ShipLaserGunPresentation shipLaserGunPresentation;
        [SerializeField] private AsteroidPresentation asteroidPresentationPrefab;
        [SerializeField] private BulletPresentation bulletPresentationPrefab;
        [SerializeField] private UfoPresentation ufoPresentationPrefab;

        [SerializeField] private Transform asteroidParent;
        [SerializeField] private Transform ufoParent;
        [SerializeField] private Transform bulletParent;

        [SerializeField] private GameplayUiScreen gameplayUiScreen;
        [SerializeField] private MenuUiScreen waitingForStartUiScreen;
        [SerializeField] private GameOverUiScreen gameOverUiScreen;

        [SerializeField] private Camera gameCamera;

        [SerializeField] private ShipExplosion explosion;
        [SerializeField] private BrokenShip brokenShip;

        private GameController gameController;

        private void Awake()
        {
            MonoMemoryPool<BulletPresentation> bulletMemoryPool = new MonoMemoryPool<BulletPresentation>(
                new MonoFactory<BulletPresentation>(
                    bulletPresentationPrefab, false, bulletParent
                ),
                new MemoryPoolSettings(0, 1000, PoolExpandMethods.Double)
            );

            var asteroidEntityFactorySettings = new RandomizedEntityFactorySettings(1f, 1.5f, 0.1f, 1f);
            var asteroidAutoSpawnStrategySettings = new AutoSpawnStrategySettings(10, 0.3f, 10);
            var ufoEntityFactorySettings = new RandomizedEntityFactorySettings(1f, 1f, 1f, 1f);
            var ufoAutoSpawnStrategySettings = new AutoSpawnStrategySettings(2, 3f, 1);

            gameController = new GameController(
                gameplayUiScreen, waitingForStartUiScreen, gameOverUiScreen,
                gameCamera,
                shipPresentation,
                shipGunPresentation, shipLaserGunPresentation,
                new MonoFactory<ShipExplosion>(explosion, true, null),
                new MonoFactory<BrokenShip>(brokenShip, true, null),
                new MonoFactory<AsteroidPresentation>(asteroidPresentationPrefab, true, asteroidParent),
                bulletMemoryPool, bulletMemoryPool,
                new MonoFactory<UfoPresentation>(ufoPresentationPrefab, true, ufoParent),
                asteroidEntityFactorySettings, asteroidAutoSpawnStrategySettings,
                ufoEntityFactorySettings, ufoAutoSpawnStrategySettings,
                new PlayerInput()
            );

            gameController.Initialize();
        }

        private void Update()
        {
            gameController.Tick(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            gameController.FixedTick(Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            gameController.Dispose();
        }
    }
}