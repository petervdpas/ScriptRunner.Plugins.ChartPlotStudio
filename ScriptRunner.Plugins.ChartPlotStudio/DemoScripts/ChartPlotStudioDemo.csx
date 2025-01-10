/*
{
    "TaskCategory": "Plugins",
    "TaskName": "ChartPlot Studio Demo Script",
    "TaskDetail": "A test script for the ChartPlot Studio Plugin",
    "RequiredPlugins": ["ChartPlot Studio"]
}
*/

var chartPlotStudio = new ChartPlotStudio();

// Setup the chart service with default configuration
var logger = GetLogger("ChartPlotter");
var chartPlotter = new ChartPlotter(logger: logger);
chartPlotter.Setup(
    chartType: ChartType.Pie, 
    title: "Test Chart", 
    labels: new[] { "Category A", "Category B", "Category C" },
    colors: new[] { "Red", "Blue", "Green" },
    showLegend: true,
    pieExplodeFraction: 0.1,
    pieSliceLabelDistance: 1.2  
);
// Set chart data
chartPlotter.SetData(new[] { 45.0, 30.0, 25.0 });

// Display the chart dialog using the service
var result = await chartPlotStudio.GetChartPlotDialogAsync(
    chartPlotter,
    "Chart Dialog Test", // Title for the dialog
    800,                 // Dialog width
    600                  // Dialog height
);

// Dump result of the first chart dialog
Dump($"First Chart Result: {result}");

// If the first dialog is closed, prepare a second chart (Line diagram)
if (result != null)
{
    // Setup the chart service for the second chart (Line chart)
    chartPlotter.Setup(
        chartType: ChartType.Line,
        title: "Test Line Chart",
        labels: null,
        colors: null,
        keepColorsNull: true,
        showLegend: false
    );
    // Set data for the line chart
    chartPlotter.SetData(new[] { 10.0, 20.0, 15.0, 25.0, 30.0, 20.0 });
    
    // Display the Line chart dialog
    var secondResult = await chartPlotStudio.GetChartPlotDialogAsync(
        chartPlotter,
        "Line Chart Dialog", // Title for the dialog
        800,                 // Dialog width
        600                  // Dialog height
    );

    // Dump result of the second chart dialog
    Dump($"Second Chart Result: {secondResult}");

    // If the second dialog is closed, prepare a third chart (Histogram)
    if (secondResult != null)
    {
        // Setup the chart service for the third chart (Histogram)
        chartPlotter.Setup(
            chartType: ChartType.Histogram,
            title: "Test Histogram Chart",
            labels: null,
            colors: new[] { "Purple" },
            showLegend: true,
            yLabel: "Frequency",
            xLabel: "Values",
            histogramBinCount: 5
        );
        // Set data for the histogram
        chartPlotter.SetData(new[] { 10.0, 20.0, 20.0, 15.0, 25.0, 30.0, 20.0, 25.0, 15.0 });

        // Display the Histogram chart dialog
        var thirdResult = await chartPlotStudio.GetChartPlotDialogAsync(
            chartPlotter,
            "Histogram Chart Dialog", // Title for the dialog
            800,                      // Dialog width
            600                       // Dialog height
        );

        // Dump result of the third chart dialog
        Dump($"Third Chart Result: {thirdResult}");
    }
}

return "ChartPlot Studio Demo completed";