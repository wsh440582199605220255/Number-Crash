using UnityEngine;

public class WaterWave_Single : PostEffectBase
{
    //距离系数  
    public float distanceFactor = 60.0f;
    //时间系数  
    public float timeFactor = -30.0f;
    //sin函数结果系数  
    public float totalFactor = 1.0f;
    //波纹宽度  
    public float waveWidth = 0.3f;
    //波纹扩散的速度  
    public float waveSpeed = 0.3f;

    private float waveStartTime;
    private Vector2 startPos = new Vector2(0.5f, 0.5f);

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        //计算波纹移动的距离，根据enable到目前的时间*速度求解  
        float curWaveDistance = (Time.time - waveStartTime) * waveSpeed;
        //设置一系列参数  
        _Material.SetFloat("_distanceFactor", distanceFactor);
        _Material.SetFloat("_timeFactor", timeFactor);
        _Material.SetFloat("_totalFactor", totalFactor);
        _Material.SetFloat("_waveWidth", waveWidth);
        _Material.SetFloat("_curWaveDis", curWaveDistance);
        _Material.SetVector("_startPos", startPos);
        Graphics.Blit(source, destination, _Material);
    }

    public void CreateWave(Vector3 inputPos) {
        //将mousePos转化为（0，1）区间  
        startPos = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);
        waveStartTime = Time.time;

        this.enabled = true;
    }
}