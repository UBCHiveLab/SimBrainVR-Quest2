using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedEnable : MonoBehaviour
{

    public Behaviour tobeEnabled;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5.0f);
        tobeEnabled.enabled = true;
    }

    
}
