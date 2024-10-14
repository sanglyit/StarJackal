using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSetup : MonoBehaviour
{
        [SerializeField] private GameObject BulletMG;
        [SerializeField] private GameObject MinigunBullet;
        [SerializeField] private GameObject SAWPBullet;
        [SerializeField] private GameObject Smol_Exp;
        [SerializeField] private GameObject Smol_Gold;

        private void Start()
        {
            ObjectPool.Instance.CreatePool("BulletMG", BulletMG, 30);
            ObjectPool.Instance.CreatePool("MinigunBullet", MinigunBullet, 100);
            ObjectPool.Instance.CreatePool("SAWPBullet", SAWPBullet, 20);
            ObjectPool.Instance.CreatePool("Smol_Exp", Smol_Exp, 200);
            ObjectPool.Instance.CreatePool("Smol_Gold", Smol_Gold, 200);
        }
}
