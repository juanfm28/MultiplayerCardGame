using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNameOverview : MonoBehaviour
{
    public TMPro.TextMeshProUGUI roomLabel;
    public void Awake()
    {
        roomLabel.text = "Room: " + FindObjectOfType<NetworkRunner>()?.SessionInfo.Name;
    }
}
