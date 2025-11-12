using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Central manager for all UI screens and panels.
/// Handles showing/hiding screens, transitions, and global UI state.
/// Singleton that persists across scenes.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("UI Screens")]
    [SerializeField] private List<UIScreen> allScreens = new List<UIScreen>();

    [Header("Settings")]
    [SerializeField] private bool hideOnStart = true;

    private UIScreen currentScreen;
    private Stack<UIScreen> screenHistory = new Stack<UIScreen>();

    protected override void Awake()
    {
        base.Awake();

        // Auto-register all screens in children if list is empty
        if (allScreens.Count == 0)
        {
            allScreens.AddRange(GetComponentsInChildren<UIScreen>(true));
            Debug.Log($"UIManager: Auto-registered {allScreens.Count} screens");
        }

        if (hideOnStart)
        {
            HideAllScreens();
        }
    }

    /// <summary>
    /// Shows a specific UI screen by name
    /// </summary>
    /// <param name="screenName">Name of the screen to show</param>
    /// <param name="addToHistory">Whether to add current screen to history</param>
    public void ShowScreen(string screenName, bool addToHistory = true)
    {
        UIScreen screen = allScreens.Find(s => s.ScreenName == screenName);
        if (screen != null)
        {
            ShowScreen(screen, addToHistory);
        }
        else
        {
            Debug.LogWarning($"UIManager: Screen '{screenName}' not found");
        }
    }

    /// <summary>
    /// Shows a specific UI screen
    /// </summary>
    /// <param name="screen">The screen to show</param>
    /// <param name="addToHistory">Whether to add current screen to history</param>
    public void ShowScreen(UIScreen screen, bool addToHistory = true)
    {
        if (screen == null)
        {
            Debug.LogWarning("UIManager: Attempted to show null screen");
            return;
        }

        // Add current screen to history for back navigation
        if (addToHistory && currentScreen != null && currentScreen != screen)
        {
            screenHistory.Push(currentScreen);
        }

        // Hide current screen
        if (currentScreen != null && currentScreen != screen)
        {
            currentScreen.Hide();
        }

        // Show new screen
        currentScreen = screen;
        currentScreen.Show();

        Debug.Log($"UIManager: Showing screen '{screen.ScreenName}'");
    }

    /// <summary>
    /// Goes back to previous screen in history
    /// </summary>
    public void GoBack()
    {
        if (screenHistory.Count > 0)
        {
            UIScreen previousScreen = screenHistory.Pop();
            ShowScreen(previousScreen, addToHistory: false);
        }
        else
        {
            Debug.LogWarning("UIManager: No screens in history to go back to");
        }
    }

    /// <summary>
    /// Hides the current screen
    /// </summary>
    public void HideCurrentScreen()
    {
        if (currentScreen != null)
        {
            currentScreen.Hide();
            currentScreen = null;
        }
    }

    /// <summary>
    /// Hides all UI screens
    /// </summary>
    public void HideAllScreens()
    {
        foreach (var screen in allScreens)
        {
            if (screen != null)
            {
                screen.Hide();
            }
        }
        currentScreen = null;
        screenHistory.Clear();
    }

    /// <summary>
    /// Registers a screen with the manager
    /// </summary>
    /// <param name="screen">Screen to register</param>
    public void RegisterScreen(UIScreen screen)
    {
        if (screen != null && !allScreens.Contains(screen))
        {
            allScreens.Add(screen);
            Debug.Log($"UIManager: Registered screen '{screen.ScreenName}'");
        }
    }

    /// <summary>
    /// Unregisters a screen from the manager
    /// </summary>
    /// <param name="screen">Screen to unregister</param>
    public void UnregisterScreen(UIScreen screen)
    {
        if (screen != null && allScreens.Contains(screen))
        {
            allScreens.Remove(screen);
            Debug.Log($"UIManager: Unregistered screen '{screen.ScreenName}'");
        }
    }

    /// <summary>
    /// Shows a temporary popup message
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <param name="icon">Optional icon to show</param>
    public void ShowPopup(string message, Sprite icon = null)
    {
        // TODO: Implement popup UI system
        Debug.Log($"[POPUP] {message}");
    }

    /// <summary>
    /// Shows a confirmation dialog with callbacks
    /// </summary>
    /// <param name="message">Confirmation message</param>
    /// <param name="onConfirm">Action to call if user confirms</param>
    /// <param name="onCancel">Action to call if user cancels</param>
    public void ShowConfirmation(string message, System.Action onConfirm, System.Action onCancel = null)
    {
        // TODO: Implement confirmation dialog UI
        Debug.Log($"[CONFIRM] {message}");

        // For now, auto-confirm for testing
        onConfirm?.Invoke();
    }

    /// <summary>
    /// Checks if a specific screen is currently visible
    /// </summary>
    /// <param name="screenName">Name of the screen</param>
    /// <returns>True if screen is visible</returns>
    public bool IsScreenVisible(string screenName)
    {
        return currentScreen != null && currentScreen.ScreenName == screenName;
    }

    /// <summary>
    /// Gets the currently visible screen
    /// </summary>
    /// <returns>Current screen or null</returns>
    public UIScreen GetCurrentScreen()
    {
        return currentScreen;
    }

    /// <summary>
    /// Debug: Print all registered screens
    /// </summary>
    [ContextMenu("Debug: Print All Screens")]
    public void DebugPrintScreens()
    {
        Debug.Log("=== UI MANAGER - REGISTERED SCREENS ===");
        Debug.Log($"Total Screens: {allScreens.Count}");
        Debug.Log($"Current Screen: {(currentScreen != null ? currentScreen.ScreenName : "None")}");
        Debug.Log($"History Count: {screenHistory.Count}");

        foreach (var screen in allScreens)
        {
            if (screen != null)
            {
                Debug.Log($"- {screen.ScreenName} (Visible: {screen.IsVisible})");
            }
        }
        Debug.Log("=======================================");
    }
}
