using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player,particle;
    Coroutine shake_inq = null;
    Vector3 originalpos;
    public PlayerParty pp;

    // Start is called before the first frame update
    void Start()
    {
    }

    IEnumerator LightShake(float duration, float magnitude)
    {
        float elapsed = 0.0f;
        originalpos = transform.localPosition;

        while (elapsed < duration)
        {
            //float x = Random.Range(-1f,1f) * magnitude;
            float y = Random.Range(-1f,1f) * magnitude;

            transform.localPosition = new Vector3(originalpos.x,y,originalpos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalpos;
        shake_inq = null;
    }

    public void StartShake(float duration, float magnitude, bool priority)
    {
        shake_inq = StartCoroutine(LightShake(duration,magnitude));
    }

    // Update is called once per frame
    void Update()
    {
        player = pp.a_party[pp.active];
        if (shake_inq == null)
            transform.position = new Vector3(player.transform.position.x,transform.position.y,transform.position.z);
        
        if (Input.GetKeyDown("p"))
            particle.SetActive(!particle.activeSelf);
    }
}
