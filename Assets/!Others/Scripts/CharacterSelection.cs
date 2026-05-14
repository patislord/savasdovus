using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterSelection : NetworkBehaviour
{
    [SerializeField] private List<GameObject> beans = new List<GameObject>();
    [SerializeField] private GameObject characterSelectorPanel;
    [SerializeField] private GameObject canvasObject;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!isOwned)
            canvasObject.SetActive(false);
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
