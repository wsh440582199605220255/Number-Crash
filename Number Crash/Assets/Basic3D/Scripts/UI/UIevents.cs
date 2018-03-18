using UnityEngine;
namespace Basic3D
{
    public class UIevents : MonoBehaviour
    {

        public void TryToFly() { _AndroidInput.wantFly = true; }
        public void NotToFly() { _AndroidInput.wantFly = false; }

        public void TryToReset() { _AndroidInput.wantReset = true; }
        public void NotToReset() { _AndroidInput.wantReset = false; }


    }
}