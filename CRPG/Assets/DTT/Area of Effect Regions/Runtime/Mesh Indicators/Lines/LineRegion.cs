using UnityEngine;

namespace DTT.AreaOfEffectRegions
{
    /// <summary>
    /// An implementation of <see cref="LineRegionBase"/> with added textures.
    /// </summary>
    [ExecuteAlways]
    public class LineRegion : LineRegionBase
    {
        /// <summary>
        /// The body of the line.
        /// </summary>
        [SerializeField]
        private MeshRenderer _bodyRenderer;
        
        /// <summary>
        /// The end point, head, of the line.
        /// </summary>
        [SerializeField]
        private MeshRenderer _headRenderer;
        
        /// <summary>
        /// The body of the line.
        /// </summary>
        [SerializeField]
        private Transform _bodyTransform;
        
        /// <summary>
        /// The end point, head, of the line.
        /// </summary>
        [SerializeField]
        private Transform _headTransform;
        
        /// <summary>
        /// The shader ID of the _FillProgress property.
        /// </summary>
        private static readonly int ProgressShaderID = Shader.PropertyToID("_FillProgress");

        /// <summary>
        /// Updates the position of the head and the body according to the line data.
        /// </summary>
        private void Update()
        {
            if (_bodyTransform == null || _headTransform == null)
                return;

            float bodyPart = _bodyTransform.localScale.z / Length;
            _bodyRenderer.sharedMaterial.SetFloat(ProgressShaderID, Mathf.InverseLerp(0, bodyPart, FillProgress));
            _headRenderer.sharedMaterial.SetFloat(ProgressShaderID, Mathf.InverseLerp(bodyPart, 1, FillProgress));
        }
    }
}