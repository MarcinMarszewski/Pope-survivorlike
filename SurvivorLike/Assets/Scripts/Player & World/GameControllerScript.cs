using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    float spawnTime;
    [SerializeField]
    GameObject dummy;
    [SerializeField]
    GameObject player;
    [SerializeField]
    Image iconImage;
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    Text gameTime;
    [SerializeField]
    Text skillPointIndicator;
    [SerializeField]
    Canvas menu;

    public bool isPaused = false;

    float lastSpawnTime=0;
    System.Random rnd = new System.Random();

    void Start()
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag("SkillButton");
        foreach (var item in arr)
        {
            item.GetComponent<SkillButtonScript>().CheckAndUpdateSkillAvailability();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lastSpawnTime + spawnTime < Time.time) SpawnDummy();
        iconImage.sprite = playerController.Spells[playerController.CurrentChosenSpellPos].GetComponent<BasicSpellScript>().Icon;

        if (Input.GetKeyDown(KeyCode.P)) HandlePause();

        int minutes = (int)(Time.time / 60f);
        int seconds = (int)Time.time - (minutes * 60);
        string min, sec;
        if (minutes < 10) min = "0" + minutes.ToString();
        else min = minutes.ToString();
        if (seconds < 10) sec = "0" + seconds.ToString();
        else sec = seconds.ToString();
        gameTime.text = min + ":" + sec;
    }

    void SpawnDummy()
    {
        GameObject obj =Instantiate<GameObject>(dummy, player.transform.position+new Vector3(rnd.Next(60,90)/10.0f,0,0), Quaternion.identity);
        obj.transform.RotateAround(player.transform.position, new Vector3(0, 0, 1), rnd.Next(0, 359));
        obj.transform.rotation = Quaternion.identity;

        lastSpawnTime = Time.time;
    }

    public void UpdateSkillPointIndicator()
    {
        skillPointIndicator.text = player.GetComponent<PlayerController>().skillPoints.ToString();
        if (playerController.skillPoints>0)
        {
            skillPointIndicator.enabled = true;
            return;
        }
        skillPointIndicator.enabled = false;
    }

    private void HandlePause()
    {
        isPaused = !isPaused;
        menu.enabled = isPaused;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;

        GameObject[] arr = GameObject.FindGameObjectsWithTag("SkillButton");
        foreach (var item in arr)
        {
            item.GetComponent<SkillButtonScript>().CheckAndUpdateSkillAvailability();
        }
    }
}
