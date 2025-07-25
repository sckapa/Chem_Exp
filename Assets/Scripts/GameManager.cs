using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject testTube;
    [SerializeField]
    private GameObject testTubeLiquid;

    [SerializeField]
    private GameObject flask1;
    [SerializeField]
    private GameObject flask2;
    [SerializeField]
    private GameObject flaskLiquid1;
    [SerializeField]
    private GameObject flaskLiquid2;

    [SerializeField]
    private Transform pourPosition;
    [SerializeField]
    private Transform flaskPosition;

    [SerializeField] 
    private Color targetColor = new Color(0.8f, 0.6f, 1.0f); 
    [SerializeField]
    private float duration = 3f;

    [SerializeField]
    private Animator animator;

    private Vector3 testTubeInitPos;
    private Quaternion testTubeInitRot;

    private Vector3 FlaskInitPos;

    private void Start()
    {
        StartCoroutine(MoveTestTubeToPourPosition());
        StartCoroutine(MoveFlaskToPourPosition(2));
    }

    private void OnApplicationQuit()
    {
        testTubeLiquid.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.4f);

        flaskLiquid1.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.5f);
        flaskLiquid1.GetComponent<Renderer>().sharedMaterial.SetColor("_Tint", new Color(0.75f, 0.75f, 0.75f, 1f));

        flaskLiquid2.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.5f);
        flaskLiquid2.GetComponent<Renderer>().sharedMaterial.SetColor("_Tint", new Color(0.75f, 0.75f, 0.75f, 1f));
    }

    private IEnumerator MoveTestTubeToPourPosition()
    {
        float elapsed = 0;
        testTubeInitPos = testTube.transform.position;
        testTubeInitRot = testTube.transform.rotation;

        while (elapsed < duration)
        {
            testTube.transform.position = Vector3.Lerp(testTubeInitPos, pourPosition.position, elapsed / duration);
            testTube.transform.rotation = Quaternion.Lerp(testTubeInitRot, pourPosition.rotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSecondsRealtime(0.5f);

        StartCoroutine(Pour());
    }

    private IEnumerator MoveFlaskToPourPosition(int flaskNumber)
    {
        GameObject flask = null;
        if (flaskNumber == 1)
        {
            flask = flask1;
        }
        else if (flaskNumber == 2)
        {
            flask = flask2;
        }
        else
        {
            Debug.Log("Wrong parameter!");
        }

        float elapsed = 0;
        FlaskInitPos = flask.transform.position;

        while (elapsed < duration)
        {
            flask.transform.position = Vector3.Lerp(FlaskInitPos, flaskPosition.position, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(FillFlask(flaskNumber));
    }

    private IEnumerator MoveTestTubeToInitPosition()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            testTube.transform.position = Vector3.Lerp(pourPosition.position, testTubeInitPos, elapsed / duration);
            testTube.transform.rotation = Quaternion.Lerp(pourPosition.rotation, testTubeInitRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator MoveFlaskToInitPosition(int flaskNumber)
    {
        GameObject flask = null;
        if (flaskNumber == 1)
        {
            flask = flask1;
        }
        else if (flaskNumber == 2)
        {
            flask = flask2;
        }
        else
        {
            Debug.Log("Wrong parameter!");
        }

        float elapsed = 0;

        while (elapsed < duration)
        {
            flask.transform.position = Vector3.Lerp(flaskPosition.position, FlaskInitPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Pour()
    {
        float lerpTimer = 0;
        float lerpDuration = 3; 

        while (lerpTimer < lerpDuration)
        {
            float t = lerpTimer / lerpDuration;
            float currentValue = Mathf.Lerp(0.4f, 0.5f, t);
            testTubeLiquid.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", currentValue);
            lerpTimer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FillFlask(int flaskNumber)
    {
        GameObject flaskLiquid = null;
        if (flaskNumber == 1)
        {
            flaskLiquid = flaskLiquid1;
        }
        else if (flaskNumber == 2)
        {
            flaskLiquid = flaskLiquid2;
        }
        else
        {
            Debug.Log("Wrong parameter!");
        }

        float lerpTimer = 0;
        float lerpDuration = 3;

        while (lerpTimer < lerpDuration)
        {
            float t = lerpTimer / lerpDuration;
            float currentValue = Mathf.Lerp(0.5f, 0.4f, t);
            flaskLiquid.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", currentValue);
            lerpTimer += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ChangeFlaskLiquidColor(flaskNumber));
    }

    private IEnumerator ChangeFlaskLiquidColor(int flaskNumber)
    {
        GameObject flaskLiquid = null;
        if (flaskNumber == 1)
        {
            flaskLiquid = flaskLiquid1;
        }
        else if (flaskNumber == 2)
        {
            flaskLiquid = flaskLiquid2;
        }
        else
        {
            Debug.Log("Wrong parameter!");
        }

        float elapsedTime = 0f;
        Color col = new Color(0.75f, 0.75f, 0.75f, 1f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            flaskLiquid.GetComponent<Renderer>().sharedMaterial.SetColor("_Tint", Color.Lerp(col, targetColor, t));
            yield return null;
        }

        SetAnimTrigger();
        StartCoroutine(MoveFlaskToInitPosition(2));
        StartCoroutine(MoveTestTubeToInitPosition());
    }

    private void SetAnimTrigger()
    {
        animator.SetTrigger("Excited");
    }

}
