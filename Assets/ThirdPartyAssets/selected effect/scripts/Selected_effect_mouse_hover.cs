﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyGameStudio.Jeremy
{
    public class Selected_effect_mouse_hover : MonoBehaviour
    {
        public bool is_selected = false;

        private Material material;

        // Start is called before the first frame update
        void Start()
        {
            if (this.GetComponent<MeshRenderer>() == null)
            {
                this.material = this.GetComponent<SkinnedMeshRenderer>().material;
            }
            else
            {
                this.material = this.GetComponent<MeshRenderer>().material;
            }
        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == this.gameObject.name)
                {
                    this.change_to_selected();
                }
                else
                {
                    this.change_to_not_selected();
                }
            }
            else
            {
                this.change_to_not_selected();
            }

        }

        private void change_to_selected()
        {
            if (this.is_selected != true)
            {
                this.is_selected = true;
                GameObject.FindGameObjectWithTag("audio_selected").GetComponent<AudioSource>().Play();
            }

            if (this.material.GetInt("is_selected") != 1)
                this.material.SetInt("is_selected", 1);
        }

        private void change_to_not_selected()
        {
            if (this.is_selected != false)
            {
                this.is_selected = false;
                GameObject.FindGameObjectWithTag("audio_selected").GetComponent<AudioSource>().Play();
            }

            if (this.material.GetInt("is_selected") != 0)
                this.material.SetInt("is_selected", 0);
        }
    }
}