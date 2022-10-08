using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CellProperties : MonoBehaviour
{
    public int column;
    public int row;
    public string status;
    public bool mid=false;// if this is a peg in middle, then particles will be created

    public ParticleSystem BlueParticle;

    public int getColumn()
    {
        return column;
    }
    public int getRow()
    {
        return row;
    }
    public string getStatus()
    {
        return status;
    }
    public void SetColumn(int newColumn)
    {
        column = newColumn;
    }
    public void SetRow(int newRow)
    {
        row = newRow;
    }
    public void SetStatus(string newStatus)
    {
        status = newStatus;
    }
    public void DestroySelf()
    {
        if (mid)
        {
            // if destroyed cell is a mid cell, then display particles and play the sound
            ParticleSystem particle = BlueParticle;
            Instantiate(particle, transform.position, Quaternion.identity);
            SoundManagerScript.PlaySound();
        }
        Destroy(gameObject);
    }
}
