using UnityEngine.Networking;

namespace Assets.Scripts.Tank
{
    using JetBrains.Annotations;
    using UnityEngine;

    public class TankEnemyUiBehaviour : MonoBehaviour
    {
        private Transform nameCanvas;
        private Transform photoCanvas;

        // Update is called once per frame
        [UsedImplicitly]
        private void Update()
        {
            this.EnemyUiLookAtThis();
        }

        private void EnemyUiLookAtThis()
        {
            if (this.nameCanvas != null)
            {
                this.nameCanvas.LookAt(this.gameObject.transform);
                this.nameCanvas.forward = -this.nameCanvas.forward;
                this.photoCanvas.LookAt(this.gameObject.transform);
                this.photoCanvas.forward = -this.photoCanvas.forward;
                return;
            }

            foreach (var tankManager in GameManager.m_Tanks)
            {
                if (tankManager.m_Instance == this.gameObject)
                {
                    continue;
                }

                this.nameCanvas = tankManager.m_Instance.transform.FindChild("NameCanvas");
                this.photoCanvas = tankManager.m_Instance.transform.FindChild("PhotoCanvas");
                break;
            }
        }
    }
}
