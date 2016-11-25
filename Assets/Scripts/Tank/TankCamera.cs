using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Tank
{
    public class TankCamera : NetworkBehaviour
    {
        [UsedImplicitly]
        private GameObject turret;

        private GameObject playerCamera;

        /// <summary>
        /// The on start local player.
        /// </summary>
        public override void OnStartLocalPlayer()
        {
            this.AttachCameraToTank();
            base.OnStartLocalPlayer();
        }

        // Use this for initialization
        [UsedImplicitly]
        private void Start ()
        {
            var renderers = this.transform.Find("TankRenderers");
            this.turret = renderers.transform.Find("TankTurret").gameObject;
        }

        [UsedImplicitly]
        private void Update()
        {
            if (this.playerCamera != null && this.turret != null)
            {
                this.turret.transform.forward = this.playerCamera.transform.forward;
            }
        }

        /// <summary>
        /// The attach camera to tank.
        /// </summary>
        [UsedImplicitly]
        private void AttachCameraToTank()
        {
            var renderers = this.transform.Find("TankRenderers");
            this.turret = renderers.transform.Find("TankTurret").gameObject;

            this.playerCamera = GameObject.FindGameObjectWithTag("MainCamera");

            this.playerCamera.transform.parent = this.gameObject.transform;
#if UNITY_ANDROID
            this.playerCamera.transform.localPosition = new Vector3(0, 2.2f, -1);
            this.gameObject.transform.forward = this.playerCamera.transform.forward;
#else 
        this.playerCamera.transform.localPosition = new Vector3(0, 2.1f, -1);
        this.playerCamera.transform.forward = this.turret.transform.forward;
#endif

            this.playerCamera.transform.localRotation = Quaternion.identity;
        }
    }
}
