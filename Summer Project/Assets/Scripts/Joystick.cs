using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Axis Direction Enumeration for Joystick
/// </summary>
public enum JoysticAxis
{
    Both,
    Horizontal,
    Vertical
}

/// <summary>
/// Joystick for Unity
/// </summary>
/// <example>Simple movement with Joystick:
/// <code>
///  public class TestPlayer : MonoBehaviour
///  {
///      public Joystick PlayerJoystick;
///      public float Speed = 10;
///
///      void Update()
///      {
///          // Movement
///          transform.Translate(Vector3.up * Speed * PlayerJoystick.Vertical * Time.deltaTime);
///          transform.Translate(Vector3.right * Speed * PlayerJoystick.Horizontal * Time.deltaTime);
///      }
///  }
/// </code>
/// </example>
public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Stick Move")]

    /// <summary>
    /// Stick sensitive value (range from 0 to 1)
    /// Do not enter a value that exceeds the range.
    /// </summary>
    [Range(0.0f, 1.0f)]
    public float StickSensitive = 1;

    /// <summery>
    /// Stick movement limit
    /// </summery>
    public int StickMoveLimit = 10;

    // Stick movement axix direction
    // Restricting movement horizontal or vertically.
    [SerializeField] private JoysticAxis axisDirection;

    // Joystick parts
    [Header("Rects")]
    public RectTransform BackgroundRect;
    public RectTransform HandleRect;

    // When AxisDircetion changes, the background sprites changes.
    // If the sprite is null, it is not changed.
    [Header("Background Sprites")]
    public Sprite CircleBackground;
    public Sprite HorizontalBackground;
    public Sprite VerticalBackground;

    /// <summary>
    /// Restricting movement horizontal or vertically
    /// </summary>
    public JoysticAxis AxisDirection {
        set { SetAxisDirection(value); }
        get { return axisDirection; }
    }

    /// <summary>
    /// The stick position
    /// </summary>
    public Vector2 Position { get { return position; } }

    /// <summary>
    /// The stick horizontal position
    /// </summary>
    public float Horizontal { get { return position.x; } }

    /// <summary>
    /// The stick vertical position
    /// </summary>
    public float Vertical { get { return position.y; } }

    private Vector2 position = Vector2.zero;
    private Vector2 lastPosition;




    private void Awake()
    {
    }


    }

    private void SetAxisDirection(JoysticAxis value)
    {
        // Set an AxisDirection
        axisDirection = value;

        // Get an Image instance
        Image backgroundImage = BackgroundObject.GetComponent<Image>();

        // Bath
        if (CircleBackground != null && axisDirection == JoysticAxis.Both)
            backgroundImage.sprite = CircleBackground;

        // Horizontal
        else if (HorizontalBackground != null && axisDirection == JoysticAxis.Horizontal)
            backgroundImage.sprite = HorizontalBackground;

        // Vertical
        else if (VerticalBackground != null && axisDirection == JoysticAxis.Vertical)
            backgroundImage.sprite = VerticalBackground;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Reset position
        position = Vector2.zero;
        
        // Reset handle anchoredPosition
        HandleRect.anchoredPosition = Vector2.zero;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            BackgroundRect, eventData.position, eventData.pressEventCamera, out position))
        {
            // Set position
            position /= BackgroundRect.sizeDelta * StickSensitive;

            // Check AxisDirection
            if (axisDirection == JoysticAxis.Horizontal) position.y = 0;
            else if (axisDirection == JoysticAxis.Vertical) position.x = 0;

            // When position is too far, normalize position magnitude to 1
            if (position.magnitude > 1.0f) position = position.normalized;

            // Set handle anchoredPosition
            handleRectTransform.anchoredPosition = position * (backgroundRectTransform.position / StickMoveLimit);
        }
    }
}
