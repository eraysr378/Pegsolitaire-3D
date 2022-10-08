using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveButtonActivation : MonoBehaviour
{
    public GameManager gameManager;
    //if cell is not moving, then AutoMove button will be able to be clicked
    public void AutoMove()
    {
        if (!gameManager.isFirstCellMoving)
        {
            gameManager.ActivateAutoMove = true;
        }
    }
}
