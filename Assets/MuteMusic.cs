using UnityEngine;
using UnityEngine.UI;

public class MuteMusic : MonoBehaviour
{

    private void Start() {
        GetComponent<Toggle>().onValueChanged.AddListener(delegate {
            MuteMusicToggle(GetComponent<Toggle>());
        });
    }
   public void MuteMusicToggle(Toggle val)
   {
        SoundManager.Instance?.ToggleMuteMusic(val.isOn);
   }
}
