using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class boardScript : MonoBehaviour {

	public GameObject[] cardValidPositions;
	public GameObject[] gameCards;
	public GameObject winningPanel;

	public List<int> legalStepsAtCurrentState;
	public Stack<int> solutionToThisState;
	public bool manualPlaying = true;
	public string currentStatus = "";
	public int gameplayTimer = 180;
	public int framesPerAStarTimerReset = 100;

	public Text gameplayTimerUI;
	public Image gameplayTimerBackground;
	public Image star1, star2, star3;

	public Sprite fullStar, emptyStar;

	private float astarMovementTimer = 0f;

	void Start() {
		
		while (currentStatus == "")
			currentStatus = _8Puzzle.generateRandomGameState();
		
		matchNumbersWithValidPositions();
		
		legalStepsAtCurrentState = _8Puzzle.getLegalMovementsInString(currentStatus);
	}
	
	void Update () 
	{
		var remainingGameplayTime = gameplayTimer - (int)Time.timeSinceLevelLoad;
		gameplayTimerUI.text = $"{remainingGameplayTime / 60} : {remainingGameplayTime % 60}";
		
		if (remainingGameplayTime < 20)
			gameplayTimerBackground.color = Color.red;
		
		if(remainingGameplayTime == 0 && manualPlaying)
        {
			Time.timeScale = 0f;
			FindObjectOfType<UIFunctionsScript>().transform.Find("gameFailurePanel")
				.gameObject.SetActive(true);
        }


        if (manualPlaying)
        {
			var clickedCard = detectClickedGameCard();
			if (clickedCard != -1 && checkIfLegalCard(clickedCard))
			{
				var stepsToMoveBlankCard = currentStatus.IndexOf(clickedCard.ToString()) - currentStatus.IndexOf('0');
				moveNumbersWithStep(stepsToMoveBlankCard);
			}
        }
		else
        {
			astarMovementTimer++;
			if (solutionToThisState.Count != 0 && astarMovementTimer > framesPerAStarTimerReset)
			{
				moveNumbersWithStep(solutionToThisState.Pop());
				astarMovementTimer = 0;
			}
			else if(solutionToThisState.Count == 0) manualPlaying = true;
        }
    }

	int detectClickedGameCard()
    {
		if(Input.GetMouseButtonDown(0))
        {
			Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			var clickedObject = Physics2D.Raycast(camRay.origin, camRay.direction, 10f);
			if (clickedObject)
				return Convert.ToInt32(clickedObject.collider.gameObject.name);
        }
		return -1;
    }
	bool checkIfLegalCard(int cardNumber)
    {
		int blankCardPositionInString = currentStatus.IndexOf('0');
		int blankCardNextPositionInString = currentStatus.IndexOf(cardNumber.ToString());
		if (legalStepsAtCurrentState.Contains(blankCardNextPositionInString - blankCardPositionInString))
			return true;
		else return false;
    }
	void moveNumbersWithStep(int step)
    {
		int zeroPositionInStateString = currentStatus.IndexOf('0');
		int numberToReplaceZero;
		
		numberToReplaceZero = (int)Char.GetNumericValue
			(currentStatus[zeroPositionInStateString + step]);

		currentStatus = _8Puzzle.swapTwoCharInAString
			(currentStatus, zeroPositionInStateString, zeroPositionInStateString + step);
		swapZeroCardWithAnother(gameCards[numberToReplaceZero]);

		if (_8Puzzle.hasReachedFinalState(currentStatus))
		{
			matchStarsWithScore();
			winningPanel.SetActive(true);
		}

		legalStepsAtCurrentState =_8Puzzle.getLegalMovementsInString(currentStatus);
    }
	void matchNumbersWithValidPositions()
    {
        for (int i = 0; i < currentStatus.Length; i++)
        {
			int valueAtThisPlace = (int)Char.GetNumericValue(currentStatus[i]);
			gameCards[valueAtThisPlace].transform.position = cardValidPositions[i].transform.position;
        }
    }
	void matchStarsWithScore()
    {
		int score = (int)((gameplayTimer - Time.timeSinceLevelLoad) / (gameplayTimer / 60));
        switch (score)
        {
			case 0:
				star1.sprite = fullStar;
				star2.sprite = emptyStar;
				star3.sprite = emptyStar;
				break;
			case 1:
				star1.sprite = fullStar;
				star2.sprite = fullStar;
				star3.sprite = emptyStar;
				break;
			case 2:
				star1.sprite = fullStar;
				star2.sprite = fullStar;
				star3.sprite = fullStar;
				break;
        }
    }
	void swapZeroCardWithAnother(GameObject otherCard)
    {
		Vector3 tempPosition = otherCard.transform.position;
		otherCard.transform.position = gameCards[0].transform.position;
		gameCards[0].transform.position = tempPosition;
	}
}
