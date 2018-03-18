using UnityEngine;
using System.Threading;

/// <summary>
/// ˵������TasharenFogOfWar�����ֲ������NGUI���߷���
///       ��ԭģ������˾�����Ż��������У�
///     1��ȥ���߶�ͼHeightMap
///     2��ֻ����ʹ��radiusˢͼ�ķ�ʽ������ˢͼ��ʽһ���Ƴ�
///     3��Shader������Ϊ����ģ�ͷ�ʽ
///     4��Shader������ͼ��ԭ���������Ż���һ��
///     5�����Ӷ������ѡ�ϵͳ���ء���Ⱦ���ء��������ص�
///     
/// @by xiao_D 2017-12-14
/// </summary>
[DefaultExecutionOrder(-100)]
public class FOWSystem : MonoSingleton<FOWSystem>
{
    public enum State
	{
		Blending,
		NeedUpdate,
		UpdateTexture,
	}
    
	protected Transform mTrans;
	protected Vector3 mOrigin = Vector3.zero;
	protected Vector3 mSize = Vector3.one;

	// Revealers that the thread is currently working with
	static BetterList<IFOWRevealer> mRevealers = new BetterList<IFOWRevealer>();

	// Revealers that have been added since last update
	static BetterList<IFOWRevealer> mAdded = new BetterList<IFOWRevealer>();

	// Revealers that have been removed since last update
	static BetterList<IFOWRevealer> mRemoved = new BetterList<IFOWRevealer>();

	// Color buffers -- prepared on the worker thread.
	protected Color32[] mBuffer0;
	protected Color32[] mBuffer1;
	protected Color32[] mBuffer2;

	// textures -- we'll be blending in the shader
	protected Texture2D mTexture;

	// Whether some color buffer is ready to be uploaded to VRAM
	protected float mBlendFactor = 0f;
	protected float mNextUpdate = 0f;
	protected State mState = State.Blending;

	Thread mThread;
    volatile bool mThreadWork;
    protected int mTextureSizeSqr;
	public float worldSize = 512f;
	public int textureSize = 512;
	public float updateFrequency = 0.2f;
	public float textureBlendTime = 0.5f;
	public int blurIterations = 2;
    public float worldToTex;

    /// <summary>
    /// How many offset radius will be adjust. 
    /// </summary>

    public float radiusOffset = 0f;
    public bool enableSystem = true;
    public bool enableRender = true;
    public bool enableFog = true;
    /// <summary>
    /// If debugging is enabled, the time it takes to calculate the fog of war will be shown in the log window.
    /// </summary>
    public bool enableDebug = false;

	public Texture2D Texture { get { return mTexture; } }  
	public float BlendFactor { get { return mBlendFactor; } }
    public void RenderImmediately()
    {
        UpdateBuffer();
        mState = State.UpdateTexture;
        UpdateTexture();
        mBlendFactor = 1.0f;
    }
    static public void AddRevealer (IFOWRevealer rev)
	{
        if (rev != null) lock (mAdded) mAdded.Add(rev);
	}
	static public void RemoveRevealer (IFOWRevealer rev)
    {
        if (rev != null) lock (mRemoved) mRemoved.Add(rev);             
    }
    protected override void Init()
	{
        worldToTex = textureSize / worldSize;
       
		mOrigin = transform.position;
        mOrigin.x -= worldSize * 0.5f;
        mOrigin.z -= worldSize * 0.5f;

        mTextureSizeSqr = textureSize * textureSize;
		mBuffer0 = new Color32[mTextureSizeSqr];
		mBuffer1 = new Color32[mTextureSizeSqr];
		mBuffer2 = new Color32[mTextureSizeSqr];
        mRevealers.Clear();
        mRemoved.Clear();
        mAdded.Clear();
        
        // Add a thread update function -- all visibility checks will be done on a separate thread
        mThread = new Thread(ThreadUpdate);
        mThreadWork = true;
        mThread.Start();

        GetComponentInChildren<Projector>(true).gameObject.SetActive(true);
    }

    /// <summary>
    /// Ensure that the thread gets terminated.
    /// </summary>
    public override void Dispose()
    {
        if (mThread != null)
        {
            mThreadWork = false;
            mThread.Join();
            mThread = null;
        }

        mBuffer0 = null;
        mBuffer1 = null;
        mBuffer2 = null;

        if (mTexture != null)
        {
            Destroy(mTexture);
            mTexture = null;
        }
    }

	void Update ()
	{
        if (!enableSystem) return;
		if (textureBlendTime > 0f) mBlendFactor = Mathf.Clamp01(mBlendFactor + Time.deltaTime / textureBlendTime);
		else mBlendFactor = 1f;

		if (mState == State.Blending)
		{
			float time = Time.time;

			if (mNextUpdate < time)
			{
				mNextUpdate = time + updateFrequency;
				mState = State.NeedUpdate;
			}
		}
		else if (mState != State.NeedUpdate) UpdateTexture();
	}

