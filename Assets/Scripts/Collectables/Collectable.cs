using System;
using System.Collections;
using UnityEngine;

namespace Collectables
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private CollectableManager collectableManager;
        [SerializeField] private ParticleSystem explosion;
        [SerializeField] private AudioClip sfx;

        private LTSeq _animation;

        private void OnEnable()
        {
            StartCoroutine(LoopAnimation());
        }

        private IEnumerator LoopAnimation()
        {
            while (true)
            {
                _animation = LeanTween.sequence();
                _animation.append(LeanTween.moveLocalY(this.gameObject, -0.125f, 0f).setEase(LeanTweenType.easeInOutSine));
                _animation.append(LeanTween.moveLocalY(this.gameObject, 0.125f, 1f).setEase(LeanTweenType.easeInOutSine));
                _animation.append(LeanTween.moveLocalY(this.gameObject, -0.125f, 1f).setEase(LeanTweenType.easeInOutSine));
                yield return new WaitForSeconds(2f);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Vector3 oldPosition = transform.position;
            EventManager.TriggerSoundEffect(sfx);
            collectableManager.OnCollectableGrabbed(this);
            explosion.transform.position = oldPosition;
            explosion.Clear();
            explosion.Play();
        }
    }
}
