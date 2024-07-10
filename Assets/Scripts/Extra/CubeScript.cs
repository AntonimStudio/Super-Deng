using System.Collections;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    [SerializeField] public GameObject topSide;
    [SerializeField] public GameObject downSide;
    [SerializeField] public GameObject rightSide;
    [SerializeField] public GameObject leftSide;
    [SerializeField] public GameObject UntouchableSide;
    [SerializeField] private TheMostSideFinding theMost;
    public GameObject player;
    public bool havePlayer = false;
    public bool isUpOrDown = false;
    private bool transferInProgress = false;


    private void Update()
    {

        if (havePlayer && !transferInProgress)
        { 
            if (Input.GetKeyDown(KeyCode.W))
            {
                transferInProgress = true;
                if (!isUpOrDown) { StartCoroutine(TransferPlayerTop(topSide)); }
                else { StartCoroutine(TransferPlayer(topSide)); }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                transferInProgress = true;
                if (!isUpOrDown) { StartCoroutine(TransferPlayerTop(downSide)); }
                else { StartCoroutine(TransferPlayer(downSide)); }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                transferInProgress = true;
                StartCoroutine(TransferPlayer(rightSide));
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                transferInProgress = true;
                StartCoroutine(TransferPlayer(leftSide));
            }
        }
    }

    private IEnumerator TransferPlayer(GameObject theOtherOne)
    {
        yield return new WaitForSeconds(0.1f);
        if (!theOtherOne.GetComponent<CubeScript>().havePlayer)
        {
            player.transform.position = theOtherOne.transform.position;
            player.transform.SetParent(theOtherOne.transform);
            theOtherOne.GetComponent<CubeScript>().ReceiveBall(player);
            player = null;
            havePlayer = false;
        }
        transferInProgress = false;
    }

    private IEnumerator TransferPlayerTop(GameObject theOtherOne)
    {
        yield return new WaitForSeconds(0.1f);
        if (!theOtherOne.GetComponent<CubeScript>().havePlayer)
        {
            player.transform.position = theOtherOne.transform.position;
            player.transform.SetParent(theOtherOne.transform);
            theOtherOne.GetComponent<CubeScript>().ReceiveBall(player);
            theOtherOne.GetComponent<CubeScript>().topSide = theMost.nearMostCube; 
            theOtherOne.GetComponent<CubeScript>().downSide = theMost.farMostCube;
            theOtherOne.GetComponent<CubeScript>().rightSide = theMost.rightMostCube;
            theOtherOne.GetComponent<CubeScript>().leftSide = theMost.leftMostCube;
            player = null;
            havePlayer = false;
        }
        transferInProgress = false;
    }

    private IEnumerator TransferPlayerDown(GameObject theOtherOne)
    {
        yield return new WaitForSeconds(0.1f);
        if (!theOtherOne.GetComponent<CubeScript>().havePlayer)
        {
            player.transform.position = theOtherOne.transform.position;
            player.transform.SetParent(theOtherOne.transform);
            theOtherOne.GetComponent<CubeScript>().ReceiveBall(player);
            theOtherOne.GetComponent<CubeScript>().topSide = theMost.nearMostCube; 
            theOtherOne.GetComponent<CubeScript>().downSide = theMost.farMostCube;
            theOtherOne.GetComponent<CubeScript>().rightSide = theMost.rightMostCube;
            theOtherOne.GetComponent<CubeScript>().leftSide = theMost.leftMostCube;
            player = null;
            havePlayer = false;
        }
        transferInProgress = false;
    }

    public void ReceiveBall(GameObject newBall)
    {
        player = newBall;
        player.transform.position = transform.position;
        havePlayer = true;
    }
}
