using System.Collections;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    //public CharacterController characterController;
    public Camera playerCamera;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>(true);

        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            playerCamera.enabled = true;

            AudioListener playerAudioListener = playerCamera.GetComponent<AudioListener>();

            if (playerAudioListener != null)
                playerAudioListener.enabled = true;
        }

        StartCoroutine(RemoveOtherCameras());

        Vector3 spawnPoint = Random.insideUnitSphere * 10f;
        spawnPoint.y = 3;
        transform.position = spawnPoint;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isLocalPlayer && playerCamera != null)
            Destroy(playerCamera.gameObject);
    }

    private IEnumerator RemoveOtherCameras()
    {
        for (int i = 0; i < 3; i++)
        {
            RemoveOtherCameraObjects();
            yield return null;
        }
    }

    private void RemoveOtherCameraObjects()
    {
        Camera localCamera = playerCamera != null
            ? playerCamera
            : GetComponentInChildren<Camera>(true);

        if (localCamera == null)
            return;

        Camera[] cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include);

        foreach (Camera camera in cameras)
        {
            if (camera == null || camera == localCamera)
                continue;

            AudioListener audioListener = camera.GetComponent<AudioListener>();

            if (audioListener != null)
                audioListener.enabled = false;

            Destroy(camera.gameObject);
        }

        AudioListener[] audioListeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Include);

        foreach (AudioListener audioListener in audioListeners)
        {
            if (audioListener == null || audioListener.gameObject == localCamera.gameObject)
                continue;

            audioListener.enabled = false;
        }
    }
}
