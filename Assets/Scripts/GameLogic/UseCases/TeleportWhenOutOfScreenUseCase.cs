using UnityEngine;

namespace Asteroid.GameLogic.UseCases
{
    public class TeleportWhenOutOfScreenUseCase
    {
        public TeleportWhenOutOfScreenUseCase(LevelHelper levelHelper)
        {
            level = levelHelper;
        }

        private readonly LevelHelper level;

        /// <summary>
        /// Checks if object with given size is fully outside of game screen so it's need to teleport to another side
        /// </summary>
        /// <param name="scale"> World space size of object</param>
        /// <param name="position"> World space position of object</param>
        /// <param name="newPosition">
        /// When returns true - osition to which the object needs to be teleported. <br/>
        /// When false - Vector.zero to reduce memory footprint
        /// </param>
        /// <returns> True if object needs to be teleported</returns>
        public bool IsNeedToTeleport(float scale, Vector3 position, out Vector3 newPosition)
        {
            if (position.x > level.Right + scale)
            {
                newPosition = new Vector3(level.Left - scale, position.y, position.z);
                return true;
            }

            if (position.x < level.Left - scale)
            {
                newPosition = new Vector3(level.Right + scale, position.y, position.z);
                return true;
            }

            if (position.y < level.Bottom - scale)
            {
                newPosition = new Vector3(position.x, level.Top + scale, position.z);
                return true;
            }

            if (position.y > level.Top + scale)
            {
                newPosition = new Vector3(position.x, level.Bottom - scale, position.z);
                return true;
            }

            newPosition = Vector3.zero;
            return false;
        }
    }
}