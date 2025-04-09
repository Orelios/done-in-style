using UnityEngine;
using FMOD.Studio;
public class PauseMenuSounds : MonoBehaviour
{
    public void PhoneActivated(){ AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneActivate); }

    public void PhoneDeactivated(){ AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneDeactivate); }

    public void PhoneHover(){ AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneHover); }

    public void PhoneClick(){ AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneClick); }

    public void PhoneExit(){ AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.PhoneExit); }

    public void MenuHover() { AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.MainMenuHover); }

    public void MenuClick() { AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.MainMenuClick); }

    public void MenuExit() { AudioManager.instance.PlayOneShotNoLocation(FMODEvents.instance.MainMenuExit); } 
}
