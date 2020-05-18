    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    GameObject[] soldiersP1;
    Camera[] camerasP1;
    GameObject[] soldiersP2;
    Camera[] camerasP2;
    GameObject active;
    int currTurn = 0;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        soldiersP1 = GameObject.FindGameObjectsWithTag("soldierP1");

        camerasP1 = new Camera[soldiersP1.Length];
        for (int i = 0; i < soldiersP1.Length; i++)
        {
            camerasP1[i] = soldiersP1[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
        for (int i = 0; i < camerasP1.Length; i++)
        {
            if (i != 0)
            {
                camerasP1[i].enabled = false;
            }
        }

        soldiersP2 = GameObject.FindGameObjectsWithTag("soldierP2");
        camerasP2 = new Camera[soldiersP2.Length];

        for (int i = 0; i < soldiersP2.Length; i++)
        {
            camerasP2[i] = soldiersP2[i].transform.Find("cam").gameObject.GetComponent<Camera>();
        }
        for (int i = 0; i < camerasP2.Length; i++)
        {
            if (i != 0)
            {
                camerasP2[i].enabled = false;
            }
        }



        active = soldiersP1[soldiersP1.Length - 1];
        soldiersP1[soldiersP1.Length - 1].GetComponent<Movement>().activate();
        camerasP1[soldiersP1.Length - 1].enabled = true;
    }

    //Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (currTurn % 2 == 0)
            {
                active.GetComponent<Movement>().deactivate();
                foreach (Camera cam in camerasP1)
                {
                    cam.enabled = false;
                }
                active = soldiersP2[0];
                active.GetComponent<Movement>().activate();
                camerasP2[0].enabled = true;
            }
            else
            {
                active.GetComponent<Movement>().deactivate();
                foreach (Camera cam in camerasP2)
                {
                    cam.enabled = false;
                }
                active = soldiersP1[0];
                active.GetComponent<Movement>().activate();
                camerasP1[0].enabled = true;
            }
            currTurn++;
        }
        if (currTurn % 2 == 0)
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Input.GetKeyDown("" + i))
                    {
                        if (i <= soldiersP1.Length)
                        {
                            active.GetComponent<Movement>().deactivate();
                            active = soldiersP1[i - 1];
                            soldiersP1[i - 1].GetComponent<Movement>().activate();
                            camerasP1[i - 1].enabled = true;
                            for (int j = 0; j < camerasP1.Length; j++)
                            {
                                if (j != i - 1)
                                {
                                    camerasP1[j].enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.anyKeyDown)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (Input.GetKeyDown("" + i))
                    {
                        if (i <= soldiersP2.Length)
                        {
                            active.GetComponent<Movement>().deactivate();
                            active = soldiersP2[i - 1];
                            soldiersP2[i - 1].GetComponent<Movement>().activate();
                            camerasP2[i - 1].enabled = true;
                            for (int j = 0; j < camerasP2.Length; j++)
                            {
                                if (j != i - 1)
                                {
                                    camerasP2[j].enabled = false;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
