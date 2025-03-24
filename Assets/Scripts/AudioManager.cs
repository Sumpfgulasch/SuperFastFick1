using FMOD.Studio;
using UnityEngine;

public class FmodParameter {
    public const string ENEMY_STATE = "EnemyState";
    public const string PLAYER_STATE = "PlayerState";
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayRunSound()
    {
        // if (runAudio != null && !runAudio.isPlaying)
        //     runAudio.Play();
    }

    public void StopRunSound()
    {
        // if (runAudio != null && runAudio.isPlaying)
        //     runAudio.Stop();
    }

    public void PlayAttackSound()
    {
        // if (attackAudio != null)
        //     attackAudio.PlayOneShot(attackAudio.clip);
    }

    public EventInstance Play3DAudio(FMOD.GUID audioEvent, Transform target) {
        var instance = FMODUnity.RuntimeManager.CreateInstance(audioEvent);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(target));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, target.gameObject);
        instance.start();
        return instance;
    }

    public void StopAudio(EventInstance instance) {
        if (instance.isValid()) {
            instance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void SetLocalParameter(EventInstance fmodEvent,string fmodParameter, float value) {
        if (!fmodEvent.isValid()) {
            return;
        }

        fmodEvent.setParameterByName(fmodParameter, value);
    }
}