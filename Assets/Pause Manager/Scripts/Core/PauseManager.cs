using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if PAUSE_MANAGER_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
#if PAUSE_MANAGER_REWIRED
using Rewired;
#endif
#if STEAMWORKS_NET
using Steamworks;
#endif

namespace PauseManagement.Core
{
	/// <summary>
	/// 
	/// </summary>
	public class PauseManager : MonoBehaviour
	{
		public const string Version = "1.4.0";

		public delegate void PauseDelegateAction(bool paused);

		public static event PauseDelegateAction PauseAction;

		/// <summary>
		/// Use Unity's timeScale to stop time when paused ?
		/// </summary>
		public bool useTimeScale = true;

		/// <summary>
		/// Use Unity's Input Manager button to pause ?
		/// </summary>
		[SerializeField]
		private bool useUnityInputManager = true;

		/// <summary>
		/// The list of buttons to pause/resume.
		/// Can be used for local multiplayer (eg. Player 1 Pause, Player 2 Pause, etc).
		/// Default is one entry with "Cancel" value.
		/// </summary>
		[SerializeField]
		private string[] m_ButtonsList = null;

		/// <summary>
		/// Use Unity's Input System
		/// </summary>
		public bool useUnityInputSystem = false;

#if PAUSE_MANAGER_INPUT_SYSTEM
		/// <summary>
		/// The pause's input action
		/// </summary>
		public InputAction pauseAction = null;

		/// <summary>
		/// Use Input Action Asset's reference ?
		/// </summary>
		public bool useActionReference = false;

		/// <summary>
		/// The Input Action Asset's reference to apply to pauseInputAction
		/// </summary>
		public InputActionReference pauseActionReference = null;
#endif

		/// <summary>
		/// Use Rewired
		/// </summary>
		public bool useRewired = false;

#if PAUSE_MANAGER_REWIRED
		/// <summary>
		/// Pause when controller is disconnected.
		/// </summary>
		[SerializeField]
		private bool m_PauseOnControllerDisconnect = true;

		/// <summary>
		/// Resume when controller is connected.
		/// </summary>
		[SerializeField]
		private bool m_ResumeOnControllerConnect = true;

		/// <summary>
		/// Check all players for input.
		/// </summary>
		[SerializeField]
		private bool m_CheckAllPlayers = true;

		/// <summary>
		/// Optionally include the System Player ?
		/// </summary>
		[SerializeField]
		private bool m_IncludeSystemPlayer = false;

		/// <summary>
		/// The ID of players used to check for input.
		/// </summary>
		[SerializeField]
		private int[] m_PlayerIds = null;

		/// <summary>
		/// The name of the actions that represent Pause/Resume
		/// </summary>
		[SerializeField]
		private string[] m_ActionNames = null;

		/// <summary>
		/// The list of players acquired from Rewired
		/// </summary>
		private IList<Player> m_Players = null;
#endif

		/// <summary>
		/// Assign custom pause button from PlayerPrefs
		/// </summary>
		[SerializeField]
		private bool assignKeyFromPrefs = false;

		/// <summary>
		/// The list of property's name from PlayerPrefs to pause/resume.
		/// Can be used for local multiplayer (eg. Player 1 Pause, Player 2 Pause, etc).
		/// Default is one entry with 'Cancel' value.
		/// </summary>
		[SerializeField]
		private List<PauseProperty> m_PropertiesList = null;

		/// <summary>
		/// Custom keys for pausing, if you don't use Unity's Input Manager.
		/// </summary>
		[SerializeField]
		private List<KeyCode> m_PauseKeys = null;

#if STEAMWORKS_NET
		/// <summary>
		/// Pause when Steam Overlay is active.
		/// Resume when Steam Overlay is inactive.
		/// </summary>
		[SerializeField]
		private bool m_PauseOnSteamOverlayActive = true;
#endif

		/// <summary>
		/// Events triggered when paused
		/// </summary>
		[SerializeField]
		private UnityEvent pauseEvent = null;

		/// <summary>
		/// Events triggered when resumed
		/// </summary>
		[SerializeField]
		private UnityEvent resumeEvent = null;

