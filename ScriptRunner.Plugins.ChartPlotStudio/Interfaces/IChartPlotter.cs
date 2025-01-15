using System;
using ScottPlot;
using ScottPlot.Avalonia;
using ScriptRunner.Plugins.ChartPlotStudio.Enums;
using ScriptRunner.Plugins.ChartPlotStudio.Models;

namespace ScriptRunner.Plugins.ChartPlotStudio.Interfaces;

/// <summary>
///     Defines methods for setting up, configuring, and rendering charts.
/// </summary>
public interface IChartPlotter
{
    /// <summary>
    ///     Configures the chart with the specified settings.
    /// </summary>
    /// <param name="chartType">The type of chart to create (e.g., Pie, Histogram, Line).</param>
    /// <param name="title">The title of the chart.</param>
    /// <param name="labels">Optional labels for the chart elements (e.g., for axes or slices).</param>
    /// <param name="colors">Optional list of color strings for the chart elements.</param>
    /// <param name="baseColorHex">Optional base color in hex format to generate random shades for chart elements.</param>
    /// <param name="keepColorsNull">Whether to keep colors null and use default color behavior.</param>
    /// <param name="showLegend">Whether to display the legend on the chart.</param>
    /// <param name="xLabel">Label for the X-axis (if applicable).</param>
    /// <param name="yLabel">Label for the Y-axis (if applicable).</param>
    /// <param name="pieExplodeFraction">Optional explode fraction for pie chart slices.</param>
    /// <param name="pieSliceLabelDistance">Optional label distance for pie chart slices.</param>
    /// <param name="histogramBinCount">Optional number of bins for histograms.</param>
    /// <param name="lineShowMarkers">Optional flag to show markers on line-charts.</param>
    /// <param name="barWidthPercentage">Optional percentage width of bars for bar charts (default is 80%).</param>
    void Setup(
        ChartType chartType,
        string title,
        string[]? labels = null,
        string[]? colors = null,
        string? baseColorHex = null,
        bool keepColorsNull = false,
        bool showLegend = false,
        string? xLabel = null,
        string? yLabel = null,
        double? pieExplodeFraction = null,
        double? pieSliceLabelDistance = null,
        int? histogramBinCount = null,
        bool? lineShowMarkers = null,
        double? barWidthPercentage = null);

    /// <summary>
    ///     Retrieves the current chart configuration.
    /// </summary>
    /// <returns>
    ///     The current <see cref="ChartConfig" /> instance if configured.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the chart configuration has not been set up.
    /// </exception>
    ChartConfig GetConfig();

    /// <summary>
    ///     Sets the data for the chart.
    /// </summary>
    /// <param name="data">An array of data values to be used in the chart.</param>
    void SetData(double[] data);

    /// <summary>
    ///     Retrieves the data set for the chart.
    /// </summary>
    /// <returns>
    ///     An array of data values used in the chart.
    /// </returns>
    double[] GetData();

    /// <summary>
    ///     Creates the chart and renders it on the specified plot.
    /// </summary>
    /// <param name="avaPlot">The <see cref="AvaPlot" /> instance where the chart will be rendered.</param>
    /// <exception cref="ArgumentException">
    ///     Thrown if required parameters for a specific chart type are missing.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the chart configuration has not been set up.
    /// </exception>
    void CreateChart(AvaPlot avaPlot);

    /// <summary>
    ///     Parses a color string into a <see cref="Color" /> object compatible with ScottPlot.
    /// </summary>
    /// <param name="colorString">
    ///     The color string to parse (e.g., "Red", "#FF5733", "rgb(0,255,0)").
    /// </param>
    /// <returns>A <see cref="Color" /> object.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the color string cannot be parsed.
    /// </exception>
    Color ParseColor(string colorString);

    /// <summary>
    ///     Converts a <see cref="Color" /> object to its hexadecimal string representation.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> object to convert.</param>
    /// <returns>
    ///     A string representing the color in hexadecimal format (e.g., "#RRGGBB").
    /// </returns>
    string ColorToHex(Color color);
}