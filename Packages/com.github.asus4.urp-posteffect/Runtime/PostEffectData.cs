namespace URPPostEffect
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Rendering;

    internal enum PostEffectMode
    {
        Blit,
        Custom,
    }

    [System.Serializable]
    internal sealed class CommandBufferEvent : UnityEvent<CommandBuffer> { }

    [System.Serializable]
    internal sealed class PostEffectData
    {
        public bool enabled = true;
        public PostEffectMode mode = PostEffectMode.Blit;

        // Mode.Blit
        public Material material = null;

        // Mode.Custom
        public CommandBufferEvent customEvent = null;

        public bool IsActive
        {
            get
            {
                if (!enabled) return false;
                return mode switch
                {
                    PostEffectMode.Blit => material != null,
                    PostEffectMode.Custom => customEvent != null,
                    _ => throw new System.NotImplementedException($"Unknown mode: {mode}"),
                };
            }
        }
    }
}
