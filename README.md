# ScriptRunner.Plugins.ChartPlotStudio

![License](https://img.shields.io/badge/license-MIT-green)  
![Version](https://img.shields.io/badge/version-1.0.0-blue)

A powerful plugin for **ScriptRunner**, enabling seamless creation and interaction with customizable charts.  
This plugin integrates with ScottPlot and Avalonia to deliver dynamic charting capabilities within your scripting environment.

---

## 🚀 Features

- **Chart Creation**: Supports multiple chart types such as Pie, Histogram, Line, and Bar charts.
- **Modal Dialogs**: Interactive chart rendering through modal dialogs powered by Avalonia.
- **Customizable Configurations**: Adjust chart properties like colors, labels, legends, and dimensions.
- **Seamless Integration**: Designed to work natively with the ScriptRunner ecosystem.
- **Dynamic Updates**: Modify chart configurations in real-time with a user-friendly interface.

---

## 📦 Installation

### Plugin Activation

Place the `ScriptRunner.Plugins.ChartPlotStudio` plugin assembly in the `Plugins` folder of your ScriptRunner project.  
The plugin will be automatically discovered and activated.

---

## 📖 Usage

### Writing a Script

Here’s an example script demonstrating how to use the ChartPlot Studio plugin to display multiple types of charts interactively:

```csharp
/*
{
    "TaskCategory": "Plugins",
    "TaskName": "ChartPlot Studio Demo Script",
    "TaskDetail": "A test script for the ChartPlot Studio Plugin"
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
```

---

### Features Highlighted in the Script:

- **Interactive Modal Dialogs**:
    - The script demonstrates how to use `ChartPlotStudio` to display multiple chart dialogs sequentially.
- **Dynamic Chart Customization**:
    - Modify chart types (e.g., `Pie`, `Line`, `Histogram`) and properties (e.g., colors, labels, dimensions) at runtime.
- **Real-Time Chart Updates**:
    - The `ChartPlotStudio` plugin allows for live interaction and customization of chart parameters.

---

## 🔧 Configuration

### Chart Configuration Properties

You can customize the following properties of a chart via `IChartPlotter.Setup`:
- **ChartType**: Specify the type of chart (e.g., `Pie`, `Histogram`, `Line`, `Bar`).
- **Title**: Set the chart's title.
- **Labels**: Provide labels for the chart elements (e.g., Pie slices or Bar categories).
- **Colors**: Customize chart colors using hex strings.
- **Legend Visibility**: Control whether to display the legend.
- **Axis Labels**: Set the X and Y-axis labels for applicable chart types.

---

## 🌟 Advanced Features

### Real-Time Updates

The `ChartDialog` binds to a `ChartDialogModel`, enabling dynamic updates to chart properties.  
For example, users can change chart types or colors in real-time.

### Supported Chart Types

The following chart types are supported:
- **Pie Chart**: Ideal for proportional data visualization.
- **Histogram**: Displays frequency distributions.
- **Line Chart**: Suitable for trend analysis.
- **Bar Chart**: Useful for categorical comparisons.

### Custom Colors

Customize colors using hexadecimal values or predefined color names:

```csharp
chartPlotter.Setup(
    ChartType.Bar,
    "Example Chart",
    colors: new[] { "#FF0000", "#00FF00", "#0000FF" }
);
```

---

## 📄 Contributing

1. Fork this repository.
2. Create a feature branch (`git checkout -b feature/YourFeature`).
3. Commit your changes (`git commit -m 'Add YourFeature'`).
4. Push the branch (`git push origin feature/YourFeature`).
5. Open a pull request.

---

## Author

Developed with **💡 creativity** by **Peter van de Pas**.

For any questions or feedback, feel free to open an issue or contact me directly!

---

## 🔗 Links

- [ScriptRunner Plugins Repository](https://github.com/petervdpas/ScriptRunner.Plugins)

---

## License

This project is licensed under the [MIT License](./LICENSE). 