using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenu : MonoBehaviour
{
    public Ring Data;
    public RingCakePiece RingCakePiecePrefab;
    public float GapWidthDegree = 1f;
    public Action<string> callback;
    protected RingCakePiece[] Pieces;
    protected RingMenu Parent;
    [HideInInspector]
    public string Path;

    private void Start()
    {
        var stepLength = 360f / Data.Elements.Length;
        var iconDist = Vector3.Distance(RingCakePiecePrefab.icon.transform.position, RingCakePiecePrefab.cakePiece.transform.position); 
        //Position it
        Pieces = new RingCakePiece[Data.Elements.Length]; 

        for (int i = 0; i < Data.Elements.Length; i++)
        {
            Pieces[i] = Instantiate(RingCakePiecePrefab, transform);
            //set root element
            Pieces[i].transform.localPosition = Vector3.zero; 
            Pieces[i].transform.localRotation = Quaternion.identity;

            //set cake piece

            Pieces[i].cakePiece.fillAmount = 1f / Data.Elements.Length - GapWidthDegree / 360f;
            Pieces[i].cakePiece.transform.localPosition = Vector3.zero;
            Pieces[i].cakePiece.transform.localRotation = Quaternion.Euler(0, 0, -stepLength / 2f + GapWidthDegree / 2f + i * stepLength);
            Pieces[i].cakePiece.color = new Color(1f, 1f, 1f, .5f);

            //set icon
            Pieces[i].icon.transform.localPosition = Pieces[i].cakePiece.transform.localPosition + Quaternion.AngleAxis(i * stepLength, Vector3.forward) * Vector3.up * iconDist;
            Pieces[i].icon.sprite = Data.Elements[i].Icon; 
        }
    }

    private void Update()
    {
        
    }
}
