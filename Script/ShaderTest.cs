// ShaderTest.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShaderTest : MonoBehaviour
{
    [Header("Target UI Elements")]
    public Image TargetImage;
    public Image EffectImage;
    public Sprite TargetSprite;

    [Header("Animation Settings")]
    public float AnimationDuration = 3f;

    [Header("Optional Parameters")]

    public bool IsAnimating = false;

    public enum ShaderType
    {
        BlinkEye, Circle, CurtainPull, Fade, FadeSlideGlobal, FadeSlideWave,
        GridComplex, GridTime, PolygonShrink, ClockWipe, LerpSlices, Sawtooth,
        Split, Wave
    }

    public ShaderType CurrentShader = ShaderType.Wave;

    private Dictionary<ShaderType, string> shaderMap;

    private void Awake()
    {
        shaderMap = new Dictionary<ShaderType, string>
        {
            { ShaderType.BlinkEye, TransitionShader.BlinkEyePath },
            { ShaderType.Circle, TransitionShader.CirclePath },
            { ShaderType.CurtainPull, TransitionShader.CurtainPullPath },
            { ShaderType.Fade, TransitionShader.FadePath },
            { ShaderType.FadeSlideGlobal, TransitionShader.FadeSlideGlobalPath },
            { ShaderType.FadeSlideWave, TransitionShader.FadeSlideWavePath },
            { ShaderType.GridComplex, TransitionShader.GridComplexPath },
            { ShaderType.GridTime, TransitionShader.GridTimePath },
            { ShaderType.PolygonShrink, TransitionShader.PolygonShrinkPath },
            { ShaderType.ClockWipe, TransitionShader.ClockWipePath },
            { ShaderType.LerpSlices, TransitionShader.LerpSlicesPath },
            { ShaderType.Sawtooth, TransitionShader.SawtoothPath },
            { ShaderType.Split, TransitionShader.SplitPath },
            { ShaderType.Wave, TransitionShader.WavePath }
        };
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsAnimating)PlayShaderAnimation();

        if (Input.GetKeyDown(KeyCode.A) && !IsAnimating)TestGridComplexWithShape(ShaderPropertyValue.Rectangular);
        if (Input.GetKeyDown(KeyCode.W) && !IsAnimating)TestGridComplexWithShape(ShaderPropertyValue.Doors);
        if (Input.GetKeyDown(KeyCode.S) && !IsAnimating)TestGridComplexWithShape(ShaderPropertyValue.Radius);

        if (Input.GetKeyDown(KeyCode.V) && !IsAnimating)ShaderHelper.AnimateZoom(TargetImage, 3f, null, 2f, () => Debug.Log("Zoom done"));
        if (Input.GetKeyDown(KeyCode.B) && !IsAnimating)ShaderHelper.AnimateZoom(TargetImage, 3f, new Vector2(0f, 0f), 2f, () => Debug.Log("Zoom done"));
        if (Input.GetKeyDown(KeyCode.N) && !IsAnimating)ShaderHelper.AnimateZoom(TargetImage, 3f, new Vector2(0.5f, 0.5f), 2f, () => Debug.Log("Zoom done"));
        if (Input.GetKeyDown(KeyCode.M) && !IsAnimating)ShaderHelper.AnimateZoom(TargetImage, 3f, new Vector2(1f, 1f), 2f, () => Debug.Log("Zoom done"));
        if (Input.GetKeyDown(KeyCode.L) && !IsAnimating)ShaderHelper.AnimatePan(TargetImage, new Vector2(0.2f, 0.3f), 2f, () => Debug.Log("Pan done"));

        if (Input.GetKeyDown(KeyCode.U))ShaderHelper.SetSpeedLine(EffectImage, SpeedLineType.AnimeSpeedLine);
        if (Input.GetKeyDown(KeyCode.I) )ShaderHelper.SetSpeedLine(EffectImage, SpeedLineType.SpeedLine1);
        if (Input.GetKeyDown(KeyCode.O) )ShaderHelper.SetSpeedLine(EffectImage, SpeedLineType.SpeedLine2);
        if (Input.GetKeyDown(KeyCode.P))ShaderHelper.SetCurtainPull(EffectImage);
        if (Input.GetKeyDown(KeyCode.R)) ShaderHelper.ResetEffectImage(EffectImage);


    }

    private void PlayShaderAnimation()
    {
        Sprite originalSprite = TargetImage.sprite;
        if (!shaderMap.TryGetValue(CurrentShader, out string shaderPath))
        {
            Debug.LogWarning("Shader not found: " + CurrentShader);
            return;
        }

        IsAnimating = true;

        var request = new ShaderAnimationRequest(
            shaderName: shaderPath,
            targetImage: TargetImage,
            targetSprite: TargetSprite,
            duration: AnimationDuration//,
            //customParams: new Dictionary<string, object>
            //{
            //{ ShaderProperty.Pivot, new Vector4(0, 0, 0, 0) }
            //}
        );

        ShaderHelper.TransitionAnimation(request);

        DOVirtual.DelayedCall(AnimationDuration + 1f, () =>
        {
            IsAnimating = false;
            Debug.Log("Transition Done, Restore to original sprite");
            TargetImage.sprite = originalSprite;
        });
    }



    public void TestGridComplexWithShape(float shape)
    {
        Sprite originalSprite = TargetImage.sprite;
        if (TargetImage == null || TargetSprite == null)
        {
            Debug.LogWarning("TargetImage or TargetSprite is null.");
            return;
        }

        if (!shaderMap.TryGetValue(ShaderType.GridComplex, out string shaderPath))
        {
            Debug.LogWarning("GridComplex shader path not found.");
            return;
        }

        IsAnimating = true;

        var request = new ShaderAnimationRequest(
            shaderName: shaderPath,
            targetImage: TargetImage,
            targetSprite: TargetSprite,
            duration: AnimationDuration,
            customParams: new Dictionary<string, object>
            {
                { ShaderProperty.Shape, (float)shape }  
            }
        );

        ShaderHelper.TransitionAnimation(request);

        DOVirtual.DelayedCall(AnimationDuration, () =>
        {
            IsAnimating = false;
            Debug.Log("GridComplex transition done with shape: " + shape);
            TargetImage.sprite = originalSprite;

        });
    }

}
