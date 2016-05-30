﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{
    private int playerID;
    private GameObject playerObj;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private PlayerBuilding playerBuilding;
	private RuntimeVariables runtimeVariables;    
    
    private KeyCode ActionMoveUp;
    private KeyCode ActionMoveDown;
    private KeyCode ActionShoot;
    private KeyCode ActionBuildFirewall;
    
	public bool isControllable;

	private float firewallTimer =0;
	private float firewallTimerThreshold = .5f;
	private bool  isPlayerBuildingFirewall = false;

	private NPCControl npcRef;
    
    void Start() 
    {
        playerID = GetComponent<PlayerProperties>().playerID;            
        playerObj = gameObject;
        playerMovement = GetComponent<PlayerMovement>();
        playerShooting = GetComponent<PlayerShooting>();
        playerBuilding = GetComponent<PlayerBuilding>();
        runtimeVariables = RuntimeVariables.GetInstance();
                    
        SetupControls();
    }
	
	void Update() 
    {
        if (!isControllable)
            return;
            
        ProcessActions();
	}
    
    void SetupControls()
    {
        Player0Controls();
        Player1Controls();
    }
    
    void Player0Controls()
    {
        if (playerID == 0)
        {
            ActionMoveUp = KeyCode.W;
            ActionMoveDown = KeyCode.S;
            
            if (!runtimeVariables.isPlayer0Toggled)
            {
                ActionShoot = KeyCode.A;
                ActionBuildFirewall = KeyCode.D;
            } else {
                ActionShoot = KeyCode.D;
                ActionBuildFirewall = KeyCode.A;
            }
        }
    }
    
    void Player1Controls()
    {
        if (playerID == 1)
        {
            ActionMoveUp = KeyCode.UpArrow;
            ActionMoveDown = KeyCode.DownArrow;
            
			if (!runtimeVariables.isPlayer1Toggled)
            {
                ActionShoot = KeyCode.LeftArrow;
                ActionBuildFirewall = KeyCode.RightArrow;
            } else {
                ActionShoot = KeyCode.RightArrow;
                ActionBuildFirewall = KeyCode.LeftArrow;
            }
        }
    }
    
    void ProcessActions()
    {
		if (Input.GetKeyDown(ActionMoveUp) && !isPlayerBuildingFirewall)
        {
			npcRef.AddAction(ActionMoveUp);

            playerMovement.MovePlayerUp(playerObj);
			//npc.AddAction(MovingUp);
        }
        
		if (Input.GetKeyDown(ActionMoveDown) && !isPlayerBuildingFirewall)
        {
			npcRef.AddAction(ActionMoveDown);

            playerMovement.MovePlayerDown(playerObj);
        }
        
        if (Input.GetKeyDown(ActionShoot))
        {
			npcRef.AddAction(ActionShoot);

            playerShooting.ShootProjectile(playerObj, playerID);
        }
        
        if (Input.GetKey(ActionBuildFirewall))
        {
			if (Input.GetKeyDown (ActionBuildFirewall))
				npcRef.AddAction(ActionBuildFirewall);

			isPlayerBuildingFirewall = true;
			firewallTimer+=Time.deltaTime;
			if (firewallTimer >=firewallTimerThreshold)
			{
				playerBuilding.HandleBuild(playerObj, playerID, playerMovement.CurrentLane);
				firewallTimer = 0f;
			}

          
        }
		else{
			isPlayerBuildingFirewall = false;
			firewallTimer = 0f;
		}
    }

	public int GetPlayerID
	{
		get { return playerID; }
	}

	public void SetNPCref(NPCControl reference)
	{
		npcRef = reference;
	}
}
