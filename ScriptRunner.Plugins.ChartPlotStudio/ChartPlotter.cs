using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ScottPlot;
using ScottPlot.Avalonia;
using ScottPlot.Statistics;
using ScriptRunner.Plugins.ChartPlotStudio.Enums;
using ScriptRunner.Plugins.ChartPlotStudio.Interfaces;
using ScriptRunner.Plugins.ChartPlotStudio.Models;
using ScriptRunner.Plugins.Logging;

namespace ScriptRunner.Plugins.ChartPlotStudio;

/// <summary>
///     Provides methods to generate various types of charts using ScottPlot and Avalonia.
/// </summary>
public class ChartPlotter : IChartPlotter
{
    private readonly IPluginLogger _logger;
    private ChartConfig? _chartConfig;
    private double[] _data = [];

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChartPlotter" /> class.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}" /> instance for logging.</param>
    public ChartPlotter(IPluginLogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    ///     Sets up the chart service with configuration parameters for various chart types.
    /// </summary>
    /// <param name="chartType">The type of chart to create (e.g., Pie, Histogram, Line).</param>
    /// <param name="title">The title of the chart.</param>
    /// <param name="labels">Optional labels for the chart (e.g., for axes or slices).</param>
    /// <param name="colors">Optional list of color strings for the chart elements.</param>
    /// <param name="baseColorHex">Optional base color in hex to generate random shades for chart elements.</param>
    /// <param name="keepColorsNull"></param>
    /// <param name="showLegend">Whether to display the legend on the chart.</param>
    /// <param name="xLabel">Label for the X-axis (if applicable).</param>
    /// <param name="yLabel">Label for the Y-axis (if applicable).</param>
    /// <param name="pieExplodeFraction">Optional explode fraction for pie chart slices.</param>
    /// <param name="pieSliceLabelDistance">Optional label distance for pie chart slices.</param>
    /// <param name="histogramBinCount">Optional number of bins for histograms.</param>
    /// <param name="lineShowMarkers">Optional flag to show markers on line-charts.</param>
    /// <param name="barWidthPercentage">Optional percentage width of bars for bar charts (default is 80%).</param>
    public void Setup(
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
        double? barWidthPercentage = null)
    {
        _chartConfig = ChartConfigFactory.CreateDefaultConfig(chartType);

        var baseColor = string.IsNullOrWhiteSpace(baseColorHex)
            ? new Color(128, 128, 128) // Default gray
            : ParseColor(baseColorHex);

        // Set common properties
        _chartConfig.ChartType = chartType;
        _chartConfig.Title = title;
        _chartConfig.Labels = labels;
        _chartConfig.BaseColor = baseColor;
        _chartConfig.KeepColorsNull = keepColorsNull;
        _chartConfig.ShowLegend = showLegend;
        _chartConfig.XLabel = xLabel ?? "X-Axis";
        _chartConfig.YLabel = yLabel ?? "Y-Axis";

        _chartConfig.Colors = colors != null && !_chartConfig.KeepColorsNull
            ? ConvertColorStringsToColors(colors.ToList())
            : GenerateRandomColors(labels?.Length ?? 5, baseColor, 30);

        switch (_chartConfig.ChartType)
        {
            // Set pie chart-specific properties
            case ChartType.Pie:
                _chartConfig.PieExplodeFraction = pieExplodeFraction;
                _chartConfig.PieSliceLabelDistance = pieSliceLabelDistance;
                break;
            // Set histogram-specific properties
            case ChartType.Histogram:
                _chartConfig.HistogramBinCount = histogramBinCount;
                break;
            // Set line chart-specific properties
            case ChartType.Line:
                _chartConfig.LineShowMarkers = lineShowMarkers;
                break;
            // Set bar chart-specific properties
            case ChartType.Bar:
                _chartConfig.BarWidthPercentage = barWidthPercentage;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(chartType), chartType, null);
        }
    }

    /// <summary>
    ///     Retrieves the current chart configuration.
    /// </summary>
    /// <returns>The current <see cref="ChartConfig" /> if configured; otherwise, throws an exception.</returns>
    public ChartConfig GetConfig()
    {
        if (_chartConfig == null)
            throw new InvalidOperationException(
                "ChartConfig is not set. Call Setup before accessing the configuration.");

        return _chartConfig;
    }

    /// <summary>
    ///     Sets the dataset for the chart.
    /// </summary>
    public void SetData(double[] data)
    {
        _data = data;
    }

