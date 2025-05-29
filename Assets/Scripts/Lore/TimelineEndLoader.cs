using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class TimelineEndLoader : MonoBehaviour
{
    public PlayableDirector director;

    void Start()
    {
        if (director != null) director.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector pd)
    {
        if (pd == director)
        {
            SceneManager.LoadScene("Level1");
        }
    }

    void OnDestroy()
    {
        if (director != null) director.stopped -= OnPlayableDirectorStopped;
    }
}
