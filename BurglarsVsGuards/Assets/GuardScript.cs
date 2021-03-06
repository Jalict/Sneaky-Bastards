﻿using UnityEngine;
using System.Collections;

public class GuardScript : MonoBehaviour
	{
		public GUIStyle style;
		GameObject currentProp = null;
		propTypes currentPropType;
		GameObject CurrentPropToTaze = null;
		PropsToHideIn CurrentPropToHideIn;
		bool readyToTaze = false;
		bool readyToSearch = false;
		bool tazing = false;
		float searchingCounterTime = 0;
		bool hidden = false;

		movement Movement;

		public GameObject progressBar;
		public GameObject RBButton;
    
    	private GameObject progress;
    	private GameObject ActionButton;

    	HideNSteal thiefScript;

    	bool myRBButtonIsReady = true;

    	static int totalDeath = 0;
    	static bool showingDeathScreen = false;


	// Use this for initialization
	void Awake ()
	{
		progress = (GameObject)Instantiate(progressBar, transform.position, Quaternion.identity);
		progress.renderer.enabled = false;

		Movement = gameObject.GetComponent<movement>();

        ActionButton = (GameObject)Instantiate(RBButton, transform.position, Quaternion.identity);
        //ActionButton.transform.renderer.enabled = false;
	}
	
	public void ProgressBar(bool newState, Vector3 newPosition)
    {
    	progress.renderer.enabled = newState;
    	if(!newState) progress.GetComponent<ProgressBar>().SetProgressTime(0f);
    	else progress.transform.position = newPosition;
    }

    public void ProgressBar(bool newState)
    {
    	progress.renderer.enabled = newState;
    	if(!newState) progress.GetComponent<ProgressBar>().SetProgressTime(0f);
    	else Debug.Log("Did you forget a new position for the progress bar?");
    }

    public void SetProgressTime (float newTime)
    {
    	progress.GetComponent<ProgressBar>().SetProgressTime(newTime);
    }

	// Update is called once per frame
	void Update ()
	{
		if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
        {
            myRBButtonIsReady = true;
        }

		if(totalDeath > 1)
		{
			if(!showingDeathScreen)
			{
				showingDeathScreen = true;
				//ShowDeathScreen();
			}
			
		}

		if(readyToTaze && currentPropType == propTypes.thief && currentProp != null)
		{			
			if(Vector2.Distance(transform.position, currentProp.transform.position) < 2)
			{
				if(!tazing)
				{
					ActionButton.transform.position = currentProp.transform.position + new Vector3(0, 0, -3f);
        			ActionButton.transform.renderer.enabled = true;
        		}
    			if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
        		{
        		    //Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
        		    ActionButton.transform.renderer.enabled = false;
        		    tazing = true;
        		    thiefScript.GettingTazed(true);
        		    if(Network.connections.Length > 0)
        		    	thiefScript.networkView.RPC("NetGettingTazed",RPCMode.AllBuffered,true);
        		    Movement.SetHiding(true);
        		    //SPILNOGETLYD!!!!
	
        		    //ProgressBar(true, pos);
        		    //progress.transform.position = pos;
        		    //progress.renderer.enabled = true;
        		    //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
        		}
	
        		if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
        		{
        		    //if (progress != null)
        		    //{
        		        //hidingCounterTime = 0;
        		        //ActionButton.transform.renderer.enabled = true;
        		        //ProgressBar(false);
        		        Movement.SetHiding(false);
        		        if(tazing)
        		        {
        		        	tazing = false;
    						DeathCount();
        		        	thiefScript.KillFromTaze(this);
        		    		if(Network.connections.Length > 0)
        		        		thiefScript.networkView.RPC("NetKillFromTaze". RPCMode.AllBuffered);
        		        	currentProp = null;
                			currentPropType = propTypes.nothing;
                			readyToTaze = false;
							thiefScript = null;
        		        }


        		        
        		        //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
        		        //Destroy(progress);
        		    //}
        		}
	
        		if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
        		{
        		    //hidingCounterTime += Time.deltaTime;
        		    //bool hideNotRide = false;
        		    //if (progress != null)
        		    //{
        		    //    if(currentPropType == propTypes.hiding)
        		    //    {
        		    //    	SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
        		    //    	hideNotRide = true;
        		    //    }
        		    //    else if(currentPropType == propTypes.car)
        		    //    {
        		    //    	SetProgressTime(ridingCounterTime / CurrentCarToRideIn.RidingTime);
        		    //    	hideNotRide = false;
        		    //    }
        		    //    
        		    //    //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
        		    //}
				}
			} else
			{
				ActionButton.transform.renderer.enabled = false;
			}
			
		}

		if(readyToSearch && currentPropType == propTypes.hiding && currentProp != null)
		{
			if(Vector2.Distance(transform.position, currentProp.transform.position) < (currentProp.transform.localScale.x+currentProp.transform.localScale.y)/2)
			{
				//if(!tazing)
				//{
				//	ActionButton.transform.position = currentProp.transform.position + new Vector3(0, 0, -3f);
        		//	ActionButton.transform.renderer.enabled = true;
        		//}
    			if (OuyaInput.GetButtonDown(OuyaButton.RB, Movement.observedPlayer))
        		{
        		    Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
        		    ActionButton.transform.renderer.enabled = false;
        		    //tazing = true;
        		    //thiefScript.GettingTazed(true);
        		    Movement.SetHiding(true);
        		    //SPILNOGETLYD!!!!
	
        		    ProgressBar(true, pos);
        		    //progress.transform.position = pos;
        		    //progress.renderer.enabled = true;
        		    //progress = (GameObject)Instantiate(progressBar, pos, Quaternion.identity);
        		}
	
        		if (OuyaInput.GetButtonUp(OuyaButton.RB, Movement.observedPlayer))
        		{
        		    //if (progress != null)
        		    //{
        		        searchingCounterTime = 0;
        		        ActionButton.transform.renderer.enabled = true;
        		        ProgressBar(false);
        		        Movement.SetHiding(false);
        		        //if(tazing)
        		        //{
        		        //	tazing = false;
        		        //	thiefScript.KillFromTaze(this);
        		        //	currentProp = null;
                		//	currentPropType = propTypes.nothing;
                		//	readyToTaze = false;
						//	thiefScript = null;
        		        //}


        		        
        		        //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
        		        //Destroy(progress);
        		    //}
        		}
	
        		if (OuyaInput.GetButton(OuyaButton.RB, Movement.observedPlayer) && myRBButtonIsReady)
        		{
        		    searchingCounterTime += Time.deltaTime;
        		    //bool hideNotRide = false;
        		    if (progress != null)
        		    {
        		        if(currentPropType == propTypes.hiding)
        		        {
        		        	SetProgressTime(searchingCounterTime / CurrentPropToHideIn.HidingTime);
        		        	//hideNotRide = true;
        		        }
        		        //else if(currentPropType == propTypes.car)
        		        //{
        		        //	SetProgressTime(ridingCounterTime / CurrentCarToRideIn.RidingTime);
        		        //	//hideNotRide = false;
        		        //}
        		        
        		        //progress.GetComponent<ProgressBar>().SetProgressTime(hidingCounterTime / CurrentPropToHideIn.HidingTime);
        		    }
        		    if (searchingCounterTime >= CurrentPropToHideIn.HidingTime)
                	{
	
                	    // has hidden in prop - her sker magien
                	    
                	    //if(hidingInCar)
                	    //{
                	    //	hidingInCar = false;
                	    //	thievesInCar--;
                	    //}
                	    //hidden = false;
                	    Movement.SetHiding(false);
                	    transform.renderer.enabled = true;
                	    collider2D.enabled = true;
                	    CurrentPropToHideIn.SomeoneIsHidingInHere = false;
                	    CurrentPropToHideIn.KickOutThief();
                	    CurrentPropToHideIn = null;
                	    currentProp = null;
                	    ActionButton.transform.renderer.enabled = false;
                	    searchingCounterTime = 0;
                	    myRBButtonIsReady = false;
                	    //SetPickupDisplay(true);
                	    ProgressBar(false);
                	}
				}
			} else
			{
				ActionButton.transform.renderer.enabled = false;
			}
		}
	}

	void OnGUI()
	{
		if(showingDeathScreen)GUI.Label(new Rect (Screen.width/2, Screen.height/2, 200,200),"The Burglars FAILED!",style);
	}

	public void DeathCount()
	{
		totalDeath++;
	}

	private void OnTriggerEnter2D(Collider2D coll)
    {
    	if (coll.gameObject.tag == "Thief")
        {
        	if (!coll.gameObject.GetComponent<HideNSteal>().GetBeingTazed())
            {
                currentProp = coll.gameObject;
                currentPropType = propTypes.thief;
                readyToTaze = true;
                //Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                //ActionButton.transform.position = pos;
                //ActionButton.transform.renderer.enabled = true;
                thiefScript = currentProp.GetComponent<HideNSteal>();
            }
            else
            {
                currentProp = null;
                //CurrentPropToPickUp = null;
                //readyToPickup = false;

                return;
            }
        } else if (coll.gameObject.tag == "CanHideIn")

        {
        	if (!coll.gameObject.GetComponent<PropsToHideIn>() != null)
            {
                print("YES!!!");
                currentProp = coll.gameObject;
                currentPropType = propTypes.hiding;
                CurrentPropToHideIn = coll.gameObject.GetComponent<PropsToHideIn>();
                readyToSearch = true;
                Vector3 pos = currentProp.transform.position + new Vector3(0, 0, -3f);
                ActionButton.transform.position = pos;
                ActionButton.transform.renderer.enabled = true;
                //thiefScript = currentProp.GetComponent<HideNSteal>();
            }
            else
            {
                currentProp = null;
                //CurrentPropToPickUp = null;
                //readyToPickup = false;

                return;
            }
        }
    }
}
