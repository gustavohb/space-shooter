using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickSound : MonoBehaviour
{
    [SerializeField] SoundLibrary.Sound sound;

    private Button button { get { return GetComponent<Button>(); } }

    private void Start()
    {
        button.onClick.AddListener(() => PlaySoud());
    }


    private void PlaySoud()
    {
        AudioManager.Instance.PlaySound2D(sound);
    }
}
