using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject testTube;
    [SerializeField]
    private Transform pourPosition;
    [SerializeField]
    private float pourDuration = 1f;

    [SerializeField]
    private GameObject testTubeShader;

    private void Start()
    {
        StartCoroutine(MoveTestTubeToPourPosition());
    }

    IEnumerator MoveTestTubeToPourPosition()
    {
        float elapsed = 0;
        Vector3 startPos = testTube.transform.position;
        Quaternion startRot = testTube.transform.rotation;

        while (elapsed < pourDuration)
        {
            testTube.transform.position = Vector3.Lerp(startPos, pourPosition.position, elapsed / pourDuration);
            testTube.transform.rotation = Quaternion.Lerp(startRot, pourPosition.rotation, elapsed / pourDuration);
            testTubeShader.GetComponent<Renderer>().sharedMaterial.SetFloat("_FillAmount", Mathf.Min(elapsed / pourDuration, 0.6f));

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}