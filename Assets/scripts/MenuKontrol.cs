using Mirror;
using UnityEngine;

public class MenuKontrol : MonoBehaviour
{
    // Butonlara tıkladığımızda bu fonksiyon çalışacak
    public void ButonaBasildi(int secilenNumara)
    {
        // Oyuna bağlanan kendi karakterimizi buluyoruz
        if (NetworkClient.localPlayer != null)
        {
            // İçindeki kodu çalıştırıp numarayı yolluyoruz
            NetworkClient.localPlayer.GetComponent<karakter1sc>().CmdKarakterCagir(secilenNumara);
            
            // Karakter seçildiğine göre menüyü gizle
            gameObject.SetActive(false);
        }
    }
}