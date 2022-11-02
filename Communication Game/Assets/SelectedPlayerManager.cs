using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectedPlayerManager : MonoBehaviour
{
    
    
    
    private int currentSelectedOneIndex = 0;
    private int currentSelectedTwoIndex = 1;

    /*public PlayerPrefs FirstPlayer;
    public PlayerPrefs SecondPlayer;
    */


    private int selectedOneId = -1;
    private int selectedTwoId = -1;
    private bool confirmedOne;
    private bool confirmedTwo;

    [SerializeField] Image player1;
    [SerializeField] Image player2;

    [SerializeField] private TextMeshProUGUI playerOneText;
    [SerializeField] private TextMeshProUGUI playerTwoText;

    private CharacterData[] slottedPlayers = new CharacterData[4];

    private void Awake()
    {
            
    }
    
    void Start()
    {
        slottedPlayers = GameManager.instance.slottedPlayers;
        ShowSprite();
    }

    public void Left(int id)
    {
        switch (id)
        {
            case 1:
                if(confirmedOne)
                    return;
                currentSelectedOneIndex -= 1;
                Debug.Log("ClickL1");
                if (currentSelectedOneIndex < 0)
                {
                    
                    currentSelectedOneIndex = slottedPlayers.Length - 1;
                }
               
                
                break;
            case 2:
                if(confirmedTwo)
                    return;
                currentSelectedTwoIndex -= 1;
                Debug.Log("ClickL2");
                if (currentSelectedTwoIndex < 0)
                {
                    currentSelectedTwoIndex = slottedPlayers.Length - 1;
                }
                
                break;
        }
        
        ShowSprite();
    }

    void ShowSprite()
    {
        player1.sprite = slottedPlayers[currentSelectedOneIndex].image;
        player2.sprite = slottedPlayers[currentSelectedTwoIndex].image;
        playerOneText.text = $"Player 1: {slottedPlayers[currentSelectedOneIndex].Name}";
        playerTwoText.text = $"Player 2: {slottedPlayers[currentSelectedTwoIndex].Name}";

       
        
        
        
    }
    public void Right(int id)
    {
        
        switch (id)
        {
            case 1:
                if(confirmedOne)
                    return;
                currentSelectedOneIndex += 1;
                if (currentSelectedOneIndex >= slottedPlayers.Length)
                {
                    
                    currentSelectedOneIndex = 0;
                }
                Debug.Log("ClickR1");
                break;
            case 2:
                if(confirmedTwo)
                    return;
                currentSelectedTwoIndex += 1;
                if (currentSelectedTwoIndex >= slottedPlayers.Length)
                {
                    
                    currentSelectedTwoIndex = 0;
                }
                Debug.Log("ClickR2");
                break;
        }
        
        ShowSprite();
    }
    
    // Start is called before the first frame update

    public void Confirm(int id)
    {
        
        switch (id)
        {
            case 1:
                if (confirmedOne)
                {
                    
                    CancelSelection(1);
                    break;
                }

                if (currentSelectedOneIndex != selectedTwoId)
                {
                    selectedOneId = currentSelectedOneIndex;
                    confirmedOne = true;
                    playerOneText.color = Color.green;
                }
                break;
            case 2:
                if (confirmedTwo)
                {
                    CancelSelection(2);
                    break;
                }

                if (currentSelectedTwoIndex != selectedOneId)
                {
                    selectedTwoId = currentSelectedTwoIndex;
                    confirmedTwo = true;
                    playerTwoText.color = Color.green;
                }
                break;
        }

        if (confirmedOne && confirmedTwo)
        {
            PlayerPrefs.SetInt("FirstPlayer", selectedOneId);
            PlayerPrefs.SetInt("SecondPlayer",  selectedTwoId);
            SpawnLevel();
        }
        
    }

    public void CancelSelection(int id)
    {
        switch (id)
        {
            case 1:
                if(!confirmedOne)
                    return;
                confirmedOne = false;
                selectedOneId = -1;
                playerOneText.color = Color.white;
                break;
            case 2:
                if(!confirmedTwo)
                    return;
                confirmedTwo = false;
                selectedTwoId = -1;
                playerTwoText.color = Color.white;
                break;
        }
    }
    void SpawnLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    
    
    
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
