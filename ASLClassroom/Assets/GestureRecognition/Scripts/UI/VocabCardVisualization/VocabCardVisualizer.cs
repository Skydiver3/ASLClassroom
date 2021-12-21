using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabCardVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;

    //Array of displayed cards
    private List<GameObject> _completedCards = new List<GameObject>();
    private List<GameObject> _queuedCards = new List<GameObject>();
    private GameObject _activeCard;

    [SerializeField] private Transform activeCardSlot;
    [SerializeField] private Transform completedCardSlot;
    [SerializeField] private Transform queuedCardSlot;

    [SerializeField] private float cardDistance = 0.1f;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float cardMoveDuration = 1.0f;

    private Coroutine activateCardCoroutine;
    private bool activateCardCoroutineRunning;
    private Coroutine completeCardCoroutine;
    private bool completeCardCoroutineRunning;
    private Coroutine queuedCardCoroutine;
    private bool queuedCardCoroutineRunning;

    #region Card Commands
    public void InitCards(int n)
    {
        if (_queuedCards.Count == 0)
        {
            SpawnCards(n);
        }
    }

    public void RemoveCards()
    {
        HideCards();
    }

    public void DrawCard()
    {
        _activeCard = _queuedCards[0];
        _queuedCards.RemoveAt(0);

        if (activateCardCoroutineRunning) ArrangeCards();

        activateCardCoroutine = StartCoroutine(MoveCardToActive(_activeCard.transform));
    }

    public void PutAwayCard()
    {
        _completedCards.Add(_activeCard);
        Transform completedCard = _activeCard.transform;
        _activeCard = null;

        if (completeCardCoroutineRunning) ArrangeCards();

        completeCardCoroutine = StartCoroutine(MoveCardToCompleted(completedCard));
    }

    public void QueueCard()
    {
        _queuedCards.Add(_activeCard);
        Transform queuedCard = _activeCard.transform;
        _activeCard = null;

        if (queuedCardCoroutineRunning) ArrangeCards();

        queuedCardCoroutine = StartCoroutine(MoveCardToQueued(queuedCard));
    }

    public void NextCard()
    {
        if (_activeCard) PutAwayCard();
        if (_queuedCards.Count > 0) DrawCard();
    }

    #endregion

    #region Place/Show Card Commands
    private void SpawnCards(int n)
    {
        //fetch cards from card pool
        Transform[] poolCards = queuedCardSlot.GetComponentsInChildren<Transform>(true);
        List<Transform> salvagedCards = new List<Transform>();

        if (poolCards.Length > 0)
        {
            if (poolCards.Length < n)
            {
                //salvage all
                salvagedCards = new List<Transform>(poolCards);
                salvagedCards.RemoveAt(0);
            }
            else
            {
                //salvage as many as possible
                //(but don't include parent transform)
                for (int i = 1; i < n; i++)
                {
                    salvagedCards.Add(poolCards[i]);
                }                
            }

            //activate cards and add to queue
            for (int i = 0; i < salvagedCards.Count; i++)
            {
                GameObject salvagedCard = salvagedCards[i].gameObject;
                salvagedCard.SetActive(true);
                _queuedCards.Add(salvagedCard);
            }
        }

        //and spawn the rest
        for (int i = salvagedCards.Count - 1; i < n - 1; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, queuedCardSlot);
            _queuedCards.Add(newCard);
        }

        ArrangeCards();
    }

    private void HideCards()
    {
        StopAllCoroutines();
        activateCardCoroutineRunning = false;
        queuedCardCoroutineRunning = false;
        completeCardCoroutineRunning = false;

        if (_activeCard) _queuedCards.Add(_activeCard);
        foreach (GameObject card  in _completedCards)
        {
            _queuedCards.Add(card);
        }
        foreach (GameObject card in _queuedCards)
        {
            if (card) card.transform.SetParent(queuedCardSlot);
            if (card) card.SetActive(false);
        }

        _activeCard = null;
        _completedCards.Clear();
        _queuedCards.Clear();
    }


    private void ArrangeCards()
    {
        StopAllCoroutines();
        activateCardCoroutineRunning = false;
        queuedCardCoroutineRunning = false;
        completeCardCoroutineRunning = false;

        for (int i = 0; i < _completedCards.Count; i++)
        {
            _completedCards[i].transform.position = completedCardSlot.position + Vector3.right * i * cardDistance;
        }

        if(_activeCard) _activeCard.transform.position = activeCardSlot.position;

        for (int i = 0; i < _queuedCards.Count; i++)
        {
            _queuedCards[i].transform.position = queuedCardSlot.position + Vector3.right * i * cardDistance;
        }

    }
    #endregion

    #region Move Card Coroutines

    private IEnumerator MoveCardToActive(Transform card)
    {
        activateCardCoroutineRunning = true;

        Vector3 startPos = card.position;
        Vector3 endPos = activeCardSlot.position;
        float seconds = cardMoveDuration;

        //move in x steps in y seconds
        //speed = steps/second
        //total steps = speed*seconds
        //steps in one second = speed -> (1/number of steps per second) seconds between steps
        float totalSteps = seconds * speed;
        float stepDelay = 1 / speed;

        for (float i = 0; i < 1; i+=(1/totalSteps))
        {
            Vector3 nextPos = Vector3.Slerp(startPos, endPos, i);
            card.position = nextPos;
            yield return new WaitForSecondsRealtime(stepDelay);
        }
        card.position = endPos;
        activateCardCoroutineRunning = false;
    }
    private IEnumerator MoveCardToCompleted(Transform card)
    {
        completeCardCoroutineRunning = true;

        Vector3 startPos = card.position;
        Vector3 endPos = completedCardSlot.position + Vector3.right * _completedCards.Count * cardDistance;
        float seconds = cardMoveDuration;

        //move in x steps in y seconds
        //speed = steps/second
        //total steps = speed*seconds
        //steps in one second = speed -> (1/number of steps per second) seconds between steps
        float totalSteps = seconds * speed;
        float stepDelay = 1 / speed;

        for (float i = 0; i < 1; i += (1 / totalSteps))
        {
            Vector3 nextPos = Vector3.Slerp(startPos, endPos, i);
            card.position = nextPos;
            yield return new WaitForSecondsRealtime(stepDelay);
        }

        card.position = endPos;
        completeCardCoroutineRunning = false;
    }
    private IEnumerator MoveCardToQueued(Transform card)
    {
        queuedCardCoroutineRunning = true;

        Vector3 startPos = card.position;
        Vector3 endPos = queuedCardSlot.position + Vector3.right * _queuedCards.Count * cardDistance;
        float seconds = cardMoveDuration;

        //move in x steps in y seconds
        //speed = steps/second
        //total steps = speed*seconds
        //steps in one second = speed -> (1/number of steps per second) seconds between steps
        float totalSteps = seconds * speed;
        float stepDelay = 1 / speed;

        for (float i = 0; i < 1; i += (1 / totalSteps))
        {
            Vector3 nextPos = Vector3.Slerp(startPos, endPos, i);
            card.position = nextPos;
            yield return new WaitForSecondsRealtime(stepDelay);
        }

        card.position = endPos;
        queuedCardCoroutineRunning = false;
    }

    #endregion
}
