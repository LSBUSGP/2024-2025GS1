using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthNormalsFeature : ScriptableRendererFeature
{
    class RenderPass : ScriptableRenderPass
    {
        private Material material;
        private RTHandle destinationHandle;
        private List<ShaderTagId> shaderTags;
        private FilteringSettings filteringSettings;

        public RenderPass(Material material) : base()
        {
            this.material = material;
            this.shaderTags = new List<ShaderTagId>() {
                new ShaderTagId("DepthOnly"),
            };
            this.filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
            destinationHandle = RTHandles.Alloc("_DepthNormalsTexture", name: "_DepthNormalsTexture");
        }

        // Configure the pass by creating a temporary render texture and ready it for rendering
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(Shader.PropertyToID(destinationHandle.name), cameraTextureDescriptor, FilterMode.Point);
            ConfigureTarget(destinationHandle);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            // Create the draw settings
            var drawSettings = CreateDrawingSettings(shaderTags, ref renderingData, renderingData.cameraData.defaultOpaqueSortFlags);
            drawSettings.overrideMaterial = material;
            context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(destinationHandle.name));
        }
    }

    private RenderPass renderPass;

    public override void Create()
    {
        Material material = CoreUtils.CreateEngineMaterial("Hidden/Internal-DepthNormalsTexture");
        this.renderPass = new RenderPass(material);
        renderPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(renderPass);
    }
}