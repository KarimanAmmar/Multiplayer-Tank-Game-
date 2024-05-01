using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NGOTanks
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] int speed;
        [SerializeField] int damage;
        Rigidbody rb;
        //uint ownerId;
    }
}