		/// <summary>
		/// 
		/// </summary>
		private bool m_ExecuteEvents = true;

		/// <summary>
		/// 
		/// </summary>
		private bool m_ExecuteDelegateActions = true;

#if STEAMWORKS_NET
		/// <summary>
		/// 
		/// </summary>
		protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;
#endif

		// Reset to default values
		void Reset()
		{
			m_ButtonsList = new string[]
			{
				"Cancel"
			};
			m_PropertiesList = new List<PauseProperty>
			{
				new PauseProperty
				{
					name = "Pause",
					keyCode = KeyCode.Escape
				}
			};
			m_PauseKeys = new List<KeyCode>
			{
				KeyCode.Escape
			};

#if PAUSE_MANAGER_REWIRED
			m_PlayerIds = new int[] { 0 };
			m_ActionNames = new string[0];
#endif
		}

		// Awake is called before Start function
		void Awake()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			if (useUnityInputSystem)
			{
				if (useActionReference && pauseActionReference)
					pauseAction = pauseActionReference.action;

				pauseAction.performed += _ => TogglePause();
			}
#else
			useUnityInputSystem = false;
#endif

#if PAUSE_MANAGER_REWIRED
			if (useRewired)
			{
				if (ReInput.isReady && ReInput.players != null)
				{
					if (m_CheckAllPlayers)
						m_Players = ReInput.players.GetPlayers(m_IncludeSystemPlayer);
				}
			}
#else
			useRewired = false;
#endif

			if (assignKeyFromPrefs)
			{
				foreach (var property in m_PropertiesList)
				{
					SavePauseKeyOnPrefs(property.name, GetPauseKeyFromPrefs(property.name, property.keyCode));
				}
			}

			IsPaused = false;
		}

		// This function is called when the object becomes enabled and active
		void OnEnable()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			pauseAction.Enable();
#endif
#if PAUSE_MANAGER_REWIRED
			ReInput.ControllerConnectedEvent += OnControllerConnected;
			ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
#endif
#if STEAMWORKS_NET
			if (SteamManager.Initialized)
			{
				m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			}
#endif
		}

		// This function is called when the behaviour becomes disabled.
		void OnDisable()
		{
#if PAUSE_MANAGER_INPUT_SYSTEM
			pauseAction.Disable();
#endif
#if PAUSE_MANAGER_REWIRED
			ReInput.ControllerConnectedEvent -= OnControllerConnected;
			ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
#endif
		}

#if PAUSE_MANAGER_REWIRED
		// This function will be called when a controller is connected
		// You can get information about the controller that was connected via the args parameter
		void OnControllerConnected(ControllerStatusChangedEventArgs args)
		{
			if (m_ResumeOnControllerConnect)
				Resume();
		}

		// This function will be called when a controller is fully disconnected
		// You can get information about the controller that was disconnected via the args parameter
		void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (m_PauseOnControllerDisconnect)
				Pause();
		}
