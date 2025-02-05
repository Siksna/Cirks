using System.Collections;
using UnityEngine;

public class SetActiveScript : MonoBehaviour
{
    public GameObject gameObject2;
    public GameObject gameObject3;

    public void ToggleGameObject2AfterDelay(float delay)
    {
        StartCoroutine(ToggleGameObject2Coroutine(delay));
    }

    public void ToggleGameObject3AfterDelay(float delay)
    {
        StartCoroutine(ToggleGameObject3Coroutine(delay));
    }

    private IEnumerator ToggleGameObject2Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject2 != null)
            gameObject2.SetActive(!gameObject2.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
    }

    private IEnumerator ToggleGameObject3Coroutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gameObject3 != null)
            gameObject3.SetActive(!gameObject3.activeSelf);
            gameObject.SetActive(!gameObject.activeSelf);
    }
}
