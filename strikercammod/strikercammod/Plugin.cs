﻿
using Cinemachine;
using DevHoldableEngine;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utilla;
using BepInEx.Configuration;
using BepInEx;
using strikercammod.mainmanager;
using strikercammod.info;

namespace strikercammod
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(main.GUID, main.Name, main.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        bool isenabled = true;
        GameObject Camera;
        private ConfigEntry<float> FOV;
      

         void OnEnable()
         {
            isenabled = true;
              redo();
            Debug.Log(isenabled);


         }
        void OnDisable() 
        {
            isenabled = false;
            undosetup();
            Debug.Log(isenabled);
        }

        void Start()
        {

                   FOV = Config.Bind("FOV",
                   "change fov via config",
                   90f,
                   "simple way to change fov");
            Utilla.Events.GameInitialized += OnGameInitialized;
        }


        void OnGameInitialized(object sender, EventArgs e)
        {
            Debug.Log("setting stuff up!");
            var bundle = LoadAssetBundle("strikercammod.Reasoure.cammod");
            Camera = bundle.LoadAsset<GameObject>("cammod");
            Camera = Instantiate(Camera);
            Destroy(Camera.transform.Find("Model/Camera").gameObject.GetComponent<AudioListener>());
            GorillaTagger.Instance.thirdPersonCamera.transform.parent = Camera.transform;
            GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<CinemachineBrain>().enabled = false;
            GorillaTagger.Instance.thirdPersonCamera.transform.localPosition = new Vector3(-17.5024f, 0.7383f, 4.2542f);
            GorillaTagger.Instance.thirdPersonCamera.transform.localRotation = Quaternion.Euler(0f, 352.2897f, 0f);
            GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<Camera>().fieldOfView = FOV.Value;
            Camera.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Camera.transform.position = new Vector3(-65.0436f, 11.8873f, - 84.3991f);

            if(!isenabled)
            {
              undosetup();
            }
            else
            {
              redo(); 
            }

            Camera.AddComponent<DevHoldable>();
            Camera.AddComponent<Manager>().cam = Camera;
            
            if(Camera.GetComponent<Manager>() != null)
            {
                Debug.Log("added manager");
            }
          
        }

        void undosetup()
        {
            Debug.Log("d");
            Camera.SetActive(false);
             GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<CinemachineBrain>().enabled = true;
        }
        void redo()
        {
            Debug.Log("enabled");
            Camera.SetActive(true);
            GorillaTagger.Instance.thirdPersonCamera.GetComponentInChildren<CinemachineBrain>().enabled = false;
        }
        
                   
        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        


    }
}
