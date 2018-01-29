﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class SceneAmbassador : MonoBehaviour {

  [SerializeField]
  InputManager input;

  [SerializeField]
  PlayerManager players;

  [SerializeField]
  BaseManager bases;

  [SerializeField]
  Title title;

  [SerializeField]
  Title gameover;

  private AudioSource audio;
  private bool m_ToggleAudio;

  [SerializeField]
  bool m_Play;

  enum GameState {
    TITLE,
    PREGAME,
    PLAYING,
    GAMEOVER
  }
  GameState state;

  void SetState(GameState newstate) {
    print(newstate.ToString());

    if (state == GameState.GAMEOVER) {
      //gameover.Hide();
    }

    switch(newstate) {
      case GameState.TITLE:
        players.Reset();
        bases.Reset();
        ShowTitle();
        gameover.Hide();
        break;
      case GameState.PREGAME:
        
        break;
      case GameState.PLAYING:
        HideTitle();
        break;
      case GameState.GAMEOVER:
        gameover.Show();
        break;
    }

    state = newstate;
  }

  void Awake () {
    // initialize bindings
    initVars();
    InitPlayers();
    InitInput();

    SetState(GameState.TITLE);
  }

  void Update() {
    if (m_Play == true && m_ToggleAudio == true) {
      audio.Play();
      m_ToggleAudio = false;
    } else if (m_Play == false && m_ToggleAudio == true) {
      audio.Stop();
      m_ToggleAudio = false;
    }
  }

  void initVars() {
    audio = GetComponent<AudioSource>();
    // TODO: Uncomment me before shipping pls :D
    //m_Play = true;
    audio.volume = 0.5f;
    m_ToggleAudio = true;
  }

  void InitInput() {
    input.OnDevicePressX += delegate (CharacterActionz actions) {
      if (state == GameState.GAMEOVER) {
        SetState(GameState.TITLE);
      } else {
        // if we're in anything other than ENDGAME,
        // and this device is NOT mapped to a player
        // assign this device to an inactive player
        // and activate it
        if (!players.AreActionsMapped(actions)) {
          if (players.MapActions(actions)) {
            if (state == GameState.TITLE) {
              SetState(GameState.PLAYING);
            }

            // we just created a new player
            Player player = players.GetPlayer(actions);

            // assign it to a base and set its color
            player.SetBase(bases.GetBase());
          }
        }
      }
    };
  }

  void InitPlayers() {
    players.OnMagicEvent += delegate() {
      SetState(GameState.GAMEOVER);
    };
  }

  void ShowTitle() {
    title.Show();
  }
  void HideTitle() {
    title.Hide();
  }
}
