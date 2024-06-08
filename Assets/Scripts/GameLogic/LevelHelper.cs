using System;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Asteroid.GameLogic
{
    public class LevelHelper
    {
        private readonly Camera camera;

        public LevelHelper(Camera camera)
        {
            this.camera = camera;
        }

        public float Bottom => -ExtentHeight;

        public float Top => ExtentHeight;

        public float Left => -ExtentWidth;

        public float Right => ExtentWidth;

        public float ExtentHeight => camera.orthographicSize;

        public float Height => ExtentHeight * 2.0f;

        public float ExtentWidth => camera.aspect * camera.orthographicSize;

        public float Width => ExtentWidth * 2.0f;

        public Vector3 GetRandomPositionOutsideOfScreen(float scale)
        {
            var side = (Side)Random.Range(0, (int)Side.Count);
            var rand = Random.Range(0.0f, 1.0f);

            return side switch
            {
                Side.Top => new Vector3(Left + rand * Width, Top + scale, 0),
                Side.Bottom => new Vector3(Left + rand * Width, Bottom - scale, 0),
                Side.Right => new Vector3(Right + scale, Bottom + rand * Height, 0),
                Side.Left => new Vector3(Left - scale, Bottom + rand * Height, 0),
                Side.Count => throw new InvalidEnumArgumentException(),
                _ => throw new NotSupportedException()
            };
        }

        private enum Side
        {
            Top,
            Bottom,
            Left,
            Right,
            Count
        }
    }
}