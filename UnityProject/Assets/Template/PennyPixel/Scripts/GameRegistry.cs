
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRegistry : MonoBehaviour
{
    readonly List<PlayerPlatformerController> _allPlayers = new List<PlayerPlatformerController>();
    readonly List<CageBehaviour> _allCages = new List<CageBehaviour>();

    public static GameRegistry Instance
    {
        get; private set;
    }

    public List<PlayerPlatformerController> AllPlayers
    {
        get { return _allPlayers; }
    }

    public List<CageBehaviour> AllCages
    {
        get { return _allCages; }
    }

    public void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(PlayerPlatformerController player)
    {
        _allPlayers.Add(player);
    }

    public void RemovePlayer(PlayerPlatformerController player)
    {
        _allPlayers.Remove(player);
    }

    public void AddCage(CageBehaviour cage)
    {
        _allCages.Add(cage);
    }

    public void RemoveCage(CageBehaviour cage)
    {
        _allCages.Remove(cage);
    }
}
