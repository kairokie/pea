using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerFeedback : MonoBehaviour
{
    public Text precisionText;

    // Start is called before the first frame update
    void Start()
    {
        precisionText.text = " ";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NoteHit(string test)
    {
        StartCoroutine(NoteHitCoroutine(test));
    }

    public IEnumerator NoteHitCoroutine(string precision)
    {
        precisionText.text = precision;
        yield return new WaitForSeconds(0.3f);
        precisionText.text = " ";
    }
}
