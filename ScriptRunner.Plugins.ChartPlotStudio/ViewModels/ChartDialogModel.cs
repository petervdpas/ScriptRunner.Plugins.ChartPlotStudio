using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using ScottPlot.Avalonia;
using ScriptRunner.Plugins.ChartPlotStudio.Enums;
using ScriptRunner.Plugins.ChartPlotStudio.Interfaces;
using ScriptRunner.Plugins.ChartPlotStudio.Models;

namespace ScriptRunner.Plugins.ChartPlotStudio.ViewModels;

/// <summary>
/// Represents the view model for the chart dialog, managing the chart configuration and user interactions.
/// </summary>
public class ChartDialogModel : ReactiveObject
{
    private readonly Window? _dialog;
    private AvaPlot? _avaPlot;
    private string? _baseColorHex;
    private ChartType _selectedChartType;
    private Color _selectedColor;
    private string _statusMessage = "Ready";

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartDialogModel"/> class.
    /// </summary>
    /// <param name="dialog">The parent dialog window associated with this view model.</param>
    /// <param name="chartPlotter">The chart plotter instance for managing chart creation and configuration.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="chartPlotter"/> is null.
    /// </exception>
    public ChartDialogModel(Window dialog, IChartPlotter chartPlotter)
    {
        _dialog = dialog;
        ChartPlotter = chartPlotter ?? throw new ArgumentNullException(nameof(chartPlotter));

        ChartTypes = new ObservableCollection<ChartType>((ChartType[])Enum.GetValues(typeof(ChartType)));

        QuitCommand = ReactiveCommand.Create(CloseDialog);
        ApplyChangesCommand = ReactiveCommand.Create(ApplyChanges);

        ChartConfig = chartPlotter.GetConfig();
        SelectedChartType = ChartConfig.ChartType;
        if (ChartConfig.BaseColor != null)
        {
            BaseColorHex = ChartPlotter.ColorToHex(ChartConfig.BaseColor.Value);
            SelectedColor = ToAvaloniaColor(ChartConfig.BaseColor.Value); // Use conversion here
        }

        UpdateChart();
    }

    /// <summary>
    /// Gets or sets the currently selected color.
    /// </summary>
    public Color SelectedColor
    {
        get => _selectedColor;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedColor, value);
            // Synchronize with BaseColorHex when color changes in ColorPicker
            BaseColorHex = $"#{value.R:X2}{value.G:X2}{value.B:X2}";
            ChartConfig.BaseColor = ToScottPlotColor(value);
        }
    }

    /// <summary>
    /// Gets the chart plotter instance for managing charts.
    /// </summary>
    private IChartPlotter ChartPlotter { get; }

    /// <summary>
    /// Gets the current chart configuration.
    /// </summary>
    public ChartConfig ChartConfig { get; }

    /// <summary>
    /// Gets the collection of available chart types.
    /// </summary>
    public ObservableCollection<ChartType> ChartTypes { get; }

    /// <summary>
    /// Gets or sets the currently selected chart type.
    /// </summary>
    public ChartType SelectedChartType
    {
        get => _selectedChartType;
        set => this.RaiseAndSetIfChanged(ref _selectedChartType, value);
    }

    /// <summary>
    /// Gets or sets the status message displayed in the dialog.
    /// </summary>
    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    /// <summary>
    /// Gets or sets the base color as a hexadecimal string.
    /// </summary>
    public string? BaseColorHex
    {
        get => _baseColorHex;
        set
        {
            this.RaiseAndSetIfChanged(ref _baseColorHex, value);
            if (string.IsNullOrWhiteSpace(value)) return;

            try
            {
                ChartConfig.BaseColor = ChartPlotter.ParseColor(value);
            }
            catch
            {
                StatusMessage = "Invalid color hex.";
            }
        }
    }

    /// <summary>
    /// Gets the command to quit the dialog.
    /// </summary>
    public ReactiveCommand<Unit, Unit> QuitCommand { get; }

    /// <summary>
    /// Gets the command to apply changes to the chart configuration.
    /// </summary>
    public ReactiveCommand<Unit, Unit> ApplyChangesCommand { get; }

    /// <summary>
    /// Gets or sets the <see cref="AvaPlot"/> instance for displaying the chart.
    /// </summary>
    public AvaPlot? AvaPlot
    {
        get => _avaPlot;
        set => this.RaiseAndSetIfChanged(ref _avaPlot, value);
    }

    /// <summary>
    /// Updates the chart with the current configuration and data.
    /// </summary>
    public void UpdateChart()
    {
        if (AvaPlot == null) return;
        ChartPlotter.CreateChart(AvaPlot);
        StatusMessage = "Chart updated.";
    }

    /// <summary>
    /// Closes the dialog.
    /// </summary>
    private void CloseDialog()
    {
        _dialog?.Close("Window closed");
    }

    /// <summary>
    /// Applies changes to the chart configuration and updates the chart.
    /// </summary>
    private void ApplyChanges()
    {
        // Update the chart service with the selected chart type and regenerate the chart
        var currentConfig = ChartPlotter.GetConfig();

        var colors = currentConfig.KeepColorsNull
            ? null
            : currentConfig.Colors?.Select(ChartPlotter.ColorToHex).ToArray();

        ChartPlotter.Setup(
            SelectedChartType,
            currentConfig.Title,
            currentConfig.Labels,
            colors,
            BaseColorHex,
            currentConfig.KeepColorsNull,
            currentConfig.ShowLegend,
            currentConfig.XLabel,
            currentConfig.YLabel,
            currentConfig.PieExplodeFraction,
            currentConfig.PieSliceLabelDistance,
            currentConfig.HistogramBinCount,
            currentConfig.LineShowMarkers,
            currentConfig.BarWidthPercentage
        );
        UpdateChart();
    }

    /// <summary>
    /// Converts a <see cref="ScottPlot.Color"/> to an Avalonia <see cref="Color"/>.
    /// </summary>
    /// <param name="color">The ScottPlot color to convert.</param>
    /// <returns>The equivalent Avalonia color.</returns>
    private static Color ToAvaloniaColor(ScottPlot.Color color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// Converts an Avalonia <see cref="Color"/> to a <see cref="ScottPlot.Color"/>.
    /// </summary>
    /// <param name="color">The Avalonia color to convert.</param>
    /// <returns>The equivalent ScottPlot color.</returns>
    private static ScottPlot.Color ToScottPlotColor(Color color)
    {
        return new ScottPlot.Color(color.R, color.G, color.B, color.A);
    }
}