    /// <summary>
    ///     Retrieves the dataset.
    /// </summary>
    public double[] GetData()
    {
        return _data;
    }

    /// <summary>
    ///     Creates a chart of the specified type using the provided configuration and data.
    /// </summary>
    /// <param name="avaPlot">The <see cref="AvaPlot" /> instance where the chart will be rendered.</param>
    /// <exception cref="ArgumentException">Thrown if required parameters for a specific chart type are missing.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the specified chart type is unsupported.</exception>
    public void CreateChart(AvaPlot avaPlot)
    {
        if (_chartConfig == null)
            throw new InvalidOperationException("ChartConfig is not set. Call Setup before CreateChart.");

        // Clear the existing plot to prepare for new content
        avaPlot.Plot.Clear();

        _chartConfig.LogDebugValues(_logger);

        switch (_chartConfig.ChartType)
        {
            case ChartType.Pie:
                RenderPieChart(avaPlot, _data, _chartConfig);
                break;
            case ChartType.Histogram:
                RenderHistogram(avaPlot, _data, _chartConfig);
                break;
            case ChartType.Line:
                RenderLineChart(avaPlot, _data, _chartConfig);
                break;
            case ChartType.Bar: // New case for rendering bar charts
                RenderBarChart(avaPlot, _data, _chartConfig);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_chartConfig.ChartType),
                    $"Unsupported chart type: {_chartConfig.ChartType}");
        }

        avaPlot.Plot.Title(_chartConfig.Title);

        ConfigureLegend(avaPlot.Plot, _chartConfig);

        // Dynamically show legend
        if (_chartConfig.ShowLegend) avaPlot.Plot.ShowLegend();

        avaPlot.Refresh();
    }

    /// <summary>
    ///     Parses a color string into a ScottPlot-compatible Color object.
    /// </summary>
    /// <param name="colorString">The color string to parse (e.g., "Red", "#FF5733", "rgb(0,255,0)").</param>
    /// <returns>A ScottPlot-compatible Color object.</returns>
    /// <exception cref="ArgumentException">Thrown if the color string cannot be parsed.</exception>
    public Color ParseColor(string colorString)
    {
        try
        {
            if (colorString.StartsWith("Color #"))
            {
                // Extract hex part after "Color #"
                var hexPart = colorString.Split(' ')[1];
                return Color.FromHex(hexPart);
            }

            if (colorString.StartsWith('#'))
                // Hex format (e.g., #FF5733)
                return Color.FromHex(colorString);

            if (colorString.StartsWith("rgb", StringComparison.OrdinalIgnoreCase))
            {
                // RGB format (e.g., rgb(255,0,0))
                var rgb = colorString
                    .Replace("rgb", "", StringComparison.OrdinalIgnoreCase)
                    .Trim('(', ')')
                    .Split(',')
                    .Select(int.Parse)
                    .ToArray();

                return new Color(rgb[0], rgb[1], rgb[2]);
            }

            // Convert named colors to hex values and recursively parse
            var colorHex = colorString.ToLowerInvariant() switch
            {
                "red" => "#FF0000",
                "blue" => "#0000FF",
                "green" => "#008000",
                "yellow" => "#FFFF00",
                "orange" => "#FFA500",
                "purple" => "#800080",
                "pink" => "#FFC0CB",
                "black" => "#000000",
                "white" => "#FFFFFF",
                "gray" => "#808080",
                "cyan" => "#00FFFF",
                "magenta" => "#FF00FF",
                _ => throw new ArgumentException($"Unsupported color name: {colorString}")
            };

            // Recursively call ParseColor with the hex representation
            return Color.FromHex(colorHex);
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"Invalid color string: {colorString}", ex);
        }
    }

    /// <summary>
    ///     Converts a <see cref="Color" /> object to its hexadecimal string representation.
    /// </summary>
    /// <param name="color">The <see cref="Color" /> object to convert.</param>
    /// <returns>A string representing the color in the hexadecimal format <c>#RRGGBB</c>.</returns>
    public string ColorToHex(Color color)
    {
        return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
    }

    /// <summary>
    ///     Converts a list of color strings into an array of ScottPlot-compatible Color objects.
    /// </summary>
    /// <param name="colorStrings">A list of color strings (e.g., "Red", "#FF5733", "rgb(0,255,0)").</param>
    /// <returns>An array of Color objects.</returns>
    /// <exception cref="ArgumentException">Thrown if a color string cannot be parsed.</exception>
    private Color[] ConvertColorStringsToColors(List<string> colorStrings)
    {
        return colorStrings.Select(ParseColor).ToArray();
    }

    /// <summary>
    ///     Renders a histogram chart using the given data and configuration.
    /// </summary>
    /// <param name="avaPlot">The Avalonia plot instance to render on.</param>
    /// <param name="data">The data to create the histogram from.</param>
    /// <param name="config">Configuration for the histogram (e.g., bin count, axis labels).</param>
    private static void RenderHistogram(AvaPlot avaPlot, double[] data, ChartConfig config)
    {
        var binCount = config.HistogramBinCount ?? 10;
        var histogram = Histogram.WithBinCount(binCount, data);
        var barPlot = avaPlot.Plot.Add.Bars(histogram.Bins, histogram.Counts);

        foreach (var bar in barPlot.Bars) bar.Size = histogram.FirstBinSize * 0.8;

        // Set axis labels
        SetAxisLabels(avaPlot.Plot, config);

        avaPlot.Plot.Axes.Margins(bottom: 0);
    }

    /// <summary>
    ///     Renders a pie chart using the given data and configuration.
    /// </summary>
    /// <param name="avaPlot">The Avalonia plot instance to render on.</param>
    /// <param name="data">The data to create the pie chart from.</param>
    /// <param name="config">Configuration for the pie chart (e.g., colors, labels, explode fraction).</param>
    private static void RenderPieChart(AvaPlot avaPlot, double[] data, ChartConfig config)
    {
        // Ensure data and labels are aligned
        var dataLength = data.Length;
        var colors = config.Colors ?? GenerateDefaultColors(dataLength, config.BaseColor);
        var labels = config.Labels ?? GenerateDefaultLabels(dataLength);

        // Create the pie slices
        var slices = data.Select((t, i) => new PieSlice
        {
            Value = t,
            FillColor = colors[i % colors.Length], // Use modulo for cycling colors
            Label = labels.Length > i ? labels[i] : $"Slice {i + 1}" // Fallback for missing labels
        }).ToList();

        var pie = avaPlot.Plot.Add.Pie(slices);
        pie.ExplodeFraction = config.PieExplodeFraction ?? 0.0;
        pie.SliceLabelDistance = config.PieSliceLabelDistance ?? 1.0;
    }

    /// <summary>
    ///     Renders a line chart using the given data and configuration.
    /// </summary>
    /// <param name="avaPlot">The Avalonia plot instance to render on.</param>
    /// <param name="data">The data to create the line chart from.</param>
    /// <param name="config">Configuration for the line chart (e.g., axis labels, markers).</param>
    private static void RenderLineChart(AvaPlot avaPlot, double[] data, ChartConfig config)
    {
        var xs = new double[data.Length];
        for (var i = 0; i < xs.Length; i++) xs[i] = i;

        var scatter = avaPlot.Plot.Add.Scatter(xs, data);
        scatter.MarkerShape = config.LineShowMarkers ?? true ? MarkerShape.FilledCircle : MarkerShape.None;

        // Set axis labels
        SetAxisLabels(avaPlot.Plot, config);
    }

    /// <summary>
    ///     Renders a bar chart using the given data and configuration.
    /// </summary>
    /// <param name="avaPlot">The Avalonia plot instance to render on.</param>
    /// <param name="data">The data to create the bar chart from.</param>
    /// <param name="config">Configuration for the bar chart (e.g., bar width, axis labels).</param>
    private static void RenderBarChart(AvaPlot avaPlot, double[] data, ChartConfig config)
    {
        avaPlot.Plot.Add.Bars(data.Select((t, i) => new Bar
        {
            Position = i,
            Value = t,
            Size = config.BarWidthPercentage.HasValue
                ? config.BarWidthPercentage.Value / 100.0
                : 0.8, // Default to 80% width if not specified
            FillColor = config.Colors != null && config.Colors.Length > i
                ? config.Colors[i]
                : Colors.Gray, // Default color if not enough colors provided
            Label = config.Labels != null && config.Labels.Length > i
                ? config.Labels[i]
                : $"Bar {i + 1}" // Fallback label if none provided
        }).ToArray());

        // Set axis labels
        SetAxisLabels(avaPlot.Plot, config);

        // Adjust plot margins
        avaPlot.Plot.Axes.Margins(bottom: 0);
    }

    /// <summary>
    ///     Generates default labels for the given number of data points.
    /// </summary>
    /// <param name="count">The number of data points to generate labels for.</param>
    /// <returns>An array of default labels (e.g., "Slice 1", "Slice 2").</returns>
    private static string[] GenerateDefaultLabels(int count)
    {
        return Enumerable.Range(1, count).Select(i => $"Slice {i}").ToArray();
    }

    /// <summary>
    ///     Helper method to set X and Y labels on the plot.
    /// </summary>
    private static void SetAxisLabels(Plot plot, ChartConfig config)
    {
        plot.XLabel(config.XLabel ?? "X-axis");
        plot.YLabel(config.YLabel ?? "Y-axis");
    }

    /// <summary>
    ///     Generates an array of random colors by applying variations to a base color.
    /// </summary>
    /// <param name="count">The number of random colors to generate.</param>
    /// <param name="baseColor">The base color to apply variations to.</param>
    /// <param name="variation">
    ///     The maximum range of variation to apply to the RGB components of the base color.
    ///     Positive or negative adjustments are randomly applied within this range.
    /// </param>
    /// <returns>An array of randomly generated <see cref="Color" /> objects.</returns>
    /// <remarks>
    ///     This method keeps the alpha (opacity) value of the base color constant.
    /// </remarks>
    private static Color[] GenerateRandomColors(int count, Color baseColor, int variation)
    {
        var random = new Random();
        var colors = new Color[count];

        for (var i = 0; i < count; i++)
        {
            // Generate random variation within the specified range
            var r = ClampToByte(baseColor.R + random.Next(-variation, variation + 1));
            var g = ClampToByte(baseColor.G + random.Next(-variation, variation + 1));
            var b = ClampToByte(baseColor.B + random.Next(-variation, variation + 1));
            var a = baseColor.A; // Keep alpha constant (fully opaque)

            // Construct the new ScottPlot.Color
            colors[i] = new Color(r, g, b, a);
        }

        return colors;
    }

    /// <summary>
    ///     Generates an array of default colors based on a specified base color.
    /// </summary>
    /// <param name="count">The number of colors to generate.</param>
    /// <param name="baseColor">
    ///     The base color to derive other colors from. Defaults to <see cref="Colors.Gray" /> if not provided.
    /// </param>
    /// <returns>
    ///     An array of <see cref="Color" /> objects where each color is derived by incrementally varyIng
    ///     the red component of the base color.
    /// </returns>
    /// <remarks>
    ///     The red component of the base color is incremented by 15 for each color: cycling
    ///     through valid byte values.
    ///     If the number of colors exceeds the range of the red component,
    ///     the values will wrap around due to the modulo operation.
    /// </remarks>
    private static Color[] GenerateDefaultColors(int count, Color? baseColor = null)
    {
        var color = baseColor ?? Colors.Gray; // Use the provided base color, or default to Gray if null
        return Enumerable.Range(0, count)
            .Select(i => new Color(
                (byte)(color.R + i * 15 % 256), // Ensure wrapping with modulo operation
                color.G,
                color.B,
                color.A)) // Keep the alpha channel unchanged
            .ToArray();
    }

    /// <summary>
    ///     Clamps an integer value to the range of a byte (0 to 255).
    /// </summary>
    /// <param name="value">The integer value to clamp.</param>
    /// <returns>
    ///     A <see cref="byte" /> value representing the clamped value within the range of 0 to 255.
    /// </returns>
    /// <remarks>
    ///     This method is useful for ensuring color component values remain valid for RGB or RGBA formats.
    /// </remarks>
    private static byte ClampToByte(int value)
    {
        return (byte)Math.Clamp(value, 0, 255);
    }

    /// <summary>
    ///     Configures the legend for the plot based on the chart configuration.
    /// </summary>
    /// <param name="plot">The ScottPlot plot object to configure the legend on.</param>
    /// <param name="config">The chart configuration.</param>
    private static void ConfigureLegend(Plot plot, ChartConfig config)
    {
        if (config.ShowLegend)
        {
            var legend = plot.Legend; // Access the legend
            legend.IsVisible = true; // Ensure it's visible
            legend.Orientation = Orientation.Horizontal; // Set orientation (can be Horizontal or Vertical)
        }
        else
        {
            plot.Legend.IsVisible = false; // Hide the legend if not needed
        }
    }
}