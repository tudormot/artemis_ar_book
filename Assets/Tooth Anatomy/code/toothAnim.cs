using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toothAnim : MonoBehaviour {

	public Animation anim_A;
	public GameObject B_buton_close;
	public GameObject B_buton_stage1;
	public GameObject B_buton_stage2;
	public GameObject B_buton_closeStage1;
	public GameObject B_buton_closeStage2;
	public GameObject B_buton_closeAll;
	   
	   
	void Start () 
	{
	anim_A.Play("close");
	B_buton_stage1.active = true;	
	B_buton_close.active = false;
	B_buton_stage2.active = false;
	B_buton_closeStage1.active = false;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	

		public void button_stage_1()
	{
	anim_A.Play("stage1");
	B_buton_close.active = false;
	B_buton_stage1.active = false;
	B_buton_stage2.active = true;
	B_buton_closeStage1.active = true;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	
		public void button_close_stage_1()
	{
	anim_A.Play("closeStage1");
	B_buton_close.active = false;
	B_buton_stage1.active = true;
	B_buton_stage2.active = false;
	B_buton_closeStage1.active = false;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	
	
		public void button_stage_2()
	{
	anim_A.Play("stage2");
	B_buton_close.active = false;
	B_buton_stage1.active = false;
	B_buton_stage2.active = false;
	B_buton_closeStage1.active = false;
	B_buton_closeStage2.active = true;
	B_buton_closeAll.active = true;
	}
	
		public void button_close_stage_2()
	{
	anim_A.Play("closeStage2");
	B_buton_close.active = false;
	B_buton_stage1.active = false;
	B_buton_stage2.active = true;
	B_buton_closeStage1.active = true;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	
		public void button_close_Allstage()
	{
	anim_A.Play("closeall");
	B_buton_stage1.active = true;	
	B_buton_close.active = false;
	B_buton_stage2.active = false;
	B_buton_closeStage1.active = false;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	
	
		public void button_open2()
	{
	anim_A.Play("open2");
	}
	
			public void button_close()
	{
	anim_A.Play("closeStage2");
	B_buton_stage1.active = true;
	
	B_buton_close.active = false;
	B_buton_stage2.active = false;
	B_buton_closeStage1.active = false;
	B_buton_closeStage2.active = false;
	B_buton_closeAll.active = false;
	}
	
	
}
