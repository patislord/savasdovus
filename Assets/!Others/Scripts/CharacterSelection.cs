using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterSelection : NetworkBehaviour
{
    [SerializeField] private List<GameObject> beans = new List<GameObject>();
    [SerializeField] private GameObject characterSelectorPanel;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private Camera selectionCamera;
    [SerializeField] private Vector3 fallbackCameraPosition = new Vector3(0.13f, 1.73f, -10f);
    [SerializeField] private float fallbackCameraSize = 30f;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isOwned)
        {
            canvasObject.SetActive(false);
            SetSelectionCameraActive(false);
            return;
        }

        if (!SetSelectionCameraActive(true))
            CreateFallbackSelectionCamera();
    }

    private bool SetSelectionCameraActive(bool isActive)
    {
        if (selectionCamera == null)
            selectionCamera = GetComponentInChildren<Camera>(true);

        if (selectionCamera == null)
            return false;

        selectionCamera.gameObject.SetActive(isActive);

        AudioListener audioListener = selectionCamera.GetComponent<AudioListener>();
        if (audioListener != null)
            audioListener.enabled = isActive;

        return true;
    }

    private void CreateFallbackSelectionCamera()
    {
        GameObject cameraObject = new GameObject("SelectionCamera");
        cameraObject.transform.SetParent(transform);
        cameraObject.transform.localPosition = fallbackCameraPosition;
        cameraObject.transform.localRotation = Quaternion.identity;
        cameraObject.transform.localScale = Vector3.one;

        selectionCamera = cameraObject.AddComponent<Camera>();
        selectionCamera.clearFlags = CameraClearFlags.Skybox;
        selectionCamera.backgroundColor = new Color(0.192f, 0.302f, 0.475f, 1f);
        selectionCamera.cullingMask = ~0;
        selectionCamera.orthographic = true;
        selectionCamera.orthographicSize = fallbackCameraSize;
        selectionCamera.nearClipPlane = 0.3f;
        selectionCamera.farClipPlane = 1000f;
        selectionCamera.rect = new Rect(0f, 0f, 1f, 1f);
        selectionCamera.depth = 0;
        selectionCamera.useOcclusionCulling = true;
        selectionCamera.allowDynamicResolution = false;
        selectionCamera.stereoSeparation = 0.022f;
        selectionCamera.stereoConvergence = 10f;

        if (FindObjectsByType<AudioListener>(FindObjectsInactive.Exclude).Length == 0)
            cameraObject.AddComponent<AudioListener>();
    }

    public void SpawnNerd()
    {
        characterSelectorPanel.SetActive(false);
        CmdSpawn(0);
    }

    public void SpawnCoolbean()
    {
        characterSelectorPanel.SetActive(false);
        CmdSpawn(1);
    }

    [Command(requiresAuthority = false)]
    private void CmdSpawn(int spawnIndex, NetworkConnectionToClient sender = null)
    {
        if (sender == null)
            return;

        if (spawnIndex < 0 || spawnIndex >= beans.Count)
            return;

        GameObject oldPlayer = sender.identity != null ? sender.identity.gameObject : null;
        Vector3 spawnPosition = oldPlayer != null ? oldPlayer.transform.position : Vector3.zero;
        Quaternion spawnRotation = oldPlayer != null ? oldPlayer.transform.rotation : Quaternion.identity;

        GameObject player = Instantiate(
            beans[spawnIndex],
            spawnPosition,
            spawnRotation
        );

        if (oldPlayer != null)
        {
            NetworkServer.ReplacePlayerForConnection(sender, player, ReplacePlayerOptions.KeepActive);
            Destroy(oldPlayer, 0.1f);
        }
        else
        {
            NetworkServer.AddPlayerForConnection(sender, player);
        }
    }
}
