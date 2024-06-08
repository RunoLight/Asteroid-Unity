using UnityEngine;

namespace Asteroid.Presentation.Entity.Ship.Abstractions
{
    public interface IShipGun
    {
        /// <summary>
        /// The gun have multiple gun mounting points and using them for shot one-by-one (Left-Right-Left-...)
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNewBulletSourcePoint();
    }
}