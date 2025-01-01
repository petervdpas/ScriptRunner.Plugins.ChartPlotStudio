namespace ScriptRunner.Plugins.ChartPlotStudio.Enums;

/// <summary>
/// Represents the different types of charts that can be generated.
/// </summary>
public enum ChartType
{
    /// <summary>
    /// A chart that represents the frequency distribution of data using bars.
    /// </summary>
    Histogram,

    /// <summary>
    /// A circular chart divided into sectors, each representing a proportion of the whole.
    /// </summary>
    Pie,

    /// <summary>
    /// A chart that represents data points connected by straight line segments.
    /// Commonly used to show trends over time.
    /// </summary>
    Line,

    /// <summary>
    /// A chart that uses rectangular bars to compare values across categories.
    /// </summary>
    Bar
}