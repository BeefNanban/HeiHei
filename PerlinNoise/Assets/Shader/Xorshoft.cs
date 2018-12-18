using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xorshoft : MonoBehaviour {

    private uint[] _vec = new uint[4];

    public Xorshoft(uint seed = 100)
    {
        for(uint i = 1; i<= _vec.Length; i++)
        {
            seed = 1812433253 * (seed ^ (seed >> 30)) + i;
            _vec[i - 1] = seed;
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
