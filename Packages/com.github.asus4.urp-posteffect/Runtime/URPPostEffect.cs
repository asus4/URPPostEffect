namespace URPPostEffect
{
    using UnityEngine;

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Camera))]
    public sealed class URPPostEffect : MonoBehaviour
    {
        [SerializeField]
        private PostEffectData[] _effects;

        internal PostEffectData[] Effects => _effects;
    }
}
