using UnityEngine;

//[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class PostEffectBase : MonoBehaviour
{

    //Inspector面板上直接拖入  
    public Shader shader = null;
    private Material _material = null;
    public Material _Material {
        get {
            if (_material == null)
                _material = GenerateMaterial(shader);
            return _material;
        }
    }
    protected Camera _camera;
    protected Transform cameraTransform;

    private void Awake() {
        _camera = GetComponent<Camera>();
        cameraTransform = _camera.transform;

        _material = GenerateMaterial(shader);
    }


    //根据shader创建用于屏幕特效的材质  
    protected Material GenerateMaterial(Shader shader) {
        if (shader && shader.isSupported) {
            Material material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material) return material;
        }
        return null;
    }

}