	float mElapsed = 0f;
    public float elapsed{get{return mElapsed;}}
                              
    void ThreadUpdate()
    {
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        while (mThreadWork)
        {
            if (mState == State.NeedUpdate)
            {
                sw.Reset();
                sw.Start();
                UpdateBuffer();
                sw.Stop();
                mElapsed = 0.001f * sw.ElapsedMilliseconds;
                mState = State.UpdateTexture;
            }
            Thread.Sleep(1);
        }
#if UNITY_EDITOR
        Debug.Log("FOW thread exit!");
#endif
    }

	void OnDrawGizmosSelected ()
	{
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(new Vector3(0f, 0f, 0f), new Vector3(worldSize, 0f, worldSize));
	}

	void UpdateBuffer ()
	{
		// Add all items scheduled to be added
		if (mAdded.size > 0)
		{
			lock (mAdded)
			{
				while (mAdded.size > 0)
				{
					int index = mAdded.size - 1;
					mRevealers.Add(mAdded.buffer[index]);
					mAdded.RemoveAt(index);
				}
			}
		}

		// Remove all items scheduled for removal
		if (mRemoved.size > 0)
		{
			lock (mRemoved)
			{
				while (mRemoved.size > 0)
				{
					int index = mRemoved.size - 1;
					mRevealers.Remove(mRemoved.buffer[index]);
					mRemoved.RemoveAt(index);
				}
			}
		}

		// Use the texture blend time, thus estimating the time this update will finish
		// Doing so helps avoid visible changes in blending caused by the blended result being X milliseconds behind.
		float factor = (textureBlendTime > 0f) ? Mathf.Clamp01(mBlendFactor + mElapsed / textureBlendTime) : 1f;

		// Clear the buffer's red channel (channel used for current visibility -- it's updated right after)
		for (int i = 0, imax = mBuffer0.Length; i < imax; i++)
		{
			mBuffer0[i] = Color32.Lerp(mBuffer0[i], mBuffer1[i], factor);
			mBuffer1[i].r = 0;
		}

		// Update the visibility buffer, one revealer at a time
		for (int i = 0; i < mRevealers.size; i++){
            if (mRevealers[i].IsValid()) RevealUsingRadius(mRevealers[i], worldToTex);
        }

		// Blur the final visibility data
		for (int i = 0; i < blurIterations; i++) BlurVisibility();

		// Reveal the map based on what's currently visible
		RevealMap();
        // Merge two buffer to one
        MergeBuffer();
    }

	/// <summary>
	/// The fastest form of visibility updates -- radius-based, no line of sights checks.
	/// </summary>

	void RevealUsingRadius (IFOWRevealer r, float worldToTex)
	{
        // Position relative to the fog of war
        Vector3 pos = (r.GetPosition() - mOrigin) * worldToTex;
        float radius = r.GetRadius() * worldToTex - radiusOffset;

        // Coordinates we'll be dealing with
        int xmin = Mathf.RoundToInt(pos.x - radius);
		int ymin = Mathf.RoundToInt(pos.z - radius);
		int xmax = Mathf.RoundToInt(pos.x + radius);
		int ymax = Mathf.RoundToInt(pos.z + radius);

		int cx = Mathf.RoundToInt(pos.x);
		int cy = Mathf.RoundToInt(pos.z);

		cx = Mathf.Clamp(cx, 0, textureSize - 1);
		cy = Mathf.Clamp(cy, 0, textureSize - 1);

		int radiusSqr = Mathf.RoundToInt(radius * radius);

		for (int y = ymin; y < ymax; y++)
		{
			if (y > -1 && y < textureSize)
			{
				int yw = y * textureSize;

				for (int x = xmin; x < xmax; ++x)
				{
					if (x > -1 && x < textureSize)
					{
						int xd = x - cx;
						int yd = y - cy;
						int dist = xd * xd + yd * yd;

						// Reveal this pixel
						if (dist < radiusSqr) mBuffer1[x + yw].r = 255;
					}
				}
			}
		}
	}

	void BlurVisibility ()
    {
        Color32 c;

        for (int y = 0; y < textureSize; ++y)
        {
            int yw = y * textureSize;
            int yw0 = (y - 1);
            if (yw0 < 0) yw0 = 0;
            int yw1 = (y + 1);
            if (yw1 == textureSize) yw1 = y;

            yw0 *= textureSize;
            yw1 *= textureSize;

            for (int x = 0; x < textureSize; ++x)
            {
                int x0 = (x - 1);
                if (x0 < 0) x0 = 0;
                int x1 = (x + 1);
                if (x1 == textureSize) x1 = x;

                int index = x + yw;
                int val = mBuffer1[index].r;

                val += mBuffer1[x0 + yw].r;
                val += mBuffer1[x1 + yw].r;
                val += mBuffer1[x + yw0].r;
                val += mBuffer1[x + yw1].r;

                val += mBuffer1[x0 + yw0].r;
                val += mBuffer1[x1 + yw0].r;
                val += mBuffer1[x0 + yw1].r;
                val += mBuffer1[x1 + yw1].r;

                c = mBuffer2[index];
                c.r = (byte)(val / 9);
                mBuffer2[index] = c;
            }
        }

        // Swap the buffer so that the blurred one is used
        Color32[] temp = mBuffer1;
        mBuffer1 = mBuffer2;
        mBuffer2 = temp;
    }

