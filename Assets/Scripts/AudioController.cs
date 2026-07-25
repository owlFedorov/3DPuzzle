using UnityEngine;

namespace Puzzle3D
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioClip _preview;
        [SerializeField] private AudioClip _itemSelect;
        [SerializeField] private AudioClip _placeCorrect;
        [SerializeField] private AudioClip _placeWrong;
        [SerializeField] private AudioClip _levelComplete;

        private AudioSource _as;

        public static AudioController I { get; private set; }

        private void Awake()
        {
            I = this;

            _as = GetComponent<AudioSource>();
        }

        public void PlayPreviewSound()
        {
            _as.PlayOneShot(_preview);
        }

        public void PlayItemSelectSound()
        {
            _as.PlayOneShot(_itemSelect);
        }

        public void PlayPlaceCorrectSound()
        {
            _as.PlayOneShot(_placeCorrect);
        }

        public void PlayPlaceWrongSound()
        {
            _as.PlayOneShot(_placeWrong);
        }

        public void PlayLevelCompleteSound()
        {
            _as.PlayOneShot(_levelComplete);
        }
    }
}