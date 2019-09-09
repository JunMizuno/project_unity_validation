using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @memo. CameraのAllow Dynamic Resolutionを有効にする
/// @memo. RenderTextureのDynamic Scallingを有効にする
/// @memo. PlayerSettings -> Other Settings -> Enable Frame Timingを有効にする
/// </summary>
public class ControllRisizeBuffers : MonoBehaviour
{
    private void Start()
    {
        // @memo. この引数値で解像度の度合いを設定
        ScalableBufferManager.ResizeBuffers(0.75f, 0.75f);
    }
}
