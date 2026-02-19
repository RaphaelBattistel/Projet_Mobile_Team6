using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ColorBlindnessSelector : MonoBehaviour
{
    [SerializeField] private UniversalRenderPipelineAsset _renderPipelineAsset;
    private Material _colorBlindnessMaterial;

    private string[] _keywords =
    {
        "_COLORBLINDTYPE_NONE", "_COLORBLINDTYPE_TRITANOPIA", "_COLORBLINDTYPE_DEUTERANOPIA",
        "_COLORBLINDTYPE_PROTANOPIA"
    };

    private void Start()
    {
        ScriptableRendererData data = _renderPipelineAsset.rendererDataList[0];
        data.TryGetRendererFeature(out FullScreenPassRendererFeature feature);
        _colorBlindnessMaterial = feature.passMaterial;
    }

    public void SwitchSetting(int settingId)
    {
        int n = Mathf.RoundToInt(_colorBlindnessMaterial.GetFloat("_COLORBLINDTYPE"));
        _colorBlindnessMaterial.DisableKeyword(_keywords[n]);
        _colorBlindnessMaterial.SetFloat("_COLORBLINDTYPE", settingId);
        _colorBlindnessMaterial.EnableKeyword(_keywords[settingId]);
    }
}