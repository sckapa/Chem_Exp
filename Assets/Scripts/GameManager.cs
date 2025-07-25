using System;
using System.Collections;
using System.Security.Cryptography;
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
    private Color targetColor;
    [SerializeField]
    private float duration = 3f;

    [SerializeField]
    private Animator animator;

    private Vector3 testTubeInitPos;
    private Quaternion testTubeInitRot;

    private Vector3 FlaskInitPos;

    private bool flask1Filled = false;
    private bool flask2Filled = false;
    private bool filling = false;
    private bool testTubeMoved = false;

    private void OnApplicationQuit()
    {
        testTubeLiquid.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.4f);

        flaskLiquid1.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.5f);
        flaskLiquid1.GetComponent<Renderer>().sharedMaterial.SetColor("_Tint", new Color(0.75f, 0.75f, 0.75f, 1f));

        flaskLiquid2.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", 0.5f);
        flaskLiquid2.GetComponent<Renderer>().sharedMaterial.SetColor("_Tint", new Color(0.75f, 0.75f, 0.75f, 1f));
    }

    private void Update()
    {
        if(filling)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == testTube && (!flask1Filled || !flask2Filled))
                {
                    testTubeMoved = true;
                    StartCoroutine(MoveTestTubeToPourPosition());
                }
                else if (testTubeMoved)
                {
                    if (hit.collider.gameObject == flask1 && !flask1Filled)
                    {
                        flask1Filled = true;
                        StartCoroutine(MoveFlaskToPourPosition(1));
                    }
                    else if (hit.collider.gameObject == flask2 && !flask2Filled)
                    {
                        flask2Filled = true;
                        StartCoroutine(MoveFlaskToPourPosition(2));
                    }
                }
            }
        }
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
    }

    private IEnumerator MoveFlaskToPourPosition(int flaskNumber)
    {
        filling = true;
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

        StartCoroutine(Pour());
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

        testTubeMoved = false;
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

        filling = false;
    }

    private IEnumerator Pour()
    {
        float lerpTimer = 0;
        float lerpDuration = 3;
        float fill = testTubeLiquid.GetComponent<Renderer>().sharedMaterial.GetFloat("_FillAmount");

        while (lerpTimer < lerpDuration)
        {
            float t = lerpTimer / lerpDuration;
            float currentValue = Mathf.Lerp(fill, fill + 0.1f, t);
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
            targetColor = new Color(0.8f, 0.6f, 1.0f);
            flaskLiquid = flaskLiquid1;
        }
        else if (flaskNumber == 2)
        {
            targetColor = new Color(0.2f, 0.8f, 0.3f);
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

        StartCoroutine(SetAnimTrigger(flaskNumber));
        StartCoroutine(MoveFlaskToInitPosition(flaskNumber));
        StartCoroutine(MoveTestTubeToInitPosition());
    }

    private IEnumerator SetAnimTrigger(int flaskNumber)
    {
        if (flaskNumber == 1)
        {
            animator.SetTrigger("Excited");

        }
        else if(flaskNumber == 2)
        {
            animator.SetTrigger("Disgusted");
        }
        yield return new WaitForSeconds(3);

        animator.SetTrigger("Idle");
    }
}
