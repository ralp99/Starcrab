using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterText : MonoBehaviour {

    public float Delay = 0.035f;
    [TextArea]
    public string FullText;
    public AudioClip AudioClip;
    public AudioSource audioSource;

    private string currentText = "";

    IEnumerator TypingText()
    {
        for (int i = 0; i < FullText.Length; i++)
        {
            currentText = FullText.Substring(0, i);
              this.GetComponent<TextMesh>().text = currentText;

            if (AudioClip != null)
            {
                audioSource.PlayOneShot(AudioClip);
            }

            yield return new WaitForSeconds(Delay);
        }
    }

    
    void Start () {
        audioSource.clip = AudioClip;
        StartCoroutine(TypingText());
	}

}
