using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR; 

/* 
Utility class to deterimine VR device bitmasks 
https://steamcommunity.com/app/358720/discussions/0/1842367319519332721/
*/

public static class CrossDevice { 
    public enum Type { Vive, OculusTouch } 
    
    public static Type type { get; private set; } 
    public static ulong button_grab { get; private set; } 
    public static ulong button_grabTip { get; private set; } 
    public static ulong button_teleport { get; private set; } 
    public static ulong button_context { get; private set; } 
    public static ulong button_delete { get; private set; } 
    
    const string model_vive1 = "Vive MV"; 
    const string model_rift1 = "Oculus Rift CV1"; 
    
    public static void Init() { 
        // follow-up with devices here .. https://docs.unity3d.com/Manual/OpenVRControllers.html
        string model = UnityEngine.XR.XRDevice.model != null ? UnityEngine.XR.XRDevice.model : ""; 
        if ( model.IndexOf("Rift") >= 0 ) { 
            type = Type.OculusTouch; 
            ulong buttonXA = (1ul << (int)EVRButtonId.k_EButton_A); 
            button_grab = SteamVR_Controller.ButtonMask.Grip; 
            button_grabTip = SteamVR_Controller.ButtonMask.Trigger; 
            button_teleport = buttonXA; 
            button_context = SteamVR_Controller.ButtonMask.ApplicationMenu; 
            button_delete = SteamVR_Controller.ButtonMask.Touchpad; 
        } 
        else { 
            type = Type.Vive; 
            button_grab = SteamVR_Controller.ButtonMask.Trigger; 
            button_grabTip = SteamVR_Controller.ButtonMask.Trigger; 
            button_teleport = SteamVR_Controller.ButtonMask.Touchpad; 
            button_context = SteamVR_Controller.ButtonMask.ApplicationMenu; 
            button_delete = SteamVR_Controller.ButtonMask.Grip; 
        } 
    } 
    
    
} 
    