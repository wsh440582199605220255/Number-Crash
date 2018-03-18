using System;
public abstract class Singleton<T> where T : class, new()
{
	private static T m_instance;
	public static T instance
	{
		get
        {
            if (m_instance == null)
		    {
                m_instance = Activator.CreateInstance<T>();
			    if (m_instance != null)
			    {
                    (m_instance as Singleton<T>).Init();
			    }
		    }
            return m_instance;
        }
	}

    /*
     * 没有任何实现的函数，用于保证MonoSingleton在使用前已创建
     */
    public void Startup()
    {

    }


    public static void Release()
	{
		if (m_instance != null)
		{
            m_instance = (T)((object)null);
		}
	}

    public virtual void Init()
    {

    }

    public abstract void Dispose();

}
