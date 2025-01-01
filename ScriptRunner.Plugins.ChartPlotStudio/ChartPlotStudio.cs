using System.Threading.Tasks;
using ScriptRunner.Plugins.ChartPlotStudio.Dialogs;
using ScriptRunner.Plugins.ChartPlotStudio.Interfaces;
using ScriptRunner.Plugins.ChartPlotStudio.ViewModels;
using ScriptRunner.Plugins.Utilities;

namespace ScriptRunner.Plugins.ChartPlotStudio;

/// <summary>
/// Provides functionalities for rendering and interacting with chart dialogs.
/// </summary>
public class ChartPlotStudio : IChartPlotStudio
{
    /// <summary>
    /// Displays a modal dialog to render a chart based on the specified parameters.
    /// </summary>
    /// <param name="chartPlotter">
    /// An instance of <see cref="IChartPlotter" />
    /// used to generate and configure the chart.
    /// </param>
    /// <param name="title">The title of the dialog window.</param>
    /// <param name="width">The width of the dialog window in pixels. Defaults to 640.</param>
    /// <param name="height">The height of the dialog window in pixels. Defaults to 200.</param>
    /// <returns>
    /// A <see cref="Task" /> representing the asynchronous operation.
    /// The task result is a <see cref="string" /> indicating the result of the dialog interaction,
    /// or <c>null</c> if the dialog was canceled.
    /// </returns>
    /// <remarks>
    /// This method initializes a <see cref="ChartDialog" /> and binds it to
    /// a <see cref="ChartDialogModel" /> instance with the provided chart parameters.
    /// The dialog renders the chart using the <see cref="IChartPlotter" /> and opens as a modal window.
    /// The method supports customization of the dialog's dimensions and title.
    /// </remarks>
    public async Task<string?> GetChartPlotDialogAsync(
        IChartPlotter chartPlotter,
        string title, 
        int width = 640, 
        int height = 200)
    {
        var dialog = new ChartDialog
        {
            Title = title,
            Width = width,
            Height = height
        };

        var viewModel = new ChartDialogModel(dialog, chartPlotter);
        dialog.DataContext = viewModel;

        return await DialogHelper.ShowDialogAsync(dialog.ShowDialog<string?>);
    }
}