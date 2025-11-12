using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all UI screens/panels.
/// Provides common functionality for showing, hiding, and managing UI screens.
/// Inherit from this class for all major UI panels (Inventory, Crafting, etc.)
/// </summary>
public class UIScreen : MonoBehaviour
{
    [Header("Screen Settings")]
    [SerializeField] private string screenName;
    [SerializeField] private bool showOnAwake = false;
    [SerializeField] private bool registerWithManager = true;

    [Header("Animation (Optional)")]
    [SerializeField] private bool useAnimation = false;
    [SerializeField] private float animationDuration = 0.3f;

    [Header("Events")]
    public UnityEvent OnScreenShown;
    public UnityEvent OnScreenHidden;

    private CanvasGroup canvasGroup;

    // Public Properties
    public string ScreenName => screenName;
    public bool IsVisible { get; private set; }

    protected virtual void Awake()
    {
        // Auto-assign screen name if empty
        if (string.IsNullOrEmpty(screenName))
        {
            screenName = gameObject.name;
        }

        // Get or add CanvasGroup for fade animations
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null && useAnimation)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Register with UIManager
        if (registerWithManager && UIManager.Instance != null)
        {
            UIManager.Instance.RegisterScreen(this);
        }

        // Set initial visibility
        if (!showOnAwake)
        {
            gameObject.SetActive(false);
            IsVisible = false;
        }
    }

    protected virtual void Start()
    {
        if (showOnAwake)
        {
            Show();
        }
    }

    protected virtual void OnDestroy()
    {
        // Unregister from UIManager
        if (registerWithManager && UIManager.Instance != null)
        {
            UIManager.Instance.UnregisterScreen(this);
        }
    }

    /// <summary>
    /// Shows this screen
    /// </summary>
    public virtual void Show()
    {
        gameObject.SetActive(true);
        IsVisible = true;

        if (useAnimation && canvasGroup != null)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        OnShow();
        OnScreenShown?.Invoke();
    }

    /// <summary>
    /// Hides this screen
    /// </summary>
    public virtual void Hide()
    {
        IsVisible = false;

        if (useAnimation && canvasGroup != null)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            gameObject.SetActive(false);
        }

        OnHide();
        OnScreenHidden?.Invoke();
    }

    /// <summary>
    /// Toggle visibility
    /// </summary>
    public void Toggle()
    {
        if (IsVisible)
            Hide();
        else
            Show();
    }

    /// <summary>
    /// Called when screen is shown (override in subclasses for custom behavior)
    /// </summary>
    protected virtual void OnShow()
    {
        // Override in subclasses to add custom show behavior
    }

    /// <summary>
    /// Called when screen is hidden (override in subclasses for custom behavior)
    /// </summary>
    protected virtual void OnHide()
    {
        // Override in subclasses to add custom hide behavior
    }

    /// <summary>
    /// Fade in animation coroutine
    /// </summary>
    private System.Collections.IEnumerator FadeIn()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / animationDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
    }

    /// <summary>
    /// Fade out animation coroutine
    /// </summary>
    private System.Collections.IEnumerator FadeOut()
    {
        if (canvasGroup == null) yield break;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / animationDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
