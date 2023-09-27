    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.PlayerLoop;

    public class UIPlayerInfo3D : MonoBehaviour
    {
        [SerializeField] private GameObject _ghostGraphic;
        [SerializeField] private Transform _ghostParent;
        [SerializeField] private GameObject _heartGraphic;
        [SerializeField] private Transform _heartParent;
        [SerializeField] private GameObject _scoreBoardObj;

        private UIScore _scoreBoard;
        private int _oldScore = 0;
        
        private List<Enemy> _ghosts = new ();

        private int _ghostCount = 0;

        private void OnEnable()
        {
            PlayerController.Instance.GettingCaught += UpdateLives;
            EnemyManager.Instance.EnemyUpdate += UpdateGhostCount;
            PlayerController.Instance.updatePreScore += UpdatePreScore;
            PlayerController.Instance.updateScore += UpdateScore;
            //GameOverseer.Instance.Playing += UpdateAll;
        }

        private void OnDisable()
        {
            PlayerController.Instance.GettingCaught -= UpdateLives;
            EnemyManager.Instance.EnemyUpdate -= UpdateGhostCount;
            PlayerController.Instance.updatePreScore -= UpdatePreScore;
            PlayerController.Instance.updateScore -= UpdateScore;
            //GameOverseer.Instance.Playing -= UpdateAll;
        }

        private void Start()
        {
            _scoreBoard = _scoreBoardObj.GetComponent<UIScore>(); 
            UpdateLives();
        }

        private void UpdateGhostCount()
        {
            _ghosts = EnemyManager.Instance.enemies;
            
            int nbChildren = _ghostParent.childCount;

            if (nbChildren != _ghosts.Count)
            {
                //new icons
                Vector3 ghostGraphicPosition = new (-180f, 4.7f, 11f);

                for (int i = 0; i < _ghosts.Count; i++)
                {
                    if (i >= nbChildren)
                    {
                        ghostGraphicPosition.x = -180f +  (-0.55f * i);
                        var _ghostGraph = Instantiate(_ghostGraphic, _ghostParent);
                        _ghostGraph.transform.localPosition = ghostGraphicPosition;
                        var ghostIcon = _ghostGraph.GetComponent<UIGhost>();
                        ghostIcon.SetGhost(_ghosts[i]);    
                    }
                }
            }
        }

        private void UpdateLives()
        {
            int nbChildren = _heartParent.childCount;
            int maxLives = GameOverseer.Instance.maxLives;
            int currentLives = GameOverseer.Instance.currentLives; 
            
            Vector3 heartGraphicPos = new (-197.8f, 4.7f, 11f);

            // add new heart blocks 
            if (maxLives > nbChildren)
            {
                for (int i = 0; i < PlayerController.Instance.maxLives; i++)
                {
                    if (i > nbChildren || nbChildren == 0)
                    {
                        heartGraphicPos.x = -197.8f + (0.6f * i);
                
                        var liveHeart = Instantiate(_heartGraphic, _heartParent);
                        liveHeart.transform.localPosition = heartGraphicPos;                        
                    }
                }    
            }
            
            // cycle through heart blocks and set them accordingly            
            for (int i = 0; i < PlayerController.Instance.maxLives; i++)
            {
                UIHeartBlock thisHeart = _heartParent.GetChild(i).gameObject.GetComponent<UIHeartBlock>();
                
                if (currentLives - 1 >= i)
                {
                    thisHeart.TurnOn();
                }
                else
                {
                    thisHeart.TurnOff();
                }
            }
        }

        private void UpdateScore()
        {
            int currentScore = PlayerController.Instance.score;

            int scoreAdded = currentScore - _oldScore;
            
            _scoreBoard.UpdateScore(scoreAdded, _oldScore);

            _oldScore = currentScore;
        }

        private void UpdatePreScore()
        {
            _scoreBoard.UpdatePreScore();
        }
    }