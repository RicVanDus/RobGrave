using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIPlayerInfo3D : MonoBehaviour
{
    [SerializeField] private GameObject _ghostGraphic;
    [SerializeField] private Transform _ghostParent;
    
    private List<Enemy> _ghosts = new ();

    private int _ghostCount = 0;
    
    private void OnEnable()
    {
        //PlayerController.Instance.GettingCaught += UpdateLives;
        EnemyManager.Instance.EnemyUpdate += UpdateGhostCount;
        //GameOverseer.Instance.Playing += UpdateAll;
    }

    private void OnDisable()
    {
        //PlayerController.Instance.GettingCaught -= UpdateLives;
        EnemyManager.Instance.EnemyUpdate -= UpdateGhostCount;
        //GameOverseer.Instance.Playing -= UpdateAll;
    }

    private void UpdateGhostCount()
    {
        _ghosts = EnemyManager.Instance.enemies;
        
        int nbChildren = _ghostParent.childCount;

        if (nbChildren != _ghosts.Count)
        {
            //new icons
            Vector3 _ghostGraphicPosition = new (-180f, 4.7f, 11f);

            for (int i = 0; i < _ghosts.Count; i++)
            {
                if (i >= nbChildren)
                {
                    _ghostGraphicPosition.x = -180f +  (-0.55f * i);
                    var _ghostGraph = Instantiate(_ghostGraphic, _ghostParent);
                    _ghostGraph.transform.localPosition = _ghostGraphicPosition;
                    var ghostIcon = _ghostGraph.GetComponent<UIGhost>();
                    ghostIcon.SetGhost(_ghosts[i]);    
                }
            }
        }
    }
}