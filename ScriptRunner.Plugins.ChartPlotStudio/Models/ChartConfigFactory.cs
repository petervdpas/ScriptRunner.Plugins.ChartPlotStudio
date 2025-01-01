using System;
using ScottPlot;
using ScriptRunner.Plugins.ChartPlotStudio.Enums;

namespace ScriptRunner.Plugins.ChartPlotStudio.Models;

/// <summary>
///     Factory for creating default configurations for different chart types.
/// </summary>
public static class ChartConfigFactory
{
    /// <summary>
    ///     Creates a default chart configuration based on the specified chart type.
    /// </summary>
    /// <param name="chartType">The type of chart for which to create a configuration.</param>
    /// <returns>A <see cref="ChartConfig" /> instance preconfigured for the specified chart type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the chart type is unsupported.</exception>
    public static ChartConfig CreateDefaultConfig(ChartType chartType)
    {
        return chartType switch
        {
            ChartType.Pie => new ChartConfig
            {
                Title = "Pie Chart",
                PieExplodeFraction = 0.0,
                PieSliceLabelDistance = 1.0,
                Colors = [Colors.Red, Colors.Orange, Colors.Yellow, Colors.Green, Colors.Blue]
            },
            ChartType.Histogram => new ChartConfig
            {
                Title = "Histogram",
                HistogramBinCount = 10
            },
            ChartType.Line => new ChartConfig
            {
                Title = "Line Chart",
                LineShowMarkers = true
            },
            ChartType.Bar => new ChartConfig
            {
                Title = "Bar Chart",
                BarWidthPercentage = 80
            },
            _ => throw new ArgumentOutOfRangeException(nameof(chartType), $"Unsupported chart type: {chartType}")
        };
    }
}