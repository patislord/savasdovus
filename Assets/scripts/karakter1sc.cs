using Mirror;
using UnityEngine;

public class karakter1sc : NetworkBehaviour
{
    [Command]
    public void CmdKarakterCagir(int karakterNumarasi)
    {
        // 1. Network Manager'daki listeden numarasına göre karakteri bul
        GameObject secilenPrefab = NetworkManager.singleton.spawnPrefabs[karakterNumarasi];
        
        // 2. Karakteri dünyada oluştur
        GameObject yeniKarakter = Instantiate(secilenPrefab);
        
        // 3. Karakteri ağa dahil et ve kontrolü sana (butona basan kişiye) ver
        NetworkServer.Spawn(yeniKarakter, connectionToClient);
    }
}