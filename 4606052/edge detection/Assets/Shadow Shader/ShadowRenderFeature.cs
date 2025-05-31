using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable]
public class ShadowRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader m_compositeShader;

    private Material m_compositeMaterial;
    private CustomRenderPass m_ScriptablePass;

    // Called when the renderer feature is created. Initializes the material and render pass.
    public override void Create()
    {
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);
        m_ScriptablePass = new CustomRenderPass(m_compositeMaterial);
    }

    // Cleans up resources when the feature is disposed.
    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_compositeMaterial);
    }

    // Configures input requirements and target handles for the custom render pass.
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_ScriptablePass.ConfigureInput(ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color);
            m_ScriptablePass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }

    // Enqueues the custom render pass to the renderer for game cameras.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }

    [Serializable]
    class CustomRenderPass : ScriptableRenderPass
    {
        private Material m_compositeMaterial;
        private RenderTextureDescriptor m_Descriptor;
        private RTHandle m_CameraColorTarget;
        private RTHandle m_CameraDepthTarget;

        private bool isReady = false;

        // Sets material and defines when the pass should run in the render pipeline.
        public CustomRenderPass(Material material)
        {
            m_compositeMaterial = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        // Stores camera color and depth targets for use during execution.
        public void SetTarget(RTHandle cameraColorTargetHandle, RTHandle cameraDepthTargetHandle)
        {
            m_CameraColorTarget = cameraColorTargetHandle;
            m_CameraDepthTarget = cameraDepthTargetHandle;
            isReady = !(m_CameraColorTarget == null || m_CameraDepthTarget == null);
        }

        // Called before the render pass executes. Captures the camera descriptor.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            m_Descriptor = renderingData.cameraData.cameraTargetDescriptor;
        }

        // Main render logic: copies the internal screen-space shadowmap to a custom global texture.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!isReady) return;

            CommandBuffer cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, new ProfilingSampler("Shadow Post Processing Effect")))
            {
                cmd.SetGlobalTexture("_ShadowScreenSpace", (RenderTexture)Shader.GetGlobalTexture("_ScreenSpaceShadowmapTexture"));
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }
    }
}