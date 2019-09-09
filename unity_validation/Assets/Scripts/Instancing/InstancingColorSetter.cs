﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// @memo. Instancingシェーダー用のスクリプト

// @memo. Unityが実行モード出ない場合でもスクリプトを実行する為の記述
[ExecuteInEditMode]
public class InstancingColorSetter : MonoBehaviour
{
    Color color = default;
    new Renderer renderer = default;
    // @memo. 変数値を経由させる為のもの
    MaterialPropertyBlock props = default;

    // @memo. シェーダー側で定義した変数名に合わせる
    static readonly int id = Shader.PropertyToID("_Color");

    private void Start()
    {
        color = Random.ColorHSV();
        renderer = GetComponent<Renderer>();
        props = new MaterialPropertyBlock();
    }

    private void Update()
    {
        // @memo. ID取らずとも、直接変数名を指定しても良い模様
        props.SetColor(id, color);
        renderer.SetPropertyBlock(props);
    }
}
