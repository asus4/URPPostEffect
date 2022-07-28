namespace URPPostEffect
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    /// <summary>
    /// Modified from
    /// https://github.com/keijiro/SimplePostEffects/blob/main/LICENSE
    /// The Unlicense LICENSE
    /// </summary>
    sealed class PostEffectPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData data)
        {
            Camera camera = data.cameraData.camera;
            if (!camera.TryGetComponent(out URPPostEffect effect))
            {
                return;
            }
            var effects = effect.Effects;
            if (effects == null)
            {
                return;
            }

            foreach (var effectData in effect.Effects)
            {
                if (!effectData.IsActive) continue;

                var cmd = CommandBufferPool.Get("URP Post Effect");
                switch (effectData.mode)
                {
                    case PostEffectMode.Blit:
                        Blit(cmd, ref data, effectData.material, 0);
                        break;
                    case PostEffectMode.Custom:
                        effectData.customEvent.Invoke(cmd);
                        break;
                    default:
                        Debug.LogError($"Unknown mode: {effectData.mode}");
                        break;
                }
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
        }
    }

    public sealed class URPPostEffectFeature : ScriptableRendererFeature
    {
#pragma warning disable IDE0044
        [SerializeField]
        private RenderPassEvent _renderPassEvent = RenderPassEvent.AfterRendering;
#pragma warning restore IDE0044

        private PostEffectPass _pass;

        public override void Create()
        {
            _pass = new PostEffectPass()
            {
                renderPassEvent = _renderPassEvent,
            };
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData data)
        {
            // Apply post-effects only in scene cameras.
            var cameraType = data.cameraData.cameraType;
            if (cameraType == CameraType.Game || cameraType == CameraType.VR)
            {
                renderer.EnqueuePass(_pass);
            }
        }
    }

}
