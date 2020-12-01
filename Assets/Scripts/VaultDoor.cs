using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VaultDoor : MonoBehaviour,
    IUseable
{
    public UnityEvent DoorOpened;

    public void OnUse()
    {
        var player = GameObject.FindObjectOfType<PlayerActions>();
        if (player.hasKey)
        {
            DoorOpened?.Invoke();
            GameState.Instance.SetGameState(PlayerState.State.Alarm);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
