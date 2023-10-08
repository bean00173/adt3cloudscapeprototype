using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour {

	public bool sheep;
	private int count;

	public GameObject explodePs;
	public UnityEvent onExplode = new UnityEvent();

	void Update ()
	{
		transform.position += transform.forward * Time.deltaTime * 4;
		transform.Rotate(Vector3.up * Time.deltaTime * 64);
	}

    private void OnMouseDown()
    {
        if (sheep)
        {
			if(count < 10)
            {
				this.GetComponent<AudioSource>().Play();
				count++;
			}
            else
            {
				this.GetComponent<AudioSource>().clip = AudioManager.instance.GetClip(AudioType.effects, "Sheep_Explode");
				Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
				foreach(Renderer renderer in renderers)
                {
					renderer.enabled = false;
                }

				this.GetComponent<AudioSource>().Play();
				explodePs.SetActive(true);
				onExplode?.Invoke();
				sheep = false;
				Invoke(nameof(DisableSheep), 2f);
			}
			
		}
    }

	private void DisableSheep()
    {
		Destroy(this.gameObject);
    }
}
