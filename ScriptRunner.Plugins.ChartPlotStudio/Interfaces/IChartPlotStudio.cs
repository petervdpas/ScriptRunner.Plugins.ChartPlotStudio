using System.Threading.Tasks;

namespace ScriptRunner.Plugins.ChartPlotStudio.Interfaces;

/// <summary>
/// Defines the interface for functionalities related to rendering and interacting with chart dialogs.
/// </summary>
public interface IChartPlotStudio
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
    /// This method is used to display a chart dialog configured with the provided parameters.
    /// The dialog binds to a view model and renders a chart using the <see cref="IChartPlotter" /> implementation.
    /// </remarks>
    Task<string?> ShowChartDialogAsync(
        IChartPlotter chartPlotter,
        string title,
        int width = 640,
        int height = 200);
}