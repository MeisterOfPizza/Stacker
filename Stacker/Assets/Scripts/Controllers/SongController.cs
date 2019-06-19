using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable 0649

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
            while (true)
            {
                if (!source.isPlaying)
                {
                    currentSong = GetNextSong();
                    source.clip = currentSong;
                    source.PlayDelayed(playDelay);
                }

                source.volume = AudioController.MusicVolume;

                yield return new WaitForEndOfFrame();
            }
        }

        private AudioClip GetNextSong()
        {
            List<AudioClip> audioClips = songs.ToList();
            audioClips.Remove(currentSong);
            return audioClips[Random.Range(0, audioClips.Count)];
        }

    }

}
