using Asteroid.Presentation.Entity.Ship.Abstractions;
using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship
{
    public class ShipGunPresentation : MonoBehaviour, IShipGun
    {
        [SerializeField] public Transform leftBulletPoint;
        [SerializeField] public Transform rightBulletPoint;

        private bool isShootFromLeftPoint;

        public Vector2 GetNewBulletSourcePoint()
        {
            isShootFromLeftPoint = !isShootFromLeftPoint;
            return isShootFromLeftPoint ? leftBulletPoint.position : rightBulletPoint.position;
        }
    }
}