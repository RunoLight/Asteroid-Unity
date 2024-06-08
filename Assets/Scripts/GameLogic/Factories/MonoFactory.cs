using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace Asteroid.GameLogic.Factories
{
    public class MonoFactory<TValue> : IFactory<TValue> where TValue : MonoBehaviour
    {
        private readonly TValue prefab;
        private readonly Transform parent;
        private readonly bool enableByDefault;
    
        public MonoFactory(TValue prefab, bool enableByDefault, [AllowNull] Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.enableByDefault = enableByDefault;
        }

        public TValue Create()
        {
            var spawned = parent != null ? Object.Instantiate(prefab, parent) : Object.Instantiate(prefab);
            spawned.gameObject.SetActive(enableByDefault);
            return spawned;
        }
    }
}