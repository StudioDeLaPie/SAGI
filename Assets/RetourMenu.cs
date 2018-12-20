using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetourMenu : MonoBehaviour {

	public void RetourAuMenu()
    {
        LobbyManager.s_Singleton.backDelegate.Invoke();
    }

}
