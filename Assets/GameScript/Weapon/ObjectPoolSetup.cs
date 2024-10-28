using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSetup : MonoBehaviour
{
        [SerializeField] private GameObject BulletMG;
        [SerializeField] private GameObject DualBullet;
        [SerializeField] private GameObject MinigunBullet;
        [SerializeField] private GameObject SAWPBullet;
        [SerializeField] private GameObject Smol_Exp;
        [SerializeField] private GameObject Smol_Gold;

        private void Start()
        {
            ObjectPool.Instance.CreatePool(BulletMG, 40);
            ObjectPool.Instance.CreatePool(DualBullet, 40);
            ObjectPool.Instance.CreatePool(MinigunBullet, 90);
            ObjectPool.Instance.CreatePool(SAWPBullet, 10);
            ObjectPool.Instance.CreatePool(Smol_Exp, 50);
            ObjectPool.Instance.CreatePool(Smol_Gold, 50);
        }
}
