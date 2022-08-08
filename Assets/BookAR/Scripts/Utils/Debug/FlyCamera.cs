using UnityEngine;

namespace BookAR.Scripts.Utils.Debug
{
    public class FlyCamera : MonoBehaviour {
     
        /*
        Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
        Converted to C# 27-02-13 - no credit wanted.
        Simple flycam I made, since I couldn't find any others made public.  
        Made simple to use (drag and drop, done) for regular keyboard layout  
        wasd : basic movement
        shift : Makes camera accelerate
        space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/
         
         
        float mainSpeed = 20.0f; //regular speed
        float shiftAdd = 60.0f; //multiplied by how long shift is held.  Basically running
        float maxShift = 500.0f; //Maximum speed when holdin gshift
        float camSens = 0.15f; //How sensitive it with mouse
        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun= 1.0f;
         
        void Update () {
            lastMouse = Input.mousePosition - lastMouse ;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0 );
            var transform2 = transform;
            var eulerAngles = transform2.eulerAngles;
            lastMouse = new Vector3(eulerAngles.x + lastMouse.x , eulerAngles.y + lastMouse.y, 0);
            eulerAngles = lastMouse;
            transform2.eulerAngles = eulerAngles;
            lastMouse =  Input.mousePosition;
            //Mouse  camera angle done.  
           
            //Keyboard commands
            float f = 0.0f;
            Vector3 p = GetBaseInput();
            if (Input.GetKey (KeyCode.LeftShift)){
                totalRun += Time.deltaTime;
                p  = p * (totalRun * shiftAdd);
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else{
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }
           
            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.Space)){ //If player wants to move on X and Z axis only
                Transform transform1;
                (transform1 = transform).Translate(p);
                var position = transform1.position;
                newPosition.x = position.x;
                newPosition.z = position.z;
                position = newPosition;
                transform1.position = position;
            }
            else{
                transform.Translate(p);
            }
           
        }
         
        private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
            var pVelocity = new Vector3();
            if (Input.GetKey (KeyCode.W)){
                pVelocity += new Vector3(0, 0 , 1);
            }
            if (Input.GetKey (KeyCode.S)){
                pVelocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey (KeyCode.A)){
                pVelocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey (KeyCode.D)){
                pVelocity += new Vector3(1, 0, 0);
            }
            return pVelocity;
        }
    }
}