	/// <summary>
	/// Reveal the map by updating the green channel to be the maximum of the red channel.
	/// </summary>

	void RevealMap ()
	{
		for (int index = 0; index < mTextureSizeSqr; ++index)
        {
            if (mBuffer1[index].g < mBuffer1[index].r)
            {
                mBuffer1[index].g = mBuffer1[index].r;
            }
        }
	}
    void MergeBuffer()
    {
        for (int index = 0; index < mTextureSizeSqr; index++)
        {
            mBuffer0[index].b = mBuffer1[index].r;
            mBuffer0[index].a = mBuffer1[index].g;
        }
    }
    void UpdateTexture ()
	{
        if (!enableRender) return;
      
		if (mTexture == null)
        {
            // Native ARGB format is the fastest as it involves no data conversion
            mTexture = new Texture2D(textureSize, textureSize, TextureFormat.ARGB32, false);

			mTexture.wrapMode = TextureWrapMode.Clamp;

            mTexture.SetPixels32(mBuffer0);
            mTexture.Apply();
			mState = State.Blending;
		}
		else if (mState == State.UpdateTexture)
		{
			mTexture.SetPixels32(mBuffer0);
			mTexture.Apply();
			mBlendFactor = 0f;
            mState = State.Blending;
        }
	}

    public bool IsVisible(Vector3 pos)
    {
        // ˵����
        // ԭ�ж�ʽΪ��mBuffer0[index].r > 0 || mBuffer1[index].r > 0, ===> ȱ���ǽ�ɫ���˵�̫����ԭ������Ϸ�趨�������̫������
        // ֮���޸�Ϊ��Blend(mBuffer0[index], mBuffer1[index]).r > 0, ===> ������ֹ���Ķ����ͷ�����ȫͼ�ҷɵ�����
        // ���ȷ��Ϊ��mBuffer0[index].r > value1 || mBuffer1[index].r > value2, ===> ����value�����ɵ���ǰ��һ�����������ӳ٣���һ�����������ӳ�
        //
        // �ڶ��ַ�ʽ����Bug��ԭ���ǣ�mBuffer0.rg���ǽ���ģ���mBuffer1.rg�ڼ�������л�������䣨��351�У�
        // ������µ��ж�Blend.r(���ֵ) > value��valueΪ��0ֵʱ����IsVisible�жϽ�����ܻ��������
        // ��������˵��������������3֡�ڣ����������ɼ��Ľ�ɫ�ᱻ�ж�Ϊ�������������󣺿ɼ�--->���ɼ�--->�ɼ�
        // �����������Ϸ��ɵ���ʷbug�У�
        // 1������Ѫ������---����ȫ�̿ɼ���Ѫ����������3֡�ڣ��м�֡�жϲ��ɼ�����һ�λ���ػ���
        // 2��������ƾ����ʧһ֡---ͬ�����м�֡����Ϊ���ɼ����·�����ͻȻ����һ֡��Ī������
        // 
        // ע�⣺===>******������Ҫ��
        // 1���ϸ���˵����ɫ�ܶ�ʱ��Ȼ���ܵ����߼��ϵĿɼ�������===>��һ����������3֡�ڣ�����֡���ܶ�ʱ�Ӿ��Ͼͻᶶ����
        // 2������Ϊ�˷�ֹ���ֲ㶶��������Ĵ�����ʽ�����֣�
        //    A��������߼�����Ʊ��֣��򵱽�ɫ�ɼ��Ժ��趨һ���߼��ӳ٣���1�룩�������ʱ����ǿ�б��ֽ�ɫ�Ŀɼ���
        //    B�����ֲ�Ŀɼ������߼����룬ʹ�ñ������жϿɼ��ԣ������ʵ���������˵��������valueֵ�������͡���������===>******�Ƽ����򵥱���
        //
        // by wsh @ 2017-07-29
        if (mBuffer0 == null || mBuffer1 == null)
        {
            return false;
        }

        pos -= mOrigin;
        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);
        int index = cx + cy * textureSize;
        return mBuffer0[index].r > 64 || mBuffer1[index].r > 0;
    }

    public bool IsExplored(Vector3 pos)
    {
        if (mBuffer0 == null) return false;
        pos -= mOrigin;

        int cx = Mathf.RoundToInt(pos.x * worldToTex);
        int cy = Mathf.RoundToInt(pos.z * worldToTex);

        cx = Mathf.Clamp(cx, 0, textureSize - 1);
        cy = Mathf.Clamp(cy, 0, textureSize - 1);
        return mBuffer0[cx + cy * textureSize].g > 0;
    }
}