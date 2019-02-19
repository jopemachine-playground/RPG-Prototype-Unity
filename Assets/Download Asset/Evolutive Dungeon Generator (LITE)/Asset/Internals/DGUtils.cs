using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum eDGModuleType {NONE=-1, ACCESS, PATH, POINT};


public class DGRandom {
	
	static public void setSeed (int seed) {
		Random.InitState(seed);
	}

	static public int Value() {
		return Random.Range(0, 1000000);
	}

	static public int Range(int min, int max) {
		return min + (DGRandom.Value() % (max-min));
	}

}


public class DGRangedValue {
	//#######################################################
	// ERROR
	//#######################################################
	private bool   errorStatus = false;
	private string errorMsg = "";

	public void errorClear() {
		errorStatus = false;
		errorMsg = "";
	}

	public bool error() {
		return errorStatus;
	}

	public void errorSet(string msg) {
		errorMsg = msg+"\n";
	}

	public string errorGet() {
		return errorMsg;
	}

	//#######################################################
	// RANGED VALUE
	//#######################################################
	public int max   = 100;
	public int min   = 1;
	public int value = 1;

	public DGRangedValue (int min=1, int max=10) {
		this.max   = max;
		this.min   = min;
		this.value = min;
	}

	public DGRangedValue (int min, int value, int max) {
		this.max   = max;
		this.min   = min;
		this.value = value;
	}

	public void adjust() {
		if(min<0) min=0;
		if(min>max) min=max;
		if(value>max) value=max;
		if(value<min) value=min;
	}

	public void setRange(int max, int min) {
		this.max = max;
		this.min = min;
		adjust();
	}

	public void setValue(int value) {
		this.value = value;
		adjust();
	}

	public bool check() {
		if(min > max) {
			errorSet("MIN cannot be greater than MAX");
			return true; }

		if(min < 0) {
			errorSet("MIN cannot be smaller than 0");
			return true; }

		if(value > max) {
			errorSet("VALUE cannot be greater than MAX");
			return true; }

		if(value < min) {
			errorSet("VALUE cannot be smaller than MIN");
			return true; }
		
		return false;
	}

}


