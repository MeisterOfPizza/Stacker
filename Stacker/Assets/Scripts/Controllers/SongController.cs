using System.Collections;
using UnityEngine;

namespace Stacker.Controllers
{

    class SongController : Controller<SongController>
    {

        #region Editor

        [SerializeField] private AudioSource source;

        [Space]
        [SerializeField] private AudioClip[] songs;

        [Space]
        [SerializeField] private float playDelay = 2.5f;

        #endregion

        #region Private variables

        private AudioClip currentSong;

        #endregion

        public override void OnAwake()
        {
            StartCoroutine(SongCycle());
        }

        private IEnumerator SongCycle()
        {
            currentSong = songs[Random.Range(0, songs.Length)];
            source.clip = currentSong;
            source.PlayDelayed(playDelay);

            while (source.isPlaying)
            {
                source.volume = AudioController.MusicVolume;

                yield return new WaitForEndOfFrame();
            }
            
            StartCoroutine(SongCycle());
        }

    }

}
