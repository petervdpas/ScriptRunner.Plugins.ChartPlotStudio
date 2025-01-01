using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ScottPlot.Avalonia;
using ScriptRunner.Plugins.ChartPlotStudio.ViewModels;

namespace ScriptRunner.Plugins.ChartPlotStudio.Dialogs;

/// <summary>
/// Represents a dialog window for displaying and interacting with a chart.
/// </summary>
public partial class ChartDialog : Window
{
    private readonly AvaPlot? _avaPlot;
    private bool _isClosingHandled;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChartDialog"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the AvaPlot control cannot be found in the XAML template.
    /// </exception>
    public ChartDialog()
    {
        InitializeComponent();

        var avaPlot = this.FindControl<AvaPlot>("AvaPlotOne");
        _avaPlot = avaPlot ?? throw new InvalidOperationException("Could not find AvaPlotOne control.");

        DataContextChanged += OnDataContextChanged;

        Closing += OnDialogClosing;
    }

    /// <summary>
    /// Handles changes to the <see cref="StyledElement.DataContext"/> property.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing data for the change.</param>
    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is not ChartDialogModel viewModel) return;

        viewModel.AvaPlot = _avaPlot;
        viewModel.UpdateChart();
    }

    /// <summary>
    /// Handles the dialog's closing event to ensure proper cleanup and command execution.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event arguments containing details about the closing event.</param>
    private void OnDialogClosing(object? sender, WindowClosingEventArgs e)
    {
        if (_isClosingHandled || DataContext is not ChartDialogModel viewModel) return;

        _isClosingHandled = true;
        e.Cancel = false;

        viewModel.QuitCommand.Execute().Subscribe(_ => { });
    }
    
    /// <summary>
    /// Initializes the dialog's XAML components.
    /// </summary>
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}