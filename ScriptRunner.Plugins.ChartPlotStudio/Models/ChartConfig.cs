using System.Linq;
using Microsoft.Extensions.Logging;
using ScottPlot;
using ScriptRunner.Plugins.ChartPlotStudio.Enums;

namespace ScriptRunner.Plugins.ChartPlotStudio.Models;

/// <summary>
/// Represents the configuration settings for generating various chart types.
/// </summary>
public class ChartConfig
{
    /// <summary>
    /// Gets or sets the type of chart to be generated.
    /// </summary>
    /// <value>
    /// The default value is <see cref="ChartType.Bar"/>.
    /// </value>
    public ChartType ChartType { get; set; } = ChartType.Bar;

    /// <summary>
    /// Gets or sets the title of the chart.
    /// </summary>
    /// <value>
    /// The default value is an empty string.
    /// </value>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the labels for the chart elements (e.g., axes or pie slices).
    /// </summary>
    public string[]? Labels { get; set; }

    /// <summary>
    /// Gets or sets the colors used for chart elements.
    /// </summary>
    public Color[]? Colors { get; set; }

    /// <summary>
    /// Gets or sets the base color used to generate random shades for chart elements.
    /// </summary>
    public Color? BaseColor { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to keep the colors null.
    /// </summary>
    public bool KeepColorsNull { get; set; }

    /// <summary>
    /// Gets or sets the label for the X-axis.
    /// </summary>
    /// <value>
    /// The default value is "X-Axis".
    /// </value>
    public string? XLabel { get; set; } = "X-Axis";

    /// <summary>
    /// Gets or sets the label for the Y-axis.
    /// </summary>
    /// <value>
    /// The default value is "Y-Axis".
    /// </value>
    public string? YLabel { get; set; } = "Y-Axis";

    /// <summary>
    /// Gets or sets a value indicating whether to display the legend on the chart.
    /// </summary>
    public bool ShowLegend { get; set; }

    /// <summary>
    /// Gets or sets the fraction by which pie chart slices are exploded.
    /// </summary>
    public double? PieExplodeFraction { get; set; }

    /// <summary>
    /// Gets or sets the distance of pie slice labels from the center.
    /// </summary>
    public double? PieSliceLabelDistance { get; set; }

    /// <summary>
    /// Gets or sets the number of bins for histograms.
    /// </summary>
    public int? HistogramBinCount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show markers on line-charts.
    /// </summary>
    public bool? LineShowMarkers { get; set; }

    /// <summary>
    /// Gets or sets the width percentage of bars for bar charts.
    /// </summary>
    /// <value>
    /// The default is 80% if not specified.
    /// </value>
    public double? BarWidthPercentage { get; set; }

    /// <summary>
    /// Logs all the property values of the ChartConfig instance.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> instance used for logging.</param>
    public void LogDebugValues(ILogger logger)
    {
        logger.LogDebug("ChartConfig Values:");
        logger.LogDebug("ChartType: {ChartType}", ChartType);
        logger.LogDebug("Title: {Title}", Title);
        logger.LogDebug("Labels: {Labels}", Labels != null ? string.Join(", ", Labels) : "null");
        logger.LogDebug("Colors: {Colors}", Colors != null ? string.Join(", ", Colors.Select(c => c.ToString())) : "null");
        logger.LogDebug("BaseColor: {BaseColor}", BaseColor?.ToString() ?? "null");
        logger.LogDebug("KeepColorsNull: {KeepColorsNull}", KeepColorsNull);
        logger.LogDebug("XLabel: {XLabel}", XLabel);
        logger.LogDebug("YLabel: {YLabel}", YLabel);
        logger.LogDebug("ShowLegend: {ShowLegend}", ShowLegend);
        logger.LogDebug("PieExplodeFraction: {PieExplodeFraction}", PieExplodeFraction?.ToString() ?? "null");
        logger.LogDebug("PieSliceLabelDistance: {PieSliceLabelDistance}", PieSliceLabelDistance?.ToString() ?? "null");
        logger.LogDebug("HistogramBinCount: {HistogramBinCount}", HistogramBinCount?.ToString() ?? "null");
        logger.LogDebug("LineShowMarkers: {LineShowMarkers}", LineShowMarkers?.ToString() ?? "null");
        logger.LogDebug("BarWidthPercentage: {BarWidthPercentage}", BarWidthPercentage?.ToString() ?? "null");
    }
}