#endif

		// Update is called once per frame
		void Update()
		{
#if !PAUSE_MANAGER_INPUT_SYSTEM
			useUnityInputSystem = false;
#endif

			if (useUnityInputSystem) return;

#if PAUSE_MANAGER_REWIRED
			if (useRewired)
			{
				if (!ReInput.isReady || ReInput.players == null) return;

				if (m_CheckAllPlayers)
				{
					if (m_Players == null)
						m_Players = ReInput.players.GetPlayers(m_IncludeSystemPlayer);

					foreach (var player in m_Players)
					{
						foreach (var actionName in m_ActionNames)
						{
							if (player.GetButtonDown(actionName))
								TogglePause();
						}
					}
				}
				else
				{
					foreach (var playerId in m_PlayerIds)
					{
						var player = ReInput.players.GetPlayer(playerId);
						if (player != null)
						{
							foreach (var actionName in m_ActionNames)
							{
								if (player.GetButtonDown(actionName))
									TogglePause();
							}
						}
					}
				}

				return;
			}
#endif

			if (useUnityInputManager)
			{
				foreach (var buttonName in m_ButtonsList)
					if (Input.GetButtonDown(buttonName))
						TogglePause();
			}
			else if (assignKeyFromPrefs)
			{
				foreach (var property in m_PropertiesList)
				{
					if (Input.GetKeyDown(property.keyCode))
						TogglePause();
				}
			}
			else
			{
				foreach (var key in m_PauseKeys)
					if (Input.GetKeyDown(key))
						TogglePause();
			}
		}

		void OnApplicationPause(bool pause)
		{
			if (pause && !IsPaused)
				Pause();
		}

		public void TogglePause()
		{
			if (!IsPaused)
				Pause();
			else
				Resume();
		}

		public void Pause()
		{
			if (useTimeScale)
				StopTime();

			IsPaused = true;

			if (m_ExecuteEvents)
				pauseEvent.Invoke();

			if (m_ExecuteDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void Resume()
		{
			ResetTime();

			IsPaused = false;

			if (m_ExecuteEvents)
				resumeEvent.Invoke();

			if (m_ExecuteDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void StopTimeDelayed(float time)
		{
			Invoke(nameof(StopTime), time);
		}

		public void StopTime()
		{
			Time.timeScale = 0;
		}

		public void ResetTimeDelayed(float time)
		{
			Invoke(nameof(ResetTime), time);
		}

		public void ResetTime()
		{
			Time.timeScale = 1;
		}

		public void SavePauseKeyOnPrefs(string key, KeyCode keyCode)
		{
			PlayerPrefs.SetString(key, keyCode.ToString());
		}

		public KeyCode GetPauseKeyFromPrefs(string key, KeyCode defaultValue)
		{
			return (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(key, defaultValue.ToString()));
		}

		public void AddPauseProperty(string name, KeyCode keyCode)
		{
			AddPauseProperty(new PauseProperty
			{
				name = name,
				keyCode = keyCode
			});
		}

		public void AddPauseProperty(PauseProperty property)
		{
			m_PropertiesList.Add(property);

			SavePauseKeyOnPrefs(property.name, property.keyCode);
		}

		public void SetPauseProperty(string name, KeyCode keyCode)
		{
			if (m_PropertiesList.Exists(prop => prop.name == name))
			{
				var index = m_PropertiesList.FindIndex(prop => prop.name == name);

				m_PropertiesList[index] = new PauseProperty
				{
					name = name,
					keyCode = keyCode
				};

				SavePauseKeyOnPrefs(name, keyCode);
			}
			else
			{
				AddPauseProperty(name, keyCode);
			}
		}

		public void RemovePauseProperty(string name)
		{
			if (m_PropertiesList.Exists(prop => prop.name == name))
			{
				m_PropertiesList.RemoveAll(prop => prop.name == name);

				if (PlayerPrefs.HasKey(name))
				{
					PlayerPrefs.DeleteKey(name);
				}
			}
		}

		public void AddPauseKey(KeyCode keyCode)
		{
			m_PauseKeys.Add(keyCode);
		}

		public void RemovePauseKey(KeyCode keyCode)
		{
			RemovePauseKey(m_PauseKeys.FindIndex(key => key == keyCode));
		}

		public void RemovePauseKey(int index)
		{
			if (index < 0 || index >= m_PauseKeys.Count) return;

			m_PauseKeys.RemoveAt(index);
		}

#if STEAMWORKS_NET
		private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
		{
			if (m_PauseOnSteamOverlayActive)
			{
				if (pCallback.m_bActive != 0)
				{
					// Steam Overlay has been activated
					Pause();
				}
				else
				{
					// Steam Overlay has been closed
					Resume();
				}
			}
		}
#endif

		public static bool IsPaused { get; set; }

		public bool ExecuteEvents
		{
			set
			{
				m_ExecuteEvents = value;
			}
		}

		public bool ExecuteDelegateActions
		{
			set
			{
				m_ExecuteDelegateActions = value;
			}
		}

		[System.Serializable]
		public struct PauseProperty
		{
			public string name;
			public KeyCode keyCode;
		}
	}
}