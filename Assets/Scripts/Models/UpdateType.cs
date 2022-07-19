public enum UpdateType
{
    /// <summary>
    /// An item consumption failed, no value is updated.
    /// </summary>
    Failed = 0xFD,

    /// <summary>
    /// The maximum value is updated.
    /// </summary>
    Maximum = 0xFE,

    /// <summary>
    /// The current value is updated.
    /// </summary>
    Current = 0xFF
}