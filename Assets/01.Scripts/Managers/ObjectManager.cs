using UnityEngine;

public class ObjectManager : MonoBehaviour 
{
    public static ObjectManager Instance { get; private set; }

    /*ObjectPool*/
    //=======================================//

    public ExpPool ExpPool { get; private set; }

    /*생명 주기*/
    //=======================================//

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        ExpPool = GetComponentInChildren<ExpPool>();
    }
}
