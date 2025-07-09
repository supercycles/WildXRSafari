using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnimatedButterflies
{

    /// <summary>
    /// Change the wings animation speed to :
    /// - given speed or
    /// - a random speed between given minimum speed and maximum speed.
    /// </summary>

    [RequireComponent(typeof(Animator))]
    public class ChangeSpeedAnimation : MonoBehaviour
    {
        [Header("Given speed")]
        [SerializeField] float wingsSpeed = 1;

        [Header("Randomized speed")]
        [SerializeField] bool randomizeSpeed = true;
        [SerializeField] float minimumSpeed = 0.2f;
        [SerializeField] float maximumSpeed = 1f;

        private Animator wingAnimator;
        private void Start()
        {
            //Get the wings animator
            wingAnimator = GetComponent<Animator>();

            //If the wings animator is found, change its speed;
            if (wingAnimator != null)
            {
                //If randomized speed : select a random speed
                if (randomizeSpeed)
                {
                    float random = Random.Range(minimumSpeed, maximumSpeed);
                    wingAnimator.speed = random;
                }
                else //set given speed
                {
                    wingAnimator.speed = wingsSpeed;
                }
            }
        }
    }
}