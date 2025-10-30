using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public abstract class Skill : MonoBehaviour
{
    [Header("Skill Info")]
    public string skillName = "New Skill"; //이름 나중에 생각
    public float cooldown = 2f; //쿨타임
    protected bool isRunning = false; //스킬 사용중인지

    protected virtual void Start()
    {
        StartCoroutine(AutoCastRoutine());
    }

    IEnumerator AutoCastRoutine()
    {
        isRunning = true;

        while (isRunning)
        {
            Activate();
            yield return new WaitForSeconds(cooldown);
        }
    }

    public abstract void Activate();
}
