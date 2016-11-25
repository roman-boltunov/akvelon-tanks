namespace Assets.Scripts.Tank
{
    using JetBrains.Annotations;
    using UnityEngine;

    public class TankName : MonoBehaviour
    {

        private Transform enemyNameTransform;
    
        // Update is called once per frame
        [UsedImplicitly]
        private void Update ()
        {
            this.NameLookAtEnemy();
        }

        private void NameLookAtEnemy()
        {
            if (this.enemyNameTransform != null)
            {
                this.enemyNameTransform.LookAt(this.gameObject.transform);
                this.enemyNameTransform.forward = -this.enemyNameTransform.forward;
                return;
            }

            foreach (var tankManager in GameManager.m_Tanks)
            {
                if (tankManager.m_Instance == this.gameObject)
                {
                    continue;
                }

                this.enemyNameTransform = tankManager.m_Instance.transform.FindChild("NameCanvas");
                break;
            }
        }
    }
}
